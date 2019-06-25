module TestFacade

open System
open Xunit
open System.Collections.Generic
open Itminus.LandLord.Engine.Card
open Itminus.LandLord.Engine.Facade
     

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
