namespace LandLord.Shared
{

    public abstract class JokerCard : PlayingCard
    { 
        public virtual JokerType JokerType { get; set; }

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
           return (((int) Defines.JokerValue) << 2 ) + suitValue(JokerType);
        }

    }
    public class BlackJokerCard : JokerCard
    {
        public override JokerType JokerType { get; set; } = JokerType.Black;
        public override string PrettyString()
        {
            return "🐼";
        }
    }
    public class RedJokerCard : JokerCard
    {
        public override JokerType JokerType { get; set; } = JokerType.Red;
        public override string PrettyString()
        {
            return "🃏";
        }
    }
}