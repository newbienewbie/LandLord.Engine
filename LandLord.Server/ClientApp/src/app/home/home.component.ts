import { Component } from '@angular/core';
import { Room } from '../models/room';
import { HttpClient } from '@angular/common/http';
import { RoomStateWatcherService } from '../services/state-watcher.service';
import { Player } from '../models/Player';
import { SignalrService } from '../services/signalr.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  rooms: Room[] =[];

  constructor(private httpClient: HttpClient, private signalRService: SignalrService,private stateWatcher: RoomStateWatcherService) {
  }

  ngOnInit(){
    this.httpClient.get("/api/GameRoom", {})
      .subscribe((r: Room[]) => {
        this.rooms = r;
        console.log(1,this.rooms);
      });
  }

  // create a room and get prepared
  createRoom()
  {
    this.httpClient.put("/api/GameRoom", {})
      .subscribe((r: Room) => {
        this.rooms.push(r);
        return this.signalRService.JoinRoom(r.id);
      });
  }
}
