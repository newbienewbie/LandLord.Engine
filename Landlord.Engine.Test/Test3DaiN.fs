﻿module Test3DaiN

     
 open System
 open Xunit
 open System.Collections.Generic
 open Itminus.LandLord.Engine.Card
 open Itminus.LandLord.Engine.SanDaiN


    [<Fact>]
    let ``测试三带一`` () =

        let cards =
            [   NormalCard( CardValue.Three, Heart);
                NormalCard( CardValue.Three, Diamond);
                NormalCard( CardValue.Three, Club);
                NormalCard( CardValue.Seven, Diamond); ] 

        let x = match cards with 
            | SanDai1(list) -> "3+1"
            | _ -> "Empty"

        Assert.Equal("3+1", x)
        

