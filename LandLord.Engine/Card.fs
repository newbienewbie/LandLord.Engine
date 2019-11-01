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

    let ConvertCardValueString( cv : CardValue ) =
        match cv with
        | CardValue.Ace -> "A"
        | CardValue.Two -> "2"
        | CardValue.Three -> "3"
        | CardValue.Four -> "4"
        | CardValue.Five -> "5"
        | CardValue.Six -> "6"
        | CardValue.Seven -> "7"
        | CardValue.Eight -> "8"
        | CardValue.Nine -> "9"
        | CardValue.Ten -> "10"
        | CardValue.Jack -> "J"
        | CardValue.Queen -> "Q"
        | CardValue.King -> "K"

    type JokerType = 
        | Black = 20 
        | Red  =  21

    type Suit = 
        | Spade   = 0 
        | Club    = 1 
        | Diamond = 2 
        | Heart   = 3

    let ConverSuitToString( suit: Suit) =
        match suit with
        | Suit.Spade ->  "♠️"
        | Suit.Club -> "♣️"
        | Suit.Diamond -> "♦️"
        | Suit.Heart -> "️♥️"

    type PlayingCard =  
        | Joker of JokerType
        | NormalCard of CardValue * Suit

    type PlayingCard with 
        member this.ConvertToString() = 
            match this with
            | Joker(j) -> 
                match j with | JokerType.Black -> "🐼" | JokerType.Red -> "🃏"
            | NormalCard(c,s) ->
                let s = ConverSuitToString(s)
                let v = ConvertCardValueString(c)
                s + v

    type PlayerCard = 
        | Shadowed
        | PlayingCard of PlayingCard

    let getWeight (considerSuit:bool) (card: PlayingCard) = 
        let suitValue suit = 
            if considerSuit then int suit else 0
        match card with
        | NormalCard(v, suit) -> 
            let left = (int v) <<< 2
            left + suitValue suit
        | Joker(j) -> (int j) <<< 2 

    let compare (considerSuit:bool) (card1: PlayingCard) (card2: PlayingCard) =
        let c1 = getWeight considerSuit card1
        let c2 = getWeight considerSuit card2
        c1 - c2

    let sort (cards: PlayingCard list) = 
        cards 
        |> List.sortBy (getWeight true)

    let rec getCardsValues (cards: PlayingCard list) = 

        let mutable values : CardValue list = []

        let rec getValues cards = 
            match cards with
            | [NormalCard(v, suit)] -> 
                values <- values @ [v]
                true
            | NormalCard(v, suit) :: tail -> 
                values <- values @ [v]
                getValues tail
            | _ -> false

        match getValues cards with
        | true -> Some values
        | false -> None

    let (|Single| _ |) (cards: PlayingCard list) =
        match cards with
        | [NormalCard(card, suit)] -> Some(NormalCard(card,suit))
        | [Joker(j)] -> Some(Joker(j))
        | _ -> None


    let (|Double|_|) (cards: PlayingCard list) =
        match cards with
        | [NormalCard(a, _); NormalCard(b, _)]
            when a = b
                -> Some(cards)
        | _ -> None


    let (|Trible|_|) (cards: PlayingCard list) =
        match cards with
        | [NormalCard(a, _); NormalCard(b, _); NormalCard(c, _)]
            when a = b && b = c
                -> Some(cards)
        | _ -> None


    let (|LianShun|_|) (num: int) (cards: PlayingCard list) = 

        let cardsContinuous (cards: PlayingCard list) : bool= 

            let rec _cardsContinuous (cards: PlayingCard list) (prev: int) = 
                match cards with
                | [ NormalCard(a, _);] when prev + 1 = int a 
                    -> true
                | NormalCard(v, _)::tail when prev + 1 = int v 
                    -> _cardsContinuous tail (int v)
                | _ -> false

            let sortedCards = 
                cards 
                |> List.sortBy (fun c -> 
                    match c with 
                    | NormalCard(v, suit) -> int v
                    | Joker(j) -> int j 
                )

            match sortedCards with
            | NormalCard(v, _) :: tail -> _cardsContinuous tail (int v)
            | _ -> false

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

    // not enabled
    let (|LianShun13|_|) (cards: PlayingCard list) = 
        match cards with
        | LianShun 13 cards -> Some(cards)
        | _ -> None


    let (|ShunZi|_|) (dup: int) (len: int) (cards: PlayingCard list) =

        let checkDanLianShun (values: CardValue list)= 
            let x = values |> List.map int |> List.sort 

            let rec test (values: int list, prev : int) = 
                match values with
                | [a] -> true
                | head :: tail when head = prev + 1 -> test(tail, head)
                | _ -> false

            match x with 
            | head :: tail -> test(tail,head)
            | _ -> false

        let checkDuoLianShun (dup: int) (len: int) (cards: CardValue list) = 
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

        match cards |> getCardsValues with
        | Some(values) when checkDuoLianShun dup len values 
            -> Some(cards)
        | _ -> None
