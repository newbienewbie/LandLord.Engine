import { Injectable } from '@angular/core';
import { Room } from '../models/room';
import { GameRoomDetail } from '../models/room-detail';

@Injectable({
  providedIn: 'root'
})
export class RoomStateWatcherService {

  constructor() { }

  onChangeState: (room: GameRoomDetail) => any = null;

  chanageState(state: GameRoomDetail)
  {
    if (this.onChangeState != null) {
      this.onChangeState(state);
    }
  }

}
