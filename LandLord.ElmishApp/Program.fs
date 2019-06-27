// Learn more about F# at http://fsharp.org

open System
open Elmish
open Itminus.LandLord.Engine.Card
open Itminus.LandLord.Engine
open System.Collections.Generic

type Player = 
   {
       name: string;
       cards: PlayingCard list;
   }

type Model = 
    {
        players: Player list;
        prevTurn: int;
        prevCards: PlayingCard list;
    }

type Msg = 
    | AddUser of Player
    | AddUserSucceed of Player
    | AddUserFail of Player
    | Shuffle of PlayingCard list * PlayingCard list * PlayingCard list
    | StartPlayingCard of PlayingCard list
    | PlayCard of int * PlayingCard list
    | PlayCardSucceed of int * PlayingCard list
    | PlaycardFail of int * PlayingCard list
    | Win 


let init () =
    let cards1 = 
        [ 
            NormalCard(CardValue.Ace, Suit.Diamond); 
            NormalCard(CardValue.Ace, Suit.Club); 
            NormalCard(CardValue.Ace, Suit.Heart); 
            NormalCard(CardValue.Ace, Suit.Spade); 
            NormalCard(CardValue.Three, Suit.Spade); 
            NormalCard(CardValue.Three, Suit.Club); 
            NormalCard(CardValue.Two, Suit.Spade); 
            NormalCard(CardValue.King, Suit.Spade); 
        ]
    let cards2 = 
        [ 
            NormalCard(CardValue.Ten, Suit.Diamond); 
            NormalCard(CardValue.Ten, Suit.Club); 
            NormalCard(CardValue.Ten, Suit.Heart); 
            NormalCard(CardValue.Ten, Suit.Spade); 
            NormalCard(CardValue.Three, Suit.Diamond); 
            NormalCard(CardValue.Three, Suit.Heart); 
            NormalCard(CardValue.Two, Suit.Club); 
            NormalCard(CardValue.Queen, Suit.Heart); 
        ]

    let cards3 = 
        [ 
            NormalCard(CardValue.Jack, Suit.Heart); 
        ]

    {
        players = [];
        prevTurn = -1;
        prevCards = [];
    },
    Cmd.batch 
        [ AddUser({ name="player1"; cards =[] }) |> Cmd.ofMsg ;
          AddUser({ name="player2"; cards =[] }) |> Cmd.ofMsg ;
          AddUser({ name="player3"; cards =[] }) |> Cmd.ofMsg ;
          Shuffle(cards1, cards2, cards3) |> Cmd.ofMsg ; 
          StartPlayingCard([NormalCard(CardValue.King, Suit.Spade)]) |> Cmd.ofMsg 
        ]

let update msg state = 
    match msg with
    | AddUser player ->
        if List.length state.players < 4 then 
            { state with players = state.players @[player] },
            AddUserSucceed player |> Cmd.ofMsg
        else 
            state, AddUserFail player |> Cmd.ofMsg
    | Shuffle (cards1, cards2, cards3) ->
        let players= 
            List.zip state.players [cards1; cards2; cards3]
            |> List.map (fun z -> 
                let player, cards = z
                {player with cards= cards; }
            )
        { 
           state with players = players ; 
        },
        Cmd.none
    | StartPlayingCard cards ->
        let turn = 0
        let _players = 
            let currentPlayer = state.players.[turn]
            let currentPlayerRawCards = currentPlayer.cards
            let remainingCards = 
                (Set.ofList currentPlayerRawCards) - (Set.ofList cards)
                |> List.ofSeq
                |> sort
            state.players 
            |> List.mapi (fun i v -> 
                if i = turn then { v with cards = remainingCards} else v
            )
        { state with prevTurn = 0; players = _players },
        PlayCardSucceed(turn,cards) |> Cmd.ofMsg
    | PlayCard (turn, cards) ->
        match Facade.canPlay (state.prevCards) cards with
        | true ->
            let _players = 
                let currentPlayer = state.players.[turn]
                let currentPlayerRawCards = currentPlayer.cards
                let newCards = 
                    (Set.ofList currentPlayerRawCards) - (Set.ofList cards)
                    |> List.ofSeq
                    |> sort
                state.players 
                |> List.mapi (fun i v -> 
                    if i = turn then { v with cards = newCards} else v
                )
            { state with prevTurn = state.prevTurn + 1; players = _players },
            PlayCardSucceed(turn,cards) |> Cmd.ofMsg
        | false -> 
            state, 
            PlaycardFail(turn,cards) |> Cmd.ofMsg
    | Win -> state, Cmd.none
    | _ -> state,Cmd.none


let view (model:Model) dispatch =  
    printfn "***********************"
    printf "%A\n" model
    printfn "***********************"


[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"
    let program = Program.mkProgram init update view
    program
    |> Program.run
    0 // return an integer exit code
