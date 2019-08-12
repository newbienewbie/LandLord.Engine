import {PlayingCard} from './room-detail' ;


export enum Status { Success = "success", Fail = "fail" }


export interface CallbackArgument {
  kind: Status;
}


export interface PlayCardsSucceededArg extends CallbackArgument
{
  kind: Status.Success;
  index: number;
  cards: PlayingCard[];
}
export interface PlayCardsFailedArg extends PlayCardsSucceededArg { }
export type PlayCardsCallbackArg = PlayCardsSucceededArg | PlayCardsFailedArg;




interface BeLandLordSucceededArg extends CallbackArgument
{
  kind: Status.Success;
  landLordIndex: number;
}
interface BeLandLordFailedArg extends CallbackArgument
{
  kind: Status.Fail
}
export type BeLandLordArg = BeLandLordSucceededArg | BeLandLordFailedArg;






