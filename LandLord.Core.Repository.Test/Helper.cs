using LandLord.Core.Room;
using LandLord.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LandLord.Core.Repository.Test
{
    class Helper
    {
        public static bool GameRoomEqual(GameRoom room1, GameRoom room2)
        {
            Assert.Equal(room1.Id,room2.Id);
            Assert.Equal(room1.LandLordIndex, room2.LandLordIndex);
            Assert.Equal(room1.CurrentTurn, room2.CurrentTurn);
            Assert.Equal(room1.PrevIndex, room2.PrevIndex);
            Assert.Equal(room1.WinnerIndex, room2.WinnerIndex);

            Assert.True(DistinctListEqual(room1.Cards[0], room2.Cards[0]));
            Assert.True(DistinctListEqual(room1.Cards[1], room2.Cards[1]));
            Assert.True(DistinctListEqual(room1.Cards[2], room2.Cards[2]));
            Assert.True(DistinctListEqual(room1.ReservedCards, room2.ReservedCards));
            Assert.True(DistinctListEqual(room1.PrevCards, room2.PrevCards));
            return true;
        }

        internal static bool DistinctListEqual<T>(IList<T> cards1, IList<T> cards2)
            where T : PlayerCard
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

            bool IsDisctinctList<T>(IList<T> cards)
            {
                var cards2 = cards.Distinct();
                return cards2.Count() == cards.Count();
            }
        }

    }
}
