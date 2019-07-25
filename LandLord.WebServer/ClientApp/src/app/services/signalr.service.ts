import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { RoomStateWatcherService } from './state-watcher.service';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {

  private connection: signalR.HubConnection
  private thenable: Promise<void>;

  constructor(private stateWatcher: RoomStateWatcherService ) {
    this.connection= new signalR.HubConnectionBuilder()
      .withUrl('/gamehub')
      .build();
    this.setup();
    this.start();
  }

  public setup(){
    this.connection.on("ReceiveState", (state) => {
      console.log("ReceiveState", state);
      this.stateWatcher.chanageState(state);
    });
    this.connection.on("ReceiveError", (error) => {
      console.log("ReceiveError", error);
    });
    this.connection.on("AddingToRoomSucceeded", roomId => {
      console.log("AddingToRoomSucceeded", roomId);
    });
    this.connection.on("PlayCards", (index, cards) => {
      console.log("PlayCards", index, cards);
    });
    this.connection.on("Win", (index) => {
      console.log("Win", index);
    });
  }

  public start() {
    this.thenable = this.connection
      .start();
    this.thenable
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err))
  }

  public PullLatestState(roomId: string)
  {
    this.thenable.then(() =>
      this.connection.invoke("PushLatestStateToCurrentPlayer", roomId)
    );
  }

  public JoinRoom(roomId:string)
  {
    this.thenable.then(() => {
      this.connection.invoke("AddToRoom", roomId);
    });
  }

  public StartPlayingCards(roomId:string, cards)
  {
    this.thenable.then(() => {
      this.connection.invoke("StartPlayingCards", roomId, cards);
    }) 
  }

  public PlayCards(roomId:string, cards)
  {
    this.thenable.then(() => {
      this.connection.invoke("PlayCards", roomId, cards);
    }) 
  }
}
