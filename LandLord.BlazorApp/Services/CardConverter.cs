using Itminus.LandLord.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandLord.BlazorApp.Services
{

    class CardConverterService
    {

        public string PlayingCardToString(Card.PlayingCard cardShape)
        {
            if (cardShape.IsJoker)
            {
                var j = cardShape.Tag;
                return this.joker(JokerType[j]);
            }
            else if (cardShape.IsNormalCard) {
                switch (cardShape) {
                    case Card.JokerType j:
                        j.
                    case Card.
                }
                let v = cardShape.;
                let s = cardShape.fields[1];
                return `${ this.suit(Suit[s])}${ this.cardVale(CardValue[v])}`;
            }
            return "Error Card Shape";
        }

        private string joker(string j)
        {
            switch (j.ToLower())
            {
                case "red": return "🃏";
                case "black": return "🐼";
                default: throw new Exception($"unknow joker type {nameof(j)}={j}");
            }
        }

        private string suit(string s)
        {
            switch (s.ToLower())
            {
                case "heart":
                    return "️♥️";
                case "diamond":
                    return "♦️";
                case "club":
                    return "♣️";
                case "spade":
                    return "♠️";
                default:
                    return s;
            }
        }

        private string cardVale(string v)
        {
            switch (v.ToLower())
            {
                case "ace": return "A";
                case "two": return "2";
                case "three": return "3";
                case "four": return "4";
                case "five": return "5";
                case "six": return "6";
                case "seven": return "7";
                case "eight": return "8";
                case "nine": return "9";
                case "ten": return "10";
                case "jack": return "J";
                case "queen": return "Q";
                case "king": return "K";
                default: return v;
            }
        }

    }
}
