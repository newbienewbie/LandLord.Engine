module TestBomb

open System
open Xunit
open System.Collections.Generic
open Itminus.LandLord.Engine.Card
open Itminus.LandLord.Engine.Bomb
     

[<Fact>]
let ``测试Bomb 3337`` () =

    let cards =
        [   NormalCard( CardValue.Three, Suit.Heart);
            NormalCard( CardValue.Three, Suit.Diamond);
            NormalCard( CardValue.Three, Suit.Club);
            NormalCard( CardValue.Seven, Suit.Diamond); ] 

    let x = match cards with 
        | Bomb(list) -> true
        | _ -> false 

    Assert.Equal(false , x)
    
[<Fact>]
let ``测试Bomb 2222`` () =

    let cards =
        [   NormalCard( CardValue.Two, Suit.Heart);
            NormalCard( CardValue.Two, Suit.Diamond);
            NormalCard( CardValue.Two, Suit.Club);
            NormalCard( CardValue.Two, Suit.Spade); ] 

    let x = match cards with 
        |Bomb(list) -> true
        |_ -> false 

    Assert.Equal(true, x)

[<Fact>]
let ``测试Bomb 大鬼小鬼`` () =

    let cards =
        [   Joker(JokerType.Red);
            Joker(JokerType.Black) ] 

    let x = match cards with 
        |Bomb(list) -> true
        |_ -> false 

    Assert.Equal(true, x)

[<Fact>]
let ``测试Bomb 大鬼大鬼`` () =

    let cards =
        [   Joker(JokerType.Red);
            Joker(JokerType.Red) ] 

    let x = match cards with 
        |Bomb(list) -> true
        |_ -> false 

    Assert.Equal(false, x)

[<Fact>]
let ``测试Bomb 小鬼小鬼`` () =

    let cards =
        [   Joker(JokerType.Black);
            Joker(JokerType.Black) ] 

    let x = match cards with 
        |Bomb(list) -> true
        |_ -> false 

    Assert.Equal(false, x)

