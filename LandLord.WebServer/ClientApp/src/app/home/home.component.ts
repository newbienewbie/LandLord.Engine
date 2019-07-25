import { Component } from '@angular/core';
import { Room } from '../models/room';
import { Player } from '../models/Player';
import { HttpClient } from '@angular/common/http';
import { SignalrService } from '../services/signalr.service';
import { RoomStateWatcherService } from '../services/state-watcher.service';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {

  rooms: Room[]

  constructor(private httpClient: HttpClient, private signalRService: SignalrService, private stateWatcher: RoomStateWatcherService) {
    this.httpClient.get("/api/GameRoom", {})
      .subscribe((r: Room[]) => {
        this.rooms = r;
        console.log(1,this.rooms);
      });
  }

  ngOnInit(){
    var room = new Room();

    var p1 = new Player();
    p1.id = "abc";
    p1.name = "p1";
    var p2 = new Player();
    p2.id = "bca";
    p2.name = "p2";
    var p3 = new Player();
    p3.id = "cab";
    p3.name = "p3";

    room.id = "abcde";
    room.players = [ p1,p2,p3 ];
    this.rooms = [ room ];
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
