namespace Itminus.LandLord.Engine

open System
open System.Collections.Generic
open Card
open System.Linq


type Player()= 
    member val ConnectionId:string = String.Empty with get, set
    member val Id:string = String.Empty with get,set
    member val Name:string = String.Empty with get,set
    member val StillActive = false with get,set
    member this.IsEmpty 
        with get() = 
            this.ConnectionId = String.Empty && this.Id = String.Empty && this.Name = String.Empty && this.StillActive = false

    // An empty instance
    static member Empty = 
        let p = Player()
        p.StillActive <- false
        p

/// used by GameRoom::FindPlayer()
[<AllowNullLiteral>]
type PlayerFindings(index: int, player: Player) = 
    member val Index = index with get,set
    member val Player = player with get,set

/// a POCO that describes the meta data of GameRoom

[<AllowNullLiteral>]
type IGameRoomMetaData = 
    abstract member Id :Guid with get
    abstract member Players: IList<Player> with get
    abstract member LandLordIndex: int with get
    abstract member CurrentTurn: int with get
    abstract member PrevIndex: int with get
    abstract member PrevCards: IList<PlayingCard>with get
    abstract member Cards: IList<IList<PlayerCard>> with get
    abstract member ReservedCards: IList<PlayingCard>with get
    abstract member HasFinished: bool with get

type GameRoomMetaData() = 

    let mutable _id:Guid = Guid.NewGuid() 
    let mutable _landLordIndex: int = -1 
    let mutable _currentTurn: int = 0 
    let mutable _prevIndex: int = -1 
    let mutable _prevCards: IList<PlayingCard>= (new List<PlayingCard>() :> IList<_>) 
    let mutable _players: IList<Player> = (new List<Player>() :> IList<_>) 
    let mutable _cards: IList<IList<PlayerCard>> = (new List<IList<PlayerCard>>() :> IList<_>)
    let mutable _reservedCards: IList<PlayingCard>= (new List<PlayingCard>() :> IList<_>)
    let mutable _finished: bool = false
   
    interface IGameRoomMetaData with
        member this.Id with get (): Guid = _id
        member this.Cards with get (): IList<IList<PlayerCard>> = _cards
        member this.CurrentTurn with get (): int = _currentTurn
        member this.LandLordIndex with get (): int = _landLordIndex
        member this.PrevCards with get(): IList<PlayingCard>= _prevCards 
        member this.PrevIndex with get (): int = _prevIndex
        member this.Players with get() = _players
        member this.ReservedCards with get() = _reservedCards
        member this.HasFinished with get() = _finished

    member this.Id 
        with get() = (this:>IGameRoomMetaData).Id
        and set (v: Guid) = _id <- v
    member this.Cards 
        with get() = (this:>IGameRoomMetaData).Cards
        and set (v: IList<IList<PlayerCard>>) = _cards <- v
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
    member this.HasFinished 
        with get() = (this:> IGameRoomMetaData).HasFinished
        and set(v) = _finished <- v


/// don't create GameRoom instance by `new`, use `GameRoom.Create()` instead.
/// the `new ` won't add 3 empty cards list for the `Cards` property ,
///     if so, each time the Repository deserialize a room, 
///         it will add three extra empty cards list before pushing those that are already recorded in database
type GameRoom() = 
    inherit GameRoomMetaData() 

    static member Create(id: Guid) = 
        let room = GameRoom()
        room.Id <- id
        // initialize the _cards with 3 emtpy cards list
        room.Cards.Add(new List<PlayerCard>())
        room.Cards.Add(new List<PlayerCard>())
        room.Cards.Add(new List<PlayerCard>())
        // intilize the _players with 3 empty player
        room.Players.Add(Player.Empty)
        room.Players.Add(Player.Empty)
        room.Players.Add(Player.Empty)
        room

    // Create, Shuffle, Deal , and leave 3 cards for the LandLord
    static member Prepare()  = 
        let room = Guid.NewGuid() |> GameRoom.Create 
        let cards = Facade.createFullCards() |>  Facade.shuffle 
        let (reserved, (cards1, cards2, cards3)) = Facade.deal cards
        // change PlayingCard list to IList<PlayerCard>
        let convertCardList (cards: PlayingCard list) : IList<PlayerCard>=
            let result = cards.Select(fun c -> PlayingCard c).ToList()
            result :> IList<_>
        room.Cards.[0] <- convertCardList cards1
        room.Cards.[1] <- convertCardList cards2
        room.Cards.[2] <- convertCardList cards3
        room.ReservedCards <- ((new List<PlayingCard>(reserved)) :> IList<_> )
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

    member this.AddUser(nth: int, player: Player) = 
        if nth >=0 && nth < 3 && this.Players.[nth].IsEmpty then
            this.Players.[nth] <- player
            true
        else 
            false

    member this.AddUser(player: Player) = 
        let nth = 
            this.Players
                .Select(fun p i -> (p, i))
                .Where(fun t ->
                    let p = (fst t)
                    p.IsEmpty
                )
                .Select(fun (p,i) -> i)
                .FirstOrDefault()
        this.AddUser(nth, player)

    member this.AppendCards(cards: IList<PlayingCard>) = 
        let originalCards = this.Cards.[this.LandLordIndex]
        for c in cards do
            originalCards.Add(PlayingCard c)


type GameRoom with

    member private this.playCards(nth: int , cards: IList<PlayingCard>) : bool = 
        let cards' = cards.Select(fun c -> PlayingCard c)
        if nth < 3 && nth >= 0 then 
            let originalCards = this.Cards.[nth]
            let remaining = originalCards.Except(cards').ToList()
            
            this.Cards.[nth] <- remaining
            this.PrevIndex <- nth
            this.PrevCards <- cards
            this.CurrentTurn <- (this.CurrentTurn + 1) % 3
            true
        else 
            failwith "the nth must be an int between [0,2]"


    /// the LandLord will be the first one who plays cards
    member this.StartPlayingCards (cards: IList<PlayingCard>) : bool= 
        let turn =  this.LandLordIndex
        // check whether the game has started
        if this.Cards.[turn].Count <> 20 then
            false
        else if List.ofSeq cards |> Facade.canStartPlaying then
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


type GameRoom with
    /// finds a player by connection id, 
    /// returns null if not found
    member this.FindPlayer(userId: string ): PlayerFindings = 
        let kvs = 
            this.Players
            |> Seq.mapi (fun k v -> k, v)
            |> Seq.filter (fun (i, c) -> c.Id = userId)

        if Seq.isEmpty kvs then 
            null
        else 
            let i, p = Seq.head kvs
            PlayerFindings(i, p)

    static member private ShadowCardsList(fromCardsList: IList<IList<PlayerCard>>, nth: int) = 
        if nth < 0 || nth > 2 then 
            let msg = sprintf "invalid argument of nth: %A" nth
            raise (ArgumentException(msg))

        let cardsList = List<IList<PlayerCard>>()
        cardsList.Add(List<PlayerCard>())
        cardsList.Add(List<PlayerCard>())
        cardsList.Add(List<PlayerCard>())


        // copy nth player's cards
        let fromCards= 
            // sort by weight
            fromCardsList.[nth].AsEnumerable()
            |> Seq.sortBy (fun x -> 
                match x with
                | PlayingCard c -> getWeight true c
                | Shadowed -> failwith "unsupported card type"
            )
        let toCards = cardsList.[nth]
        for c in fromCards do 
            toCards.Add(c)

        // shadow other's cards
        for i in [0..2] do
            if i <> nth then
                cardsList.[i] <- fromCardsList.[i].Select(fun c -> Shadowed).ToList()
        cardsList :> IList<IList<PlayerCard>>

    member private this.ShadowCopy() = 
        let room = GameRoom.Create(this.Id)
        room.LandLordIndex <- this.LandLordIndex 
        room.CurrentTurn <- this.CurrentTurn
        room.PrevCards <- this.PrevCards
        room.PrevIndex <- this.PrevIndex
        room.Cards <- this.Cards                
        room.Players <- this.Players             
        room.ReservedCards <- this.ReservedCards 
        room 

    /// shadow other cards for some player
    member this.ShadowCards(nth : int): IGameRoomMetaData = 
        if nth > 2 || nth < 0 then 
            let msg = sprintf "invalid nth: %A" nth 
            raise (ArgumentException(msg))
        else
            let metadata = this.ShadowCopy()
            let newCardsList = GameRoom.ShadowCardsList(this.Cards, nth)
            metadata.Cards <- newCardsList 
            // we don't change the list of players / reserverdCards
            //    because they won't be changed: this interface has no setter
            metadata :> IGameRoomMetaData
            
