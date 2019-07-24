import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { GameRoomDetail } from '../models/room-detail';
import { HttpClient } from '@angular/common/http';
import { SignalrService } from '../services/signalr.service';
import { RoomStateWatcherService } from '../services/state-watcher.service';

@Component({
  selector: 'app-room-detail',
  templateUrl: './room-detail.component.html',
  styleUrls: ['./room-detail.component.css']
})
export class RoomDetailComponent implements OnInit {

  id: string;
  room: GameRoomDetail = new GameRoomDetail();
  index: number;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private httpClient: HttpClient,
    private signalRService: SignalrService,
    private stateWatcher: RoomStateWatcherService,
  ) {
    this.route.params.subscribe(o => {
      this.id = o["id"];
      // 向服务器请求 GameRoom细节
      this.httpClient.get(`/api/GameRoom/${this.id}`, {})
        .subscribe((r: GameRoomDetail) => {
          this.room = r;
          console.log("GameRoolDetail",this.room);
        });
    });
    this.stateWatcher.onChangeState = state=> this.room = state ;
  }


  ngOnInit() {

  }

}
