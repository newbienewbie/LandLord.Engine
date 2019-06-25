namespace Itminus.LandLord.Engine

module Facade=

    open Card
    open SanDaiN
    open Bomb
    

    let canPlay prevCards cards =

        let gt card1 card2 = 
            let d = compare false card1 card2
            d > 0

        let seqGt (cards1: PlayingCard list) (cards2: PlayingCard list) = 
            let card1 = cards1 |> List.head
            let card2 = cards2 |> List.head
            gt card1 card2

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
//      | (SanDai1 cards1, SanDai1 cards2) when seqGt cards2 cards1 -> true
        | (Bomb b1, Bomb b2) 
            -> if seqGt b2 b1 then true else false
        // the prev mustn't be a bomb because it would be matched by the above (bomb1,bomb2) pattern
        | (_, Bomb b1) -> true           
        | _ -> false
            
