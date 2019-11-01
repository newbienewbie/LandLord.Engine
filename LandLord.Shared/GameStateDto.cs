﻿using Itminus.LandLord.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace LandLord.Shared
{

    /// the game state returned to client 
    public class GameStateDto
    {
        /// <summary>
        /// current game room state for this particualr client
        /// </summary>
        public IGameRoomMetaData GameRoom { get; set; }

        /// <summary>
        /// current player's turn index
        /// </summary>
        public int TurnIndex { get; set; }
    }
}
