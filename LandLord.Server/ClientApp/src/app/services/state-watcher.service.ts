import { Injectable } from '@angular/core';
import { Room } from '../models/room';
import { GameRoomDetail, GameState } from '../models/room-detail';

@Injectable({
  providedIn: 'root'
})
export class RoomStateWatcherService {

  constructor() { }

  onChangeState: (room: GameState) => any = null;
  onPlayCardsSucceeded: (index, cards) => any = null;
  onPlayCardsFailed: (index, cards) => any = null;

  chanageState(state: GameState)
  {
    if (this.onChangeState != null) {
      this.onChangeState(state);
    }
  }

  playCardsSucceeded(index, cards)
  {
    if (this.onPlayCardsSucceeded != null) {
      this.onPlayCardsSucceeded(index,cards);
    }
  }

  playCardsFailed(index, cards)
  {
    if (this.onPlayCardsFailed!= null) {
      this.onPlayCardsFailed(index,cards);
    }
  }
}
