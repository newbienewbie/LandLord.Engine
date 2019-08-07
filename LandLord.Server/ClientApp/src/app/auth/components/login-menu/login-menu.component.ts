import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth-service.service';
import { User } from '../../models';

@Component({
  selector: 'auth-login-menu',
  templateUrl: './login-menu.component.html',
  styleUrls: ['./login-menu.component.css']
})
export class LoginMenuComponent implements OnInit {

  user: User;

  constructor(protected authService: AuthService) { 
  }

  ngOnInit() {
    this.authService.currentUser.subscribe(u => this.user = u);
  }

}
