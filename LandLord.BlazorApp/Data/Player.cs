using LandLord.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandLord.BlazorApp.Data
{
    public class Player
    {
        public string ConnectionId { get; set; } = String.Empty;
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool StillActive { get; set; } = false;
    }
    /// a POCO that describes the meta data of GameRoom
    public class PlayerTurnIndexes
    {
        public int Me { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
    }

    public class PlayerAndCardsWrapper
    {
        public int TurnIndex { get; set; }
        public Player Player { get; set; }
        public IList<PlayerCard> Cards { get; set; }

        internal static PlayerAndCardsWrapper Create(GameRoomMetaData room, int turnIndex)
        {
            return new PlayerAndCardsWrapper
            {
                TurnIndex = turnIndex,
                Player = room.Players[turnIndex],
                Cards = room.Cards[turnIndex],
            };
        }
    }

    public class RoomDesk
    {
        public PlayerAndCardsWrapper Me { get; set; }
        public PlayerAndCardsWrapper Left { get; set; }
        public PlayerAndCardsWrapper Right { get; set; }
    }
}
