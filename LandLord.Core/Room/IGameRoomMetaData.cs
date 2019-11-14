using LandLord.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace LandLord.Core.Room
{
    public interface IGameRoomMetaData
    {
        Guid Id { get; set; }
        IList<Player> Players { get; set; }
        int LandLordIndex { get; set; }
        int CurrentTurn { get; set; }
        int PrevIndex { get; set; }
        IList<PlayingCard> PrevCards { get; set; }
        IList<IList<PlayerCard>> Cards { get; set; }
        IList<PlayingCard> ReservedCards { get; set; }
        bool HasFinished { get; set; }
        int WinnerIndex { get; }

    }



}
