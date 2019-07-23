using Itminus.LandLord.Engine;
using LandLord.Engine.Repository;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Itminus.LandLord.Engine.Card;

namespace LandLord.WebServer.Services
{
    public interface IGameHubClient
    {
        Task AddingToRoomSucceeded(Guid roomId);
        Task RemoveFromRoomSucceeded(Guid roomId);
        Task PlayCards(int index, IList<PlayingCard> cards);
        Task Win(int index);
    }

    public class GameHub : Hub<IGameHubClient>
    {
        private readonly IServiceProvider _sp;

        public GameHub(IServiceProvider sp)
        {
            this._sp = sp;
        }

        public async Task AddToRoom(Guid roomId)
        {
            var roomIdStr = roomId.ToString();
            var connectionId = Context.ConnectionId;
            await Groups.AddToGroupAsync(connectionId, roomIdStr);
            using (var scope = this._sp.CreateScope())
            {
                var roomRepo = scope.ServiceProvider.GetRequiredService<GameRoomRepository>();
                var room = roomRepo.Load(roomId);
                room.AddUser(new Player {
                    ConnectionId =connectionId,
                    Name = Context.UserIdentifier,
                });
                roomRepo.Save(room);
            }
            await Clients.Group(roomIdStr).AddingToRoomSucceeded(roomId);
        }

        public async Task RemoveFromRoom(Guid roomId)
        {
            var roomName = roomId.ToString();
            var connectionId = Context.ConnectionId;
            await Groups.RemoveFromGroupAsync(connectionId, roomName);
            using (var scope = this._sp.CreateScope())
            {
                var roomRepo = scope.ServiceProvider.GetRequiredService<GameRoomRepository>();
                var room = roomRepo.Load(roomId);
                // todo: remove player
                roomRepo.Save(room);
            }
            await Clients.Group(roomName).RemoveFromRoomSucceeded(roomId);
        }

        public async Task StartPlayingCards(Guid roomId, List<PlayingCard> cards)
        {
            Func<GameRoom, int, bool> play = (room, index) => room.StartPlayingCards(cards);
            await PlayCardsCore(roomId,cards,play);
        }

        public async Task PlayCards(Guid roomId, List<PlayingCard> cards)
        {
            Func<GameRoom, int, bool> play = (room, index) => room.PlayCards(index,cards);
            await PlayCardsCore(roomId,cards,play);
        }

        private async Task PlayCardsCore(Guid roomId, List<PlayingCard> cards, Func<GameRoom,int,bool> play)
        {
            var roomName = roomId.ToString();
            var connectionId = Context.ConnectionId;
            using (var scope = this._sp.CreateScope())
            {
                var roomRepo = scope.ServiceProvider.GetRequiredService<GameRoomRepository>();
                var room = roomRepo.Load(roomId);
                var kv= room.Players.Select((p, i) => new { Player = p, Index = i })
                    .Where(pair => pair.Player.ConnectionId == connectionId)
                    .Single();
                if(kv == null) {
                    // false
                } else {
                    var index = kv.Index;
                    var player = kv.Player;
                    var succeeded = play(room,index);
                    if (!succeeded) {
                        // process false
                    }
                    else {
                        await Clients.Group(roomName).PlayCards(index, cards);
                        roomRepo.Save(room);
                    }
                }
            }
        }

    }
}
