import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { GameRoomDetail, GameState } from '../models/room-detail';
import { HttpClient } from '@angular/common/http';
import { SignalrService } from '../services/signalr.service';
import { RoomStateWatcherService } from '../services/state-watcher.service';
import { CardConverterService } from '../services/card-converter.service';
import { forEach } from '@angular/router/src/utils/collection';

@Component({
  selector: 'app-room-detail',
  templateUrl: './room-detail.component.html',
  styleUrls: ['./room-detail.component.css']
})
export class RoomDetailComponent implements OnInit {

  id: string;
  state: GameState = new GameState();
  index: number;

  selections : Boolean[];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private httpClient: HttpClient,
    private signalRService: SignalrService,
    private stateWatcher: RoomStateWatcherService,
    protected cardConverter: CardConverterService,
  ) {
    this.route.params.subscribe(o => {
      this.id = o["id"];
      this.signalRService.PullLatestState(this.id);
    });
    this.stateWatcher.onChangeState = state => {
      this.state = state;
    }
  }


  ngOnInit() {
    this.selections = new Array(20);
  }

  playCards(){
    console.log("now trying to playcards. Current state is ", this.state);
    let cards = this.state.gameRoom.cards[this.state.turnIndex].map(c => c.fields[0]);
    let selectedCards= this.selections.map((s,i) => s? cards[i] : null ).filter(c => !!c);
    console.log("try to play ", selectedCards);
    this.signalRService.StartPlayingCards(this.state.gameRoom.id, selectedCards);
  }

  selectCard(o) {
    let index = o.index;
    this.selections[index] = !this.selections[index];
  }

}
