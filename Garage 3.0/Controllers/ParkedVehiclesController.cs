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
                ParkingSpotNumber = p.ParkingSpot.SpotNumber,
                ArrivalTime = p.ArrivalTime
            });

            return View(await viewModel.ToListAsync());
        }


        

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> SearchAvailableSpots(string size, string location)
        {
            var query = _context.ParkingSpots.Where(p => !p.IsOccupied);

            if (!string.IsNullOrWhiteSpace(size))
                query = query.Where(p => p.Size.Contains(size));

            if (!string.IsNullOrWhiteSpace(location))
                query = query.Where(p => p.Location.Contains(location));

            var spots = await query.Select(p => new
            {
                id = p.Id,
                spotNumber = p.SpotNumber,
                size = p.Size,
                location = p.Location,
                parkingCost = p.ParkingCost
            }).ToListAsync();

            return Json(new { spots });
        }


        // GET: ParkedVehicles/Create
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Create()
        {
            // Add available vehicle types to ViewBag for dropdown
            ViewBag.VehicleTypes = new SelectList(_context.VehicleTypes, "Id", "Name");


            // Get available sizes and locations for search filters
            ViewBag.Sizes = await _context.ParkingSpots
                .Select(p => p.Size)
                .Distinct()
                .ToListAsync();

            ViewBag.Locations = await _context.ParkingSpots
                .Select(p => p.Location)
                .Distinct()
                .ToListAsync();

            return View();
        }



        // POST: ParkedVehicles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Create(ParkedVehicleCreateViewModel vehicleViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if the vehicle is already parked
                    var existingVehicle = await _context.ParkedVehicles
                        .FirstOrDefaultAsync(v => v.RegistrationNumber == vehicleViewModel.RegistrationNumber);
                    if (existingVehicle != null)
                    {
                        ModelState.AddModelError("", "A vehicle with this registration number is already parked.");
                        //ViewBag.ParkingSpots = new SelectList(_context.ParkingSpots.Where(s => !s.IsOccupied), "Id", "SpotNumber");
                        //ViewBag.VehicleTypes = new SelectList(_context.VehicleTypes, "Id", "Name");
                        await PrepareViewBagForCreate();
                        return View(vehicleViewModel);
                    }

                    // Retrieve the logged-in user's information
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

                    // Check if the user is over 18 years old
                    if (user.Age < 18)
                    {
                        ModelState.AddModelError("", "Only users over 18 years old can park vehicles.");
                        //ViewBag.ParkingSpots = new SelectList(_context.ParkingSpots.Where(s => !s.IsOccupied), "Id", "SpotNumber");
                        //ViewBag.VehicleTypes = new SelectList(_context.VehicleTypes, "Id", "Name");
                        await PrepareViewBagForCreate();
                        return View(vehicleViewModel);
                    }

                    // Retrieve vehicle type and parking spot based on the provided ID
                    var vehicleType = await _context.VehicleTypes.FirstOrDefaultAsync(v => v.Id == vehicleViewModel.VehicleTypeId);
                    var parkingSpot = await _context.ParkingSpots.FirstOrDefaultAsync(p => p.Id == vehicleViewModel.ParkingSpotId);


                    // Validate the selected vehicle type and parking spot
                    if (parkingSpot == null || vehicleType == null || parkingSpot.IsOccupied)
                    {

                        ModelState.AddModelError("", "Selected parking spot is no longer available.");
                        //ViewBag.ParkingSpots = new SelectList(_context.ParkingSpots.Where(s => !s.IsOccupied), "Id", "SpotNumber");
                        //ViewBag.VehicleTypes = new SelectList(_context.VehicleTypes, "Id", "Name");
                        await PrepareViewBagForCreate();
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
                        ArrivalTime = DateTime.Now,
                        ApplicationUser = user
                    };

                    _context.Add(parkedVehicle);

                    // Update the parking spot status to indicate that it is now occupied
                    parkingSpot.IsOccupied = true;
                    _context.Update(parkingSpot);

                    Console.WriteLine("Before SaveChangesAsync");
                    await _context.SaveChangesAsync(); // Save changes to the database
                    Console.WriteLine("After SaveChangesAsync");

                    // Redirect to the Index view after successful creation
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occurred: {ex.Message}");
                    ModelState.AddModelError("", "An unexpected error occurred while trying to park the vehicle.");
                }
            }
            else
            {
                ModelState.AddModelError("", "There are some errors in the form. Please correct them and try again.");
            }

            //ViewBag.ParkingSpots = new SelectList(_context.ParkingSpots.Where(s => !s.IsOccupied), "Id", "SpotNumber");
            //ViewBag.VehicleTypes = new SelectList(_context.VehicleTypes, "Id", "Name");
            await PrepareViewBagForCreate();
            return View(vehicleViewModel);
        }

        private async Task PrepareViewBagForCreate()
        {
            ViewBag.Sizes = await _context.ParkingSpots
                .Select(p => p.Size)
                .Distinct()
                .ToListAsync();

            ViewBag.Locations = await _context.ParkingSpots
                .Select(p => p.Location)
                .Distinct()
                .ToListAsync();

            ViewBag.VehicleTypes = new SelectList(_context.VehicleTypes, "Id", "Name");
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
            // Load all members and their parked vehicles
            var membersData = await _context.Users
                .Include(u => u.ParkedVehicles)
                .ThenInclude(v => v.VehicleType)
                .Include(u => u.ParkedVehicles)
                .ThenInclude(v => v.ParkingSpot) 
                .ToListAsync();

            // Calculate member data in-memory
            var members = membersData.Select(u => new MemberOverviewViewModel
            {
                MemberId = u.Id,
                FullName = $"{u.FirstName} {u.LastName}",
                NumberOfRegisteredVehicles = u.ParkedVehicles.Count,
                TotalParkingCost = u.ParkedVehicles.Sum(v => (decimal)Math.Ceiling((DateTime.Now - v.ArrivalTime).TotalHours) * 20),
                RegisteredVehicles = u.ParkedVehicles.Select(v => new VehicleDetailsViewModel
                {
                    RegistrationNumber = v.RegistrationNumber,
                    Model = v.Model,
                    Brand = v.Brand,
                    Color = v.Color,
                    VehicleType = v.VehicleType.Name,
                    ParkingSpotNumber = v.ParkingSpot != null ? v.ParkingSpot.SpotNumber : "N/A",
                    CurrentParkingCost = (decimal)Math.Ceiling((DateTime.Now - v.ArrivalTime).TotalHours) * 20
                }).ToList()
            }).ToList();

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

            // Load the user and related data
            var user = await _context.Users
                .Include(u => u.ParkedVehicles)
                .ThenInclude(v => v.VehicleType)
                .Include(u => u.ParkedVehicles)
                .ThenInclude(v => v.ParkingSpot)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            // Calculate the total parking cost and other properties in-memory
            var member = new MemberOverviewViewModel
            {
                MemberId = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                NumberOfRegisteredVehicles = user.ParkedVehicles.Count,
                TotalParkingCost = user.ParkedVehicles.Sum(v => (decimal)Math.Ceiling((DateTime.Now - v.ArrivalTime).TotalHours) * 20),
                RegisteredVehicles = user.ParkedVehicles.Select(v => new VehicleDetailsViewModel
                {
                    RegistrationNumber = v.RegistrationNumber,
                    Model = v.Model,
                    Brand = v.Brand,
                    Color = v.Color,
                    VehicleType = v.VehicleType.Name,
                    ArrivalTime = v.ArrivalTime,
                    ParkingSpotNumber = v.ParkingSpot != null ? v.ParkingSpot.SpotNumber : "N/A",
                    CurrentParkingCost = (decimal)Math.Ceiling((DateTime.Now - v.ArrivalTime).TotalHours) * 20
                }).ToList()
            };

            return View(member);
        }


    }
}
