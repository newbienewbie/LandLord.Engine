using LandLord.Core.Room;
using System;
using System.Linq;
using Xunit;

namespace LandLord.Core.Repository.Test
{
    public class UnitTest1
    {


        [Fact]
        public void Test1()
        {
            var repo = new LandLord.Core.Repository.GameRoomRepository("Filename=GameRepo.Test.db;Mode=Exclusive", "gameRooms");
            var game = GameRoom.CreateAndDeal();
            repo.Save(game);

            var game2 = repo.Load(game.Id);
            for(var i= 0; i < 3; i++) { 
                Assert.True(game2.Cards[i].All(c => c != null), "card must be null");
            }
            Assert.True(Helper.GameRoomEqual(game, game2));
        }
    }
}
