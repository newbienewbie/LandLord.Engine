namespace LandLord.Core
{

    public abstract class JokerCard : PlayingCard
    { 
        public abstract JokerType JokerType {get;}

        /// <summary>
        /// |                  n bits                     | 2 bits|
        /// |---------------------------------------------+-------|
        /// |    CardValue/JokerValue/ShadowedValue       |  Suit |
        /// </summary>
        /// <param name="considerJokerType"></param>
        /// <returns></returns>
        public override int GetWeight(bool considerJokerType) 
        {
           int suitValue(JokerType jt) => considerJokerType? (int) jt : 0 ;
           return ((int) Defines.JokerValue) << 2 + suitValue(JokerType);
        }
    }
    public class BlackJokerCard : JokerCard
    {
        public override JokerType JokerType {get;} = JokerType.Black;
        public override string PrettyString()
        {
            return "🐼";
        }
    }
    public class RedJokerCard : JokerCard
    {
        public override JokerType JokerType {get;} = JokerType.Red;
        public override string PrettyString()
        {
            return "🃏";
        }
    }
}