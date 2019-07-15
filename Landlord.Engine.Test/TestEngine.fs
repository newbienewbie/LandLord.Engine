module TestFacade

open System
open Xunit
open System.Collections.Generic
open Itminus.LandLord.Engine.Card
open Itminus.LandLord.Engine.Facade

[<Fact>]
let ``测试fullCards() 唯一性`` () =
    let cards = createFullCards()

    // no duplicate
    let cards'=
        cards
        |> List.distinct
    Assert.Equal(54, List.length cards')

    let set = Set.ofList cards
    let set' = Set.ofList cards'
    Set.isSubset set set' |> Assert.True
    Set.isSubset set' set |> Assert.True

[<Fact>]
let ``测试fullCards() 完备性`` () =
    let cards = createFullCards()

    let length = List.length cards
    // 52 + 2
    Assert.Equal(54,length)

    List.contains (Joker(JokerType.Black)) cards
    |> Assert.True

    List.contains (Joker(JokerType.Red)) cards
    |> Assert.True

    let suits: Suit seq  = unbox( Enum.GetValues(typeof<Suit>) )
    let values: CardValue seq = unbox(Enum.GetValues(typeof<CardValue>))
    for suit in suits do
        for value in values do
            let card = NormalCard(value, suit)
            List.contains (NormalCard(CardValue.Ace, Suit.Club)) cards
            |> Assert.True

[<Fact>]
let ``测试shuffle()`` () =
    let cards = createFullCards() 
    let shuffled = cards |> shuffle

    shuffled |> List.length |> (=) 54 |> Assert.True

    let s1 = cards |> Set.ofList
    let s2 = shuffled |> Set.ofList 
    Set.isSubset s1 s2 |> Assert.True
    Set.isSubset s2 s1 |> Assert.True


[<Fact>]
let ``测试deal() 唯一性`` () =
    let cards = createFullCards() |> shuffle
    let (reserved, (cards1, cards2, cards3)) = deal cards
    Assert.Equal(3, reserved |> List.length)
    Assert.Equal(17, cards1 |> List.length)
    Assert.Equal(17, cards2 |> List.length)
    Assert.Equal(17, cards3 |> List.length)
    
    let reservedSet = Set.ofList reserved
    let cards1Set = Set.ofList cards1
    let cards2Set = Set.ofList cards2
    let cards3Set = Set.ofList cards3

    Set.intersect reservedSet cards1Set 
    |> Set.count 
    |> (=) 0 
    |> Assert.True

    Set.intersect reservedSet cards2Set 
    |> Set.count 
    |> (=) 0 
    |> Assert.True

    Set.intersect reservedSet cards3Set 
    |> Set.count 
    |> (=) 0 
    |> Assert.True

    Set.intersect cards1Set cards2Set
    |> Set.count 
    |> (=) 0 
    |> Assert.True

    Set.intersect cards1Set cards3Set
    |> Set.count 
    |> (=) 0 
    |> Assert.True

    Set.intersect cards2Set cards3Set
    |> Set.count 
    |> (=) 0 
    |> Assert.True


[<Fact>]
let ``测试deal() 完备性`` () =
    let cards = createFullCards() |> shuffle
    let (reserved, (cards1, cards2, cards3)) = deal cards
    let cardsSet = Set.ofList cards
    let sets = seq {
        yield Set.ofList reserved
        yield Set.ofList cards1
        yield Set.ofList cards2
        yield Set.ofList cards3
    }
    let fullcardsSet = Set.unionMany sets
    fullcardsSet |> Set.isSubset cardsSet |> Assert.True
    cardsSet|> Set.isSubset fullcardsSet|> Assert.True

[<Fact>]
let ``测试CanStartPlaying() 3`` () =
    let cards= [NormalCard(CardValue.Three,Suit.Spade); ]
    canStartPlaying cards |> Assert.True

[<Fact>]
let ``测试CanStartPlaying() 33`` () =
    let cards= [
        NormalCard(CardValue.Three,Suit.Spade); 
        NormalCard(CardValue.Three,Suit.Diamond); 
    ]
    canStartPlaying cards |> Assert.True

[<Fact>]
let ``测试CanStartPlaying() 333`` () =
    let cards= 
        [ NormalCard(CardValue.Three,Suit.Club); 
          NormalCard(CardValue.Three,Suit.Spade); 
          NormalCard(CardValue.Three,Suit.Diamond)] 
    canStartPlaying cards |> Assert.True

[<Fact>]
let ``测试CanStartPlaying() 333+1`` () =
    let cards= [ 
        NormalCard(CardValue.Three,Suit.Club); 
        NormalCard(CardValue.Three,Suit.Spade); 
        NormalCard(CardValue.Three,Suit.Diamond);
        NormalCard(CardValue.Ace,Suit.Diamond);
    ] 
    canStartPlaying cards |> Assert.True

[<Fact>]
let ``测试CanStartPlaying() 3333`` () =
    let cards= [ 
        NormalCard(CardValue.Three,Suit.Club); 
        NormalCard(CardValue.Three,Suit.Spade); 
        NormalCard(CardValue.Three,Suit.Diamond);
        NormalCard(CardValue.Three,Suit.Heart);
    ] 
    canStartPlaying cards |> Assert.True

[<Fact>]
let ``测试CanStartPlaying() 345`` () =
    let cards= 
        [ NormalCard(CardValue.Five,Suit.Club); 
          NormalCard(CardValue.Three,Suit.Spade); 
          NormalCard(CardValue.Four,Suit.Diamond)] 
    canStartPlaying cards |> Assert.False


[<Fact>]
let ``测试CanPlay() 333 _ 543`` () =

    let prevCards = 
        [ NormalCard(CardValue.Three,Suit.Club); 
          NormalCard(CardValue.Three,Suit.Spade); 
          NormalCard(CardValue.Three,Suit.Diamond)] 

    let currentCards = 
        [ NormalCard(CardValue.Five,Suit.Club); 
          NormalCard(CardValue.Three,Suit.Spade); 
          NormalCard(CardValue.Four,Suit.Diamond)] 

    canPlay prevCards currentCards |> Assert.False


[<Fact>]
let ``测试CanPlay() 333 _ 555`` () =

    let prevCards = 
        [ NormalCard(CardValue.Three,Suit.Club); 
          NormalCard(CardValue.Three,Suit.Spade); 
          NormalCard(CardValue.Three,Suit.Diamond)] 

    let currentCards = 
        [ NormalCard(CardValue.Five,Suit.Club); 
          NormalCard(CardValue.Five,Suit.Spade); 
          NormalCard(CardValue.Five,Suit.Diamond)] 

    canPlay prevCards currentCards |> Assert.True


[<Fact>]
let ``测试CanPlay() 34567 _ 78910J`` () =

    let prevCards = 
        [ NormalCard(CardValue.Three,Suit.Club); 
          NormalCard(CardValue.Four,Suit.Spade); 
          NormalCard(CardValue.Five,Suit.Spade); 
          NormalCard(CardValue.Six,Suit.Spade); 
          NormalCard(CardValue.Seven,Suit.Spade)]

    let currentCards = 
        [ NormalCard(CardValue.Seven,Suit.Club); 
          NormalCard(CardValue.Eight,Suit.Club); 
          NormalCard(CardValue.Nine,Suit.Club); 
          NormalCard(CardValue.Ten,Suit.Club); 
          NormalCard(CardValue.Jack,Suit.Club); ]

    canPlay prevCards currentCards |> Assert.True


[<Fact>]
let ``测试CanPlay() 34567 _ JQKA2`` () =

    let prevCards = 
        [ NormalCard(CardValue.Three,Suit.Club); 
          NormalCard(CardValue.Four,Suit.Spade); 
          NormalCard(CardValue.Five,Suit.Spade); 
          NormalCard(CardValue.Six,Suit.Spade); 
          NormalCard(CardValue.Seven,Suit.Spade)]

    let currentCards = 
         [ NormalCard(CardValue.Jack,Suit.Club); 
          NormalCard(CardValue.Queen,Suit.Club); 
          NormalCard(CardValue.King,Suit.Club); 
          NormalCard(CardValue.Ace,Suit.Club); 
          NormalCard(CardValue.Two,Suit.Club); ]

    canPlay prevCards currentCards |> Assert.True

[<Fact>]
let ``测试CanPlay() 3456789 _ 910JQKA2`` () =

    let prevCards = 
        [ NormalCard(CardValue.Three,Suit.Club); 
          NormalCard(CardValue.Four,Suit.Spade); 
          NormalCard(CardValue.Five,Suit.Spade); 
          NormalCard(CardValue.Six,Suit.Spade); 
          NormalCard(CardValue.Seven,Suit.Spade);
          NormalCard(CardValue.Eight,Suit.Spade);
          NormalCard(CardValue.Nine,Suit.Spade)]

    let currentCards = 
        [ NormalCard(CardValue.Nine,Suit.Club); 
          NormalCard(CardValue.Ten,Suit.Club); 
          NormalCard(CardValue.Jack,Suit.Club); 
          NormalCard(CardValue.Queen,Suit.Club); 
          NormalCard(CardValue.King,Suit.Club); 
          NormalCard(CardValue.Ace,Suit.Club); 
          NormalCard(CardValue.Two,Suit.Club); ]

    canPlay prevCards currentCards |> Assert.True

[<Fact>]
let ``测试CanPlay() 3456789 _ 910JQKA`` () =

    let prevCards = 
        [ NormalCard(CardValue.Three,Suit.Club); 
          NormalCard(CardValue.Four,Suit.Spade); 
          NormalCard(CardValue.Five,Suit.Spade); 
          NormalCard(CardValue.Six,Suit.Spade); 
          NormalCard(CardValue.Seven,Suit.Spade);
          NormalCard(CardValue.Eight,Suit.Spade);
          NormalCard(CardValue.Nine,Suit.Spade)]

    let currentCards = 
        [ NormalCard(CardValue.Nine,Suit.Club); 
          NormalCard(CardValue.Ten,Suit.Club); 
          NormalCard(CardValue.Jack,Suit.Club); 
          NormalCard(CardValue.Queen,Suit.Club); 
          NormalCard(CardValue.King,Suit.Club); 
          NormalCard(CardValue.Ace,Suit.Club); ]

    canPlay prevCards currentCards |> Assert.False


[<Fact>]
let ``测试CanPlay() 3456789 _ 99JQKA2`` () =

    let prevCards = 
        [ NormalCard(CardValue.Three,Suit.Club); 
          NormalCard(CardValue.Four,Suit.Spade); 
          NormalCard(CardValue.Five,Suit.Spade); 
          NormalCard(CardValue.Six,Suit.Spade); 
          NormalCard(CardValue.Seven,Suit.Spade);
          NormalCard(CardValue.Eight,Suit.Spade);
          NormalCard(CardValue.Nine,Suit.Spade)]

    let currentCards = 
        [ NormalCard(CardValue.Nine,Suit.Club); 
          NormalCard(CardValue.Nine,Suit.Diamond); 
          NormalCard(CardValue.Jack,Suit.Club); 
          NormalCard(CardValue.Queen,Suit.Club); 
          NormalCard(CardValue.King,Suit.Club); 
          NormalCard(CardValue.Ace,Suit.Club); ]

    canPlay prevCards currentCards |> Assert.False

[<Fact>]
let ``测试CanPlay() 333444555666 _ 777888999101010`` () =

    let prevCards = 
        [ NormalCard(CardValue.Three,Suit.Club); 
          NormalCard(CardValue.Three,Suit.Diamond); 
          NormalCard(CardValue.Three,Suit.Heart); 
          NormalCard(CardValue.Four,Suit.Club); 
          NormalCard(CardValue.Four,Suit.Spade); 
          NormalCard(CardValue.Four,Suit.Heart); 
          NormalCard(CardValue.Five,Suit.Spade); 
          NormalCard(CardValue.Five,Suit.Club); 
          NormalCard(CardValue.Five,Suit.Diamond); 
          NormalCard(CardValue.Six,Suit.Spade); 
          NormalCard(CardValue.Six,Suit.Diamond); 
          NormalCard(CardValue.Six,Suit.Heart); ]

    let currentCards = 
        [ NormalCard(CardValue.Seven,Suit.Club); 
          NormalCard(CardValue.Seven,Suit.Diamond); 
          NormalCard(CardValue.Seven,Suit.Heart); 
          NormalCard(CardValue.Eight,Suit.Heart); 
          NormalCard(CardValue.Eight,Suit.Club); 
          NormalCard(CardValue.Eight,Suit.Spade); 
          NormalCard(CardValue.Nine,Suit.Spade); 
          NormalCard(CardValue.Nine,Suit.Club); 
          NormalCard(CardValue.Nine,Suit.Diamond); 
          NormalCard(CardValue.Ten,Suit.Spade); 
          NormalCard(CardValue.Ten,Suit.Diamond); 
          NormalCard(CardValue.Ten,Suit.Heart); ]

    canPlay prevCards currentCards |> Assert.True

[<Fact>]
let ``测试CanPlay() 3344455 _ 778899`` () =

    let prevCards = 
        [ NormalCard(CardValue.Three,Suit.Club); 
          NormalCard(CardValue.Three,Suit.Diamond); 
          NormalCard(CardValue.Four,Suit.Club); 
          NormalCard(CardValue.Four,Suit.Spade); 
          NormalCard(CardValue.Five,Suit.Spade); 
          NormalCard(CardValue.Five,Suit.Diamond);]

    let currentCards = 
        [ NormalCard(CardValue.Seven,Suit.Club); 
          NormalCard(CardValue.Seven,Suit.Diamond); 
          NormalCard(CardValue.Eight,Suit.Heart); 
          NormalCard(CardValue.Eight,Suit.Spade); 
          NormalCard(CardValue.Nine,Suit.Spade); 
          NormalCard(CardValue.Nine,Suit.Club); ]

    canPlay prevCards currentCards |> Assert.True

[<Fact>]
let ``测试CanPlay() 778899 _ 334455`` () =

    let prevCards = 
        [ NormalCard(CardValue.Seven,Suit.Club); 
          NormalCard(CardValue.Seven,Suit.Diamond); 
          NormalCard(CardValue.Eight,Suit.Heart); 
          NormalCard(CardValue.Eight,Suit.Spade); 
          NormalCard(CardValue.Nine,Suit.Spade); 
          NormalCard(CardValue.Nine,Suit.Club); ]
    let currentCards = 
        [ NormalCard(CardValue.Three,Suit.Club); 
          NormalCard(CardValue.Three,Suit.Diamond); 
          NormalCard(CardValue.Four,Suit.Club); 
          NormalCard(CardValue.Four,Suit.Spade); 
          NormalCard(CardValue.Five,Suit.Spade); 
          NormalCard(CardValue.Five,Suit.Diamond);]

    canPlay prevCards currentCards |> Assert.False

[<Fact>]
let ``测试CanPlay 3333 _ JJJJ`` () =

    let prevCards = 
        [ NormalCard(CardValue.Three,Suit.Club); 
          NormalCard(CardValue.Three,Suit.Spade); 
          NormalCard(CardValue.Three,Suit.Diamond); 
          NormalCard(CardValue.Three,Suit.Heart); ]

    let currentCards = 
         [ NormalCard(CardValue.Jack,Suit.Spade);
          NormalCard(CardValue.Jack,Suit.Diamond); 
          NormalCard(CardValue.Jack,Suit.Club); 
          NormalCard(CardValue.Jack,Suit.Heart); ]

    canPlay prevCards currentCards |> Assert.True

[<Fact>]
let ``测试CanPlay JJJJ _ 3333`` () =

    let prevCards = 
         [ NormalCard(CardValue.Jack,Suit.Spade);
          NormalCard(CardValue.Jack,Suit.Diamond); 
          NormalCard(CardValue.Jack,Suit.Club); 
          NormalCard(CardValue.Jack,Suit.Heart); ]

    let currentCards = 
        [ NormalCard(CardValue.Three,Suit.Club); 
          NormalCard(CardValue.Three,Suit.Spade); 
          NormalCard(CardValue.Three,Suit.Diamond); 
          NormalCard(CardValue.Three,Suit.Heart); ]

    canPlay prevCards currentCards |> Assert.False

[<Fact>]
let ``测试CanPlay JJJJ _ 2222`` () =

    let prevCards = 
         [ NormalCard(CardValue.Jack, Suit.Spade);
          NormalCard(CardValue.Jack, Suit.Diamond); 
          NormalCard(CardValue.Jack, Suit.Club); 
          NormalCard(CardValue.Jack, Suit.Heart); ]

    let currentCards = 
        [ NormalCard(CardValue.Two, Suit.Club); 
          NormalCard(CardValue.Two, Suit.Spade); 
          NormalCard(CardValue.Two, Suit.Diamond); 
          NormalCard(CardValue.Two,Suit.Heart); ]

    canPlay prevCards currentCards |> Assert.True

[<Fact>]
let ``测试CanPlay() 222 _ 3333`` () =

    let prevCards= 
        [ NormalCard(CardValue.Two, Suit.Club); 
          NormalCard(CardValue.Two, Suit.Diamond); 
          NormalCard(CardValue.Two,Suit.Heart); ]

    let currentCards= 
         [ NormalCard(CardValue.Three, Suit.Spade);
          NormalCard(CardValue.Three, Suit.Diamond); 
          NormalCard(CardValue.Three, Suit.Club); 
          NormalCard(CardValue.Three, Suit.Heart); ]

    canPlay prevCards currentCards |> Assert.True

[<Fact>]
let ``测试CanPlay() 222 _ AAA`` () =

    let prevCards= 
        [ NormalCard(CardValue.Two, Suit.Club); 
          NormalCard(CardValue.Two, Suit.Diamond); 
          NormalCard(CardValue.Two,Suit.Heart); ]

    let currentCards= 
         [ NormalCard(CardValue.Ace, Suit.Spade);
          NormalCard(CardValue.Ace, Suit.Diamond); 
          NormalCard(CardValue.Ace, Suit.Heart); ]

    canPlay prevCards currentCards |> Assert.False

[<Fact>]
let ``测试CanPlay() KKK _ 222`` () =

    let prevCards= 
         [ NormalCard(CardValue.King, Suit.Spade);
          NormalCard(CardValue.King, Suit.Diamond); 
          NormalCard(CardValue.King, Suit.Club); ]
    let currentCards= 
        [ NormalCard(CardValue.Two, Suit.Club); 
          NormalCard(CardValue.Two, Suit.Diamond); 
          NormalCard(CardValue.Two,Suit.Heart); ]

    canPlay prevCards currentCards |> Assert.True
