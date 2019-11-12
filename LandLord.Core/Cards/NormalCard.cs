

namespace LandLord.Core
{

    public class NormalCard : PlayingCard
    {
        public CardValue CardValue {get;set;}
        public CardSuit CardSuit {get;set;}
        public override int GetWeight(bool considerSuit) 
        {
            int suitValue(CardSuit suit) => considerSuit? (int) suit : 0;
            return ((int) CardValue) << 2 + suitValue(CardSuit);
        }
    }
}
