using LandLord.Shared;
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
                    case (JokerCard j1, JokerCard j2) 
                        when j1 != j2:
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
                    case (NormalCard { CardValue: var cv1 }, NormalCard { CardValue: var cv2 }, NormalCard { CardValue: var cv3 }, NormalCard { CardValue: var cv4 })
                        when cv1 == cv2 && cv2 == cv3 && cv3 == cv4:
                        return (true,cards);
                    default:    
                        return (false,null);
                }
            }
            return (false,null);
        }
    }
}