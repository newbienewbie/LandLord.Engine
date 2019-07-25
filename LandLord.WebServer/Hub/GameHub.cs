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


    // the game state returned to client 
    public class GameStateDto
    {
        public IGameRoomMetaData GameRoom { get; internal set; }
        public int TurnIndex { get; internal set; }
    }

    public interface IGameHubClient
    {
        Task ReceiveState(GameStateDto state);
        Task AddingToRoomSucceeded(Guid roomId);
        Task RemoveFromRoomSucceeded(Guid roomId);
        Task PlayCards(int index, IList<PlayingCard> cards);
        Task Win(int index);
        Task ReceiveError(string msg);
    }

    public class GameHub : Hub<IGameHubClient>
    {
        private readonly IServiceProvider _sp;

        public GameHub(IServiceProvider sp)
        {
            this._sp = sp;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            // find if there's a room
            var room = FindOneRecentRoom();
            if (room !=null) {
                await this.ReJoinRoom(room.Id);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
            // find if there's a room
            var room = FindOneRecentRoom();
            if (room != null)
            {
                //?
            }
        }


        private GameRoom FindOneRecentRoom() {
            using (var scope = this._sp.CreateScope())
            {
                var roomRepo = scope.ServiceProvider.GetRequiredService<GameRoomRepository>();
                var userId = Context.UserIdentifier;
                var room = roomRepo.FindAvaiableRoomByUserId(userId);
                return room;
            }
        }
        public async Task ReJoinRoom(Guid roomId)
        {
            var roomName = roomId.ToString();
            var newConnectionId = Context.ConnectionId;
            using (var scope = this._sp.CreateScope())
            {
                var roomRepo = scope.ServiceProvider.GetRequiredService<GameRoomRepository>();
                var room = roomRepo.Load(roomId);

                var findings = room.FindPlayer(Context.UserIdentifier);
                if (findings != null) {
                    var oldConnectionId = findings.Player.ConnectionId;
                    await Groups.RemoveFromGroupAsync(oldConnectionId, roomName);
                    await Groups.AddToGroupAsync(newConnectionId,roomName);
                    room.Players[findings.Index].ConnectionId = newConnectionId;
                }
                roomRepo.Save(room);
                // notify client
                await Clients.Group(roomName).ReceiveState(new GameStateDto { GameRoom= room, TurnIndex = findings.Index});
            }
        }


        public async Task PushLatestStateToCurrentPlayer(Guid roomId)
        {
            var userIdentifier = Context.UserIdentifier;
            using (var scope = this._sp.CreateScope())
            {
                var roomRepo = scope.ServiceProvider.GetRequiredService<GameRoomRepository>();
                var room = roomRepo.Load(roomId);
                var findings = room.FindPlayer(userIdentifier);
                if (findings == null)
                {
                    await Clients.Caller.ReceiveError($"current player is not found within this room!");
                }
                else {
                    var roomdata = room.ShadowCards(findings.Index);
                    await Clients.Caller.ReceiveState(new GameStateDto { GameRoom = roomdata, TurnIndex = findings.Index});
                }
            }
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
                    ConnectionId =Context.ConnectionId,
                    Name = Context.UserIdentifier,
                });
                roomRepo.Save(room);
                var findings = room.FindPlayer(Context.UserIdentifier);
                if (findings == null)
                {
                    throw new Exception("findings must not be null");
                }
                await Clients.Group(roomIdStr).AddingToRoomSucceeded(roomId);
                await Clients.Group(roomIdStr).ReceiveState(new GameStateDto {
                    GameRoom = room,
                    TurnIndex = findings.Index,
                });
            }
        }

        public async Task RemoveFromRoom(Guid roomId, string connectionId)
        {
            var roomName = roomId.ToString();
            await Groups.RemoveFromGroupAsync(connectionId, roomName);
            using (var scope = this._sp.CreateScope())
            {
                var roomRepo = scope.ServiceProvider.GetRequiredService<GameRoomRepository>();
                var room = roomRepo.Load(roomId);
                // todo: remove player
                roomRepo.Save(room);
                await Clients.Group(roomName).RemoveFromRoomSucceeded(roomId);
                await Clients.Group(roomName).ReceiveState(new GameStateDto { GameRoom= room, TurnIndex = -1});
            }
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
            var userId= Context.UserIdentifier;
            using (var scope = this._sp.CreateScope())
            {
                var roomRepo = scope.ServiceProvider.GetRequiredService<GameRoomRepository>();
                var room = roomRepo.Load(roomId);
                var findings = room.FindPlayer(userId);
                if(findings == null) {
                    // false
                } else {
                    var index = findings.Index;
                    var player = findings.Player;
                    var succeeded = play(room,index);
                    if (!succeeded) {
                        // process false
                    }
                    else {
                        await Clients.Group(roomName).PlayCards(index, cards);
                        roomRepo.Save(room);

                        foreach (var p in room.Players) {
                            var shadowed = room.ShadowCards(findings.Index);
                            await Clients.Client(p.ConnectionId).ReceiveState(new GameStateDto {
                                GameRoom = shadowed,
                                TurnIndex = findings.Index,
                            });
                        }
                    }
                }
            }
        }

    }
}
