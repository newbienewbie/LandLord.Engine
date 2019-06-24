module TestsLianDui

open System
open Xunit
open System.Collections.Generic
open Itminus.LandLord.Engine.Card

let private lianshun2 (cards: PlayingCard list) = 
    match cards with
    | DuiZi list -> 1
    | ShunZi 2 3 cards -> 3
    | ShunZi 2 4 cards -> 4
    | ShunZi 2 5 cards -> 5
    | ShunZi 2 6 cards -> 6
    | ShunZi 2 7 cards -> 7 
    | ShunZi 2 8 cards -> 8 
    | ShunZi 2 9 cards -> 9 
    | ShunZi 2 10 cards -> 10 
    | ShunZi 2 11 cards -> 11 
    | ShunZi 2 12 cards -> 12
    | _ -> 0


[<Fact>]
let ``测试5连对-顺序`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Three, Suit.Heart);
            NormalCard( CardValue.Three, Suit.Diamond);
            NormalCard( CardValue.Four, Suit.Heart);
            NormalCard( CardValue.Four, Suit.Diamond);
            NormalCard( CardValue.Five, Suit.Heart); 
            NormalCard( CardValue.Five, Suit.Diamond); 
            NormalCard( CardValue.Six, Suit.Heart); 
            NormalCard( CardValue.Six, Suit.Diamond); 
            NormalCard( CardValue.Seven, Suit.Heart); 
            NormalCard( CardValue.Seven, Suit.Diamond); ]

    let x = lianshun2 cards
    Assert.Equal(5,  x)


[<Fact>]
let ``测试5连对-乱序`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Three, Suit.Heart);
            NormalCard( CardValue.Four, Suit.Diamond);
            NormalCard( CardValue.Three, Suit.Diamond);
            NormalCard( CardValue.Four, Suit.Heart);
            NormalCard( CardValue.Five, Suit.Diamond); 
            NormalCard( CardValue.Five, Suit.Heart); 
            NormalCard( CardValue.Six, Suit.Heart); 
            NormalCard( CardValue.Seven, Suit.Heart); 
            NormalCard( CardValue.Six, Suit.Diamond); 
            NormalCard( CardValue.Seven, Suit.Diamond); ]

    let x = lianshun2 cards
    Assert.Equal(5,  x)

[<Fact>]
let ``测试5连对-不含A-2-3-4-5`` () =
    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Ace, Suit.Heart);
            NormalCard( CardValue.Ace, Suit.Diamond);
            NormalCard( CardValue.Two, Suit.Diamond); 
            NormalCard( CardValue.Two, Suit.Heart); 
            NormalCard( CardValue.Three, Suit.Heart); 
            NormalCard( CardValue.Three, Suit.Diamond); 
            NormalCard( CardValue.Four, Suit.Diamond);
            NormalCard( CardValue.Four, Suit.Heart);
            NormalCard( CardValue.Five, Suit.Diamond); 
            NormalCard( CardValue.Five, Suit.Heart); ]

    let x = lianshun2 cards
    Assert.Equal(0,  x)

[<Fact>]
let ``测试6连对-3/3/4/4/5/5/6/6/7/7/8/8`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Three, Suit.Heart);
            NormalCard( CardValue.Three, Suit.Diamond);
            NormalCard( CardValue.Four, Suit.Diamond);
            NormalCard( CardValue.Four, Suit.Heart);
            NormalCard( CardValue.Five, Suit.Heart); 
            NormalCard( CardValue.Five, Suit.Diamond); 
            NormalCard( CardValue.Six, Suit.Heart); 
            NormalCard( CardValue.Six, Suit.Diamond); 
            NormalCard( CardValue.Seven, Suit.Heart); 
            NormalCard( CardValue.Seven, Suit.Diamond); 
            NormalCard( CardValue.Eight, Suit.Heart); 
            NormalCard( CardValue.Eight, Suit.Diamond); ]

    let x = lianshun2 cards
    Assert.Equal(6, x)

[<Fact>]
let ``测试6连对-3/3/4/4/4/5/5/6/6/7/7/8/8`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Three, Suit.Heart);
            NormalCard( CardValue.Three, Suit.Diamond);
            NormalCard( CardValue.Four, Suit.Diamond);
            NormalCard( CardValue.Four, Suit.Club);
            NormalCard( CardValue.Four, Suit.Heart);
            NormalCard( CardValue.Five, Suit.Heart); 
            NormalCard( CardValue.Five, Suit.Diamond); 
            NormalCard( CardValue.Six, Suit.Heart); 
            NormalCard( CardValue.Six, Suit.Diamond); 
            NormalCard( CardValue.Seven, Suit.Heart); 
            NormalCard( CardValue.Seven, Suit.Diamond); 
            NormalCard( CardValue.Eight, Suit.Heart); 
            NormalCard( CardValue.Eight, Suit.Diamond); ]

    let x = lianshun2 cards
    Assert.Equal(0,  x)

[<Fact>]
let ``测试6连对- 9/9/10/10/J/J/Q/Q/K/K/A/A`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Nine, Suit.Heart); 
            NormalCard( CardValue.Nine, Suit.Club); 
            NormalCard( CardValue.Ten, Suit.Heart); 
            NormalCard( CardValue.Ten, Suit.Diamond  ); 
            NormalCard( CardValue.Jack, Suit.Heart); 
            NormalCard( CardValue.Jack, Suit.Club); 
            NormalCard( CardValue.Queen, Suit.Heart); 
            NormalCard( CardValue.Queen, Suit.Club); 
            NormalCard( CardValue.King, Suit.Heart); 
            NormalCard( CardValue.King, Suit.Club); 
            NormalCard( CardValue.Ace, Suit.Heart);
            NormalCard( CardValue.Ace, Suit.Heart); ]

    let x = lianshun2 cards
    Assert.Equal(6, x)

[<Fact>]
let ``测试6连对- 10/10/J/J/Q/Q//K/K/A/A/2/2`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Ten, Suit.Heart); 
            NormalCard( CardValue.Ten, Suit.Club); 
            NormalCard( CardValue.Jack, Suit.Heart); 
            NormalCard( CardValue.Jack, Suit.Diamond); 
            NormalCard( CardValue.Queen, Suit.Heart); 
            NormalCard( CardValue.Queen, Suit.Club); 
            NormalCard( CardValue.King, Suit.Heart); 
            NormalCard( CardValue.King, Suit.Club); 
            NormalCard( CardValue.Ace, Suit.Heart); 
            NormalCard( CardValue.Ace, Suit.Diamond); 
            NormalCard( CardValue.Two, Suit.Heart); 
            NormalCard( CardValue.Two, Suit.Club); ]

    let x = lianshun2 cards
    Assert.Equal(6, x)

