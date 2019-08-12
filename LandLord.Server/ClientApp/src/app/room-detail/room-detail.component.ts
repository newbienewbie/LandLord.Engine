import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { GameRoomDetail, GameState } from '../models/room-detail';
import { HttpClient } from '@angular/common/http';
import { SignalrService } from '../services/signalr.service';
import { CardConverterService } from '../services/card-converter.service';
import { PlayCardsSucceededArg, PlayCardsFailedArg } from '../models/Arguments';

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
    protected cardConverter: CardConverterService,
  ) {
    this.route.params.subscribe(o => {
      this.id = o["id"];
      // pull state
      this.signalRService.PullLatestState(this.id);
      // join room
      this.signalRService.JoinRoom(this.id);
    });

    this.signalRService.ReceiveStateObservable.subscribe({
      next: state => this.state = state,
    });

    this.signalRService.PlayCardsCallbackObservable.subscribe({
      next: (args: PlayCardsSucceededArg) => {
        let length = args.cards.length;
        this.selections = new Array( this.selections.length - length );
      },
      error: (err: PlayCardsFailedArg) => {
        alert("cannot play this cards!");
      },
    });

    this.signalRService.BeLandLordCallbackObservable.subscribe({
      next: (args) => {
          console.log("Being LandLord suceeded",args);
      },
      error: (err) => {
          console.log("Being LandLord failed", err);
      }
    });

    this.signalRService.PassCardsCallbackObservable.subscribe({
      next: (args) => {
          console.log("succeed to pass cards",args);
      },
      error: (err) => {
          console.log("fail to pass cards", err);
      }
    });

  }

  ngOnInit() {
    this.selections = new Array(20);
  }

  beLandLord() {
    let landLordIndex = this.state.gameRoom.landLordIndex 
    if (landLordIndex < 0) {
      this.signalRService.BeLandLord(this.id);
    } else {
      throw new Error(`cannot be a landlord because there's already one whose index=${landLordIndex}`);
    }
  }

  playCards(){
    console.log("now trying to playcards. Current state is ", this.state);
    let cards = this.state.gameRoom.cards[this.state.turnIndex].map(c => c.fields[0]);
    let selectedCards= this.selections.map((s,i) => s? cards[i] : null ).filter(c => !!c);
    console.log("try to play ", selectedCards);
    if (this.state.gameRoom.currentTurn == this.state.turnIndex) {
      this.signalRService.PlayCards(this.state.gameRoom.id, selectedCards);
    }
  }

  passCards(){
    console.log("pass cards. Current state is ", this.state);
    if (this.state.gameRoom.currentTurn == this.state.turnIndex) {
      this.signalRService.PassCards(this.state.gameRoom.id);
    }
  }

  selectCard(o) {
    let index = o.index;
    let old = this.selections[index];
    console.log("select: ", index , old);
    this.selections[index] = ! old;
    console.log("select: ", index , this.selections[index]);
  }

  getDeskTurnIndexes() {
    let me= this.state.turnIndex;
    let left=  (me + 3 - 1 ) % 3;
    let right= (me + 3 + 1) % 3;
    return { left, me, right };
  }

  playerClass(playerIndex) {
    let deskTurnIndexes = this.getDeskTurnIndexes();
    if (playerIndex == deskTurnIndexes.left) { return "left"; }
    else if (playerIndex == deskTurnIndexes.me) { return "me"; }
    else if (playerIndex == deskTurnIndexes.right) { return "right"; }
    else {
      throw new Error(`unknown player index ${playerIndex}`);
    }
  }

}
