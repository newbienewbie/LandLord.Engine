
using System;
using System.Collections.Generic;
using System.Linq;

namespace LandLord.Core.Patterns
{

    public delegate (bool, IList<PlayingCard>) PatternCheck(IList<PlayingCard> cards);

    public static partial class Patterns
    {
        public static (bool, IList<PlayingCard>) Single(IList<PlayingCard> cards)
        {
            if(cards.Count == 1)
            {
                switch(cards.First())
                {
                    case NormalCard nc :
                        return (true, cards);
                    case JokerCard jc :
                        return (true, cards);
                    default:
                        return (false,null);
                }
            }
            return (false, null);
        }

        public static (bool, IList<PlayingCard>) Double(IList<PlayingCard> cards)
        {
            if(cards.Count == 2)
            {
                var first = cards[0];
                var second = cards[1];
                if(first.GetWeight(false) == second.GetWeight(false)) {
                    return (true, cards);
                }
            }
            return (false, null);
        }
        public static (bool, IList<PlayingCard>) Trible(IList<PlayingCard> cards)
        {
            if(cards.Count == 3)
            {
                var first = cards[0].GetWeight(false);
                var second = cards[1].GetWeight(false);
                var third = cards[2].GetWeight(false);
                if(first == second && first == third) {
                    return (true, cards);
                }
            }
            return (false, null);
        }

        public static (bool, IList<PlayingCard>) DanLianShun(int num, IList<PlayingCard> cards)
        {
            bool cardsContinuous(IList<PlayingCard> cards)
            {
                bool cardsContinuous(IList<PlayingCard> cards, int prev)
                {
                    var first = cards.FirstOrDefault();
                    if (first == null) return true;
                    if(first is NormalCard nc )
                    {
                        var cv = (int)nc.CardValue;
                        if( cv == prev + 1)
                        {
                            return cardsContinuous(cards.Skip(1).ToList(), cv);
                        }
                    }
                    return false; 
                }
                cards = Facade.Sort(cards);
                var first = cards.First();
                if(first is NormalCard nc)
                {
                    return cardsContinuous(cards.Skip(1).ToList(), (int) nc.CardValue );
                }
                return false;
            }

            if(cards.Count == num)
            {
                if (cardsContinuous(cards)) {
                    return (true, cards);
                };
            }
            return (false, null);
        }

        public static (bool, IList<PlayingCard>) DuoLianShun(int dup, int len, IList<PlayingCard> cards)
        {
            bool checkDanLianShun(IList<CardValue> values)
            {
                var x = values.Select(v => (int) v).OrderBy(v => v);
                bool test(IList<int> values, int prev )
                {
                    if(values.Count == 0 ) return true; 
                    var first = values.First();
                    if(values.Count == 1 && first == prev +1) return true; 
                    if(first == prev +1 ) return test(values.Skip(1).ToList(), first);
                    return false;
                }
                if( values.Count == 0 || values.Count == 1)
                    return true;
                var first = x.First();
                return test(x.Skip(1).ToList(), first);
            }

            bool checkDuoLianShun(int dup, int len, IList<CardValue> cards)
            {
                var x = cards.Select(i => (int)i).OrderBy(i => i);
                var group = x.GroupBy(i => i, i => i, (k, g)=> (k,g));
                if(group.Count() == len)
                {
                    var notDuiZi = 
                        group.Select(gi => {
                            var (key, g) = gi;
                            return g.Count();
                        }).Any(count => count != dup);
                    if(notDuiZi)
                        return false;
                    else {
                        var disctinctCardValues = group.Select(gi => gi.k)
                            .Select(v => (CardValue) Enum.ToObject(typeof(CardValue), v))
                            .ToList();
                        return checkDanLianShun( disctinctCardValues);
                    }

                }
                return false;
            }

            return Facade.GetCardsValues(cards) switch {
                (true, var values) when checkDuoLianShun(dup, len ,values) => (true, cards),
                _ => (false, null)
            };
        }


    }

}