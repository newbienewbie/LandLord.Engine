import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../services/auth-service.service';
import { User } from '../../models';


@Component({
  selector: 'auth-login-menu',
  templateUrl: './login-menu.component.html',
  styleUrls: ['./login-menu.component.css']
})
export class LoginMenuComponent implements OnInit {

  user: User;

  constructor(protected authService: AuthService, private http: HttpClient) { 
  }

  ngOnInit() {
    this.authService.currentUser.subscribe(u => this.user = u);
  }

  onLogout() {
    this.authService.logout();
    this.http.post("/identity/account/logout", {
      headers: {
        "Content-Type":"x-www-urlencoded",
      },
    })
  }

}
