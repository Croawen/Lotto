import { Injectable } from '@angular/core';
import { HubService } from './hub.service';
import { LotteryModel } from '../models/LotteryModel';
import { PointLL } from '../models/PointLL';
import { NewTicketModel } from '../models/NewTicketModel';
import { Subject } from 'rxjs';

@Injectable()
export class GameService {

  public LotteryTimerObservable = new Subject<LotteryModel>();
  public PlayerHasTicket: boolean;
  private rollId = 0;
  private oldRollId = 0;

  constructor(private hubService: HubService) { }

  initGameService() {
    this.hubService.lottery.subscribe((lottery: LotteryModel) => {
      this.rollId = lottery.rollId;
      this.LotteryTimerObservable.next(lottery);
      this.PlayerHasTicket = false;
    });
  }

  setNewTicket(latitude: PointLL) {
    this.hubService.setNewTicket(new NewTicketModel(latitude.lng, latitude.lat, this.rollId)).then(() => {
      this.PlayerHasTicket = true;
    });
  }

  checkWin() {
    return new Promise((resolve, reject) => {
      this.hubService.checkWin(this.oldRollId).then((isWin) => {
        this.oldRollId = this.rollId;
        resolve(isWin);
      });
    });
  }
}

