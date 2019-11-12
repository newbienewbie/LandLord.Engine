using System;

namespace LandLord.Core
{
    
    public enum CardValue
    {
        Three  = 3,  Four  = 4, Five  = 5 , Six  = 6 , Seven  = 7 , Eight  = 8 , Nine = 9 , Ten = 10
        , Jack = 11 , Queen = 12 , King = 13
        , Ace = 14 , Two  = 15
    }

    public enum CardSuit { Spade=0 , Club=1 , Diamond=2 , Heart=3, }
    
    public enum JokerType { Black = 0, Red= 1}
    
    public static class Defines
    {
        public const int JokerValue = 20;
        public const int ShadowedValue = 42;

    }



}
