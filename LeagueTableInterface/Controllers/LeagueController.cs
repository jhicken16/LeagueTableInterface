using LeagueTableInterface.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeagueTableInterface.Controllers
{
   
    public class LeagueController : Controller
    { 
        private readonly ApplicationDBContext dbContext; 
        public LeagueController(ApplicationDBContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var leagues = await dbContext.Leagues.ToListAsync();
            return View(leagues);
        }
    }
}
