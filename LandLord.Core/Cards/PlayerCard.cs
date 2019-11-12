using System;
using System.Collections.Generic;
using System.Linq;

namespace LandLord.Core
{
    
    public abstract class PlayerCard { }

    public abstract class Shadowed : PlayerCard 
    {
        public int GetWeight() => Defines.ShadowedValue;
    }

    public abstract class PlayingCard : PlayerCard , IEquatable<PlayingCard>
    {
        public bool Equals(PlayingCard other)
        {
            return this.GetWeight(true) == other.GetWeight(true);
        }

        public abstract int GetWeight(bool considerSuit) ;

    }


}
