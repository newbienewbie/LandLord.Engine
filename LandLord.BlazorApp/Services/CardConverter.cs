using Itminus.LandLord.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandLord.BlazorApp.Services
{
    public class CardConverterService
    {

        public string PlayingCardToString(Card.PlayingCard cardShape)
        {
            return cardShape.ConvertToString();
        }
        public string PlayingCardToStyle(Card.PlayingCard cardShape)
        {
            return cardShape.ConvertToString();
        }

        public string PlayerCardToString(Card.PlayerCard cardShape)
        {
            return cardShape.ConvertToString();
        }
        public string PlayerCardToStyle(Card.PlayerCard cardShape)
        {
            return cardShape.ConvertToString();
        }

    }
}
