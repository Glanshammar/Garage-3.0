using Garage_3._0.Data;
using Garage_3._0.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Garage_3._0.Models;


namespace Garage_3._0.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Member")]
        [HttpGet]
        public async Task<IActionResult> AvailableParkingSpots(string size, string location)
        {
            var query = _context.ParkingSpots.Where(p => !p.IsOccupied);

            if (!string.IsNullOrWhiteSpace(size))
                query = query.Where(p => p.Size.Contains(size));

            if (!string.IsNullOrWhiteSpace(location))
                query = query.Where(p => p.Location.Contains(location));

            var spots = await query.Select(p => new ParkingSpotViewModel
            {
                Id = p.Id,
                SpotNumber = p.SpotNumber,
                Size = p.Size,
                Location = p.Location,
                ParkingCost = p.ParkingCost
            }).ToListAsync();

            return Json(new { spots });
        }

       
    }
}
