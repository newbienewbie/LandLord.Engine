namespace LandLord.Core.Tests

open System
open LandLord.Core
open LandLord.Core.Patterns
open System.Linq

module TestHelper = 
    
    // get len of pattern DuoLianShun
    let lianshun (dup: int) (cards': PlayingCard list) :int = 

        let cards = List.toSeq(cards').ToList()

        let check dup len =
            let f, _ = Patterns.DuoLianShun(dup, len, cards).ToTuple()
            f

        let getLen dup = 
            let mutable len = 0
            // 三连 ~ 十二连
            for i= 12 downto 3 do
                if check dup i then 
                    len <- i
            len

        getLen dup

