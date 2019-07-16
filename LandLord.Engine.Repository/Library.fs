namespace LandLord.Engine.Repository

open LiteDB
open LiteDB.FSharp
open Itminus.LandLord.Engine
open System

type GameRoomRepository(dbName) =
    
    let mapper = FSharpBsonMapper()
    let dbName:string = dbName   
    let roomCollectionName: string = "rooms"

    member this.Load(id: Guid)=
        use db = new LiteDatabase(dbName, mapper)
        let roomCollection = db.GetCollection<GameRoom>(roomCollectionName);
        let id' = BsonValue id
        roomCollection.FindById(id')
        
    member this.Save(room: GameRoom) =
        use db = new LiteDatabase(dbName, mapper)
        let roomCollection = db.GetCollection<GameRoom>(roomCollectionName);
        roomCollection.Upsert(room)
        
    member this.FindAll () =
        use db = new LiteDatabase(dbName, mapper)
        let roomCollection = db.GetCollection<GameRoom>(roomCollectionName);
        roomCollection.FindAll()

