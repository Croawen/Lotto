import { Component, OnInit } from '@angular/core';
import { LoginModel } from '../models/LoginModel';
import { AuthService } from '../services/auth.service';
import { LoginComponent } from '../login/login.component';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  public credentials = new LoginModel();

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  signUp() {
    this.authService.signUp(this.credentials);
  }

}
