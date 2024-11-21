﻿using System;
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
    public class ParkedVehiclesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ParkedVehiclesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ParkedVehicles
        public async Task<IActionResult> Index()
        {
            var viewModel = _context.ParkedVehicles.Select(p => new ParkedVehicleIndexViewModel
            {
                RegistrationNumber = p.RegistrationNumber,
                ApplicationUser = p.ApplicationUser,
                ParkingSpot = p.ParkingSpot,
                VehicleType = p.VehicleType
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
        public async Task<IActionResult> Create(ParkedVehicleCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Log all validation errors to the console
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine("ModelState Error: " + error.ErrorMessage);
                }

                ModelState.AddModelError("", "There are some errors in the form. Please correct them and try again.");

                // Re-populate dropdown lists
                ViewBag.ParkingSpots = new SelectList(_context.ParkingSpots.Where(s => !s.IsOccupied), "Id", "SpotNumber");
                ViewBag.VehicleTypes = new SelectList(_context.VehicleTypes, "Id", "Name");
                return View(model);
            }



            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if the vehicle is already parked
                    var existingVehicle = await _context.ParkedVehicles
                        .FirstOrDefaultAsync(v => v.RegistrationNumber == model.RegistrationNumber);
                    if (existingVehicle != null)
                    {
                        // Add model error if the vehicle is already parked
                        ModelState.AddModelError("", "A vehicle with this registration number is already parked.");

                        // Re-populate dropdown lists in case of validation failure
                        ViewBag.ParkingSpots = new SelectList(_context.ParkingSpots.Where(s => !s.IsOccupied), "Id", "SpotNumber");
                        ViewBag.VehicleTypes = new SelectList(_context.VehicleTypes, "Id", "Name");
                        return View(model);
                    }

                    // Retrieve vehicle type and parking spot information by ID
                    var vehicleType = await _context.VehicleTypes.FirstOrDefaultAsync(v => v.Id == model.VehicleTypeId);
                    var parkingSpot = await _context.ParkingSpots.FirstOrDefaultAsync(p => p.Id == model.ParkingSpotId);

                    // Validate the selected vehicle type and parking spot
                    if (parkingSpot == null || vehicleType == null)
                    {
                        // Add model error if either parking spot or vehicle type is invalid
                        ModelState.AddModelError("", "Invalid parking spot or vehicle type selected.");

                        // Re-populate dropdown lists in case of validation failure
                        ViewBag.ParkingSpots = new SelectList(_context.ParkingSpots.Where(s => !s.IsOccupied), "Id", "SpotNumber");
                        ViewBag.VehicleTypes = new SelectList(_context.VehicleTypes, "Id", "Name");
                        return View(model);
                    }

                    // Create and save the parked vehicle
                    var parkedVehicle = new ParkedVehicle
                    {
                        RegistrationNumber = model.RegistrationNumber,
                        Model = model.Model,
                        Brand = model.Brand,
                        Color = model.Color,
                        VehicleType = vehicleType,
                        ParkingSpot = parkingSpot,
                        ApplicationUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name)
                    };

                    _context.Add(parkedVehicle);

                    // Update the parking spot status to occupied
                    parkingSpot.IsOccupied = true;
                    _context.Update(parkingSpot);

                    Console.WriteLine("Before SaveChangesAsync");
                    await _context.SaveChangesAsync(); // Save changes to the database
                    Console.WriteLine("After SaveChangesAsync");

                    // Redirect to the index view after successful creation
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
            return View(model);
        }








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
    }
}
