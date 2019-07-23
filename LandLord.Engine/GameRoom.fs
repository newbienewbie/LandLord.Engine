namespace Itminus.LandLord.Engine

open System
open System.Collections.Generic
open Card
open System.Linq

type Player()= 
    member val ConnectionId:string = String.Empty with get, set
    member val Name:string = "" with get,set

/// a POCO that describes the meta data of GameRoom
type IGameRoomMetaData = 
    abstract member Id :Guid with get
    abstract member Players: IList<Player> with get
    abstract member LandLordIndex: int with get
    abstract member CurrentTurn: int with get
    abstract member PrevIndex: int with get
    abstract member PrevCards: IList<PlayingCard> with get
    abstract member Cards: IList<IList<PlayingCard>> with get
    abstract member ReservedCards: IList<PlayingCard> with get

type GameRoomMetaData() = 

    let mutable _id:Guid = Guid.NewGuid() 
    let mutable _landLordIndex: int = -1 
    let mutable _currentTurn: int = 0 
    let mutable _prevIndex: int = -1 
    let mutable _prevCards: IList<PlayingCard> = (new List<PlayingCard>() :> IList<_>) 
    let mutable _players: IList<Player> = (new List<Player>() :> IList<_>) 
    let mutable _cards: IList<IList<PlayingCard>> = (new List<IList<PlayingCard>>() :> IList<_>)
    let mutable _reservedCards: IList<PlayingCard> = (new List<PlayingCard>() :> IList<_>)
   

    do 
        // initialize the _cards with 3 emtpy cards list
        _cards.Add(new List<PlayingCard>())
        _cards.Add(new List<PlayingCard>())
        _cards.Add(new List<PlayingCard>())

    interface IGameRoomMetaData with
        member this.Id with get (): Guid = _id
        member this.Cards with get (): IList<IList<PlayingCard>> = _cards
        member this.CurrentTurn with get (): int = _currentTurn
        member this.LandLordIndex with get (): int = _landLordIndex
        member this.PrevCards with get(): IList<PlayingCard> = _prevCards 
        member this.PrevIndex with get (): int = _prevIndex
        member this.Players with get() = _players
        member this.ReservedCards with get() = _reservedCards

    member this.Id 
        with get() = (this:>IGameRoomMetaData).Id
        and set (v: Guid) = _id <- v
    member this.Cards 
        with get() = (this:>IGameRoomMetaData).Cards
        and set (v: IList<IList<PlayingCard>>) = _cards <- v
    member this.Players 
        with get() = (this:>IGameRoomMetaData).Players
        and set (v: IList<Player>) =  _players <- v
    member this.ReservedCards 
        with get() = (this:>IGameRoomMetaData).ReservedCards
        and internal set(v: IList<PlayingCard>) = _reservedCards <- v
    member this.CurrentTurn 
        with get() = (this:>IGameRoomMetaData).CurrentTurn
        and set (v: int) =  _currentTurn <- v
    member this.LandLordIndex 
        with get() = (this:>IGameRoomMetaData).LandLordIndex
        and set (v: int) = _landLordIndex <- v
    member this.PrevCards 
        with get() = (this:>IGameRoomMetaData).PrevCards
        and set (v: IList<PlayingCard>) = _prevCards <- v
    member this.PrevIndex 
        with get() = (this:>IGameRoomMetaData).PrevIndex
        and set (v: int) = _prevIndex <- v

type GameRoom() = 
    inherit GameRoomMetaData() 

    static member Create(id: Guid) = 
        let room = GameRoom()
        room.Id <- id
        room

    // Create, Shuffle, Deal , and leave 3 cards for the LandLord
    static member Prepare()  = 
        let room = Guid.NewGuid() |> GameRoom.Create 
        let cards = Facade.createFullCards() |>  Facade.shuffle 
        let (reserved, (cards1, cards2, cards3)) = Facade.deal cards
        // change PlayingCard list to IList<PlayingCard>
        let convertCardList (cards: PlayingCard list) : IList<PlayingCard> =
            let result = new List<PlayingCard>(cards)
            result :> IList<_>
        room.Cards.[0] <- convertCardList cards1
        room.Cards.[1] <- convertCardList cards2
        room.Cards.[2] <- convertCardList cards3
        room.ReservedCards <- (reserved |> convertCardList)
        room

    static member FromMetaData(data : IGameRoomMetaData) : GameRoom =
        let room = GameRoom.Create(data.Id)
        room.LandLordIndex <- data.LandLordIndex 
        room.CurrentTurn <- data.CurrentTurn
        room.Cards <- data.Cards
        room.Players <- data.Players
        room.ReservedCards <- data.ReservedCards
        room.PrevCards <- data.PrevCards
        room.PrevIndex <- data.PrevIndex
        room 

     member this.ExportMetaData() = 
         let origin = this :> IGameRoomMetaData
         origin   

type GameRoom with 

    member this.AddUser(player: Player) = 
        let count = this.Players.Count
        if count < 3 then
            this.Players.Add(player)
            true
        else 
            false

    member this.AppendCards(cards: IList<PlayingCard>) = 
        let originalCards = this.Cards.[this.LandLordIndex]
        for c in cards do
            originalCards.Add(c)


type GameRoom with

    member private this.playCards(nth: int , cards: IList<PlayingCard>) : bool = 
        if nth < 3 && nth >= 0 then 
            let originalCards = this.Cards.[nth]
            let remaining = originalCards.Except(cards).ToList()
            
            this.Cards.[nth] <- remaining
            this.PrevIndex <- nth
            this.PrevCards <- cards
            this.CurrentTurn <- this.CurrentTurn + 1
            true
        else 
            failwith "the nth must be an int between [0,2]"

    member this.StartPlayingCards (cards: IList<PlayingCard>) : bool= 
        let turn = 0 
        if List.ofSeq cards |> Facade.canStartPlaying then
            this.playCards(turn, cards ) 
        else
            false

    // nth player plays cards
    member this.PlayCards (nth: int, cards:IList<PlayingCard>) : bool= 
        let prevCards = this.PrevCards |> List.ofSeq
        if List.ofSeq cards |> Facade.canPlay prevCards then
            this.playCards(nth, cards)
        else
            false

    // nth player passes by
    member this.Pass(nth: int, cards:IList<PlayingCard>) : bool= 
        this.CurrentTurn <- this.CurrentTurn + 1
        true

    member this.HasWin (nth: int) : bool = 
        let cards = this.Cards.[nth]
        cards.Count = 0

