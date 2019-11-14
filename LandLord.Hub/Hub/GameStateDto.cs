using LandLord.Core.Room;
using System;
using System.Collections.Generic;
using System.Text;

namespace LandLord.Hub
{
    /// the game state returned to client 
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
}
