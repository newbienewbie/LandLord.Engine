namespace Itminus.LandLord.Engine

module Card =

    type CardValue = 
        | Three  = 3
        | Four  = 4
        | Five  = 5
        | Six  = 6
        | Seven  = 7
        | Eight  = 8
        | Nine = 9
        | Ten = 10
        | Jack = 11
        | Queen = 12
        | King = 13
        | Ace = 14
        | Two  = 15

    type JokerType = | Red | Black

    type Suit = | Spade | Club | Diamond | Heart

    type PlayingCard =  
        | Joker of JokerType
        | NormalCard of CardValue * Suit
        

    let internal checkLianShun (values: CardValue list)= 
        let x = values |> List.map int |> List.sort 

        let rec test (values: int list, prev : int) = 
            match values with
            | [a] -> true
            | head :: tail when head = prev + 1 -> test(tail, head)
            | _ -> false

        match x with 
        | head :: tail -> test(tail,head)
        | _ -> false

    let (|DanZhang| _ |) (cards: PlayingCard list) =
        match cards with
        | [danzhang] -> Some(cards)
        | _ -> None

    let (|LianShun5|_|) (cards: PlayingCard list) = 
        match cards with
        | [NormalCard(a,_); NormalCard(b,_); NormalCard(c,_); NormalCard(d,_); NormalCard(e,_)]  
            when checkLianShun [ a; b; c; d; e ]
                -> Some(cards)
        | _ -> None

    let (|LianShun6|_|) (cards: PlayingCard list) = 
        match cards with
        | [NormalCard(a,_); NormalCard(b,_); NormalCard(c,_); NormalCard(d,_); NormalCard(e,_); NormalCard(f,_)]  
            when checkLianShun [a; b; c; d; e; f; ] 
                -> Some(cards)
        | _ -> None

    let (|LianShun7|_|) (cards: PlayingCard list) = 
        match cards with
        | [NormalCard(a,_); NormalCard(b,_); NormalCard(c,_); NormalCard(d,_); NormalCard(e,_); NormalCard(f,_); NormalCard(g,_);]  
            when checkLianShun [a; b; c; d; e; f; g]
                -> Some(cards)
        | _ -> None

    let (|LianShun8|_|) (cards: PlayingCard list) = 
        match cards with
        | [NormalCard(a,_); NormalCard(b,_); NormalCard(c,_); NormalCard(d,_); NormalCard(e,_); NormalCard(f,_); NormalCard(g,_); NormalCard(h ,_)]  
            when checkLianShun [a; b; c; d; e; f; g; h]
                -> Some(cards)
        | _ -> None

    let (|LianShun9|_|) (cards: PlayingCard list) = 
        match cards with
        | [NormalCard(a,_); NormalCard(b,_); NormalCard(c,_); NormalCard(d,_); NormalCard(e,_); NormalCard(f,_); NormalCard(g,_); NormalCard(h ,_) ; NormalCard(i , _)]  
            when checkLianShun [a; b; c; d; e; f; g; h; i]
                -> Some(cards)
        | _ -> None

    let (|LianShun10|_|) (cards: PlayingCard list) = 
        match cards with
        | [NormalCard(a,_); NormalCard(b,_); NormalCard(c,_); NormalCard(d,_); NormalCard(e,_); NormalCard(f,_); NormalCard(g,_); NormalCard(h ,_) ; NormalCard(i , _); NormalCard(j, _)]  
            when checkLianShun [a; b; c; d; e; f; g; h; i; j]
                -> Some(cards)
        | _ -> None

    let (|LianShun11|_|) (cards: PlayingCard list) = 
        match cards with
        | [NormalCard(a,_); NormalCard(b,_); NormalCard(c,_); NormalCard(d,_); NormalCard(e,_); NormalCard(f,_); NormalCard(g,_); NormalCard(h ,_) ; NormalCard(i , _); NormalCard(j, _); NormalCard(k, _)]  
            when checkLianShun [a; b; c; d; e; f; g; h; i; j; k]
                -> Some(cards)
        | _ -> None

    let (|LianShun12|_|) (cards: PlayingCard list) = 
        match cards with
        | [NormalCard(a,_); NormalCard(b,_); NormalCard(c,_); NormalCard(d,_); NormalCard(e,_); NormalCard(f,_); NormalCard(g,_); NormalCard(h ,_) ; NormalCard(i , _); NormalCard(j, _); NormalCard(k, _); NormalCard(l, _)]  
            when checkLianShun [a; b; c; d; e; f; g; h; i; j; k; l]
                -> Some(cards)
        | _ -> None



    let internal checkLianShunForDup (dup: int) (len: int) (cards: CardValue list) = 
        let x = cards |> List.map int |> List.sort

        // group
        let groups = x |> List.groupBy(fun i -> i) |> List.map(fun t -> snd t)

        // check group length
        match groups |> List.length with
        | l when l = len ->
            let notDuiZi =
                groups
                |> List.map(fun g->List.length g)
                |> List.exists(fun count->count <> dup)
            if notDuiZi then
                false
            else 
                let noneDupcardValues =
                    groups
                    |> List.map(fun i->i |> List.head |> enum )
                checkLianShun noneDupcardValues
        | _ -> false 



    let (|DuiZi|_|) (cards: PlayingCard list) =
        match cards with
        | [NormalCard(a, _); NormalCard(b, _)]
            when a = b
                -> Some(cards)
        | _ -> None


    let (|SanZhang|_|) (cards: PlayingCard list) =
        match cards with
        | [NormalCard(a, _); NormalCard(b, _); NormalCard(c, _)]
            when a = b && b = c
                -> Some(cards)
        | _ -> None


    let(|ShunZi|_|) (dup: int) (len: int) (cards: PlayingCard list) =

        let mutable values: CardValue list = []
        let rec fillValues(cards: PlayingCard list) = 
            match cards with
            | [ (NormalCard(c, suit))]  ->
                values<- values @ [c]
                true
            | NormalCard(c, suit)::tail->
                values<- values @ [c]
                fillValues tail
            | _ -> false

        if fillValues cards then 
            if checkLianShunForDup dup len values then
                Some(cards)
            else 
                None
        else 
            None
