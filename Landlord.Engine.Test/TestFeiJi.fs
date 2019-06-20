module TestFeiJi

open System
open Xunit
open System.Collections.Generic
open Itminus.LandLord.Engine.Card

let private lianshun3 (cards: PlayingCard list) = 
    match cards with
    | SanZhang list -> 1
    | ShunZi 3 3 cards -> 3
    | ShunZi 3 4 cards -> 4
    | ShunZi 3 5 cards -> 5
    | ShunZi 3 6 cards -> 6
    | ShunZi 3 7 cards -> 7 
    | ShunZi 3 8 cards -> 8 
    | ShunZi 3 9 cards -> 9 
    | ShunZi 3 10 cards -> 10 
    | ShunZi 3 11 cards -> 11 
    | ShunZi 3 12 cards -> 12
    | _ -> 0


[<Fact>]
let ``测试5连飞机-顺序`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Three, Heart);
            NormalCard( CardValue.Three, Diamond);
            NormalCard( CardValue.Three, Club);
            NormalCard( CardValue.Four, Heart);
            NormalCard( CardValue.Four, Club);
            NormalCard( CardValue.Four, Diamond);
            NormalCard( CardValue.Five, Heart); 
            NormalCard( CardValue.Five, Club); 
            NormalCard( CardValue.Five, Diamond); 
            NormalCard( CardValue.Six, Heart); 
            NormalCard( CardValue.Six, Club); 
            NormalCard( CardValue.Six, Diamond); 
            NormalCard( CardValue.Seven, Heart); 
            NormalCard( CardValue.Seven, Club); 
            NormalCard( CardValue.Seven, Diamond); ]

    let x = lianshun3 cards
    Assert.Equal(5,  x)


[<Fact>]
let ``测试5连飞机-乱序`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Three, Heart);
            NormalCard( CardValue.Four, Diamond);
            NormalCard( CardValue.Three, Club);
            NormalCard( CardValue.Four, Club);
            NormalCard( CardValue.Three, Diamond);
            NormalCard( CardValue.Four, Heart);
            NormalCard( CardValue.Five, Diamond); 
            NormalCard( CardValue.Five, Heart); 
            NormalCard( CardValue.Five, Club); 
            NormalCard( CardValue.Six, Heart); 
            NormalCard( CardValue.Six, Club); 
            NormalCard( CardValue.Seven, Heart); 
            NormalCard( CardValue.Seven, Club); 
            NormalCard( CardValue.Six, Diamond); 
            NormalCard( CardValue.Seven, Diamond); ]

    let x = lianshun3 cards
    Assert.Equal(5,  x)

[<Fact>]
let ``测试5连飞机-不含A-2-3-4-5`` () =
    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Ace, Heart);
            NormalCard( CardValue.Ace, Diamond);
            NormalCard( CardValue.Ace, Club);
            NormalCard( CardValue.Two, Diamond); 
            NormalCard( CardValue.Two, Club); 
            NormalCard( CardValue.Two, Heart); 
            NormalCard( CardValue.Three, Heart); 
            NormalCard( CardValue.Three, Club); 
            NormalCard( CardValue.Three, Diamond); 
            NormalCard( CardValue.Four, Diamond);
            NormalCard( CardValue.Four, Club);
            NormalCard( CardValue.Four, Heart);
            NormalCard( CardValue.Five, Diamond); 
            NormalCard( CardValue.Five, Club); 
            NormalCard( CardValue.Five, Heart); ]

    let x = lianshun3 cards
    Assert.Equal(0,  x)

[<Fact>]
let ``测试6连飞机-3/3/3/4/4/4/5/5/5/6/6/6/7/7/7/8/8/8`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Three, Heart);
            NormalCard( CardValue.Three, Diamond);
            NormalCard( CardValue.Three, Club);
            NormalCard( CardValue.Four, Diamond);
            NormalCard( CardValue.Four, Club);
            NormalCard( CardValue.Four, Heart);
            NormalCard( CardValue.Five, Heart); 
            NormalCard( CardValue.Five, Diamond); 
            NormalCard( CardValue.Five, Club); 
            NormalCard( CardValue.Six, Heart); 
            NormalCard( CardValue.Six, Club); 
            NormalCard( CardValue.Six, Diamond); 
            NormalCard( CardValue.Seven, Heart); 
            NormalCard( CardValue.Seven, Club); 
            NormalCard( CardValue.Seven, Diamond); 
            NormalCard( CardValue.Eight, Heart); 
            NormalCard( CardValue.Eight, Club); 
            NormalCard( CardValue.Eight, Diamond); ]

    let x = lianshun3 cards
    Assert.Equal(6, x)

[<Fact>]
let ``测试6连飞机-3/3/3/4/4/4/5/6/7/7/8/8`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Three, Heart);
            NormalCard( CardValue.Three, Diamond);
            NormalCard( CardValue.Three, Club);
            NormalCard( CardValue.Four, Diamond);
            NormalCard( CardValue.Four, Club);
            NormalCard( CardValue.Four, Heart);
            NormalCard( CardValue.Five, Heart); 
            NormalCard( CardValue.Six, Diamond); 
            NormalCard( CardValue.Seven, Heart); 
            NormalCard( CardValue.Seven, Diamond); 
            NormalCard( CardValue.Eight, Heart); 
            NormalCard( CardValue.Eight, Diamond); ]

    let x = lianshun3 cards
    Assert.Equal(0,  x)

[<Fact>]
let ``测试6连飞机- 9/9/10/10/J/J/Q/Q/K/K/A/A`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Nine, Heart); 
            NormalCard( CardValue.Nine, Club); 
            NormalCard( CardValue.Nine, Diamond); 
            NormalCard( CardValue.Ten, Heart); 
            NormalCard( CardValue.Ten, Club); 
            NormalCard( CardValue.Ten, Diamond  ); 
            NormalCard( CardValue.Jack, Heart); 
            NormalCard( CardValue.Jack, Club); 
            NormalCard( CardValue.Jack, Club); 
            NormalCard( CardValue.Queen, Heart); 
            NormalCard( CardValue.Queen, Diamond); 
            NormalCard( CardValue.Queen, Club); 
            NormalCard( CardValue.King, Heart); 
            NormalCard( CardValue.King, Diamond); 
            NormalCard( CardValue.King, Club); 
            NormalCard( CardValue.Ace, Heart);
            NormalCard( CardValue.Ace, Diamond);
            NormalCard( CardValue.Ace, Heart); ]

    let x = lianshun3 cards
    Assert.Equal(6, x)

[<Fact>]
let ``测试6连飞机- 10/10/10/J/J/J/Q/Q/Q/K/K/K/A/A/A/2/2/2`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Ten, Heart); 
            NormalCard( CardValue.Ten, Club); 
            NormalCard( CardValue.Ten, Diamond); 
            NormalCard( CardValue.Jack, Heart); 
            NormalCard( CardValue.Jack, Club); 
            NormalCard( CardValue.Jack, Diamond); 
            NormalCard( CardValue.Queen, Heart); 
            NormalCard( CardValue.Queen, Club); 
            NormalCard( CardValue.Queen, Diamond); 
            NormalCard( CardValue.King, Heart); 
            NormalCard( CardValue.King, Diamond); 
            NormalCard( CardValue.King, Club); 
            NormalCard( CardValue.Ace, Heart); 
            NormalCard( CardValue.Ace, Diamond); 
            NormalCard( CardValue.Ace, Club); 
            NormalCard( CardValue.Two, Heart); 
            NormalCard( CardValue.Two, Club); 
            NormalCard( CardValue.Two, Club); ]

    let x = lianshun3 cards
    Assert.Equal(6, x)

