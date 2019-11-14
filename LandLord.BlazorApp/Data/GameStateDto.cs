using LandLord.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace LandLord.BlazorApp.Data
{
    public class GameStateDto
    {
        /// <summary>
        /// current game room state for this particualr client
        /// </summary>
        public GameRoomMetaData GameRoom { get; set; }

        /// <summary>
        /// current player's turn index
        /// </summary>
        public int TurnIndex { get; set; }
    }


    public class GameRoomMetaData
    {
        public Guid Id { get; set; }
        public List<Player> Players { get; set; }
        public int LandLordIndex { get; set; }
        public int CurrentTurn { get; set; }
        public int PrevIndex { get; set; }
        public List<PlayingCard> ReservedCards { get; set; }
        public List<PlayingCard> PrevCards { get; set; }
        public List<List<PlayerCard>> Cards { get; set; }
        public bool HasFinished { get; set; }
        public int WinnerIndex { get; }
        public virtual PlayerTurnIndexes GetPlayerIndexesOnDesk(int meTurnIndex)
        {
            var me = meTurnIndex;
            var left = (me + 3 - 1) % 3;
            var right = (me + 3 + 1) % 3;
            return new PlayerTurnIndexes
            {
                Me = me,
                Left = left,
                Right = right,
            };
        }
        public virtual RoomDesk GetPlayersAndCardsOnDesk(int meTurnIndex)
        {
            var indexes = this.GetPlayerIndexesOnDesk(meTurnIndex);
            return new RoomDesk
            {
                Me = PlayerAndCardsWrapper.Create(this, indexes.Me),
                Left = PlayerAndCardsWrapper.Create(this, indexes.Left),
                Right = PlayerAndCardsWrapper.Create(this, indexes.Right),
            };
        }
    }
 
}
