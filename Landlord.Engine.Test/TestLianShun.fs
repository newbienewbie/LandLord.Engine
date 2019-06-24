module TestsLianShun

open System
open Xunit
open System.Collections.Generic
open Itminus.LandLord.Engine.Card

let lianshun (cards: PlayingCard list) = 
    match cards with
    | Single list -> 1
    | LianShun5 list -> 5
    | LianShun6 list -> 6
    | LianShun7 list -> 7
    | LianShun8 list -> 8
    | LianShun9 list -> 9
    | LianShun10 list -> 10
    | LianShun11 list -> 11
    | LianShun12 list -> 12
    | _ -> 0

[<Fact>]
let ``测试连顺-单张不是顺子`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Seven, Suit.Heart); ]

    let x = match cards with 
    |LianShun 1 cards -> 1
    |_ -> 0

    Assert.Equal(0,  x)

[<Fact>]
let ``测试5连顺-顺序`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Three, Suit.Heart);
            NormalCard( CardValue.Four, Suit.Diamond);
            NormalCard( CardValue.Five, Suit.Heart); 
            NormalCard( CardValue.Six, Suit.Heart); 
            NormalCard( CardValue.Seven, Suit.Heart); ]

    let x = lianshun cards
    Assert.Equal(5,  x)


[<Fact>]
let ``测试5连顺-乱序`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Three, Suit.Heart);
            NormalCard( CardValue.Six, Suit.Heart); 
            NormalCard( CardValue.Five, Suit.Heart); 
            NormalCard( CardValue.Four, Suit.Diamond);
            NormalCard( CardValue.Seven, Suit.Heart); ]

    let x = lianshun cards
    Assert.Equal(5,  x)

[<Fact>]
let ``测试5连顺-不含A-2-3-4-5`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Ace, Suit.Heart);
            NormalCard( CardValue.Two, Suit.Heart); 
            NormalCard( CardValue.Three, Suit.Heart); 
            NormalCard( CardValue.Four, Suit.Diamond);
            NormalCard( CardValue.Five, Suit.Heart); ]

    let x = lianshun cards
    Assert.Equal(0,  x)

[<Fact>]
let ``测试6连顺`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Three, Suit.Heart);
            NormalCard( CardValue.Four, Suit.Diamond);
            NormalCard( CardValue.Five, Suit.Heart); 
            NormalCard( CardValue.Six, Suit.Heart); 
            NormalCard( CardValue.Seven, Suit.Heart); 
            NormalCard( CardValue.Eight, Suit.Heart); ]

    let x = lianshun cards
    Assert.Equal(6, x)

[<Fact>]
let ``测试6连顺- 9/10/J/Q/K/A`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Nine, Suit.Heart); 
            NormalCard( CardValue.Ten, Suit.Heart); 
            NormalCard( CardValue.Jack, Suit.Heart); 
            NormalCard( CardValue.Queen, Suit.Heart); 
            NormalCard( CardValue.King, Suit.Heart); 
            NormalCard( CardValue.Ace, Suit.Heart); ]

    let x = lianshun cards
    Assert.Equal(6, x)

[<Fact>]
let ``测试6连顺- 10/J/Q/K/A/2`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Ten, Suit.Heart); 
            NormalCard( CardValue.Jack, Suit.Heart); 
            NormalCard( CardValue.Queen, Suit.Heart); 
            NormalCard( CardValue.King, Suit.Heart); 
            NormalCard( CardValue.Ace, Suit.Heart); 
            NormalCard( CardValue.Two, Suit.Heart); ]

    let x = lianshun cards
    Assert.Equal(6, x)

[<Fact>]
let ``测试7连顺-3/4/5/6/7/8/9`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Three, Suit.Heart);
            NormalCard( CardValue.Four, Suit.Diamond);
            NormalCard( CardValue.Five, Suit.Heart); 
            NormalCard( CardValue.Six, Suit.Heart); 
            NormalCard( CardValue.Seven, Suit.Heart); 
            NormalCard( CardValue.Eight, Suit.Heart); 
            NormalCard( CardValue.Nine, Suit.Heart); ]

    let x = lianshun cards
    Assert.Equal(7,  x)

[<Fact>]
let ``测试7连顺-9/10/J/Q/K/A/2`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Nine, Suit.Heart);
            NormalCard( CardValue.Ten, Suit.Diamond);
            NormalCard( CardValue.Jack, Suit.Heart); 
            NormalCard( CardValue.Queen, Suit.Heart); 
            NormalCard( CardValue.King, Suit.Heart); 
            NormalCard( CardValue.Ace, Suit.Heart); 
            NormalCard( CardValue.Two, Suit.Heart); ]

    let x = lianshun cards
    Assert.Equal(7,  x)

[<Fact>]
let ``测试7连顺-10/J/Q/K/A/2/3`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Ten, Suit.Diamond);
            NormalCard( CardValue.Jack, Suit.Heart); 
            NormalCard( CardValue.Queen, Suit.Heart); 
            NormalCard( CardValue.King, Suit.Heart); 
            NormalCard( CardValue.Ace, Suit.Heart); 
            NormalCard( CardValue.Two, Suit.Heart); 
            NormalCard( CardValue.Three, Suit.Heart); ]

    let x = lianshun cards
    Assert.Equal(0,  x)
