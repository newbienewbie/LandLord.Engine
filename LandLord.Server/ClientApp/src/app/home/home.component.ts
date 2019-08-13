import { Component } from '@angular/core';
import { Room } from '../models/room';
import { HttpClient } from '@angular/common/http';
import { Player } from '../models/Player';
import { SignalrService } from '../services/signalr.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  rooms: Room[] =[];

  constructor(private httpClient: HttpClient) {
  }

  ngOnInit(){
  }
}
