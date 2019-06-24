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

    let (|FeiJiDai2|_|) (cards: PlayingCard list) = 

        let ``check3+3+1+1`` (cards: PlayingCard list) : bool = 
            match getCardsValues cards with
            | Some values ->
                let groups = 
                    values
                    |> List.groupBy (fun c -> c) 
                    |> List.map (fun g -> snd g)
                    |> List.sortBy (fun i -> List.length i )
                match groups with 
                | [[v1]; [v2]; [f1;f2;f3]; [s1; s2; s3] ] 
                    when f1 = f2 && f2 = f3 && v1 <> f1 && v2 <> f1
                         && s1 = s2 && s2 = s3 && v1 <> s1 && v2 <> s1
                            -> true
                | _ -> false
            | None -> false

        match cards with
        | list  when ``check3+3+1+1`` list -> Some(cards)
        | _ -> None


    let (|FeiJiDai3|_|) (cards: PlayingCard list) = 
        let ``check3+3+3+1+1+1`` (cards: PlayingCard list) : bool = 
            match getCardsValues cards with
            | Some values ->
                let groups = 
                    values 
                    |> List.groupBy (fun c -> c)
                    |> List.map snd 
                    |> List.sortBy List.length
                match groups with
                | [[v1]; [v2]; [v3]; [f1; f2; f3]; [s1; s2; s3]; [t1; t2; t3]]
                    when f1 = f2 && f2 = f3 && s1 = s2 && s2 = s3 && t1 = t2 && t2 = t3
                        && v1 <> f1 && v1 <> s1 && v1 <> t1
                        && v2 <> f1 && v2 <> s1 && v3 <> t1
                        && v3 <> f1 && v3 <> s1 && v3 <> t3
                        && f1 <> s1 && f1 <> t1 && s1 <> t1
                           -> true
                | _ -> false
            | None -> false

        match cards with
        | list when ``check3+3+3+1+1+1`` list -> Some cards 
        | _ -> None

