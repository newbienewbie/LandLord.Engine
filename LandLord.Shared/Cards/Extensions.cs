using System;
using System.Collections.Generic;
using System.Text;

namespace LandLord.Shared
{
    internal static class Extensions
    {
        internal static string PrettyString(this CardSuit suit) => suit switch
        {
            CardSuit.Spade => "♠️",
            CardSuit.Club => "♣️",
            CardSuit.Diamond => "♦️",
            CardSuit.Heart => "️♥️",
            _ => throw new Exception("unknow suit")
        };
        internal static string PrettyString(this CardValue value) => value switch
        {
            CardValue.Ace => "A",
            CardValue.Two => "2",
            CardValue.Three => "3",
            CardValue.Four => "4",
            CardValue.Five => "5",
            CardValue.Six => "6",
            CardValue.Seven => "7",
            CardValue.Eight => "8",
            CardValue.Nine => "9",
            CardValue.Ten => "10",
            CardValue.Jack => "J",
            CardValue.Queen => "Q",
            CardValue.King => "K",
            _ => throw new Exception("unknow suit")
        };
    }
}
