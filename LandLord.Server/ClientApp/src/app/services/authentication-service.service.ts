import { Injectable } from '@angular/core';
import { HttpClient } from 'selenium-webdriver/http';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationServiceService {
  //  currentUserSubject: BehaviorSubject<User>;
  //  currentUser: Observable<User>;

  //constructor(private httpClient: HttpClient) {
  //  this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
  //  this.currentUser = this.currentUserSubject.asObserable();
  //}
}
