using Itminus.LandLord.Engine;
using LandLord.Hub.CallbackArguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using static Itminus.LandLord.Engine.Card;

namespace LandLord.Hub
{

    // the game state returned to client 
    public class GameStateDto
    {
        /// <summary>
        /// current game room state for this particualr client
        /// </summary>
        public IGameRoomMetaData GameRoom { get; internal set; }

        /// <summary>
        /// current player's turn index
        /// </summary>
        public int TurnIndex { get; internal set; }
    }


    public interface IGameHubClient
    {
        Task ReceiveState(GameStateDto state);
        Task AddingToRoomSucceeded(Guid roomId);
        Task RemoveFromRoomSucceeded(Guid roomId);
        Task Win(int index);
        Task ReceiveError(string msg);

        Task PlayCardsCallback(PlayCardsCallbackArgs args);
        Task BeLandLordCallback(BeLandLordCallbackArgs args);
        Task PassCardsCallback(PassCardsCallbackArgs args);
    }
}
