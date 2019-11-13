namespace LandLord.Core.Tests


module TestFacade =

    open System
    open Xunit
    open System.Collections.Generic
    open LandLord.Core
    open System.Linq


    [<Fact>]
    let ``测试GetWeight()`` () =
        let aceClub = 
            (NormalCard(CardValue=CardValue.Ace, CardSuit=CardSuit.Club)).GetWeight(true)
        let aceClub' =
            (NormalCard(CardValue=CardValue.Ace, CardSuit=CardSuit.Club)).GetWeight(true)
        let twoDiamond = (NormalCard( CardValue=CardValue.Two, CardSuit=CardSuit.Diamond)).GetWeight(true)
        let jack = (NormalCard( CardValue=CardValue.Jack, CardSuit=CardSuit.Diamond)).GetWeight(true)
        let three = (NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Diamond)).GetWeight(true)

        ((<) aceClub twoDiamond) |> Assert.True
        ((=) aceClub aceClub') |> Assert.True
        ((>) aceClub three) |> Assert.True
        ((>) aceClub jack) |> Assert.True
        ((>) twoDiamond three) |> Assert.True
        ((<) three jack) |> Assert.True

    [<Fact>]
    let ``测试FullCards() 唯一性`` () =
        let cards = Facade.CreateFullCards() |> Seq.toList

        // no duplicate
        let cards'= cards.Distinct() |> Seq.toList
        Assert.Equal(54, List.length cards')

        let set = Set.ofList cards
        let set' = Set.ofList cards'
        Set.isSubset set set' |> Assert.True
        Set.isSubset set' set |> Assert.True

    [<Fact>]
    let ``测试fullCards() 完备性`` () =
        let cards = Facade.CreateFullCards() |> Seq.toList

        let length = List.length cards
        // 52 + 2
        Assert.Equal(54,length)

        let bj : PlayingCard =  BlackJokerCard() :> PlayingCard
        let rj : PlayingCard =  RedJokerCard() :> PlayingCard

        List.contains bj cards |> Assert.True
        List.contains rj cards |> Assert.True

        let suits: CardSuit seq  = unbox( Enum.GetValues(typeof<CardSuit>) )
        let values: CardValue seq = unbox(Enum.GetValues(typeof<CardValue>))
        for suit in suits do
            for value in values do
                let card:PlayingCard = NormalCard(CardValue=value, CardSuit=suit) :> PlayingCard
                List.contains (card) cards |> Assert.True

    [<Fact>]
    let ``测试Shuffle()`` () =
        let cardSeq = Facade.CreateFullCards() 
        let shuffled = cardSeq |> Facade.Shuffle |> Seq.toList

        shuffled |> List.length |> (=) 54 |> Assert.True

        let s1 = cardSeq |> Seq.toList|> Set.ofList
        let s2 = shuffled |> Set.ofList 
        Set.isSubset s1 s2 |> Assert.True
        Set.isSubset s2 s1 |> Assert.True


    [<Fact>]
    let ``测试deal() 唯一性`` () =
        let cards = Facade.CreateFullCards() |> Facade.Shuffle
        let (reserved, cardsAlloc) = Facade.Deal(cards).ToTuple()
        let (cards1, cards2, cards3) = cardsAlloc.ToTuple()
        let reserved = reserved |> Seq.toList
        let cards1 = cards1 |> Seq.toList
        let cards2 = cards2 |> Seq.toList
        let cards3 = cards3 |> Seq.toList
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
        let cards = Facade.CreateFullCards() |> Facade.Shuffle
        let (reserved, cardsAlloc) = Facade.Deal(cards).ToTuple()
        let (cards1, cards2, cards3) = cardsAlloc.ToTuple()
        let reserved = reserved |> Seq.toList
        let cards1 = cards1 |> Seq.toList
        let cards2 = cards2 |> Seq.toList
        let cards3 = cards3 |> Seq.toList

        let cardsSet =  cards |> Seq.toList |> Set.ofList
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
        let cards: PlayingCard list= [NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Spade); ]
        Facade.CanStartPlaying( List.toSeq(cards).ToList() ) |> Assert.True

    [<Fact>]
    let ``测试CanStartPlaying() 33`` () =
        let cards : PlayingCard list= [
            NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Spade); 
            NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Diamond); 
        ]
        Facade.CanStartPlaying( List.toSeq(cards).ToList()) |> Assert.True

    [<Fact>]
    let ``测试CanStartPlaying() 333`` () =
        let cards : PlayingCard list = 
            [ NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Club); 
              NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Spade); 
              NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Diamond)] 
        Facade.CanStartPlaying( List.toSeq(cards).ToList()) |> Assert.True

    [<Fact>]
    let ``测试CanStartPlaying() 333+1`` () =
        let cards :PlayingCard list = [ 
            NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Club); 
            NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Spade); 
            NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Diamond);
            NormalCard(CardValue=CardValue.Ace,CardSuit=CardSuit.Diamond);
        ] 
        Facade.CanStartPlaying( List.toSeq(cards).ToList()) |> Assert.True

    [<Fact>]
    let ``测试CanStartPlaying() 3333`` () =
        let cards :PlayingCard list = [ 
            NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Club); 
            NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Spade); 
            NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Diamond);
            NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Heart);
        ] 
        Facade.CanStartPlaying( List.toSeq(cards).ToList()) |> Assert.True

    [<Fact>]
    let ``测试CanStartPlaying() 345`` () =
        let cards :PlayingCard list = [ 
            NormalCard(CardValue=CardValue.Five,CardSuit=CardSuit.Club); 
            NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Spade); 
            NormalCard(CardValue=CardValue.Four,CardSuit=CardSuit.Diamond)] 
        Facade.CanStartPlaying( List.toSeq(cards).ToList()) |> Assert.False


    //[<Fact>]
    //let ``测试CanPlay() 333 _ 543`` () =

    //    let prevCards = 
    //        [ NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Diamond)] 

    //    let currentCards = 
    //        [ NormalCard(CardValue=CardValue.Five,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Four,CardSuit=CardSuit.Diamond)] 

    //    canPlay prevCards currentCards |> Assert.False


    //[<Fact>]
    //let ``测试CanPlay() 333 _ 555`` () =

    //    let prevCards = 
    //        [ NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Diamond)] 

    //    let currentCards = 
    //        [ NormalCard(CardValue=CardValue.Five,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Five,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Five,CardSuit=CardSuit.Diamond)] 

    //    canPlay prevCards currentCards |> Assert.True


    //[<Fact>]
    //let ``测试CanPlay() 34567 _ 78910J`` () =

    //    let prevCards = 
    //        [ NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Four,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Five,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Six,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Seven,CardSuit=CardSuit.Spade)]

    //    let currentCards = 
    //        [ NormalCard(CardValue=CardValue.Seven,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Eight,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Nine,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Ten,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Jack,CardSuit=CardSuit.Club); ]

    //    canPlay prevCards currentCards |> Assert.True


    //[<Fact>]
    //let ``测试CanPlay() 34567 _ JQKA2`` () =

    //    let prevCards = 
    //        [ NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Four,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Five,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Six,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Seven,CardSuit=CardSuit.Spade)]

    //    let currentCards = 
    //         [ NormalCard(CardValue=CardValue.Jack,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Queen,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.King,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Ace,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Two,CardSuit=CardSuit.Club); ]

    //    canPlay prevCards currentCards |> Assert.True

    //[<Fact>]
    //let ``测试CanPlay() 3456789 _ 910JQKA2`` () =

    //    let prevCards = 
    //        [ NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Four,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Five,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Six,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Seven,CardSuit=CardSuit.Spade);
    //          NormalCard(CardValue=CardValue.Eight,CardSuit=CardSuit.Spade);
    //          NormalCard(CardValue=CardValue.Nine,CardSuit=CardSuit.Spade)]

    //    let currentCards = 
    //        [ NormalCard(CardValue=CardValue.Nine,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Ten,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Jack,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Queen,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.King,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Ace,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Two,CardSuit=CardSuit.Club); ]

    //    canPlay prevCards currentCards |> Assert.True

    //[<Fact>]
    //let ``测试CanPlay() 3456789 _ 910JQKA`` () =

    //    let prevCards = 
    //        [ NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Four,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Five,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Six,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Seven,CardSuit=CardSuit.Spade);
    //          NormalCard(CardValue=CardValue.Eight,CardSuit=CardSuit.Spade);
    //          NormalCard(CardValue=CardValue.Nine,CardSuit=CardSuit.Spade)]

    //    let currentCards = 
    //        [ NormalCard(CardValue=CardValue.Nine,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Ten,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Jack,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Queen,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.King,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Ace,CardSuit=CardSuit.Club); ]

    //    canPlay prevCards currentCards |> Assert.False


    //[<Fact>]
    //let ``测试CanPlay() 3456789 _ 99JQKA2`` () =

    //    let prevCards = 
    //        [ NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Four,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Five,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Six,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Seven,CardSuit=CardSuit.Spade);
    //          NormalCard(CardValue=CardValue.Eight,CardSuit=CardSuit.Spade);
    //          NormalCard(CardValue=CardValue.Nine,CardSuit=CardSuit.Spade)]

    //    let currentCards = 
    //        [ NormalCard(CardValue=CardValue.Nine,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Nine,CardSuit=CardSuit.Diamond); 
    //          NormalCard(CardValue=CardValue.Jack,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Queen,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.King,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Ace,CardSuit=CardSuit.Club); ]

    //    canPlay prevCards currentCards |> Assert.False

    //[<Fact>]
    //let ``测试CanPlay() 333444555666 _ 777888999101010`` () =

    //    let prevCards = 
    //        [ NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Diamond); 
    //          NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Heart); 
    //          NormalCard(CardValue=CardValue.Four,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Four,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Four,CardSuit=CardSuit.Heart); 
    //          NormalCard(CardValue=CardValue.Five,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Five,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Five,CardSuit=CardSuit.Diamond); 
    //          NormalCard(CardValue=CardValue.Six,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Six,CardSuit=CardSuit.Diamond); 
    //          NormalCard(CardValue=CardValue.Six,CardSuit=CardSuit.Heart); ]

    //    let currentCards = 
    //        [ NormalCard(CardValue=CardValue.Seven,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Seven,CardSuit=CardSuit.Diamond); 
    //          NormalCard(CardValue=CardValue.Seven,CardSuit=CardSuit.Heart); 
    //          NormalCard(CardValue=CardValue.Eight,CardSuit=CardSuit.Heart); 
    //          NormalCard(CardValue=CardValue.Eight,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Eight,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Nine,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Nine,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Nine,CardSuit=CardSuit.Diamond); 
    //          NormalCard(CardValue=CardValue.Ten,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Ten,CardSuit=CardSuit.Diamond); 
    //          NormalCard(CardValue=CardValue.Ten,CardSuit=CardSuit.Heart); ]

    //    canPlay prevCards currentCards |> Assert.True

    //[<Fact>]
    //let ``测试CanPlay() 3344455 _ 778899`` () =

    //    let prevCards = 
    //        [ NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Diamond); 
    //          NormalCard(CardValue=CardValue.Four,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Four,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Five,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Five,CardSuit=CardSuit.Diamond);]

    //    let currentCards = 
    //        [ NormalCard(CardValue=CardValue.Seven,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Seven,CardSuit=CardSuit.Diamond); 
    //          NormalCard(CardValue=CardValue.Eight,CardSuit=CardSuit.Heart); 
    //          NormalCard(CardValue=CardValue.Eight,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Nine,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Nine,CardSuit=CardSuit.Club); ]

    //    canPlay prevCards currentCards |> Assert.True

    //[<Fact>]
    //let ``测试CanPlay() 778899 _ 334455`` () =

    //    let prevCards = 
    //        [ NormalCard(CardValue=CardValue.Seven,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Seven,CardSuit=CardSuit.Diamond); 
    //          NormalCard(CardValue=CardValue.Eight,CardSuit=CardSuit.Heart); 
    //          NormalCard(CardValue=CardValue.Eight,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Nine,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Nine,CardSuit=CardSuit.Club); ]
    //    let currentCards = 
    //        [ NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Diamond); 
    //          NormalCard(CardValue=CardValue.Four,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Four,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Five,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Five,CardSuit=CardSuit.Diamond);]

    //    canPlay prevCards currentCards |> Assert.False

    //[<Fact>]
    //let ``测试CanPlay 3333 _ JJJJ`` () =

    //    let prevCards = 
    //        [ NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Diamond); 
    //          NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Heart); ]

    //    let currentCards = 
    //         [ NormalCard(CardValue=CardValue.Jack,CardSuit=CardSuit.Spade);
    //          NormalCard(CardValue=CardValue.Jack,CardSuit=CardSuit.Diamond); 
    //          NormalCard(CardValue=CardValue.Jack,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Jack,CardSuit=CardSuit.Heart); ]

    //    canPlay prevCards currentCards |> Assert.True

    //[<Fact>]
    //let ``测试CanPlay JJJJ _ 3333`` () =

    //    let prevCards = 
    //         [ NormalCard(CardValue=CardValue.Jack,CardSuit=CardSuit.Spade);
    //          NormalCard(CardValue=CardValue.Jack,CardSuit=CardSuit.Diamond); 
    //          NormalCard(CardValue=CardValue.Jack,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Jack,CardSuit=CardSuit.Heart); ]

    //    let currentCards = 
    //        [ NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Diamond); 
    //          NormalCard(CardValue=CardValue.Three,CardSuit=CardSuit.Heart); ]

    //    canPlay prevCards currentCards |> Assert.False

    //[<Fact>]
    //let ``测试CanPlay JJJJ _ 2222`` () =

    //    let prevCards = 
    //         [ NormalCard(CardValue=CardValue.Jack, CardSuit=CardSuit.Spade);
    //          NormalCard(CardValue=CardValue.Jack, CardSuit=CardSuit.Diamond); 
    //          NormalCard(CardValue=CardValue.Jack, CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Jack, CardSuit=CardSuit.Heart); ]

    //    let currentCards = 
    //        [ NormalCard(CardValue=CardValue.Two, CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Two, CardSuit=CardSuit.Spade); 
    //          NormalCard(CardValue=CardValue.Two, CardSuit=CardSuit.Diamond); 
    //          NormalCard(CardValue=CardValue.Two,CardSuit=CardSuit.Heart); ]

    //    canPlay prevCards currentCards |> Assert.True

    //[<Fact>]
    //let ``测试CanPlay() 222 _ 3333`` () =

    //    let prevCards= 
    //        [ NormalCard(CardValue=CardValue.Two, CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Two, CardSuit=CardSuit.Diamond); 
    //          NormalCard(CardValue=CardValue.Two,CardSuit=CardSuit.Heart); ]

    //    let currentCards= 
    //         [ NormalCard(CardValue=CardValue.Three, CardSuit=CardSuit.Spade);
    //          NormalCard(CardValue=CardValue.Three, CardSuit=CardSuit.Diamond); 
    //          NormalCard(CardValue=CardValue.Three, CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Three, CardSuit=CardSuit.Heart); ]

    //    canPlay prevCards currentCards |> Assert.True

    //[<Fact>]
    //let ``测试CanPlay() 222 _ AAA`` () =

    //    let prevCards= 
    //        [ NormalCard(CardValue=CardValue.Two, CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Two, CardSuit=CardSuit.Diamond); 
    //          NormalCard(CardValue=CardValue.Two,CardSuit=CardSuit.Heart); ]

    //    let currentCards= 
    //         [ NormalCard(CardValue=CardValue.Ace, CardSuit=CardSuit.Spade);
    //          NormalCard(CardValue=CardValue.Ace, CardSuit=CardSuit.Diamond); 
    //          NormalCard(CardValue=CardValue.Ace, CardSuit=CardSuit.Heart); ]

    //    canPlay prevCards currentCards |> Assert.False

    //[<Fact>]
    //let ``测试CanPlay() KKK _ 222`` () =

    //    let prevCards= 
    //         [ NormalCard(CardValue=CardValue.King, CardSuit=CardSuit.Spade);
    //          NormalCard(CardValue=CardValue.King, CardSuit=CardSuit.Diamond); 
    //          NormalCard(CardValue=CardValue.King, CardSuit=CardSuit.Club); ]
    //    let currentCards= 
    //        [ NormalCard(CardValue=CardValue.Two, CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Two, CardSuit=CardSuit.Diamond); 
    //          NormalCard(CardValue=CardValue.Two,CardSuit=CardSuit.Heart); ]

    //    canPlay prevCards currentCards |> Assert.True


    //[<Fact>]
    //let ``测试CanPlay() 3335 _ 5554`` () =

    //    let prevCards= 
    //         [ NormalCard(CardValue=CardValue.Three, CardSuit=CardSuit.Spade);
    //          NormalCard(CardValue=CardValue.Three, CardSuit=CardSuit.Diamond); 
    //          NormalCard(CardValue=CardValue.Three, CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Five, CardSuit=CardSuit.Spade); ]
    //    let currentCards= 
    //        [ NormalCard(CardValue=CardValue.Five, CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Five, CardSuit=CardSuit.Diamond); 
    //          NormalCard(CardValue=CardValue.Five, CardSuit=CardSuit.Heart); 
    //          NormalCard(CardValue=CardValue.Four,CardSuit=CardSuit.Heart); ]

    //    canPlay prevCards currentCards |> Assert.True

    //[<Fact>]
    //let ``测试CanPlay() AAA3 _ 5554`` () =

    //    let prevCards= 
    //         [ NormalCard(CardValue=CardValue.Three, CardSuit=CardSuit.Spade);
    //          NormalCard(CardValue=CardValue.Ace, CardSuit=CardSuit.Diamond); 
    //          NormalCard(CardValue=CardValue.Ace, CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Ace, CardSuit=CardSuit.Heart); ]
    //    let currentCards= 
    //        [ NormalCard(CardValue=CardValue.Five, CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Five, CardSuit=CardSuit.Diamond); 
    //          NormalCard(CardValue=CardValue.Five, CardSuit=CardSuit.Heart); 
    //          NormalCard(CardValue=CardValue.Four,CardSuit=CardSuit.Heart); ]

    //    canPlay prevCards currentCards |> Assert.False

    //[<Fact>]
    //let ``测试CanPlay() AAA3 _ KKK2`` () =

    //    let prevCards= 
    //         [ NormalCard(CardValue=CardValue.Three, CardSuit=CardSuit.Spade);
    //          NormalCard(CardValue=CardValue.Ace, CardSuit=CardSuit.Diamond); 
    //          NormalCard(CardValue=CardValue.Ace, CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.Ace, CardSuit=CardSuit.Heart); ]
    //    let currentCards= 
    //        [ NormalCard(CardValue=CardValue.King, CardSuit=CardSuit.Club); 
    //          NormalCard(CardValue=CardValue.King, CardSuit=CardSuit.Diamond); 
    //          NormalCard(CardValue=CardValue.King, CardSuit=CardSuit.Heart); 
    //          NormalCard(CardValue=CardValue.Two,CardSuit=CardSuit.Heart); ]

    //    canPlay prevCards currentCards |> Assert.False
