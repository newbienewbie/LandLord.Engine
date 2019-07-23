namespace LandLord.Engine.Repository

open LiteDB
open LiteDB.FSharp
open Itminus.LandLord.Engine
open System
open System.Linq;

type GameRoomRepository(dbName) =
    
    let mapper = FSharpBsonMapper()
    let dbName:string = dbName   
    let roomCollectionName: string = "rooms"

    member this.Load(roomId: Guid)=
        use db = new LiteDatabase(dbName, mapper)
        let roomCollection = db.GetCollection<GameRoom>(roomCollectionName);
        let id' = BsonValue roomId
        roomCollection.FindById(id')
        
    member this.Save(room: GameRoom) =
        use db = new LiteDatabase(dbName, mapper)
        let roomCollection = db.GetCollection<GameRoom>(roomCollectionName);
        roomCollection.Upsert(room)
        
    member this.FindAll () =
        use db = new LiteDatabase(dbName, mapper)
        let roomCollection = db.GetCollection<GameRoom>(roomCollectionName);
        roomCollection.FindAll()

    member this.FindRoomByConnectionId(connId: string) =
        use db = new LiteDatabase(dbName, mapper)
        let roomCollection = db.GetCollection<GameRoom>(roomCollectionName);
        roomCollection.FindOne( fun r -> r.Players.Any(fun p -> p.ConnectionId = connId ));

