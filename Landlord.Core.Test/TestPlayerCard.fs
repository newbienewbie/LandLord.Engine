namespace LandLord.Core.Tests

module TestsPlayerCard = 

    open System
    open Xunit
    open System.Collections.Generic
    open LandLord.Core
    open System.Linq


    [<Theory>]
    [<InlineDataAttribute(true)>]
    let ``测试JokerCard::GetWeight()`` (considerSuit:bool) =
        let blackJoker= BlackJokerCard()
        let redJoker = RedJokerCard()
        Assert.True(blackJoker.GetWeight(considerSuit) < redJoker.GetWeight(considerSuit))
        Assert.Equal(blackJoker.GetWeight(false), redJoker.GetWeight(false))

    [<Theory>]
    [<InlineDataAttribute(true)>]
    [<InlineDataAttribute(false)>]
    let ``测试NormalCard::GetWeight`` (considerSuit: bool) =

        let three = NormalCard(CardValue= CardValue.Three, CardSuit = CardSuit.Diamond);
        let four = NormalCard(CardValue= CardValue.Four, CardSuit = CardSuit.Diamond);
        let five= NormalCard(CardValue= CardValue.Five, CardSuit = CardSuit.Diamond);
        let six= NormalCard(CardValue= CardValue.Six, CardSuit = CardSuit.Diamond);
        let seven= NormalCard(CardValue= CardValue.Seven, CardSuit = CardSuit.Diamond);
        let eight= NormalCard(CardValue= CardValue.Eight, CardSuit = CardSuit.Diamond);
        let nine= NormalCard(CardValue= CardValue.Nine, CardSuit = CardSuit.Diamond);
        let ten = NormalCard(CardValue= CardValue.Ten, CardSuit = CardSuit.Diamond);
        let jack= NormalCard(CardValue= CardValue.Jack, CardSuit = CardSuit.Diamond);
        let queen= NormalCard(CardValue= CardValue.Queen, CardSuit = CardSuit.Diamond);
        let king = NormalCard(CardValue= CardValue.King, CardSuit = CardSuit.Diamond);
        let ace = NormalCard(CardValue= CardValue.Ace, CardSuit = CardSuit.Diamond);
        let two = NormalCard(CardValue= CardValue.Two, CardSuit = CardSuit.Diamond);
        Assert.True(three.GetWeight(considerSuit) <  four.GetWeight(considerSuit))
        Assert.True(four.GetWeight(considerSuit) <  five.GetWeight(considerSuit))
        Assert.True(six.GetWeight(considerSuit) <  seven.GetWeight(considerSuit))
        Assert.True(seven.GetWeight(considerSuit) <  eight.GetWeight(considerSuit))
        Assert.True(eight.GetWeight(considerSuit) <  nine.GetWeight(considerSuit))
        Assert.True(nine.GetWeight(considerSuit) <  ten.GetWeight(considerSuit))
        Assert.True(ten.GetWeight(considerSuit) <  jack.GetWeight(considerSuit))
        Assert.True(jack.GetWeight(considerSuit) <  queen.GetWeight(considerSuit))
        Assert.True(queen.GetWeight(considerSuit) <  king.GetWeight(considerSuit))
        Assert.True(king.GetWeight(considerSuit) <  ace.GetWeight(considerSuit))
        Assert.True(ace.GetWeight(considerSuit) <  two.GetWeight(considerSuit))

    [<Theory>]
    [<InlineDataAttribute(true)>]
    [<InlineDataAttribute(false)>]
    let ``测试JokerCard_NormalCard_GetWeight`` (considerSuit: bool) =

        let two = NormalCard(CardValue= CardValue.Two, CardSuit = CardSuit.Diamond);
        let blackJoker= BlackJokerCard()
        let redJoker = RedJokerCard()
        Assert.True(blackJoker.GetWeight(considerSuit) > two.GetWeight(true))
        Assert.True(blackJoker.GetWeight(considerSuit) > two.GetWeight(false))
        Assert.True(redJoker.GetWeight(considerSuit) > two.GetWeight(true))
        Assert.True(redJoker.GetWeight(considerSuit) > two.GetWeight(false))
