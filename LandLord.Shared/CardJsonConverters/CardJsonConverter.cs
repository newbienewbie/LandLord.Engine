using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LandLord.Shared.CardJsonConverters
{
    public class LandLordCardJsonConverterBase<T> : JsonConverter<T>
        where T : PlayerCard
    {
        private Dictionary<PlayerCardKind,Type> _maps;

        public LandLordCardJsonConverterBase()
        {
            this._maps = new Dictionary<PlayerCardKind,Type>() {
                { PlayerCardKind.NormalCard , typeof(NormalCard)},
                { PlayerCardKind.BlackJokerCard, typeof(BlackJokerCard)},
                { PlayerCardKind.RedJokerCard, typeof(RedJokerCard) },
                { PlayerCardKind.Shadowed, typeof(Shadowed)},
            };
        }


        private const string DiscriminatedKindName = "Kind";

        protected Type FindSubTypeByEnum(PlayerCardKind kind) 
        {
            return this._maps[kind];
        }

        protected PlayerCardKind FindKindBySubType(Type subtype) 
        {
            foreach (var kvp in this._maps)
            {
                if (kvp.Value == subtype) return kvp.Key;
            }
            throw new ArgumentException($"nameof(subtype) has no coresponding enum kind");
        }

        public override bool CanConvert(Type typeToConvert)
        {
            //return typeof(PlayerCard).IsAssignableFrom(typeToConvert);
            return typeToConvert == typeof(T);
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var jsonDoc = JsonDocument.ParseValue(ref reader);
            if (jsonDoc.RootElement.TryGetProperty(DiscriminatedKindName, out var jsonElement))
            {
                var k = jsonElement.GetInt32();
                var kind = (PlayerCardKind)Enum.ToObject(typeof(PlayerCardKind) ,k);
                switch (kind)
                {
                    case PlayerCardKind.NormalCard:
                        return new NormalCard { 
                            CardSuit = (CardSuit) jsonDoc.RootElement.GetProperty(nameof(NormalCard.CardSuit)).GetInt32(), 
                            CardValue = (CardValue) jsonDoc.RootElement.GetProperty(nameof(NormalCard.CardValue)).GetInt32() ,
                        } as T;
                    case PlayerCardKind.BlackJokerCard:
                        return new BlackJokerCard{ 
                            JokerType = (JokerType) jsonDoc.RootElement.GetProperty(nameof(BlackJokerCard.JokerType)).GetInt32() ,
                        } as T;
                    case PlayerCardKind.RedJokerCard:
                        return new RedJokerCard{ 
                            JokerType = (JokerType) jsonDoc.RootElement.GetProperty(nameof(RedJokerCard.JokerType)).GetInt32() ,
                        } as T;
                    case PlayerCardKind.Shadowed:
                        return new Shadowed{} as T;
                    default:
                        throw new Exception("not a known subtype");
                }
            }
            throw new Exception("not a known subtype");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            var kind = this.FindKindBySubType(value.GetType());
            writer.WriteStartObject();
            writer.WriteNumber(DiscriminatedKindName, (int) kind);
            switch (value) {
                case NormalCard nc:
                    writer.WriteNumber(nameof(NormalCard.CardValue), (int) nc.CardValue );
                    writer.WriteNumber(nameof(NormalCard.CardSuit), (int) nc.CardSuit);
                    break;
                case BlackJokerCard bj:
                    writer.WriteNumber(nameof(BlackJokerCard.JokerType), (int) bj.JokerType);
                    break;
                case RedJokerCard rj:
                    writer.WriteNumber(nameof(RedJokerCard.JokerType), (int) rj.JokerType);
                    break;
                case Shadowed sh:
                    break;
                default:
                    throw new ArgumentException($"unsupported card-to-json conversion: {value.GetType().ToString()} :  ");
            }
            writer.WriteEndObject();
        }
    }

    public class PlayerCardJsonConverter : LandLordCardJsonConverterBase<PlayerCard>
    {
    }

    public class PlayingCardJsonConverter : LandLordCardJsonConverterBase<PlayingCard>
    {
    }
}
