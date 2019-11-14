using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LandLord.BlazorApp.Data;
using LandLord.Shared;

namespace LandLord.BlazorApp.Services
{
    public class CardConverterService
    {

        public string PlayingCardToString(PlayingCard cardShape)
        {
            return cardShape.PrettyString();
        }
        public string PlayingCardToStyle(PlayingCard cardShape)
        {
            return cardShape.PrettyString();
        }

        public string PlayerCardToString(PlayerCard cardShape)
        {
            return cardShape.PrettyString();
        }
        public string PlayerCardToStyle(PlayerCard cardShape)
        {
            return cardShape.PrettyString();
        }

    }
}
