namespace LandLord.Core.Tests
module TestFeiJi = 

    open System
    open Xunit
    open System.Collections.Generic
    open LandLord.Core
    open LandLord.Shared
    open LandLord.Core.Room

    let private lianshun3 (cards: PlayingCard list) = 
        TestHelper.lianshun 3 cards

    [<Fact>]
    let ``测试5连飞机-顺序`` () =

        let cards : PlayingCard list = 
            [   NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Heart);
                NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Diamond);
                NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Club);
                NormalCard( CardValue=CardValue.Four, CardSuit=CardSuit.Heart);
                NormalCard( CardValue=CardValue.Four, CardSuit=CardSuit.Club);
                NormalCard( CardValue=CardValue.Four, CardSuit=CardSuit.Diamond);
                NormalCard( CardValue=CardValue.Five, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Five, CardSuit=CardSuit.Club); 
                NormalCard( CardValue=CardValue.Five, CardSuit=CardSuit.Diamond); 
                NormalCard( CardValue=CardValue.Six, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Six, CardSuit=CardSuit.Club); 
                NormalCard( CardValue=CardValue.Six, CardSuit=CardSuit.Diamond); 
                NormalCard( CardValue=CardValue.Seven, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Seven, CardSuit=CardSuit.Club); 
                NormalCard( CardValue=CardValue.Seven, CardSuit=CardSuit.Diamond); ]

        let x = lianshun3 cards
        Assert.Equal(5,  x)


    [<Fact>]
    let ``测试5连飞机-乱序`` () =

        let cards : PlayingCard list = 
            [   NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Heart);
                NormalCard( CardValue=CardValue.Four, CardSuit=CardSuit.Diamond);
                NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Club);
                NormalCard( CardValue=CardValue.Four, CardSuit=CardSuit.Club);
                NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Diamond);
                NormalCard( CardValue=CardValue.Four, CardSuit=CardSuit.Heart);
                NormalCard( CardValue=CardValue.Five, CardSuit=CardSuit.Diamond); 
                NormalCard( CardValue=CardValue.Five, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Five, CardSuit=CardSuit.Club); 
                NormalCard( CardValue=CardValue.Six, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Six, CardSuit=CardSuit.Club); 
                NormalCard( CardValue=CardValue.Seven, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Seven, CardSuit=CardSuit.Club); 
                NormalCard( CardValue=CardValue.Six, CardSuit=CardSuit.Diamond); 
                NormalCard( CardValue=CardValue.Seven, CardSuit=CardSuit.Diamond); ]

        let x = lianshun3 cards
        Assert.Equal(5,  x)

    [<Fact>]
    let ``测试5连飞机-不含A-2-3-4-5`` () =
        let cards : PlayingCard list = 
            [   NormalCard( CardValue=CardValue.Ace, CardSuit=CardSuit.Heart);
                NormalCard( CardValue=CardValue.Ace, CardSuit=CardSuit.Diamond);
                NormalCard( CardValue=CardValue.Ace, CardSuit=CardSuit.Club);
                NormalCard( CardValue=CardValue.Two, CardSuit=CardSuit.Diamond); 
                NormalCard( CardValue=CardValue.Two, CardSuit=CardSuit.Club); 
                NormalCard( CardValue=CardValue.Two, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Club); 
                NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Diamond); 
                NormalCard( CardValue=CardValue.Four, CardSuit=CardSuit.Diamond);
                NormalCard( CardValue=CardValue.Four, CardSuit=CardSuit.Club);
                NormalCard( CardValue=CardValue.Four, CardSuit=CardSuit.Heart);
                NormalCard( CardValue=CardValue.Five, CardSuit=CardSuit.Diamond); 
                NormalCard( CardValue=CardValue.Five, CardSuit=CardSuit.Club); 
                NormalCard( CardValue=CardValue.Five, CardSuit=CardSuit.Heart); ]

        let x = lianshun3 cards
        Assert.Equal(0,  x)

    [<Fact>]
    let ``测试6连飞机-3/3/3/4/4/4/5/5/5/6/6/6/7/7/7/8/8/8`` () =

        let cards : PlayingCard list = 
            [   NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Heart);
                NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Diamond);
                NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Club);
                NormalCard( CardValue=CardValue.Four, CardSuit=CardSuit.Diamond);
                NormalCard( CardValue=CardValue.Four, CardSuit=CardSuit.Club);
                NormalCard( CardValue=CardValue.Four, CardSuit=CardSuit.Heart);
                NormalCard( CardValue=CardValue.Five, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Five, CardSuit=CardSuit.Diamond); 
                NormalCard( CardValue=CardValue.Five, CardSuit=CardSuit.Club); 
                NormalCard( CardValue=CardValue.Six, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Six, CardSuit=CardSuit.Club); 
                NormalCard( CardValue=CardValue.Six, CardSuit=CardSuit.Diamond); 
                NormalCard( CardValue=CardValue.Seven, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Seven, CardSuit=CardSuit.Club); 
                NormalCard( CardValue=CardValue.Seven, CardSuit=CardSuit.Diamond); 
                NormalCard( CardValue=CardValue.Eight, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Eight, CardSuit=CardSuit.Club); 
                NormalCard( CardValue=CardValue.Eight, CardSuit=CardSuit.Diamond); ]

        let x = lianshun3 cards
        Assert.Equal(6, x)

    [<Fact>]
    let ``测试6连飞机-3/3/3/4/4/4/5/6/7/7/8/8`` () =

        let cards : PlayingCard list = 
            [   NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Heart);
                NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Diamond);
                NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Club);
                NormalCard( CardValue=CardValue.Four, CardSuit=CardSuit.Diamond);
                NormalCard( CardValue=CardValue.Four, CardSuit=CardSuit.Club);
                NormalCard( CardValue=CardValue.Four, CardSuit=CardSuit.Heart);
                NormalCard( CardValue=CardValue.Five, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Six, CardSuit=CardSuit.Diamond); 
                NormalCard( CardValue=CardValue.Seven, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Seven, CardSuit=CardSuit.Diamond); 
                NormalCard( CardValue=CardValue.Eight, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Eight, CardSuit=CardSuit.Diamond); ]

        let x = lianshun3 cards
        Assert.Equal(0,  x)

    [<Fact>]
    let ``测试6连飞机- 9/9/10/10/J/J/Q/Q/K/K/A/A`` () =

        let cards : PlayingCard list = 
            [   NormalCard( CardValue=CardValue.Nine, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Nine, CardSuit=CardSuit.Club); 
                NormalCard( CardValue=CardValue.Nine, CardSuit=CardSuit.Diamond); 
                NormalCard( CardValue=CardValue.Ten, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Ten, CardSuit=CardSuit.Club); 
                NormalCard( CardValue=CardValue.Ten, CardSuit=CardSuit.Diamond  ); 
                NormalCard( CardValue=CardValue.Jack, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Jack, CardSuit=CardSuit.Club); 
                NormalCard( CardValue=CardValue.Jack, CardSuit=CardSuit.Club); 
                NormalCard( CardValue=CardValue.Queen, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Queen, CardSuit=CardSuit.Diamond); 
                NormalCard( CardValue=CardValue.Queen, CardSuit=CardSuit.Club); 
                NormalCard( CardValue=CardValue.King, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.King, CardSuit=CardSuit.Diamond); 
                NormalCard( CardValue=CardValue.King, CardSuit=CardSuit.Club); 
                NormalCard( CardValue=CardValue.Ace, CardSuit=CardSuit.Heart);
                NormalCard( CardValue=CardValue.Ace, CardSuit=CardSuit.Diamond);
                NormalCard( CardValue=CardValue.Ace, CardSuit=CardSuit.Heart); ]

        let x = lianshun3 cards
        Assert.Equal(6, x)

    [<Fact>]
    let ``测试6连飞机- 10/10/10/J/J/J/Q/Q/Q/K/K/K/A/A/A/2/2/2`` () =

        let cards : PlayingCard list = 
            [   NormalCard( CardValue=CardValue.Ten, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Ten, CardSuit=CardSuit.Club); 
                NormalCard( CardValue=CardValue.Ten, CardSuit=CardSuit.Diamond); 
                NormalCard( CardValue=CardValue.Jack, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Jack, CardSuit=CardSuit.Club); 
                NormalCard( CardValue=CardValue.Jack, CardSuit=CardSuit.Diamond); 
                NormalCard( CardValue=CardValue.Queen, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Queen, CardSuit=CardSuit.Club); 
                NormalCard( CardValue=CardValue.Queen, CardSuit=CardSuit.Diamond); 
                NormalCard( CardValue=CardValue.King, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.King, CardSuit=CardSuit.Diamond); 
                NormalCard( CardValue=CardValue.King, CardSuit=CardSuit.Club); 
                NormalCard( CardValue=CardValue.Ace, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Ace, CardSuit=CardSuit.Diamond); 
                NormalCard( CardValue=CardValue.Ace, CardSuit=CardSuit.Club); 
                NormalCard( CardValue=CardValue.Two, CardSuit=CardSuit.Heart); 
                NormalCard( CardValue=CardValue.Two, CardSuit=CardSuit.Club); 
                NormalCard( CardValue=CardValue.Two, CardSuit=CardSuit.Club); ]

        let x = lianshun3 cards
        Assert.Equal(6, x)

