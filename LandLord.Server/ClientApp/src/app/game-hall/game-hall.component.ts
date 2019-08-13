import { Component, OnInit } from '@angular/core';
import { SignalrService } from '../services/signalr.service';
import { HttpClient } from '@angular/common/http';
import { Room } from '../models/room';

@Component({
  selector: 'app-game-hall',
  templateUrl: './game-hall.component.html',
  styleUrls: ['./game-hall.component.css']
})
export class GameHallComponent implements OnInit {
    rooms: Room[];

  constructor(private httpClient: HttpClient, private signalRService: SignalrService) {
  }

  ngOnInit() {
    this.httpClient.get("/api/GameRoom", {})
      .subscribe((r: Room[]) => {
        this.rooms = r;
        console.log(1, this.rooms);
      });
  }

  // create a room and get prepared
  createRoom() {
    this.httpClient.put("/api/GameRoom", {})
      .subscribe((r: Room) => {
        this.rooms.push(r);
        return this.signalRService.JoinRoom(r.id);
      });
  }}
