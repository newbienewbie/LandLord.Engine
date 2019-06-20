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
        [   NormalCard( CardValue.Three, Heart);
            NormalCard( CardValue.Three, Diamond);
            NormalCard( CardValue.Four, Heart);
            NormalCard( CardValue.Four, Diamond);
            NormalCard( CardValue.Five, Heart); 
            NormalCard( CardValue.Five, Diamond); 
            NormalCard( CardValue.Six, Heart); 
            NormalCard( CardValue.Six, Diamond); 
            NormalCard( CardValue.Seven, Heart); 
            NormalCard( CardValue.Seven, Diamond); ]

    let x = lianshun2 cards
    Assert.Equal(5,  x)


[<Fact>]
let ``测试5连对-乱序`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Three, Heart);
            NormalCard( CardValue.Four, Diamond);
            NormalCard( CardValue.Three, Diamond);
            NormalCard( CardValue.Four, Heart);
            NormalCard( CardValue.Five, Diamond); 
            NormalCard( CardValue.Five, Heart); 
            NormalCard( CardValue.Six, Heart); 
            NormalCard( CardValue.Seven, Heart); 
            NormalCard( CardValue.Six, Diamond); 
            NormalCard( CardValue.Seven, Diamond); ]

    let x = lianshun2 cards
    Assert.Equal(5,  x)

[<Fact>]
let ``测试5连对-不含A-2-3-4-5`` () =
    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Ace, Heart);
            NormalCard( CardValue.Ace, Diamond);
            NormalCard( CardValue.Two, Diamond); 
            NormalCard( CardValue.Two, Heart); 
            NormalCard( CardValue.Three, Heart); 
            NormalCard( CardValue.Three, Diamond); 
            NormalCard( CardValue.Four, Diamond);
            NormalCard( CardValue.Four, Heart);
            NormalCard( CardValue.Five, Diamond); 
            NormalCard( CardValue.Five, Heart); ]

    let x = lianshun2 cards
    Assert.Equal(0,  x)

[<Fact>]
let ``测试6连对-3/3/4/4/5/5/6/6/7/7/8/8`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Three, Heart);
            NormalCard( CardValue.Three, Diamond);
            NormalCard( CardValue.Four, Diamond);
            NormalCard( CardValue.Four, Heart);
            NormalCard( CardValue.Five, Heart); 
            NormalCard( CardValue.Five, Diamond); 
            NormalCard( CardValue.Six, Heart); 
            NormalCard( CardValue.Six, Diamond); 
            NormalCard( CardValue.Seven, Heart); 
            NormalCard( CardValue.Seven, Diamond); 
            NormalCard( CardValue.Eight, Heart); 
            NormalCard( CardValue.Eight, Diamond); ]

    let x = lianshun2 cards
    Assert.Equal(6, x)

[<Fact>]
let ``测试6连对-3/3/4/4/4/5/5/6/6/7/7/8/8`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Three, Heart);
            NormalCard( CardValue.Three, Diamond);
            NormalCard( CardValue.Four, Diamond);
            NormalCard( CardValue.Four, Club);
            NormalCard( CardValue.Four, Heart);
            NormalCard( CardValue.Five, Heart); 
            NormalCard( CardValue.Five, Diamond); 
            NormalCard( CardValue.Six, Heart); 
            NormalCard( CardValue.Six, Diamond); 
            NormalCard( CardValue.Seven, Heart); 
            NormalCard( CardValue.Seven, Diamond); 
            NormalCard( CardValue.Eight, Heart); 
            NormalCard( CardValue.Eight, Diamond); ]

    let x = lianshun2 cards
    Assert.Equal(0,  x)

[<Fact>]
let ``测试6连对- 9/9/10/10/J/J/Q/Q/K/K/A/A`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Nine, Heart); 
            NormalCard( CardValue.Nine, Club); 
            NormalCard( CardValue.Ten, Heart); 
            NormalCard( CardValue.Ten, Diamond  ); 
            NormalCard( CardValue.Jack, Heart); 
            NormalCard( CardValue.Jack, Club); 
            NormalCard( CardValue.Queen, Heart); 
            NormalCard( CardValue.Queen, Club); 
            NormalCard( CardValue.King, Heart); 
            NormalCard( CardValue.King, Club); 
            NormalCard( CardValue.Ace, Heart);
            NormalCard( CardValue.Ace, Heart); ]

    let x = lianshun2 cards
    Assert.Equal(6, x)

[<Fact>]
let ``测试6连对- 10/10/J/J/Q/Q//K/K/A/A/2/2`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Ten, Heart); 
            NormalCard( CardValue.Ten, Club); 
            NormalCard( CardValue.Jack, Heart); 
            NormalCard( CardValue.Jack, Diamond); 
            NormalCard( CardValue.Queen, Heart); 
            NormalCard( CardValue.Queen, Club); 
            NormalCard( CardValue.King, Heart); 
            NormalCard( CardValue.King, Club); 
            NormalCard( CardValue.Ace, Heart); 
            NormalCard( CardValue.Ace, Diamond); 
            NormalCard( CardValue.Two, Heart); 
            NormalCard( CardValue.Two, Club); ]

    let x = lianshun2 cards
    Assert.Equal(6, x)

