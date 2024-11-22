using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garage_3._0.Data;
using Garage_3._0.Models;
using Garage_3._0.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Garage_3._0.Controllers
{
    [Authorize(Roles = "Admin")]

    public class ParkingSpotsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ParkingSpotsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ParkingSpots
     
        public async Task<IActionResult> Index()
        {
            var viewModel = _context.ParkingSpots
                .Select(s => new ParkingSpotViewModel
                {
                    Id = s.Id,
                    SpotNumber = s.SpotNumber,
                    Size = s.Size,
                    Location = s.Location,
                    IsOccupied = s.IsOccupied

                    // AssignedVehicleRegistration = s.IsOccupied ? s.ParkedVehicle.RegistrationNumber : null 

                });

            return View(await viewModel.ToListAsync());
        }



        // GET: ParkingSpots/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingSpot = await _context.ParkingSpots
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parkingSpot == null)
            {
                return NotFound();
            }

            return View(parkingSpot);
        }

        // GET: ParkingSpots/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ParkingSpots/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ParkingSpotViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                var parkingSpot = new ParkingSpot
                {
                    SpotNumber = viewmodel.SpotNumber,
                    Size = viewmodel.Size,
                    Location = viewmodel.Location,
                    IsOccupied = viewmodel.IsOccupied
                };

                _context.ParkingSpots.Add(parkingSpot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewmodel);
        }
   

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var spot = await _context.ParkingSpots.FindAsync(id);
            if (spot == null) return NotFound();

            // Växlar status
            spot.IsOccupied = !spot.IsOccupied;
            _context.ParkingSpots.Update(spot);
            await _context.SaveChangesAsync();

            return Ok();
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var parkingSpot = await _context.ParkingSpots.FindAsync(id);
            if (parkingSpot == null) return NotFound();

            var viewModel = new ParkingSpotViewModel
            {
                Id = parkingSpot.Id,
                SpotNumber = parkingSpot.SpotNumber,
                Size = parkingSpot.Size,
                Location = parkingSpot.Location,
                IsOccupied = parkingSpot.IsOccupied
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ParkingSpotViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var parkingSpot = await _context.ParkingSpots.FindAsync(id);
                if (parkingSpot == null) return NotFound();

                parkingSpot.SpotNumber = model.SpotNumber;
                parkingSpot.Size = model.Size;
                parkingSpot.Location = model.Location;
                parkingSpot.IsOccupied = model.IsOccupied;

                _context.Update(parkingSpot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }


        // GET: ParkingSpots/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingSpot = await _context.ParkingSpots
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parkingSpot == null)
            {
                return NotFound();
            }

            return View(parkingSpot);
        }

        // POST: ParkingSpots/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parkingSpot = await _context.ParkingSpots.FindAsync(id);
            if (parkingSpot != null)
            {
                _context.ParkingSpots.Remove(parkingSpot);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParkingSpotExists(int id)
        {
            return _context.ParkingSpots.Any(e => e.Id == id);
        }
    }
}
