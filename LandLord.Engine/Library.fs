namespace Itminus.LandLord.Engine

module Facade=

    open Card
    
    let createEngine =
        ()

    let canPlay cards prevCards =
        match (cards, prevCards) with
        |( Single card1, Single card2) -> ()
        | _ -> ()
            
