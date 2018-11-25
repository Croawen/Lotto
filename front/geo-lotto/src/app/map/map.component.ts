import { Component, OnInit } from '@angular/core';
import { ApiService } from '../services/api.service';
import { PointLL } from '../models/PointLL';
import { GameService } from '../services/game.service';
import { NgxSmartModalService } from 'ngx-smart-modal';
import { LotteryModel } from '../models/LotteryModel';

@Component({
    selector: 'app-map',
    templateUrl: './map.component.html',
    styleUrls: ['./map.component.scss']
})
export class MapComponent implements OnInit {
    public left: Number;
    public userLL = new PointLL(0, 0);
    public timer = 0;
    public showPlayerWin: boolean;
    public playersCount = 0;

    timerInterval;
    generatorRun = true;
    maxLng = 24.09;
    minLng = 14.07;
    maxLat = 54.50088;
    minLat = 49;
    deltaLng = 10.02;
    deltaLat = 5.50088;
    lng = 0;
    lat = 0;
    private Lottery = new LotteryModel();

    constructor(private apiService: ApiService, private gameService: GameService) { }

    ngOnInit() {
        this.generateRandomCircleCoords();
        this.gameService.LotteryTimerObservable.subscribe((lottery) => {
            if (this.Lottery.rollId !== lottery.rollId) {
                this.checkWin();
            }
            this.Lottery = lottery;
            this.timer = lottery.rollDate;
            this.playersCount = lottery.participantsCount;
            this.resetTimerCounter();
        });
    }

    setWinningPoint() {
        let styles = {
            'top': 100 * (this.maxLat - this.lat) / this.deltaLat + "%",
            'left': 100 * (this.lng - this.minLng) / this.deltaLng + "%",
        };
        return styles;
    }

    setUserPoint() {
        if (this.userLL.lat && this.userLL.lng) {
            return {
                'top': 100 * (this.maxLat - this.userLL.lat) / this.deltaLat + "%",
                'left': 100 * (this.userLL.lng - this.minLng) / this.deltaLng + "%",
            };
        }

    }
    generateRandomCircleCoords = async () => {
        while (true && this.generatorRun) {
            await new Promise(res => setTimeout(res, 1000));
            const random1 = 50.8377089 + Math.random() * (54.1080676 - 50.8377089);
            const random2 = 16.2950239 + Math.random() * (22.3130762 - 16.2950239);
            this.lat = random1;
            this.lng = random2;
        }
    };

    play() {
        this.apiService.getLocalization().subscribe((res) => {
            this.userLL.lat = res['ll'][0];
            this.userLL.lng = res['ll'][1];
            this.gameService.setNewTicket(this.userLL);
        });
    }

    resetTimerCounter() {
        clearInterval(this.timerInterval);
        this.timerInterval = setInterval(() => {
            if (this.timer > 0) {
                this.timer--;
            }
        }, 1000);
    }

    isPlayerLoggedIn() {
        return localStorage.getItem('isLoggedIn') === "true" ? true : false;
    }

    next() {
        this.showPlayerWin = false;
    }

    isPlayerPlaying() {
        return this.gameService.PlayerHasTicket;
    }

    checkWin() {
        this.gameService.checkWin().then((isWin) => {
            if (isWin === true) {
                this.showPlayerWin = true;
            } else if (isWin === false) {
                this.showPlayerWin = false;
            }
        });
    }
}
