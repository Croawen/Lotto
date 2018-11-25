import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { LoginModel } from '../models/LoginModel';
import 'rxjs/Rx';

@Injectable()
export class AuthService {

  url = 'http://hackyeah.azurewebsites.net/';

  constructor(private http: HttpClient) { }

  signIn(credentials: LoginModel) {
    return this.http.post(this.url + "auth/login", credentials, { headers: new HttpHeaders().set('Content-Type', 'application/json') });
  }

  signUp(credentials: LoginModel) {
    this.http.post(this.url + "auth/register", credentials, { headers: new HttpHeaders().set('Content-Type', 'application/json') }).subscribe(() => { });
  }

}
