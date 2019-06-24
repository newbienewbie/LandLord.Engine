namespace Itminus.LandLord.Engine

open Card

module Bomb = 
    
    let (|Bomb|_|) (cards: PlayingCard list)= 
        match cards with
        | [NormalCard(v1, _); NormalCard(v2, _); NormalCard(v3, _); NormalCard(v4, _) ]
            when v1 = v2 && v2 = v3 && v3 = v4 -> Some(cards)
        | [Joker(j1); Joker(j2)] 
            when j1 <> j2 -> Some(cards)
        | _ -> None
    




