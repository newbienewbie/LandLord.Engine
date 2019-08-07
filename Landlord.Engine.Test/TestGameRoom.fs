﻿module TestGameRoom

open System
open Xunit
open System.Collections.Generic
open Itminus.LandLord.Engine.Card
open Itminus.LandLord.Engine
open System.Linq


let private convertPlayerCardToPlayingCard c =
    match c with
    | Shadowed -> failwith "unknown shadowed card is not supported"
    | PlayingCard card -> card


[<Fact>]
let ``测试GameRoom Prepare() Cards 各个分组的长度`` () =
    let gameRoom = GameRoom.Prepare() 
    // every player has 17 cards
    for p in gameRoom.Cards do
        Assert.Equal(17, p.Count)

    // and reserve 3 cards for landlord
    let count2 = gameRoom.ReservedCards.Count
    Assert.Equal(3,count2)

let private getActualCardsSet (includingReserved:bool) (gameRoom: IGameRoomMetaData) =

    let cards1' = gameRoom.Cards.[0] |> Seq.map convertPlayerCardToPlayingCard |> Set.ofSeq
    let cards2' = gameRoom.Cards.[1] |> Seq.map convertPlayerCardToPlayingCard |> Set.ofSeq 
    let cards3' = gameRoom.Cards.[2] |> Seq.map convertPlayerCardToPlayingCard |> Set.ofSeq
    let reserved' = gameRoom.ReservedCards |> Set.ofSeq 
    let set= 
        cards1' 
        |> Set.union cards2' 
        |> Set.union cards3' 
    if includingReserved then 
        set |> Set.union reserved'
    else 
        set

[<Fact>]
let ``测试 Prepare() Cards的完备性和唯一性`` () =

    let fullCardsSet = Facade.createFullCards() |> Set.ofList

    let gameRoom = GameRoom.Prepare()

    Assert.Equal(17, gameRoom.Cards.[0].Count)
    Assert.Equal(17, gameRoom.Cards.[1].Count)
    Assert.Equal(17, gameRoom.Cards.[2].Count)
    Assert.Equal(3,  gameRoom.ReservedCards.Count)

    let actualCardsSet = getActualCardsSet true gameRoom 
    Assert.Equal(54, actualCardsSet.Count)

    Set.isSubset fullCardsSet actualCardsSet |> Assert.True
    Set.isSubset fullCardsSet actualCardsSet |> Assert.True


let private createTestPlayers() = 
    let createTestPlayer name connId = 
        let p =  Player()
        p.Name <- name
        p.ConnectionId <- connId
        p
    let p1 = createTestPlayer "player1" "conn1"
    let p2 = createTestPlayer "player2" "conn2"
    let p3 = createTestPlayer "player3" "conn3"
    [p1; p2; p3]

[<Fact>]
let ``test AddPlayer() normal use`` () =
    let room = GameRoom.Prepare()
    let players = createTestPlayers()
    for i in [0..2] do
        let p = players.[i]
        room.AddUser(i, p) |> Assert.True
    Assert.Equal(3, room.Players.Count)

[<Fact>]
let ``test AddPlayer() cannot add player with an index that has already had a player`` () =
    let room = GameRoom.Prepare()
    let players = createTestPlayers()
    for i in [0..2] do
        let p = players.[i]
        room.AddUser(i, p) |> Assert.True
    for i in [0..2] do
        let p = players.[i]
        room.AddUser(i, p) |> Assert.False
    Assert.Equal(3, room.Players.Count)

[<Fact>]
let ``test AddPlayer() cannot add more then 3 players`` () =
    let room = GameRoom.Prepare()
    let players = createTestPlayers()
    for i in [0..2] do
        let p = players.[i]
        room.AddUser(i, p) |> Assert.True

    room.AddUser(4, players.[0]) |> Assert.False
    Assert.Equal(3, room.Players.Count)

[<Fact>]
let ``test FindPlayer(connId)`` () =
    let room = GameRoom.Prepare()
    let players = createTestPlayers()
    for i in [0..2] do
        let p = players.[i]
        room.AddUser(i, p) |> Assert.True
    Assert.Equal(3, room.Players.Count)

    // test: find an existing player 
    for i in [0..2] do
        let p = players.[i];
        let findings = room.FindPlayer(p.Name)
        Assert.NotNull findings
        findings.Player = p |> Assert.True
        findings.Index = i |> Assert.True

    // test: find an non-existing player
    let connId = Guid.NewGuid().ToString();
    let findings = room.FindPlayer(connId)
    Assert.Null findings


let private TestShadowCards(rawCards:IList<IList<PlayerCard>>, shadowedCards: IList<IList<PlayerCard>>, nth) =  
    for i in [0..2] do
        let rawCards' = rawCards.[i] 
        let shadowedCards' = shadowedCards.[i]
        if i <> nth then 
            // should have exactly the same length
            Assert.Equal(rawCards'.Count, shadowedCards'.Count)
            // each one should be Shadowed
            shadowedCards' 
            |> Seq.iter (fun c -> Assert.Equal(c, Shadowed) )
        else 
            Assert.Equal(rawCards'.Count, shadowedCards'.Count )
            let sortedRawCards' = 
                rawCards'.AsEnumerable() 
                |> Seq.sortBy(fun x -> 
                    match x with 
                    | PlayingCard c -> Card.getWeight true c 
                    | Shadowed -> failwith "unsupported card type"
                )
            Seq.zip sortedRawCards' shadowedCards'
            |> Seq.iter (fun z ->
                let (rc, sc) = z
                Assert.Equal(rc, sc)
            )
    
[<Fact>]
let ``test ShadowCards(connId)`` () =
    let room = GameRoom.Prepare()
    let players = createTestPlayers()

    for i in [0..2] do
        let p = players.[i];
        room.AddUser(i, p) |> Assert.True
    Assert.Equal(3, room.Players.Count)

    for p in room.Players do
        let findings = room.FindPlayer(p.Name);
        Assert.NotNull findings
        let shadowedRoom = room.ShadowCards(findings.Index);
        TestShadowCards(room.Cards, shadowedRoom.Cards , findings.Index)

let private createRoomAndAddUserAndSetLandLord landlordIndex = 
    let room = GameRoom.Prepare()
    let players = createTestPlayers()

    for i in [0..2] do
        let p = players.[i];
        room.AddUser(i, p) |> Assert.True
    room.LandLordIndex <- landlordIndex
    room.AppendCards room.ReservedCards
    room 

[<Fact>]
let ``test AppendCards`` () =
    let landlordIndex = 2
    let room = createRoomAndAddUserAndSetLandLord landlordIndex
    // landlord index
    Assert.Equal(landlordIndex, room.LandLordIndex)
    // reserved cards count
    Assert.Equal(3, room.ReservedCards.Count)
    // cards count
    for index in [0..2] do
        if index <> landlordIndex then 
            Assert.Equal(17, room.Cards.[index].Count)
        else 
            Assert.Equal(20, room.Cards.[landlordIndex].Count)
    // players count
    Assert.Equal(3, room.Players.Count)

    let actualCardsSet = getActualCardsSet false room
    Assert.Equal(54, actualCardsSet.Count)
    let fullCardsSet = Facade.createFullCards() |> Set.ofList
    Set.isSubset fullCardsSet actualCardsSet |> Assert.True
    Set.isSubset fullCardsSet actualCardsSet |> Assert.True


[<Fact>]
let ``test StartPlayingCards()`` () =
    for landlordIndex in [0..2] do
        let room = createRoomAndAddUserAndSetLandLord landlordIndex
        let cards = room.Cards
        let playingCards = cards.[landlordIndex].Select(convertPlayerCardToPlayingCard).Take(1).ToList()
        room.StartPlayingCards(playingCards) |> Assert.True
        let empty = cards.[landlordIndex].Select(convertPlayerCardToPlayingCard).Intersect(playingCards)
        Assert.Equal(0, empty.Count())

