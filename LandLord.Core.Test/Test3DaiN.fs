namespace LandLord.Core.Tests
module Test3DaiN=
     
    open System
    open Xunit
    open System.Collections.Generic
    open LandLord.Core
    open LandLord.Core.Patterns
    open System.Linq

    [<Fact>]
    let ``测试三带一3337`` () =

        let cards: PlayingCard list =
            [   NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Heart);
                NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Diamond);
                NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Club);
                NormalCard( CardValue=CardValue.Seven, CardSuit=CardSuit.Diamond); ] 

        let x, _ = Patterns.ThreeWithOne(cards.ToList() :> IList<_>).ToTuple()
        Assert.True(x, "3337 should be a 3+1")
                
    [<Fact>]
    let ``测试三带一3733`` () =

        let cards: PlayingCard list =
            [   NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Heart);
                NormalCard( CardValue=CardValue.Seven, CardSuit=CardSuit.Diamond); 
                NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Diamond);
                NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Club); ] 

        let x, _ = Patterns.ThreeWithOne(cards.ToList() :> IList<_>).ToTuple()
        Assert.True(x, "3337 should be a 3+1")

    [<Fact>]
    let ``测试三带一3773`` () =

        let cards: PlayingCard list =
            [   NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Heart);
                NormalCard( CardValue=CardValue.Seven, CardSuit=CardSuit.Diamond); 
                NormalCard( CardValue=CardValue.Seven, CardSuit=CardSuit.Club); 
                NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Club); ] 

        let x, _ = Patterns.ThreeWithOne(cards.ToList() :> IList<_>).ToTuple()
        Assert.False(x, "3773 is not a 3+1")

    [<Fact>]
    let ``测试三带一3333`` () =
        let cards: PlayingCard list =
            [   NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Heart);
                NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Diamond);
                NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Club);
                NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Club);  ]

        let x, _ = Patterns.ThreeWithOne(cards.ToList() :> IList<_>).ToTuple()
        Assert.False(x,"it should be a bomb")
                

