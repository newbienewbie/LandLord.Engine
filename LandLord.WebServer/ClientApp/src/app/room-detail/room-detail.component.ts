import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { GameRoomDetail } from '../models/room-detail';

@Component({
  selector: 'app-room-detail',
  templateUrl: './room-detail.component.html',
  styleUrls: ['./room-detail.component.css']
})
export class RoomDetailComponent implements OnInit {

  id: string;
  GameRoom: GameRoomDetail

  constructor(
    private route: ActivatedRoute,
    private router: Router
  ) {


    this.route.params.subscribe(o => 
      this.id = o["id"]
      // 向服务器请求 GameRoom细节
    );
  }


  ngOnInit() {

  }

}
