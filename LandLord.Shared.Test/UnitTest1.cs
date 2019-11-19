using JsonSubTypes;
using LandLord.Core.Room;
using Newtonsoft.Json;
using System;
using Xunit;

namespace LandLord.Shared.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var room = GameRoom.CreateAndDeal();
            var playerCardJsonConverter = JsonSubtypesConverterBuilder
                .Of(typeof(PlayerCard), "Kind") // type property is only defined here
                .RegisterSubtype(typeof(NormalCard), PlayerCardKind.NormalCard)
                .RegisterSubtype(typeof(BlackJokerCard), PlayerCardKind.BlackJokerCard)
                .RegisterSubtype(typeof(RedJokerCard), PlayerCardKind.RedJokerCard)
                .RegisterSubtype(typeof(Shadowed), PlayerCardKind.Shadowed)
                .SerializeDiscriminatorProperty() // ask to serialize the type property
                .Build();
            var playingCardJsonConverter = JsonSubtypesConverterBuilder
                .Of(typeof(PlayingCard), "Kind") // type property is only defined here
                .RegisterSubtype(typeof(NormalCard), PlayerCardKind.NormalCard)
                .RegisterSubtype(typeof(BlackJokerCard), PlayerCardKind.BlackJokerCard)
                .RegisterSubtype(typeof(RedJokerCard), PlayerCardKind.RedJokerCard)
                .SerializeDiscriminatorProperty() // ask to serialize the type property
                .Build();
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(playerCardJsonConverter);
            settings.Converters.Add(playingCardJsonConverter);
            var json = JsonConvert.SerializeObject(room, settings);
            var room2 = JsonConvert.DeserializeObject<GameRoom>(json, settings);
        }
    }
}
