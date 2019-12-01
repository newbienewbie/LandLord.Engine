using LandLord.Core.Room;
using LandLord.Shared;
using LandLord.Shared.CardJsonConverters;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LandLord.Core.Repository
{
    public class GameRoomRepository
    {
        public GameRoomRepository(string dbName, string roomCollectionName) {


            var settings = new System.Text.Json.JsonSerializerOptions();
            settings.Converters.Add(new PlayerCardJsonConverter());
            settings.Converters.Add(new PlayingCardJsonConverter());

            BsonMapper.Global.RegisterType<PlayingCard>
            (
                serialize: obj => System.Text.Json.JsonSerializer.Serialize(obj, settings),
                deserialize: bson => {
                    var obj = System.Text.Json.JsonSerializer.Deserialize<PlayingCard>(bson, settings);
                    return obj;
                }
            );
            BsonMapper.Global.RegisterType<PlayerCard>
            (
                serialize: obj => System.Text.Json.JsonSerializer.Serialize(obj, settings),
                deserialize: bson => {
                    var obj = System.Text.Json.JsonSerializer.Deserialize<PlayerCard>(bson, settings);
                    return obj;
                }
            );
            this.DbName = dbName;
            RoomCollectionName = roomCollectionName;
        }
        public string DbName { get; set; }

        public string RoomCollectionName { get; set; }

        public GameRoom Load(Guid roomId)
        {
            using (var db = new LiteDatabase(this.DbName )) { 
                var roomCollection = db.GetCollection<GameRoom>(this.RoomCollectionName);
                var id = new BsonValue(roomId);
                return roomCollection.FindById(id);
            }
        }
        public void Save(GameRoom room)
        {
            using (var db = new LiteDatabase(this.DbName )) { 
                var roomCollection = db.GetCollection<GameRoom>(this.RoomCollectionName);
                roomCollection.Upsert(room);
            }
        }

        public IEnumerable<GameRoom> FindAll()
        {
            using (var db = new LiteDatabase(this.DbName )) { 
                var roomCollection = db.GetCollection<GameRoom>(this.RoomCollectionName);
                return roomCollection.FindAll().AsEnumerable();
            }
        }
        
        public GameRoom FindAvaiableRoomByUserId(string userId)
        {
            using (var db = new LiteDatabase(this.DbName )) { 
                var roomCollection = db.GetCollection<GameRoom>(this.RoomCollectionName);
                return roomCollection.FindOne(r => r.Players.Any(p => p.Name == userId && (r.RoomState != Shared.Room.GameRoomState.GameCompleted)));
            }
        }

    }
}
