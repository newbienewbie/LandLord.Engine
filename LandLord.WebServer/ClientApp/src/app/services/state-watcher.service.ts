import { Injectable } from '@angular/core';
import { Room } from '../models/room';
import { GameRoomDetail, GameState } from '../models/room-detail';

@Injectable({
  providedIn: 'root'
})
export class RoomStateWatcherService {

  constructor() { }

  onChangeState: (room: GameState) => any = null;

  chanageState(state: GameState)
  {
    if (this.onChangeState != null) {
      this.onChangeState(state);
    }
  }

}
