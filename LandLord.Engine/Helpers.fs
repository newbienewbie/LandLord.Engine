namespace Itminus.LandLord.Engine

open System
open System.Collections.Generic

open Card
open System.Linq

module Facade=
    open SanDaiN
    open Bomb

    let createFullCards () =
        let suits: Suit seq  = unbox( Enum.GetValues(typeof<Suit>) )
        let values: CardValue seq = unbox(Enum.GetValues(typeof<CardValue>))

        let cards =
            suits
            |> Seq.collect (fun suit -> 
                values
                |> Seq.map (fun value -> NormalCard(value, suit))
                |> Seq.toList
            )
            |> Seq.toList
        Joker(JokerType.Red)::Joker(JokerType.Black)::cards

    let shuffle (cards: PlayingCard list) = 

        let rand=new System.Random()
        let length = List.length cards

        let random min = 
            rand.Next(min, length)

        let swap i j (a: PlayingCard[] )  = 
            let tmp = a.[i]
            a.[i] <- a.[j]
            a.[j] <- tmp
            ()

        let array = cards |> Array.ofList

        // random
        array
        |> Array.iteri (fun i it -> 
            let r = random i
            swap i r array 
        )

        array |> List.ofArray

    let deal (cards: PlayingCard list) = 
        let reserved = cards |> List.take 3
        let dealing = cards |> List.skip 3 |> List.mapi (fun i it -> (i % 3 , it))

        let _cardsOfPlayerN nth =
            dealing 
            |> List.where (fun (i, it) -> i = nth ) 
            |> List.map (fun (i, it) -> it)

        let dealing = (_cardsOfPlayerN 0, _cardsOfPlayerN 1, _cardsOfPlayerN 2)
        reserved, dealing


    let canStartPlaying list =
        match list with
        | Single cards -> true
        | Double cards -> true
        | Trible cards -> true
        | LianShun5 cards -> true
        | LianShun6 cards -> true
        | LianShun7 cards -> true
        | LianShun8 cards -> true
        | LianShun9 cards -> true
        | LianShun10 cards -> true
        | LianShun11 cards -> true
        | LianShun12 cards -> true
        | ShunZi 2 3 cards -> true
        | ShunZi 2 4 cards -> true
        | ShunZi 2 5 cards -> true
        | ShunZi 2 6 cards -> true
        | ShunZi 2 7 cards -> true
        | ShunZi 2 8 cards -> true
        | ShunZi 2 9 cards -> true
        | ShunZi 2 10 cards -> true
        | ShunZi 2 11 cards -> true
        | ShunZi 2 12 cards -> true
        | ShunZi 2 13 cards -> true
        | ShunZi 3 3 cards -> true
        | ShunZi 3 4 cards -> true
        | ShunZi 3 5 cards -> true
        | ShunZi 3 6 cards -> true
        | ShunZi 3 7 cards -> true
        | ShunZi 3 8 cards -> true
        | ShunZi 3 9 cards -> true
        | ShunZi 3 10 cards -> true
        | ShunZi 3 11 cards -> true
        | ShunZi 3 12 cards -> true
        | ShunZi 3 13 cards -> true
        | SanDai1 cards -> true
        | Bomb cards -> true
        | _ -> false


    let canPlay prevCards cards =

        let gt card1 card2 = 
            let d = compare false card1 card2
            d > 0

        let seqGt (cards1: PlayingCard list) (cards2: PlayingCard list) = 
            let card1 = cards1 |> List.head
            let card2 = cards2 |> List.head
            gt card1 card2

        /// assume cards1 and cards2 are both the | SanDai1 | pattern
        let SanDai1GreaterThan (cards1: PlayingCard list) (cards2: PlayingCard list)=
            let dominatingCard (cards: PlayingCard list) = 
                let g =
                    cards
                    |> List.groupBy (fun i -> getWeight false i)
                    |> List.map (fun g -> snd g)
                    |> List.sortBy (fun i -> List.length i )
                match g with
                | [[c1]; [c2;c3;c4]] 
                    when (getWeight false c2) = (getWeight false c3) 
                        && (getWeight false c3) = (getWeight false c4) 
                    -> c4
                | _ -> 
                    let msg = sprintf "unsupported cards %A" cards
                    failwith msg
            let c1 = dominatingCard cards1
            let c2 = dominatingCard cards2
            gt c1 c2


        let pattern = (prevCards |> sort , cards |> sort) 
        match pattern with
        | (Single card1, Single card2) when gt card2 card1 -> true
        | (Double cards1, Double cards2) when seqGt cards2 cards1 -> true
        | (Trible cards1, Trible cards2) when seqGt cards2 cards1 -> true
        | (LianShun5 cards1, LianShun5 cards2) when seqGt cards2 cards1 -> true
        | (LianShun6 cards1, LianShun6 cards2) when seqGt cards2 cards1 -> true
        | (LianShun7 cards1, LianShun7 cards2) when seqGt cards2 cards1 -> true
        | (LianShun8 cards1, LianShun8 cards2) when seqGt cards2 cards1 -> true
        | (LianShun9 cards1, LianShun9 cards2) when seqGt cards2 cards1 -> true
        | (LianShun10 cards1, LianShun10 cards2) when seqGt cards2 cards1 -> true
        | (LianShun11 cards1, LianShun11 cards2) when seqGt cards2 cards1 -> true
        | (LianShun12 cards1, LianShun12 cards2) when seqGt cards2 cards1 -> true
        | (ShunZi 2 3 cards1, ShunZi 2 3 cards2) when seqGt cards2 cards1 -> true
        | (ShunZi 2 4 cards1, ShunZi 2 4 cards2) when seqGt cards2 cards1 -> true
        | (ShunZi 2 5 cards1, ShunZi 2 5 cards2) when seqGt cards2 cards1 -> true
        | (ShunZi 2 6 cards1, ShunZi 2 6 cards2) when seqGt cards2 cards1 -> true
        | (ShunZi 2 7 cards1, ShunZi 2 7 cards2) when seqGt cards2 cards1 -> true
        | (ShunZi 2 8 cards1, ShunZi 2 8 cards2) when seqGt cards2 cards1 -> true
        | (ShunZi 2 9 cards1, ShunZi 2 9 cards2) when seqGt cards2 cards1 -> true
        | (ShunZi 2 11 cards1, ShunZi 2 11 cards2) when seqGt cards2 cards1 -> true
        | (ShunZi 2 12 cards1, ShunZi 2 12 cards2) when seqGt cards2 cards1 -> true
        | (ShunZi 2 13 cards1, ShunZi 2 13 cards2) when seqGt cards2 cards1 -> true
        | (ShunZi 3 3 cards1, ShunZi 3 3 cards2) when seqGt cards2 cards1 -> true
        | (ShunZi 3 4 cards1, ShunZi 3 4 cards2) when seqGt cards2 cards1 -> true
        | (ShunZi 3 5 cards1, ShunZi 3 5 cards2) when seqGt cards2 cards1 -> true
        | (ShunZi 3 6 cards1, ShunZi 3 6 cards2) when seqGt cards2 cards1 -> true
        | (ShunZi 3 7 cards1, ShunZi 3 7 cards2) when seqGt cards2 cards1 -> true
        | (ShunZi 3 8 cards1, ShunZi 3 8 cards2) when seqGt cards2 cards1 -> true
        | (ShunZi 3 9 cards1, ShunZi 3 9 cards2) when seqGt cards2 cards1 -> true
        | (ShunZi 3 11 cards1, ShunZi 3 11 cards2) when seqGt cards2 cards1 -> true
        | (ShunZi 3 12 cards1, ShunZi 3 12 cards2) when seqGt cards2 cards1 -> true
        | (ShunZi 3 13 cards1, ShunZi 3 13 cards2) when seqGt cards2 cards1 -> true
        // 3+1 
        | (SanDai1 cards1, SanDai1 cards2) when SanDai1GreaterThan cards2 cards1 -> true
        | (Bomb b1, Bomb b2) -> seqGt b2 b1
        // the prev mustn't be a bomb because it would be matched by the above (bomb1,bomb2) pattern
        | (_, Bomb b1) -> true           
        | _ -> false
            


