using LandLord.Core.Room;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using LandLord.Shared.CardJsonConverters;

namespace LandLord.Shared.Test
{
    public class Serialization
    {
        static bool IsDisctinctList<T>(IList<T> cards )
        {
            var cards2 = cards.Distinct();
            return cards2.Count() == cards.Count();
        }
        static bool DistinctListEqual<T>(IList<T> cards1, IList<T> cards2)
            where T: PlayerCard
        {
            if (!IsDisctinctList(cards1))
                return false;
            if (!IsDisctinctList(cards2))
                return false;
            if (cards1.Count != cards2.Count) return false;
            foreach (var card1 in cards1)
            {
                if (cards2.Any(c2 => c2 == card1))
                    continue;
                break;
            }
            foreach (var card2 in cards2)
            {
                if (cards1.Any(c1 => c1 == card2))
                    continue;
                break;
            }
            return true;
        }
        [Fact]
        public void TestSerializationAndDeserialization()
        {
            var room = GameRoom.CreateAndDeal();
            var settings = new JsonSerializerOptions();
            settings.Converters.Add(new PlayerCardJsonConverter());
            settings.Converters.Add(new PlayingCardJsonConverter());
            var json = JsonSerializer.Serialize(room, settings);
            var room2 = JsonSerializer.Deserialize<GameRoom>(json, settings);
            Assert.Equal(room.Id, room2.Id);
            Assert.Equal(room.WinnerIndex, room2.WinnerIndex);

            Assert.True(DistinctListEqual(room.Cards[0],room2.Cards[0]));
            Assert.True(DistinctListEqual(room.Cards[1],room2.Cards[1]));
            Assert.True(DistinctListEqual(room.Cards[2],room2.Cards[2]));
            Assert.True(DistinctListEqual(room.ReservedCards, room2.ReservedCards));
            Assert.True(DistinctListEqual(room.PrevCards, room2.PrevCards));

            Assert.Equal(room.LandLordIndex, room2.LandLordIndex);
            Assert.Equal(room.CurrentTurn, room2.CurrentTurn);
            Assert.Equal(room.PrevIndex, room2.PrevIndex);
            Assert.Equal(room.WinnerIndex, room2.WinnerIndex);
        }
    }
}
