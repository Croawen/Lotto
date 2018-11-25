import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { LoginModel } from '../models/LoginModel';
import { HubService } from '../services/hub.service';
import { GameService } from '../services/game.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(private authService: AuthService, private hub: HubService, private gameService: GameService, private router: Router) { }

  public credentials = new LoginModel();

  ngOnInit() {
  }

  signIn() {
    this.authService.signIn(this.credentials)
      .subscribe((res) => {
        this.hub.connect(res['user_id']).then(() => {
          localStorage.setItem('userId', res['user_id']);
          localStorage.setItem('isLoggedIn', 'true');
          this.gameService.initGameService();
          this.router.navigate(['/map']);
        });
      });
  }
}
