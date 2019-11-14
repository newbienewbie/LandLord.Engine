using LandLord.Shared;
using System.Collections.Generic;

namespace LandLord.Core.Patterns
{
    public static partial class Patterns
    {
        public static (bool, IList<PlayingCard>) ThreeWithOne(IList<PlayingCard> cards)
        {
            bool check3with1(IList<PlayingCard> cards){
                if(cards.Count != 4) return false;
                var (result, values) = Facade.GetCardsValues(Facade.Sort(cards));
                if(!result) return false;

                var c1 = values[0];
                var c2 = values[1];
                var c3 = values[2];
                var c4 = values[3];
                if(c1 == c2 && c2 == c3 && c3 != c4)
                    return true;
                if(c1 != c2 && c2 == c3 && c3 == c4)
                    return true;
                return false;
            }
            return check3with1(cards)? 
                (true, cards) :
                (false, null) ;
        }
    }
}