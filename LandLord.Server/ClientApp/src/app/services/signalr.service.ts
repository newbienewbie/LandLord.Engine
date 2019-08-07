import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { RoomStateWatcherService } from './state-watcher.service';
import { AuthService } from '../auth/services/auth-service.service';
import { of } from 'rxjs';
import { map, take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {

  private connection: signalR.HubConnection
  private thenable: Promise<void>;

  constructor(private stateWatcher: RoomStateWatcherService, private authService: AuthService) {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('/gamehub', {
        accessTokenFactory: () => {
          let isAuthenticated = this.authService.isAuthenticated()
          if (isAuthenticated) {
            return this.authService.currentUser
              .pipe(
                map(o => o.token),
                take(1),
              )
              .toPromise<string>();
          }
          else{
            console.log("not logged, connect signalR:");
            location.href="/identity/account/login";
            return of("").toPromise();
          }
        }
      })
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


    this.connection.on("BeLandLordSucceded", index=> {
      console.log("AddingToRoomSucceeded", index);
    });

    this.connection.on("BeLandLordFailed",() => {
      console.log("AddingToRoomFailed");
    });

    this.connection.on("PlayCardsSucceeded", (index, cards) => {
      console.log("PlayCardsSucceeded", index, cards);
      this.stateWatcher.playCardsSucceeded(index,cards);
    });
    this.connection.on("PlayCardsFailed", (index, cards) => {
      console.log("PlayCardsFailed", index, cards);
      this.stateWatcher.playCardsFailed(index,cards);
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

  public BeLandLord(roomId: string) {
    this.thenable.then(() => {
      this.connection.invoke("BeLandLord",roomId);
    })
  }

  public PlayCards(roomId:string, cards)
  {
    this.thenable.then(() => {
      this.connection.invoke("PlayCards", roomId, cards);
    }) 
  }
}
