using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Itminus.LandLord.Engine;
using LandLord.Engine.Repository;
using LiteDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LandLord.WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameRoomController : ControllerBase
    {
        private GameRoomRepository _repo;

        public GameRoomController(GameRoomRepository repo)
        {
            this._repo = repo;
        }

        // GET: api/GameRoom
        [HttpGet]
        public IEnumerable<IGameRoomMetaData> Get()
        {
            return this._repo.FindAll();
        }

        // GET: api/GameRoom/5
        [HttpGet("{id}", Name = "Get")]
        public GameRoomMetaData Get(Guid id)
        {
            return this._repo.Load(new BsonValue(id));
        }

        // POST: api/GameRoom
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/GameRoom/5
        [HttpPut()]
        public IActionResult Put()
        {
            var room = GameRoom.Prepare();
            this._repo.Save(room);
            return new JsonResult(new {
                Id = room.Id,
                Players = room.Players,
            });
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
        }
    }
}
