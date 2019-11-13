namespace LandLord.Core.Tests


module TestsLianShun =

    open System
    open Xunit
    open System.Collections.Generic
    open LandLord.Core
    open System.Linq

    let lianshun (cards': PlayingCard list) = 
        let cards = List.toSeq(cards').ToList()
        let f5, _ = Patterns.Patterns.DanLianShun(5, cards).ToTuple()
        let f6, _ = Patterns.Patterns.DanLianShun(6, cards).ToTuple()
        let f7, _ = Patterns.Patterns.DanLianShun(7, cards).ToTuple()
        let f8, _ = Patterns.Patterns.DanLianShun(8, cards).ToTuple()
        let f9, _ = Patterns.Patterns.DanLianShun(9, cards).ToTuple()
        let f10, _ = Patterns.Patterns.DanLianShun(10, cards).ToTuple()
        let f11, _ = Patterns.Patterns.DanLianShun(11, cards).ToTuple()
        let f12, _ = Patterns.Patterns.DanLianShun(12, cards).ToTuple()
        if f5 then 
            5
        else if f6 then 
            6
        else if f7 then 
            7
        else if f8 then 
            8
        else if f9 then 
            9 
        else if f10 then 
            10 
        else if f11 then 
            11
        else if f12 then 
            12
        else 
            0

    [<Fact>]
    let ``测试连顺-单张不是顺子`` () =

        let cards : PlayingCard list = 
            [   NormalCard( CardValue=CardValue.Seven, CardSuit= CardSuit.Heart); ]

        let x = lianshun cards 
        Assert.Equal(0,  x)

    [<Fact>]
    let ``测试5连顺-顺序`` () =

        let cards : PlayingCard list = 
            [   NormalCard( CardValue= CardValue.Three, CardSuit= CardSuit.Heart);
                NormalCard( CardValue= CardValue.Four, CardSuit= CardSuit.Diamond);
                NormalCard( CardValue= CardValue.Five, CardSuit= CardSuit.Heart); 
                NormalCard( CardValue= CardValue.Six, CardSuit= CardSuit.Heart); 
                NormalCard(CardValue=  CardValue.Seven, CardSuit= CardSuit.Heart); ]

        let x = lianshun cards
        Assert.Equal(5,  x)


    [<Fact>]
    let ``测试5连顺-乱序`` () =

        let cards : PlayingCard list = 
            [   NormalCard(CardValue=CardValue.Three, CardSuit=CardSuit.Heart);
                NormalCard(CardValue=CardValue.Six, CardSuit=CardSuit.Heart); 
                NormalCard(CardValue=CardValue.Five, CardSuit=CardSuit.Heart); 
                NormalCard(CardValue=CardValue.Four, CardSuit=CardSuit.Diamond);
                NormalCard(CardValue=CardValue.Seven, CardSuit=CardSuit.Heart); ]

        let x = lianshun cards
        Assert.Equal(5,  x)

    [<Fact>]
    let ``测试5连顺-不含A-2-3-4-5`` () =

        let cards : PlayingCard list = 
            [   NormalCard( CardValue=CardValue.Ace, CardSuit=CardSuit.Heart);
                NormalCard( CardValue=CardValue.Two, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Four, CardSuit=CardSuit.Diamond);
                NormalCard( CardValue=CardValue.Five, CardSuit=CardSuit.Heart); ]

        let x = lianshun cards
        Assert.Equal(0,  x)

    [<Fact>]
    let ``测试6连顺`` () =

        let cards : PlayingCard list = 
            [   NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Heart);
                NormalCard( CardValue=CardValue.Four, CardSuit=CardSuit.Diamond);
                NormalCard( CardValue=CardValue.Five, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Six, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Seven, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Eight, CardSuit=CardSuit.Heart); ]

        let x = lianshun cards
        Assert.Equal(6, x)

    [<Fact>]
    let ``测试6连顺- 9/10/J/Q/K/A`` () =

        let cards : PlayingCard list = 
            [   NormalCard( CardValue=CardValue.Nine, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Ten, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Jack, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Queen, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.King, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Ace, CardSuit=CardSuit.Heart); ]

        let x = lianshun cards
        Assert.Equal(6, x)

    [<Fact>]
    let ``测试6连顺- 10/J/Q/K/A/2`` () =

        let cards : PlayingCard list = 
            [   NormalCard( CardValue=CardValue.Ten, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Jack, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Queen, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.King, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Ace, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Two, CardSuit=CardSuit.Heart); ]

        let x = lianshun cards
        Assert.Equal(6, x)

    [<Fact>]
    let ``测试7连顺-3/4/5/6/7/8/9`` () =

        let cards : PlayingCard list = 
            [   NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Heart);
                NormalCard( CardValue=CardValue.Four, CardSuit=CardSuit.Diamond);
                NormalCard( CardValue=CardValue.Five, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Six, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Seven, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Eight, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Nine, CardSuit=CardSuit.Heart); ]

        let x = lianshun cards
        Assert.Equal(7,  x)

    [<Fact>]
    let ``测试7连顺-9/10/J/Q/K/A/2`` () =

        let cards : PlayingCard list = 
            [   NormalCard( CardValue=CardValue.Nine, CardSuit=CardSuit.Heart);
                NormalCard( CardValue=CardValue.Ten, CardSuit=CardSuit.Diamond);
                NormalCard( CardValue=CardValue.Jack, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Queen, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.King, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Ace, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Two, CardSuit=CardSuit.Heart); ]

        let x = lianshun cards
        Assert.Equal(7,  x)

    [<Fact>]
    let ``测试7连顺-10/J/Q/K/A/2/3`` () =

        let cards : PlayingCard list = 
            [   NormalCard( CardValue=CardValue.Ten, CardSuit=CardSuit.Diamond);
                NormalCard( CardValue=CardValue.Jack, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Queen, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.King, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Ace, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Two, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Heart); ]

        let x = lianshun cards
        Assert.Equal(0,  x)
