import { Room } from "./room";
import { Player } from "./Player";
import { PlayerCardShape } from "../services/card-converter.service";

export interface PlayerCard {

}

export interface PlayingCard {

}

export class GameRoomDetail extends Room
{
    landLordIndex: number;
    currentTurn: number;
    prevIndex: number;
    prevCards: PlayingCard[];
    cards: PlayerCardShape[][];
    reservedCards: PlayingCard[];
}

export class GameState
{
  gameRoom: GameRoomDetail;
  turnIndex: number;
}
