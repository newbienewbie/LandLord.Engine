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
        return this.joker(JokerType[j]);
      case "NormalCard":
        let v = cardShape.fields[0];
        let s = cardShape.fields[1];
        return `${this.suit(Suit[s])}${this.cardVale(CardValue[v])}`;
    }
  }

  playingCardStyle(cardShape: PlayingCardShape) {
    let styleName = "card";
    let style = "";
    switch (cardShape.case) {
      case "Joker":
        let j = cardShape.fields[0];
        style = `${styleName}-${JokerType[j]}-Joker`;
        break;
      case "NormalCard":
        let v = cardShape.fields[0];
        let s = cardShape.fields[1];
        style = `${styleName}-${Suit[s]}-${CardValue[v]}`;
        break;
    }
    return style.toLocaleLowerCase();
  }

  private joker(j:string) {
    switch (j.toLowerCase()) {
      case "red": return "🃏";
      case "black": return ":black_joker:";
    }
  }

  private suit(s: string) {
    switch (s.toLowerCase()) {
      case "heart":
        return "️♥️";
      case "diamond":
        return "♦️";
      case "club":
        return "♣️";
      case "spade":
        return "♠️";
      default:
        return s;
    }
  }

  private cardVale(v:string) {
    switch (v.toLowerCase()) {
      case "ace": return "A";
      case "two": return "2";
      case "three": return "3";
      case "four": return "4";
      case "five": return "5";
      case "six": return "6";
      case "seven": return "7";
      case "eight": return "8";
      case "nine": return "9";
      case "ten": return "10";
      case "jack": return "J";
      case "queen": return "Q";
      case "king": return "K";
      default: return v;
    }
  }


  playerCardToString(card: PlayerCardShape)
  {
    switch(card.case){
      case "PlayingCard" :
        let j = card.fields[0];
        return this.playingCardToString(j);
      case "Shadowed":
        return "㊙️";
    }
  }

  playerCardStyle(card: PlayerCardShape) {
    let style = "";
    switch(card.case){
      case "PlayingCard":
        let j = card.fields[0];
        style = this.playingCardStyle(j);
        break;
      case "Shadowed":
        style = "card-shadowed";
        break;
    }
    return style.toLocaleLowerCase();
  }


}
