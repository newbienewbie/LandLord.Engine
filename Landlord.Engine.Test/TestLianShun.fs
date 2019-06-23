module TestsLianShun

open System
open Xunit
open System.Collections.Generic
open Itminus.LandLord.Engine.Card

let lianshun (cards: PlayingCard list) = 
    match cards with
    | DanZhang list -> 1
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
        [   NormalCard( CardValue.Seven, Heart); ]

    let x = match cards with 
    | LianShun 1 cards -> 1
    | _ -> 0

    Assert.Equal(0,  x)

[<Fact>]
let ``测试5连顺-顺序`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Three, Heart);
            NormalCard( CardValue.Four, Diamond);
            NormalCard( CardValue.Five, Heart); 
            NormalCard( CardValue.Six, Heart); 
            NormalCard( CardValue.Seven, Heart); ]

    let x = lianshun cards
    Assert.Equal(5,  x)


[<Fact>]
let ``测试5连顺-乱序`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Three, Heart);
            NormalCard( CardValue.Six, Heart); 
            NormalCard( CardValue.Five, Heart); 
            NormalCard( CardValue.Four, Diamond);
            NormalCard( CardValue.Seven, Heart); ]

    let x = lianshun cards
    Assert.Equal(5,  x)

[<Fact>]
let ``测试5连顺-不含A-2-3-4-5`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Ace, Heart);
            NormalCard( CardValue.Two, Heart); 
            NormalCard( CardValue.Three, Heart); 
            NormalCard( CardValue.Four, Diamond);
            NormalCard( CardValue.Five, Heart); ]

    let x = lianshun cards
    Assert.Equal(0,  x)

[<Fact>]
let ``测试6连顺`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Three, Heart);
            NormalCard( CardValue.Four, Diamond);
            NormalCard( CardValue.Five, Heart); 
            NormalCard( CardValue.Six, Heart); 
            NormalCard( CardValue.Seven, Heart); 
            NormalCard( CardValue.Eight, Heart); ]

    let x = lianshun cards
    Assert.Equal(6, x)

[<Fact>]
let ``测试6连顺- 9/10/J/Q/K/A`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Nine, Heart); 
            NormalCard( CardValue.Ten, Heart); 
            NormalCard( CardValue.Jack, Heart); 
            NormalCard( CardValue.Queen, Heart); 
            NormalCard( CardValue.King, Heart); 
            NormalCard( CardValue.Ace, Heart); ]

    let x = lianshun cards
    Assert.Equal(6, x)

[<Fact>]
let ``测试6连顺- 10/J/Q/K/A/2`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Ten, Heart); 
            NormalCard( CardValue.Jack, Heart); 
            NormalCard( CardValue.Queen, Heart); 
            NormalCard( CardValue.King, Heart); 
            NormalCard( CardValue.Ace, Heart); 
            NormalCard( CardValue.Two, Heart); ]

    let x = lianshun cards
    Assert.Equal(6, x)

[<Fact>]
let ``测试7连顺-3/4/5/6/7/8/9`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Three, Heart);
            NormalCard( CardValue.Four, Diamond);
            NormalCard( CardValue.Five, Heart); 
            NormalCard( CardValue.Six, Heart); 
            NormalCard( CardValue.Seven, Heart); 
            NormalCard( CardValue.Eight, Heart); 
            NormalCard( CardValue.Nine, Heart); ]

    let x = lianshun cards
    Assert.Equal(7,  x)

[<Fact>]
let ``测试7连顺-9/10/J/Q/K/A/2`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Nine, Heart);
            NormalCard( CardValue.Ten, Diamond);
            NormalCard( CardValue.Jack, Heart); 
            NormalCard( CardValue.Queen, Heart); 
            NormalCard( CardValue.King, Heart); 
            NormalCard( CardValue.Ace, Heart); 
            NormalCard( CardValue.Two, Heart); ]

    let x = lianshun cards
    Assert.Equal(7,  x)

[<Fact>]
let ``测试7连顺-10/J/Q/K/A/2/3`` () =

    let cards : PlayingCard list = 
        [   NormalCard( CardValue.Ten, Diamond);
            NormalCard( CardValue.Jack, Heart); 
            NormalCard( CardValue.Queen, Heart); 
            NormalCard( CardValue.King, Heart); 
            NormalCard( CardValue.Ace, Heart); 
            NormalCard( CardValue.Two, Heart); 
            NormalCard( CardValue.Three, Heart); ]

    let x = lianshun cards
    Assert.Equal(0,  x)
