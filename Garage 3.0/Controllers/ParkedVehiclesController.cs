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
using Microsoft.AspNetCore.Identity;

namespace Garage_3._0.Controllers
{
    [Authorize]
    public class ParkedVehiclesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ParkedVehiclesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ParkedVehicles
        public async Task<IActionResult> Index()
        {
            // Get the currently logged-in user's ID
            var currentUserId = _userManager.GetUserId(User);

            // Check if the currently logged-in user is an Admin
            var isAdmin = User.IsInRole("Admin");

            IQueryable<ParkedVehicle> parkedVehicles;

            // If the user is an Admin, they should see all vehicles
            if (isAdmin)
            {
                parkedVehicles = _context.ParkedVehicles;
            }
            else
            {
                // Otherwise, only show vehicles belonging to the logged-in user
                parkedVehicles = _context.ParkedVehicles.Where(p => p.ApplicationUserId == currentUserId);
            }

            // Create the view model
            var viewModel = parkedVehicles.Select(p => new ParkedVehicleIndexViewModel
            {
                Id = p.Id,
                RegistrationNumber = p.RegistrationNumber,
                VehicleTypeName = p.VehicleType.Name,
                OwnerName = $"{p.ApplicationUser.FirstName} {p.ApplicationUser.LastName}",
                ParkingSpotNumber = p.ParkingSpot.SpotNumber
            });

            return View(await viewModel.ToListAsync());
        }



        // GET: ParkedVehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkedVehicle = await _context.ParkedVehicles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parkedVehicle == null)
            {
                return NotFound();
            }

            return View(parkedVehicle);
        }
       

        // GET: ParkedVehicles/Create
        [Authorize(Roles = "Admin,User")]
        public IActionResult Create()
        {
            // Add available parking spots to ViewBag for dropdown
            ViewBag.ParkingSpots = new SelectList(_context.ParkingSpots.Where(s => !s.IsOccupied), "Id", "SpotNumber");

            // Add available vehicle types to ViewBag for dropdown
            ViewBag.VehicleTypes = new SelectList(_context.VehicleTypes, "Id", "Name");

            return View();
        }

        // POST: ParkedVehicles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Create(ParkedVehicleCreateViewModel vehicleViewModel)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if the vehicle is already parked
                    var existingVehicle = await _context.ParkedVehicles
                        .FirstOrDefaultAsync(v => v.RegistrationNumber == vehicleViewModel.RegistrationNumber);
                    if (existingVehicle != null)
                    {
                        // Add model error if the vehicle is already parked
                        ModelState.AddModelError("", "A vehicle with this registration number is already parked.");

                        // Re-populate dropdown lists in case of validation failure
                        ViewBag.ParkingSpots = new SelectList(_context.ParkingSpots.Where(s => !s.IsOccupied), "Id", "SpotNumber");
                        ViewBag.VehicleTypes = new SelectList(_context.VehicleTypes, "Id", "Name");
                        return View(vehicleViewModel);
                    }

                    // Retrieve the logged-in user's information
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

                    // Check if the user is over 18 years old
                    if (user.Age < 18)
                    {
                        // Add model error if the user is not allowed to park a vehicle
                        ModelState.AddModelError("", "Only users over 18 years old can park vehicles.");

                        // Re-populate dropdown lists in case of validation failure
                        ViewBag.ParkingSpots = new SelectList(_context.ParkingSpots.Where(s => !s.IsOccupied), "Id", "SpotNumber");
                        ViewBag.VehicleTypes = new SelectList(_context.VehicleTypes, "Id", "Name");
                        return View(vehicleViewModel);
                    }

                    // Retrieve vehicle type and parking spot based on the provided ID
                    var vehicleType = await _context.VehicleTypes.FirstOrDefaultAsync(v => v.Id == vehicleViewModel.VehicleTypeId);
                    var parkingSpot = await _context.ParkingSpots.FirstOrDefaultAsync(p => p.Id == vehicleViewModel.ParkingSpotId);

                    // Validate the selected vehicle type and parking spot
                    if (parkingSpot == null || vehicleType == null)
                    {
                        // Add model error if either parking spot or vehicle type is invalid
                        ModelState.AddModelError("", "Invalid parking spot or vehicle type selected.");

                        // Re-populate dropdown lists in case of validation failure
                        ViewBag.ParkingSpots = new SelectList(_context.ParkingSpots.Where(s => !s.IsOccupied), "Id", "SpotNumber");
                        ViewBag.VehicleTypes = new SelectList(_context.VehicleTypes, "Id", "Name");
                        return View(vehicleViewModel);
                    }

                    // Create a new ParkedVehicle instance and set its properties
                    var parkedVehicle = new ParkedVehicle
                    {
                        RegistrationNumber = vehicleViewModel.RegistrationNumber,
                        Model = vehicleViewModel.Model,
                        Brand = vehicleViewModel.Brand,
                        Color = vehicleViewModel.Color,
                        VehicleType = vehicleType,
                        ParkingSpot = parkingSpot,
                        ApplicationUser = user // Set the owner of the parked vehicle
                    };

                    // Add the parked vehicle to the database context
                    _context.Add(parkedVehicle);

                    // Update the parking spot status to indicate that it is now occupied
                    parkingSpot.IsOccupied = true;
                    _context.Update(parkingSpot);

                    Console.WriteLine("Before SaveChangesAsync");
                    // Save changes to the database
                    await _context.SaveChangesAsync();
                    Console.WriteLine("After SaveChangesAsync");

                    // Redirect to the Index view after successful creation
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log the exception and add a general error message to ModelState
                    Console.WriteLine($"Exception occurred: {ex.Message}");
                    ModelState.AddModelError("", "An unexpected error occurred while trying to park the vehicle.");
                }
            }
            else
            {
                // Add a general error message if ModelState is not valid
                ModelState.AddModelError("", "There are some errors in the form. Please correct them and try again.");
            }

            // If something goes wrong, re-populate dropdown lists and return the view with the model
            ViewBag.ParkingSpots = new SelectList(_context.ParkingSpots.Where(s => !s.IsOccupied), "Id", "SpotNumber");
            ViewBag.VehicleTypes = new SelectList(_context.VehicleTypes, "Id", "Name");
            return View(vehicleViewModel);
        }






        [Authorize(Roles = "Admin")]
        // GET: ParkedVehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkedVehicle = await _context.ParkedVehicles.FindAsync(id);
            if (parkedVehicle == null)
            {
                return NotFound();
            }
            return View(parkedVehicle);
        }

        // POST: ParkedVehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RegistrationNumber,Model,Brand,Color")] ParkedVehicle parkedVehicle)
        {
            if (id != parkedVehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parkedVehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParkedVehicleExists(parkedVehicle.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(parkedVehicle);
        }

        [Authorize(Roles = "Admin")]
        // GET: ParkedVehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkedVehicle = await _context.ParkedVehicles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parkedVehicle == null)
            {
                return NotFound();
            }

            return View(parkedVehicle);
        }

        [Authorize(Roles = "Admin")]
        // POST: ParkedVehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parkedVehicle = await _context.ParkedVehicles.FindAsync(id);
            if (parkedVehicle != null)
            {
                _context.ParkedVehicles.Remove(parkedVehicle);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParkedVehicleExists(int id)
        {
            return _context.ParkedVehicles.Any(e => e.Id == id);
        }




        // GET: Members/Overview
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Overview()
        {
            // Hämta alla medlemmar och deras fordon
            var members = await _context.Users
                .Include(u => u.ParkedVehicles)
                .ThenInclude(v => v.VehicleType)
                .Select(u => new MemberOverviewViewModel
                {
                    MemberId = u.Id,
                    FullName = $"{u.FirstName} {u.LastName}",
                    NumberOfRegisteredVehicles = u.ParkedVehicles.Count,
                    TotalParkingCost = u.ParkedVehicles.Sum(v => v.ParkingSpot != null ? v.ParkingSpot.ParkingCost : 0),
                    RegisteredVehicles = u.ParkedVehicles.Select(v => new VehicleDetailsViewModel
                    {
                        RegistrationNumber = v.RegistrationNumber,
                        Model = v.Model,
                        Brand = v.Brand,
                        Color = v.Color,
                        VehicleType = v.VehicleType.Name,
                        CurrentParkingCost = v.ParkingSpot != null ? v.ParkingSpot.ParkingCost : 0
                    }).ToList()
                }).ToListAsync();

            return View(members);
        }

        // GET: Members/Details/{id}
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Users
                .Include(u => u.ParkedVehicles)
                .ThenInclude(v => v.VehicleType)
                .Select(u => new MemberOverviewViewModel
                {
                    MemberId = u.Id,
                    FullName = $"{u.FirstName} {u.LastName}",
                    NumberOfRegisteredVehicles = u.ParkedVehicles.Count,
                    TotalParkingCost = u.ParkedVehicles.Sum(v => v.ParkingSpot != null ? v.ParkingSpot.ParkingCost : 0),
                    RegisteredVehicles = u.ParkedVehicles.Select(v => new VehicleDetailsViewModel
                    {
                        RegistrationNumber = v.RegistrationNumber,
                        Model = v.Model,
                        Brand = v.Brand,
                        Color = v.Color,
                        VehicleType = v.VehicleType.Name,
                        CurrentParkingCost = v.ParkingSpot != null ? v.ParkingSpot.ParkingCost : 0
                    }).ToList()
                })
                .FirstOrDefaultAsync(m => m.MemberId == id);

            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }



    }
}
