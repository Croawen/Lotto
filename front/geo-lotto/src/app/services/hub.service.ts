import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { Observable, Subject } from 'rxjs';
import { LotteryModel } from '../models/LotteryModel';
import { NewTicketModel } from '../models/NewTicketModel';

@Injectable()
export class HubService {

  private _hubConnection: signalR.HubConnection;
  private hubRoute = "http://hackyeah.azurewebsites.net/game/hub";
  public lottery = new Subject<LotteryModel>();

  constructor() { }

  connect(userId): Promise<any> {
    let promise = new Promise((resolve, reject) => {
      this._hubConnection = new signalR.HubConnectionBuilder()
        .withUrl(this.hubRoute + "?userId=" + userId)
        .configureLogging(signalR.LogLevel.Trace)
        .withHubProtocol(new signalR.JsonHubProtocol())
        .build();

      this._hubConnection
        .start()
        .then(() => {

          this._hubConnection.on('NewRollAvailable', (type: string, payload: string) => {
            this._hubConnection.invoke("InvokeNextRollData");
          });

          this._hubConnection.on('NextRollData', (lottery: LotteryModel, payload: string) => {
            this.lottery.next(lottery);
          });
          this._hubConnection.invoke("InvokeNextRollData");

          resolve();
        })
        .catch(err => {
          console.log('Error while establishing connection :(');
          reject();
        });

    });
    return promise;
  }

  setNewTicket(newTicket: NewTicketModel) {
    return this._hubConnection.invoke('InvokeBuyTicket', newTicket);
  }

  checkWin(rollId) {
    let promise = new Promise((resolve, reject) => {

      this._hubConnection.invoke('InvokeHasWonRoll', rollId).then(() => {
        this._hubConnection.on('HasWonRoll', (type: any, payload: string) => {
          resolve(type);
        });
      });
    });
    return promise;
  }
}
