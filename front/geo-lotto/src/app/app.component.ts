import { Component, OnInit } from '@angular/core';
import { AuthService } from './services/auth.service';
import { HubService } from './services/hub.service';
import { GameService } from './services/game.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {


  constructor(private authService: AuthService, private hub: HubService, private gameService: GameService, private router: Router) { }

  ngOnInit() {
    if (localStorage.getItem('isLoggedIn')) {
      this.hub.connect(localStorage.getItem('userId')).then(() => {
        this.gameService.initGameService();
        this.router.navigate(['/map']);
      });
    }
  }

  logOut() {
    localStorage.removeItem('isLoggedIn');
    localStorage.removeItem('userId');
    this.router.navigate(['/login']);
  }

  isPlayerLoggedIn() {
    return localStorage.getItem('isLoggedIn') === "true" ? true : false;
  }
}
