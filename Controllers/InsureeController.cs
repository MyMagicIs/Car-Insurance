using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarInsurance.Models;

namespace CarInsurance.Controllers
{
    public class InsureeController : Controller
    {
        private readonly InsuranceEntities _context;

        public InsureeController(InsuranceEntities context)
        {
            _context = context;
        }
        // Admin View
        public async Task<IActionResult> Admin()
        {
            var allInsurees = await _context.Insurees.ToListAsync();
            return View(allInsurees);
        }

        // GET: Insuree
        public async Task<IActionResult> Index()
        {
            return View(await _context.Insurees.ToListAsync());
        }

        // GET: Insuree/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insuree = await _context.Insurees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insuree == null)
            {
                return NotFound();
            }

            return View(insuree);
        }

        // GET: Insuree/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Insuree/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                insuree.Quote = CalculateQuote(insuree); //  Calculate the quote

                _context.Add(insuree);  //  Save to database
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));  // Redirect to Index page
            }
            return View(insuree);  //  Return the view if ModelState is invalid
        }

        // GET: Insuree/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insuree = await _context.Insurees.FindAsync(id);
            if (insuree == null)
            {
                return NotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (id != insuree.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(insuree);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsureeExists(insuree.Id))
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
            return View(insuree);
        }

        // GET: Insuree/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insuree = await _context.Insurees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insuree == null)
            {
                return NotFound();
            }

            return View(insuree);
        }

        // POST: Insuree/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var insuree = await _context.Insurees.FindAsync(id);
            if (insuree != null)
            {
                _context.Insurees.Remove(insuree);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private decimal CalculateQuote(Insuree insuree)
        {
            decimal baseQuote = 50m;

            int age = DateTime.Now.Year - insuree.DateOfBirth.Year;
            if (insuree.DateOfBirth > DateTime.Now.AddYears(-age)) age--; // Accurate birthday check

            // a-d: Age rules
            if (age <= 18)
            {
                baseQuote += 100m;
            }
            else if (age >= 19 && age <= 25)
            {
                baseQuote += 50m;
            }
            else
            {
                baseQuote += 25m;
            }

            // e: Car year before 2000
            if (insuree.CarYear < 2000)
            {
                baseQuote += 25m;
            }

            // f: Car year after 2015
            if (insuree.CarYear > 2015)
            {
                baseQuote += 25m;
            }

            // g-h: Porsche and Porsche 911 Carrera
            if (insuree.CarMake != null && insuree.CarMake.ToLower() == "porsche")
            {
                baseQuote += 25m;
                if (insuree.CarModel != null && insuree.CarModel.ToLower() == "911 carrera")
                {
                    baseQuote += 25m;
                }
            }

            // i: Speeding tickets
            baseQuote += insuree.SpeedingTickets * 10;

            // j: DUI
            if (insuree.DUI)
            {
                baseQuote *= 1.25m; // 25% increase
            }

            // k: Full coverage
            if (insuree.CoverageType)
            {
                baseQuote *= 1.50m; // 50% increase
            }

            return baseQuote;
        }


        private bool InsureeExists(int id)
        {
            return _context.Insurees.Any(e => e.Id == id);
        }
    }
}
