import { Room } from "./room";
import { Player } from "./Player";

class PlayingCard{

}

export class GameRoomDetail extends Room
{
    landLordIndex: number;
    currentTurn: number;
    prevIndex: number;
    prevCards: PlayingCard[];
    cards: PlayingCard[];
    reservedCards: PlayingCard[];
}
