import { Injectable } from '@angular/core';


enum CardValue {
  Three  = 3, Four  = 4, Five  = 5, Six  = 6, Seven  = 7, Eight  = 8, Nine = 9, Ten = 10, 
  Jack = 11, Queen = 12, King = 13, 
  Ace = 14, Two  = 15,
}

enum JokerType  {
  Black = 20,
  Red  =  21,
}

enum Suit {
  Spade   = 1,
  Club    = 2,
  Diamond = 3,
  Heart   = 4,
}

interface  PlayingCardShape 
{
  case: string,
  fields: Int32Array,
}

export interface PlayerCardShape 
{
  case: string,
  fields: Array<PlayingCardShape>,
}

@Injectable({
  providedIn: 'root'
})
export class CardConverterService {

  constructor() { }

  playingCardToString(cardShape: PlayingCardShape){
    switch(cardShape.case){
        case "Joker" :
          let j = cardShape.fields[0];
          return JokerType[j];
        case "NormalCard":
          let v = cardShape.fields[0];
          let s = cardShape.fields[1];
          return Suit[s] + CardValue[v];
    }
  }

  playerCardToString(card: PlayerCardShape)
  {
    switch(card.case){
        case "PlayingCard" :
          let j = card.fields[0];
          return this.playingCardToString(j);
        case "Shadow":
          return "Shadow";
    }
  }

}
