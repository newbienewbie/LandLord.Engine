﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace LandLord.Core
{

    public abstract class PlayerCard : IEquatable<PlayerCard>
    {
        public abstract int GetWeight();
        public bool Equals(PlayerCard other)
        {
            if (Object.ReferenceEquals(other, null))
            {
                if (Object.ReferenceEquals(this, null))
                {
                    return true;
                }
                return false;
            }
            return this.GetWeight() == other.GetWeight();
        }
        public override bool Equals(object obj)
        {
            return this.Equals(obj as PlayerCard);
        }

        public override int GetHashCode()
        {
            return this.GetWeight();
        }


        public static bool operator ==(PlayerCard lhs, PlayerCard rhs)
        {
            if (Object.ReferenceEquals(lhs, null))
            {
                if (Object.ReferenceEquals(rhs, null))
                {
                    return true;
                }
                return false;
            }
            return lhs.Equals(rhs);
        }

        public static bool operator !=(PlayerCard lhs, PlayerCard rhs) =>
            !(lhs == rhs);

        public abstract string PrettyString();
    }

    public class Shadowed : PlayerCard 
    {
        public override int GetWeight() => Defines.ShadowedValue;
        public override string PrettyString() { return "㊙️"; }
    }

    public abstract class PlayingCard : PlayerCard , IEquatable<PlayingCard>
    {
        public bool Equals(PlayingCard other)
        {
            return this.GetWeight(true) == other.GetWeight(true);
        }
        public override int GetWeight() => this.GetWeight(true);

        public abstract int GetWeight(bool considerSuit) ;

    }


}