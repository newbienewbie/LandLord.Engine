using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandLord.Core.Room
{
    public class Player
    {
        public string ConnectionId { get; set; } = String.Empty;
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool StillActive { get; set; } = false;
        public bool IsEmpty
        {
            get
            {
                return String.IsNullOrEmpty(this.ConnectionId)
                    && String.IsNullOrEmpty(this.Id)
                    && String.IsNullOrEmpty(this.Name)
                    && this.StillActive == false;
            }
        }
        /// An empty instance
        public static Player Empty { get; } = new Player() { StillActive = false };
    }


    /// used by GameRoom::FindPlayer()
    public class PlayerFindings
    {
        public PlayerFindings(int index, Player player)
        {
            this.Index = index;
            this.Player = player;
        }
        public int Index { get; set; }
        public Player Player { get; set; }
    }





}
