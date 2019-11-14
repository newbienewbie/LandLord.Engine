using LandLord.Shared;
using LandLord.Shared.Hub.CallbackArguments;
using System;
using System.Collections.Generic;
using System.Text;

namespace LandLord.BlazorApp.Data
{
    public class PlayCardsCallbackArgs : CallbackArgs
    {
        public int Index { get; set; }
        public IList<PlayingCard> Cards { get; set; }
    }
}
