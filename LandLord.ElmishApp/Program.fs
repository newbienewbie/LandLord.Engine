// Learn more about F# at http://fsharp.org

open System
open Elmish
open Itminus.LandLord.Engine.Card
open Itminus.LandLord.Engine
open System.Collections.Generic

type Player = 
   {
       name: string;
   }

type Model = 
    {
        players: Player list;
        cards: (PlayingCard list) list;
        prevTurn: int;
        prevCards: PlayingCard list;
    }

type Msg = 
    | AddUser of Player
    | AddUserSucceed of Player
    | AddUserFail of Player
    | Deal of PlayingCard list * PlayingCard list * PlayingCard list
    | AppendCards of int * PlayingCard list
    | StartPlayingCard of PlayingCard list
    | PlayCard of int * PlayingCard list
    | PlayCardSucceed of int * PlayingCard list
    | PlaycardFail of int * PlayingCard list
    | Win of int


let init () =

    let nth = 2  // 0,1,2 : the landloard

    let cards = Facade.createFullCards() |> Facade.shuffle
    let (reserved, (cards1, cards2, cards3)) = Facade.deal cards

    {
        players = [];
        cards = [];
        prevTurn = -1;
        prevCards = [];
    },
    Cmd.batch 
        [ AddUser({ name="player1" }) |> Cmd.ofMsg;
          AddUser({ name="player2" }) |> Cmd.ofMsg;
          AddUser({ name="player3" }) |> Cmd.ofMsg;
          Deal(cards1, cards2, cards3) |> Cmd.ofMsg; 
          AppendCards(nth, reserved) |> Cmd.ofMsg;
          StartPlayingCard([NormalCard(CardValue.King, Suit.Spade)]) |> Cmd.ofMsg 
        ]

let update msg state = 
    let addCardsForNth nth (cards: PlayingCard list) = 
        let cards = 
            state.cards
            |> List.mapi (fun i v -> if i = nth then v @ cards else v )
        { 
           state with cards = cards; 
        }

    let _playCardsNth nth (cards: PlayingCard list) = 
        let cards = 
            state.cards
            |> List.mapi (fun i v -> 
                let remainingCards =
                    Set.ofList state.cards.[nth] - Set.ofList cards
                    |> Set.toList
                if i = nth then remainingCards else v 
            )
        { 
           state with cards = cards; prevTurn = nth;
        }

    match msg with
    | AddUser player ->
        if List.length state.players < 3 then 
            { state with players = state.players @[player] },
            AddUserSucceed player |> Cmd.ofMsg
        else 
            state, AddUserFail player |> Cmd.ofMsg
    | Deal (cards1, cards2, cards3) ->
        { 
           state with cards = [cards1; cards2; cards3]; 
        },
        Cmd.none
    | AppendCards (nth, cards) ->
        addCardsForNth nth cards,
        Cmd.none
    | StartPlayingCard cards ->
        let turn = 0
        match Facade.canPlay (state.prevCards) cards with
        | true ->
            _playCardsNth turn cards,
            PlayCardSucceed(turn,cards) |> Cmd.ofMsg
        | false -> 
            state, 
            PlaycardFail(turn,cards) |> Cmd.ofMsg
    | PlayCard (turn, cards) ->
        match Facade.canPlay (state.prevCards) cards with
        | true ->
            _playCardsNth turn cards,
            PlayCardSucceed(turn,cards) |> Cmd.ofMsg
        | false -> 
            state, 
            PlaycardFail(turn,cards) |> Cmd.ofMsg
    | PlayCardSucceed (nth, cards) ->
        let remainings = state.cards.[nth] |> List.length 
        if remainings = 0 then 
            state,
            Win nth |> Cmd.ofMsg 
        else
            state, Cmd.none
    | Win nth -> state, Cmd.none
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
