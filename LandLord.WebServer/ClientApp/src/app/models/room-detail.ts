import { Room } from "./room";

class PlayingCard{

}

export class GameRoomDetail extends Room
{
    LandLordIndex: number;
    CurrentTurn: number;
    PrevIndex: number;
    PrevCards: PlayingCard[];
    Cards: PlayingCard[];
    ReservedCards: PlayingCard[];
}