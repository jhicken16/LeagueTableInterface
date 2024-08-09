using LeagueTableInterface.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using League.Models;
using System.Threading.RateLimiting;
using Microsoft.Net.Http.Headers;


namespace LeagueTableInterface.Controllers
{
    
    public class TeamsController : Controller
    {
        private readonly ApplicationDBContext dbContext;

        public TeamsController(ApplicationDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            List<Team> teams = await dbContext.Teams.ToListAsync();

            //order teams in order of win loss ratio.
            QuickSortTeamsByWinLose(teams);

            return View(teams);
        }
        
        [HttpPost]
        public IActionResult Index(string favTeam)
        {
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(7);
            Response.Cookies.Append("favTeam", favTeam, options);
            return RedirectToAction("Index");
        }



        [HttpGet]
        [Route("info")]
        public async Task<IActionResult> TeamInfo([FromQuery] string id)
        {
            Team team;
            List<Player> players;
            try
            {
                team = await dbContext.Teams.FirstOrDefaultAsync(x => x.TeamId == id);
                players = await dbContext.Players.Where(x => x.TeamId == id).ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
            if(team == null)
            {
                return NotFound();
            }

            TeamUtility teamWithFiles = new TeamUtility(team, players);

            return View(teamWithFiles);
        }

       



        //<-------------------------------------------------  Functions to sort Teams  ------------------------------------------------------->
        private void QuickSortTeamsByWinLose(List<Team> teams, int leftBound = 0, int rightBound = -1)
        {
            if(rightBound == -1) { rightBound = teams.Count - 1; }

            if(leftBound < rightBound) 
            {
                int pivoteIndex = Partition(teams, leftBound, rightBound);
                QuickSortTeamsByWinLose(teams, leftBound, pivoteIndex - 1);
                QuickSortTeamsByWinLose(teams, pivoteIndex, rightBound);
            }
            return;
        }
        private int Partition(List<Team> teams, int leftIndex, int rightIndex)
        {
            double pivot = teams[(rightIndex + leftIndex) / 2].CalculateWinLossRatio();
            while(leftIndex <= rightIndex)
            {
                while (teams[leftIndex].CalculateWinLossRatio() > pivot)//found some think in inner loops but continues to loop outer
                {
                    leftIndex++;
                }
                while (teams[rightIndex].CalculateWinLossRatio() < pivot)
                {
                    rightIndex--;
                }
               if(leftIndex <= rightIndex)
                {
                    Team temp = teams[leftIndex];
                    teams[leftIndex] = teams[rightIndex];
                    teams[rightIndex] = temp;
                    leftIndex++;
                    rightIndex--;
                }
            }

 
            return leftIndex;
        }
    }
}
