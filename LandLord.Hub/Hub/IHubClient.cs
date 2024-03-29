﻿using LandLord.Hub;
using LandLord.Shared.Hub.CallbackArguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace LandLord.Shared.Hub
{




    public interface IGameHubClient
    {
        Task ReceiveState(GameStateDto state);
        Task AddingToRoomSucceeded(Guid roomId);
        Task RemoveFromRoomSucceeded(Guid roomId);
        Task Win(int index);
        Task ReceiveError(string msg);

        Task StartGameCallback(StartGameCallbackArgs args);
        Task BeLandLordCallback(BeLandLordCallbackArgs args);
        Task PlayCardsCallback(PlayCardsCallbackArgs args);
        Task PassCardsCallback(PassCardsCallbackArgs args);
    }
}
