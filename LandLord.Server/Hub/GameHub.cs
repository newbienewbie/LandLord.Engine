using Itminus.LandLord.Engine;
using LandLord.Engine.Repository;
using LandLord.Server;
using Microsoft.AspNetCore.Authorization;
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
        /// <summary>
        /// current game room state for this particualr client
        /// </summary>
        public IGameRoomMetaData GameRoom { get; internal set; }

        /// <summary>
        /// current player's turn index
        /// </summary>
        public int TurnIndex { get; internal set; }
    }

    public interface IGameHubClient
    {
        Task BeLandLordSucceded(int index);
        Task BeLandLordFailed();

        Task ReceiveState(GameStateDto state);
        Task AddingToRoomSucceeded(Guid roomId);
        Task RemoveFromRoomSucceeded(Guid roomId);
        Task PlayCardsSucceeded(int index, IList<PlayingCard> cards);
        Task PlayCardsFailed(int index, IList<PlayingCard> cards);
        Task Win(int index);
        Task ReceiveError(string msg);
    }

    [Authorize(AuthenticationSchemes=GameHubConstants.GameHubAuthenticationScheme)]
    public class GameHub : Hub<IGameHubClient>
    {
        private readonly IServiceProvider _sp;

        public GameHub(IServiceProvider sp)
        {
            this._sp = sp;
        }

        //public override async Task OnConnectedAsync()
        //{
        //    await base.OnConnectedAsync();
        //    // find if there's a room
        //    var room = FindOneRecentRoom();
        //    if (room != null)
        //    {
        //        await this.ReJoinRoom(room.Id);
        //    }
        //}

        //public override async Task OnDisconnectedAsync(Exception exception)
        //{
        //    await base.OnDisconnectedAsync(exception);
        //    // find if there's a room
        //    var room = FindOneRecentRoom();
        //    if (room != null)
        //    {
        //        //?
        //    }
        //}


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
            using (var scope = this._sp.CreateScope())
            {
                var roomRepo = scope.ServiceProvider.GetRequiredService<GameRoomRepository>();
                var room = roomRepo.Load(roomId);
                var findings = await this.ReJoinRoomCore(room);
                roomRepo.Save(room);
                // notify client
                var roomName = roomId.ToString();
                await Clients.Group(roomName).ReceiveState(new GameStateDto {
                    GameRoom = room,
                    TurnIndex = findings.Index
                });
            }
        }

        private async Task<PlayerFindings> ReJoinRoomCore(GameRoom room)
        {
            var newConnectionId = Context.ConnectionId;
            var roomName = room.Id.ToString();
            var findings = room.FindPlayer(Context.UserIdentifier);
            if (findings != null)
            {
                var oldConnectionId = findings.Player.ConnectionId;
                await Groups.RemoveFromGroupAsync(oldConnectionId, roomName);
                await Groups.AddToGroupAsync(newConnectionId, roomName);
                room.Players[findings.Index].ConnectionId = newConnectionId;
                return findings;
            }
            else {
                await Groups.AddToGroupAsync(newConnectionId, roomName);
                findings = room.FindPlayer(Context.UserIdentifier);
                return findings;
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
                    Id= Context.UserIdentifier,
                    Name= Context.User.Identity.Name,
                });
                var findings = await this.ReJoinRoomCore(room);
                roomRepo.Save(room);
                if (findings == null)
                {
                    throw new Exception("findings must not be null");
                }
                await Clients.Group(roomIdStr).AddingToRoomSucceeded(roomId);
                var shadowed = room.ShadowCards(findings.Index);
                await Clients.Group(roomIdStr).ReceiveState(new GameStateDto {
                    GameRoom = shadowed,
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

        public async Task BeLandLord(Guid roomId)
        {
            var roomName = roomId.ToString();
            var userId= Context.UserIdentifier;
            using (var scope = this._sp.CreateScope())
            {
                var roomRepo = scope.ServiceProvider.GetRequiredService<GameRoomRepository>();
                var room = roomRepo.Load(roomId);

                if (room.LandLordIndex < 0) {
                    var findings = room.FindPlayer(userId);
                    if (findings != null)
                    {
                        room.LandLordIndex = findings.Index;
                        room.AppendCards(room.ReservedCards);
                        roomRepo.Save(room);

                        await Clients.Group(roomName).BeLandLordSucceded(findings.Index);
                        var shadowed = room.ShadowCards(findings.Index);
                        await Clients.Group(roomName).ReceiveState(new GameStateDto
                        {
                            GameRoom = shadowed,
                            TurnIndex = findings.Index,
                        });
                        return;
                    }
                }
                await Clients.Caller.BeLandLordFailed();   
            }
           
        }

        public async Task PlayCards(Guid roomId, List<PlayingCard> cards)
        {
            await PlayCardsCore(roomId,cards);
        }

        private async Task PlayCardsCore(Guid roomId, List<PlayingCard> cards)
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
                    var succeeded = false;
                    // only when current user is the landlord and none of his cards has not been player
                    if (room.LandLordIndex >=0 && index == room.LandLordIndex && room.Cards[index].Count == 20) {
                        succeeded = room.StartPlayingCards(cards);
                    } else {
                        succeeded = room.PlayCards(index, cards);
                    }
                    if (!succeeded) {
                        // process false
                        await Clients.Caller.PlayCardsFailed(index,cards);
                    }
                    else {
                        roomRepo.Save(room);

                        foreach (var p in room.Players) {
                            var shadowed = room.ShadowCards(findings.Index);
                            await Clients.Client(p.ConnectionId).ReceiveState(new GameStateDto {
                                GameRoom = shadowed,
                                TurnIndex = findings.Index,
                            });
                        }
                        await Clients.Group(roomName).PlayCardsSucceeded(index, cards);
                    }
                }
            }
        }

    }
}
