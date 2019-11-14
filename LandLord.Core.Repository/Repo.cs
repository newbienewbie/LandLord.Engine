using JsonSubTypes;
using LandLord.Core.Room;
using LandLord.Shared;
using LiteDB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LandLord.Core.Repository
{
    public class GameRoomRepository
    {
        public GameRoomRepository(string dbName, string roomCollectionName) {

            var playingCardJsonConverter = JsonSubtypesConverterBuilder
                .Of(typeof(PlayingCard), "Kind") // type property is only defined here
                .RegisterSubtype(typeof(NormalCard), PlayerCardKind.NormalCard)
                .RegisterSubtype(typeof(BlackJokerCard), PlayerCardKind.BlackJokerCard)
                .RegisterSubtype(typeof(RedJokerCard), PlayerCardKind.RedJokerCard)
                .SerializeDiscriminatorProperty() // ask to serialize the type property
                .Build();
            var playerCardJsonConverter = JsonSubtypesConverterBuilder
                .Of(typeof(PlayerCard), "Kind") // type property is only defined here
                .RegisterSubtype(typeof(NormalCard), PlayerCardKind.NormalCard)
                .RegisterSubtype(typeof(BlackJokerCard), PlayerCardKind.BlackJokerCard)
                .RegisterSubtype(typeof(RedJokerCard), PlayerCardKind.RedJokerCard)
                .RegisterSubtype(typeof(Shadowed), PlayerCardKind.Shadowed)
                .SerializeDiscriminatorProperty() // ask to serialize the type property
                .Build();

            var settings = new JsonSerializerSettings();
            settings.Converters.Add(playerCardJsonConverter);
            settings.Converters.Add(playingCardJsonConverter);

            BsonMapper.Global.RegisterType<PlayingCard>
            (
                serialize: obj => JsonConvert.SerializeObject(obj, settings),
                deserialize: bson => {
                    var josn = LiteDB.JsonSerializer.Serialize(bson.AsDocument);
                    return JsonConvert.DeserializeObject<PlayingCard>(bson, settings);
                }
            );
            BsonMapper.Global.RegisterType<PlayerCard>
            (
                serialize: obj => JsonConvert.SerializeObject(obj, settings),
                deserialize: bson => {
                    //var json = LiteDB.JsonSerializer.Serialize(bson.AsDocument.Values.FirstOrDefault());
                    return JsonConvert.DeserializeObject<PlayerCard>(bson.AsString, settings);
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
                return roomCollection.FindOne(r => r.Players.Any(p => p.Name == userId && r.HasFinished == false ));
            }
        }

    }
}
