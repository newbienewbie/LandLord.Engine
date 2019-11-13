using System;
using System.Collections.Generic;
using System.Linq;
using LandLord.Core.Patterns;

namespace LandLord.Core
{
    public static class Facade
    {
        
        public static int Compare(bool considerSuit, PlayingCard card1, PlayingCard card2) =>
            card1.GetWeight(considerSuit) - card2.GetWeight(considerSuit);

        public static IList<PlayingCard> Sort( IList<PlayingCard> list) 
        {
            return list.OrderBy( card => card.GetWeight(true)).ToList();
        }

        public static (bool, IList<CardValue>) GetCardsValues(IList<PlayingCard> list)
        {
            var values =new List<CardValue>();
            bool calculateValues(IList<PlayingCard> cards){
                for (int nth = 0; nth < cards.Count; nth ++) {
                    var card = cards[nth];
                    if (card is NormalCard { CardValue: var cv }) {
                        values.Add(cv);
                    } else {
                        return false;
                    }
                }
                return true;
            }
            return calculateValues(list)?
                (true, values) :
                (false, new List<CardValue>());
        }


        public static IList<PlayingCard> CreateFullCards()
        {
            var suits = Enum.GetValues(typeof(CardSuit)).Cast<CardSuit>();
            var values = Enum.GetValues(typeof(CardValue)).Cast<CardValue>();
            List<PlayingCard> cards = 
                suits
                .SelectMany(s => values,  (s , v) => new NormalCard{ CardSuit = s, CardValue = v  }  )
                .Select( n => (PlayingCard) n )
                .ToList();
            cards.Add(new BlackJokerCard());
            cards.Add(new RedJokerCard());
            return cards;
        }

        public static IList<PlayingCard> Shuffle(IList<PlayingCard> cards)
        {
            var rand= new System.Random();
            var length = cards.Count;
            int random(int min) => rand.Next(min, length);

            void swap(int i, int j, PlayingCard[] array) {
                var tmp = array[i]; 
                array[i] = array[j];
                array[j] = tmp; 
            }

            var array = cards.ToArray();
            for(var i =0 ; i < length ;  i++  )
            {
                var r = random(i);
                swap(i, r, array);
            }
            return array.ToList();
        }

        public static (IList<PlayingCard>, (IList<PlayingCard>, IList<PlayingCard>, IList<PlayingCard>) ) Deal(IList<PlayingCard> cards)
        {
            var reserved = cards.Take(3).ToList();
            var dealing = cards.Skip(3).Select((c , i) => new {Nth= i % 3, Card=c});

            IList<PlayingCard> cardsOfPlayerN(int nth) =>
                dealing.Where(d => d.Nth == nth).Select(d => d.Card).ToList();
            
            var dealed = ( cardsOfPlayerN(0), cardsOfPlayerN(1), cardsOfPlayerN(2) );
            return (reserved, dealed);
        }

        public static IDictionary<string, PatternCheck> AllowedDups {
            get{
                PatternCheck MakeDanLianShun(int num)
                {
                    PatternCheck danlianshun= (IList<PlayingCard> cards) =>{
                        return Patterns.Patterns.DanLianShun(num, cards);
                    };
                    return danlianshun;
                }

                PatternCheck MakeDuoLianShun(int dup,int num)
                {
                    PatternCheck danlianshun= (IList<PlayingCard> cards) =>{
                        return Patterns.Patterns.DuoLianShun(dup, num, cards);
                    };
                    return danlianshun;
                }

                return new Dictionary<string, PatternCheck>{
                    {"单张", Patterns.Patterns.Single},
                    {"对子", Patterns.Patterns.Double},
                    {"三张", Patterns.Patterns.Trible},
                    {"炸弹", Patterns.Patterns.Bomb},
                    {"五连(单)顺子", MakeDanLianShun(5)},
                    {"六连(单)顺子", MakeDanLianShun(6)},
                    {"七连(单)顺子", MakeDanLianShun(7)},
                    {"八连(单)顺子", MakeDanLianShun(8)},
                    {"九连(单)顺子", MakeDanLianShun(9)},
                    {"十连(单)顺子", MakeDanLianShun(10)},
                    {"十一连(单)顺子", MakeDanLianShun(11)},
                    {"十二连(单)顺子", MakeDanLianShun(12)},
                    {"三连(对)顺子", MakeDuoLianShun(2, 3)},
                    {"四连(对)顺子", MakeDuoLianShun(2, 4)},
                    {"五连(对)顺子", MakeDuoLianShun(2, 5)},
                    {"六连(对)顺子", MakeDuoLianShun(2, 6)},
                    {"七连(对)顺子", MakeDuoLianShun(2, 7)},
                    {"八连(对)顺子", MakeDuoLianShun(2, 8)},
                    {"九连(对)顺子", MakeDuoLianShun(2, 9)},
                    {"十连(对)顺子", MakeDuoLianShun(2, 10)},
                    {"十一连(对)顺子", MakeDuoLianShun(2, 11)},
                    {"十二连(对)顺子", MakeDuoLianShun(2, 12)},
                    {"三连(三张)顺子", MakeDuoLianShun(3, 3)},
                    {"四连(三张)顺子", MakeDuoLianShun(3, 4)},
                    {"五连(三张)顺子", MakeDuoLianShun(3, 5)},
                    {"六连(三张)顺子", MakeDuoLianShun(3, 6)},
                    {"七连(三张)顺子", MakeDuoLianShun(3, 7)},
                    {"八连(三张)顺子", MakeDuoLianShun(3, 8)},
                    {"九连(三张)顺子", MakeDuoLianShun(3, 9)},
                    {"十连(三张)顺子", MakeDuoLianShun(3, 10)},
                    {"十一连(三张)顺子", MakeDuoLianShun(3, 11)},
                    {"十二连(三张)顺子", MakeDuoLianShun(3, 12)},
                    {"十三连(三张)顺子", MakeDuoLianShun(3, 13)},
                };
            }
        }
        public static IDictionary<string, PatternCheck> AllowedThreeWithExtras{
            get{
                return new Dictionary<string, PatternCheck>{
                    {"三带一", Patterns.Patterns.ThreeWithOne},
                };
            }
        }


        public static bool CanStartPlaying( IList<PlayingCard> cards)
        {
            foreach(var item in AllowedDups){
                var s = item.Value.Invoke(cards) switch
                {
                    (true, _) => true,
                    _ => false,
                };
                if(s) return true;
            }
            foreach(var item in AllowedThreeWithExtras){
                var s = item.Value.Invoke(cards) switch
                {
                    (true, _) => true,
                    _ => false,
                };
                if(s) return true;
            }
            return false;
        }

        public static bool CanPlay(IList<PlayingCard> prevCards, IList<PlayingCard> cards )
        {
            bool gt(PlayingCard card1, PlayingCard card2)
            {
                return Compare(false, card1, card2) > 0;
            }

            bool seqGt(IList<PlayingCard> cards1, IList<PlayingCard> cards2)
            {
                var card1 = cards1.First();
                var card2 = cards2.First();
                return gt(card1, card2);
            }

            bool threeWithOneGt(IList<PlayingCard> cards1, IList<PlayingCard> cards2)
            {
                PlayingCard dominatingCard (IList<PlayingCard> cards)
                {
                    var g = cards.GroupBy( c => c.GetWeight(false), (k,g) => new { Key=k, G = g } ).OrderBy( i => i.G.Count() ).ToList();
                    if(g.Count() !=2)
                        throw new Exception("not a '3+1' pattern");
                    var first = g[0];
                    var second = g[1].G.ToList();
                    if(second.Count!=3)
                        throw new Exception("not a '3+1' pattern");
                    var w1 = second[0].GetWeight(false);
                    var w2 = second[1].GetWeight(false);
                    var w3 = second[3].GetWeight(false);
                    if( !(w1 == w2 && w1 == w3) )
                    {
                        throw new Exception("not a '3+1' pattern");
                    }
                    return second[3];
                }
                var c1 = dominatingCard(cards1);
                var c2 = dominatingCard(cards2);
                return gt( c1, c2 );
            }

            prevCards = Sort(prevCards);
            cards = Sort(cards);

            foreach(var allowed in AllowedThreeWithExtras)
            {
                var prev = allowed.Value.Invoke(prevCards) switch
                {
                    (true, _) => true,
                    _ => false,
                };
                if(!prev) continue;
                var curr = allowed.Value.Invoke(cards) switch
                {
                    (true, _) => true,
                    _ => false,
                };
                if(!curr) continue;
                if(prev && curr)
                {
                    if(threeWithOneGt(prevCards, cards))
                        return true;
                }
            }

            foreach(var allowed in AllowedDups)
            {
                var prev = allowed.Value.Invoke(prevCards) switch
                {
                    (true, _) => true,
                    _ => false,
                };
                if(!prev) continue;
                var curr = allowed.Value.Invoke(cards) switch
                {
                    (true, _) => true,
                    _ => false,
                };
                if(!curr) continue;
                if(prev && curr)
                {
                    if(seqGt(prevCards, cards))
                        return true;
                }
            }

            // if we go here, should check bomb:
            var (prevIsBomb, prevbomb) = Patterns.Patterns.Bomb(prevCards);
            var (isBomb, bomb) = Patterns.Patterns.Bomb(cards);
            if(!isBomb && prevIsBomb) return false;
            if(isBomb && !prevIsBomb ) return true;
            return false;
        }


    }
}