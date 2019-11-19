using System;
using System.Collections.Generic;
using System.Text;

namespace LandLord.Shared.Room
{
    public enum GameRoomState
    {
        CreatedButHasNotStarted,
        GamePlaying,             // include both landloard has been set or not
        GameCompleted,
        TerminatedUnexpectedly,
    }
}
