import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { AuthService } from '../auth/services/auth-service.service';
import { of, Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { GameState } from '../models/room-detail';
import { CallbackArgument, Status, PlayCardsSucceededArg, PlayCardsFailedArg } from '../models/Arguments';




@Injectable({
  providedIn: 'root'
})
export class SignalrService {

  private connection: signalR.HubConnection
  private thenable: Promise<void>;

  public ReceiveStateObservable: Observable<GameState>;
  public PlayCardsCallbackObservable: Observable<any>;
  public BeLandLordCallbackObservable: Observable<any>;
  public PassCardsCallbackObservable: Observable<any>;

  constructor(private authService: AuthService) {
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
    this.ReceiveStateObservable = this.createStateObserverable("ReceiveState");
    this.PlayCardsCallbackObservable = this.createObserverable("PlayCardsCallback");
    this.PassCardsCallbackObservable = this.createObserverable("PassCardsCallback");
    this.BeLandLordCallbackObservable = this.createObserverable("BeLandLordCallback");
    this.start();
  }

  private createStateObserverable(cbname: string): Observable<any> {
    return Observable.create(observer=> {
      this.connection.on(cbname, (cbarg: any) => {
        observer.next(cbarg);
      });
    });
  }

  private createObserverable(cbname: string): Observable<any> {
    return Observable.create(observer=> {
      this.connection.on(cbname, (cbarg: CallbackArgument) => {
        switch (cbarg.kind){
          case Status.Success:
            observer.next(cbarg);
            break;
          case Status.Fail:
            observer.error(cbarg);
            break;
        }
      });
    });
  }

  private setup(){
    this.connection.on("ReceiveError", (error) => {
      console.log("ReceiveError", error);
    });

    this.connection.on("AddingToRoomSucceeded", roomId => {
      console.log("AddingToRoomSucceeded", roomId);
    });

    this.connection.on("Win", (index) => {
      console.log("Win", index);
    });
  }

  private start() {
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

  public PassCards(roomId:string)
  {
    this.thenable.then(() => {
      this.connection.invoke("PassCards", roomId);
    }) 
  }

}
