using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Garage_3._0.Data;
using Garage_3._0.Models.ViewModels;


namespace Garage_3._0.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]/[action]")]
    public class StatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("Statistics/ParkingStatistics")]
        public async Task<IActionResult> ParkingStatistics()
        {
            // Count total parking spots
            int totalParkingSpots = await _context.ParkingSpots.CountAsync();

            // Count occupied and available parking spots
            int occupiedParkingSpots = await _context.ParkingSpots.CountAsync(p => p.IsOccupied);
            int availableParkingSpots = await _context.ParkingSpots.CountAsync(p => !p.IsOccupied);

            // Count parking spots by size
            var sizeCounts = await _context.ParkingSpots
                .GroupBy(p => p.Size)
                .Select(g => new { Size = g.Key, Total = g.Count(), Occupied = g.Count(p => p.IsOccupied) })
                .ToListAsync();

            var viewModel = new ParkingStatisticsViewModel
            {
                TotalParkingSpots = totalParkingSpots,
                OccupiedParkingSpots = occupiedParkingSpots,
                AvailableParkingSpots = availableParkingSpots,
                OccupiedPercentage = totalParkingSpots > 0 ? Math.Round((double)occupiedParkingSpots / totalParkingSpots * 100, 2) : 0,
                AvailablePercentage = totalParkingSpots > 0 ? Math.Round((double)availableParkingSpots / totalParkingSpots * 100, 2) : 0,
                SizeStatistics = sizeCounts.Select(sc => new SizeStatistic
                {
                    Size = sc.Size,
                    TotalSpots = sc.Total,
                    OccupiedSpots = sc.Occupied,
                    AvailableSpots = sc.Total - sc.Occupied
                }).ToList()
            };

            return View(viewModel);
        }
    }
}
