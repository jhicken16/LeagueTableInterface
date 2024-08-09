using Microsoft.AspNetCore.Mvc;
using League.Models;
using Microsoft.EntityFrameworkCore;
using LeagueTableInterface.Data;


namespace LeagueTableInterface.Controllers
{
    public class PlayerController : Controller
    {
        private readonly ApplicationDBContext _dbContext;

        public PlayerController(ApplicationDBContext dBContext)
        {
            this._dbContext = dBContext;
        }


        [Route("Players")]
        //Tis method is going to except three inputs and then return differnt renders deppending what has been submitted will return the same page.
        public async Task<IActionResult> Index(string? orderOption, string? playerName, string? position)
        {
            List<Player> players;
            List<string> positions;
            List<string> teamIDs;
            try
            {
                if(playerName != null)
                {
                    players = await _dbContext.Players.Where(p => p.Name.Contains(playerName)).ToListAsync();
                    
                }
                else if(position != null)
                {
                    players = await _dbContext.Players.Where(p => p.Position == position).ToListAsync();
                }
                else
                {
                    players = await _dbContext.Players.ToListAsync();
                }
                

                //need to check this return the correct type 
                positions = await _dbContext.Players.Select(x => x.Position).Distinct().ToListAsync();
                //Gets team ids, check return type 
                teamIDs = await _dbContext.Teams.Select(x => x.TeamId).Distinct().ToListAsync();

                if(players == null)
                {
                    return NotFound(); 
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500); 
            }

            PlayersUtility playersUtility = new PlayersUtility(players, positions, teamIDs);

            if(orderOption != null)
            {
                QuickSortPlayersByOption(playersUtility.Players, orderOption);
            }

            return View(playersUtility);
        }

        [Route("Player")]
        public async Task<IActionResult> Player([FromQuery] string id)
        {
            Player player;
            try
            {
                player = await _dbContext.Players.FirstOrDefaultAsync(x => x.PlayerId == id);
                if (player == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
            PlayerUtility playerCard = new PlayerUtility(player);
            return View(playerCard);
        }

        //Add string to the params with will be what feild to be organised by. 
        private void QuickSortPlayersByOption(List<Player> players, string orderOption, int leftBound = 0, int rightBound = -1)
        {
            if (rightBound == -1) { rightBound = players.Count - 1; }

            if (leftBound < rightBound)
            {
                int pivoteIndex = Partition(players, leftBound, rightBound, orderOption);
                QuickSortPlayersByOption(players, orderOption, leftBound, pivoteIndex - 1);
                QuickSortPlayersByOption(players, orderOption, pivoteIndex, rightBound);
            }
            return;
        }
        private int Partition(List<Player> players, int leftIndex, int rightIndex,string orderOption)
        {
            int middleIndex = (rightIndex + leftIndex) / 2;
            Player middleElement = players[middleIndex];
            int pivot = middleElement.GetFeildByString(orderOption);

            while (leftIndex <= rightIndex)
            {
                while (players[leftIndex].GetFeildByString(orderOption) > pivot)//found some think in inner loops but continues to loop outer
                {
                    leftIndex++;
                }
                while (players[rightIndex].GetFeildByString(orderOption) < pivot)
                {
                    rightIndex--;
                }
                if (leftIndex <= rightIndex)
                {
                    Player temp = players[leftIndex];
                    players[leftIndex] = players[rightIndex];
                    players[rightIndex] = temp;
                    leftIndex++;
                    rightIndex--;
                }
            }

            return leftIndex;
        }
    }
}
