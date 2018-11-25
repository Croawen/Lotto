import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class ApiService {

  constructor(private http: HttpClient) { }

  getLocalization() {
    return this.http.get('https://i10n.takeshop.pl/localize');
  }
}
