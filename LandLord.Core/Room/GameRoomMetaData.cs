using LandLord.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandLord.Core.Room
{
    public class GameRoomMetaData : IGameRoomMetaData
    {
        public virtual Guid Id { get; set; } = Guid.NewGuid();
        public virtual IList<IList<PlayerCard>> Cards { get; set; } = new List<IList<PlayerCard>>();
        public virtual int CurrentTurn { get; set; } = -1;
        public virtual int LandLordIndex { get; set; } = -1;
        public virtual IList<PlayingCard> PrevCards { get; set; } = new List<PlayingCard>();
        public virtual int PrevIndex { get; set; } = -1;
        public virtual IList<Player> Players { get; set; } = new List<Player>();
        public virtual IList<PlayingCard> ReservedCards { get; set; } = new List<PlayingCard>();
        public virtual bool HasFinished { get; set; } = false;
        public virtual int WinnerIndex
        {
            get
            {
                if (this.HasFinished)
                {
                    var r = this.Cards.Select((cards, i) => (cards, i))
                        .Where((cards, i) => cards.cards.Count == 0);
                    if (r.Count() == 1)
                    {
                        var (cards, nth) = r.First();
                        return nth;
                    }
                }
                return -1;
            }
        }


    }
}
