using LeagueTableInterface.Models.MediaFetcher;

namespace League.Models
{
    public class TeamUtility : FetchTeamMedia
    {
        public Team Team;
        public List<Player> players;
        public string[] FileNames { get; set; }

        public TeamUtility(Team team, List<Player> players)
        {
            this.Team = team;
            this.players = players;
            this.FileNames = GetFileNames(Team.TeamId);
            TurnRootsToURLs(this.FileNames); 
        }
    }
}
