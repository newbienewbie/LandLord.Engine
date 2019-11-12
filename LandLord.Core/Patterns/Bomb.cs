using System.Collections.Generic;

namespace LandLord.Core.Patterns
{
    public static partial class Patterns
    {
        public static (bool, IList<PlayingCard>) Bomb(IList<PlayingCard> cards)
        {
            if(cards.Count <2) return (false,null);
            var first = cards[0];
            var second= cards[1];
            if(cards.Count == 2)
            {
                switch((first, second)){
                    case (JokerCard j1, JokerCard j2):
                        return (true, cards);
                    default:
                        return (false,null);
                }
            }
            else if(cards.Count == 4)
            {
                var third = cards[2];
                var forth = cards[3];
                switch((first, second, third, forth))
                {
                    case (NormalCard n1, NormalCard n2, NormalCard n3, NormalCard n4) 
                        when n1.Equals(n2) && n2.Equals(n3) && n3.Equals(n4): 
                        return (true,cards);
                    default:    
                        return (false,null);
                }
            }
            return (false,null);
        }
    }
}