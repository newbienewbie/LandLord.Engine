namespace Itminus.LandLord.Engine

module SanDaiN=
    open System.Collections.Generic
    open Itminus.LandLord.Engine.Card

    let internal ``check3+1`` (values: CardValue list) : bool = 
        match List.length values with
        | 4 -> 
             let r = values |> List.map int |> List.sortBy int   
             match r with
             | [a; b; c; d;] when a=b && b=c && c <> d
                 -> true
             | [a; b; c; d;] when a<>b && b=c && c=d
                 -> true
             | _ -> false
        | _ -> false

    let (|SanDai1|_|) (cards: PlayingCard list)=
        match cards with
        | [NormalCard(a ,_) ; NormalCard(b , _); NormalCard(c, _); NormalCard(d, _)]
            when ``check3+1`` [a; b; c; d]
                -> Some(cards)
        | _ -> None

