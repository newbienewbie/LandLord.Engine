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

    type JokerType = 
        |Black = 20 
        |Red  =  21

    type Suit = 
        |Spade   = 1
        |Club    = 2
        |Diamond = 3
        |Heart   = 4

    type PlayingCard =  
        |Joker of JokerType
        |NormalCard of CardValue * Suit


    let getWeight (considerSuit:bool) (card: PlayingCard) = 
        let suitValue suit = 
            if considerSuit then int suit else 0
        match card with
        |NormalCard(v, suit) -> int v <<< 2 + suitValue suit
        |Joker(j) -> int j <<< 2 

    let compare (considerSuit:bool) (card1: PlayingCard) (card2: PlayingCard) =
        let c1 = getWeight considerSuit card1
        let c2 = getWeight considerSuit card2
        c1 - c2

    let sort (cards: PlayingCard list) = 
        cards 
        |> List.sortBy (getWeight true)


    let (|DanZhang| _ |) (cards: PlayingCard list) =
        match cards with
        | [NormalCard(card, suit)] -> Some(NormalCard(card,suit))
        | [Joker(j)] -> Some(Joker(j))
        | _ -> None


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

       

    let internal checkDanLianShun (values: CardValue list)= 
        let x = values |> List.map int |> List.sort 

        let rec test (values: int list, prev : int) = 
            match values with
            | [a] -> true
            | head :: tail when head = prev + 1 -> test(tail, head)
            | _ -> false

        match x with 
        | head :: tail -> test(tail,head)
        | _ -> false

    let private cardsContinuous (cards: PlayingCard list) : bool= 

        let rec _cardsContinuous (cards: PlayingCard list) (prev: int) = 
            match cards with
            | [ NormalCard(a, _);] when prev + 1 = int a 
                -> true
            | NormalCard(v, _)::tail when prev + 1 = int v 
                -> _cardsContinuous tail (int v)
            | _ -> false

        let sortedCards = 
            cards 
            |> List.sortBy (fun c -> match c with 
               |NormalCard(v, suit) -> int v
               |Joker(j) -> int j )

        match sortedCards with
        | NormalCard(v, _) :: tail -> _cardsContinuous tail (int v)
        | _ -> false


    /// 注意单张不是顺子， num > 1
    let (|LianShun|_|) (num: int) (cards: PlayingCard list) = 
        match cards with
        | list when cardsContinuous list  && List.length cards = num 
            -> Some(list) 
        | _ -> None


    let (|LianShun5|_|) (cards: PlayingCard list) = 
        match cards with
        | LianShun 5 cards -> Some(cards)
        | _ -> None

    let (|LianShun6|_|) (cards: PlayingCard list) = 
        match cards with
        | LianShun 6 cards -> Some(cards)
        | _ -> None

    let (|LianShun7|_|) (cards: PlayingCard list) = 
        match cards with
        | LianShun 7 cards -> Some(cards)
        | _ -> None

    let (|LianShun8|_|) (cards: PlayingCard list) = 
        match cards with
        | LianShun 8 cards -> Some(cards)
        | _ -> None

    let (|LianShun9|_|) (cards: PlayingCard list) = 
        match cards with
        | LianShun 9 cards -> Some(cards)
        | _ -> None

    let (|LianShun10|_|) (cards: PlayingCard list) = 
        match cards with
        | LianShun 10 cards -> Some(cards)
        | _ -> None

    let (|LianShun11|_|) (cards: PlayingCard list) = 
        match cards with
        | LianShun 11 cards -> Some(cards)
        | _ -> None

    let (|LianShun12|_|) (cards: PlayingCard list) = 
        match cards with
        | LianShun 12 cards -> Some(cards)
        | _ -> None


    let internal checkDuoLianShun (dup: int) (len: int) (cards: CardValue list) = 
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
                checkDanLianShun noneDupcardValues
        | _ -> false 




    let (|ShunZi|_|) (dup: int) (len: int) (cards: PlayingCard list) =

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
            if checkDuoLianShun dup len values then
                Some(cards)
            else 
                None
        else 
            None
