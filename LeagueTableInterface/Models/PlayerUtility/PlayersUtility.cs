using League.Models;

namespace League.Models
{
    public class PlayersUtility
    {
        public List<Player> Players { get; set; }
        public List<string> Positions { get; set; }
        public List<string> TeamIds { get; set; }

        public PlayersUtility(List<Player> players, List<string> positions, List<string> temaIds) 
        {
            this.Players = players;
            this.Positions = positions; 
            this.TeamIds = temaIds;
        }
    }
}
