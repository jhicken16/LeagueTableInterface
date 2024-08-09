using League.Models;
using LeagueTableInterface.Models.MediaFetcher;


namespace League.Models
{
    public class PlayerUtility : FetchTeamMedia
    {
        public Player player;
        public string[] FileNames {  get; set; } 

        public PlayerUtility(Player player)
        {
            this.player = player;
            this.FileNames = GetFileNames(player.TeamId);
            TurnRootsToURLs(this.FileNames);
        }
    }
}
