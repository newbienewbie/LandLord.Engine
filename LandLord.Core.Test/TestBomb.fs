namespace LandLord.Core.Tests
module TestBomb=

    open System
    open Xunit
    open System.Collections.Generic
    open LandLord.Core
    open LandLord.Core.Patterns
    open LandLord.Shared
    open LandLord.Core.Room
    open System.Linq


    [<Fact>]
    let ``测试Bomb 3337`` () =

        let cards : PlayingCard list =
            [   NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Heart);
                NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Diamond);
                NormalCard( CardValue=CardValue.Three, CardSuit=CardSuit.Club);
                NormalCard( CardValue=CardValue.Seven, CardSuit=CardSuit.Diamond); ] 

        let x, _ = Patterns.Bomb(cards.ToList() :> IList<_>).ToTuple()
        Assert.Equal(false , x)

        
    [<Fact>]
    let ``测试Bomb 2222`` () =

        let cards: PlayingCard list =
            [   NormalCard( CardValue=CardValue.Two, CardSuit=CardSuit.Heart);
                NormalCard( CardValue=CardValue.Two, CardSuit=CardSuit.Diamond);
                NormalCard( CardValue=CardValue.Two, CardSuit=CardSuit.Club);
                NormalCard( CardValue=CardValue.Two, CardSuit=CardSuit.Spade); ] 
        let x, _ = Patterns.Bomb(cards.ToList() :> IList<_>).ToTuple()
        Assert.Equal(true, x)

    [<Fact>]
    let ``测试Bomb 大鬼小鬼`` () =

        let cards: PlayingCard list = [ BlackJokerCard(); RedJokerCard(); ] 
        let x, _ = Patterns.Bomb(cards.ToList() :> IList<_>).ToTuple()
        Assert.Equal(true, x)

    [<Fact>]
    let ``测试Bomb 大鬼大鬼 同色-红色`` () =
        let cards: PlayingCard list = [ RedJokerCard(); RedJokerCard(); ] 
        let x, _ = Patterns.Bomb(cards.ToList() :> IList<_>).ToTuple()
        Assert.Equal(false, x)

    [<Fact>]
    let ``测试Bomb 小鬼小鬼 同色-黑色`` () =
        let cards: PlayingCard list = [ BlackJokerCard(); BlackJokerCard(); ] 
        let x, _ = Patterns.Bomb(cards.ToList() :> IList<_>).ToTuple()
        Assert.Equal(false, x)

