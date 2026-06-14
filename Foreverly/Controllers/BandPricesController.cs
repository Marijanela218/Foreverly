using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Foreverly.Data;
using Foreverly.Models;

namespace Foreverly.Controllers
{
    public class BandPricesController : Controller
    {
        private readonly AppDbContext _context;

        public BandPricesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: BandPrices
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.BandPrices.Include(b => b.Band);
            return View(await appDbContext.ToListAsync());
        }

        // GET: BandPrices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bandPrice = await _context.BandPrices
                .Include(b => b.Band)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bandPrice == null)
            {
                return NotFound();
            }

            return View(bandPrice);
        }

        // GET: BandPrices/Create
        public IActionResult Create()
        {
            ViewData["BandId"] = new SelectList(_context.Bands, "PartnerId", "PartnerId");
            return View();
        }

        // POST: BandPrices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BandId,DayOfWeek,DurationHours,Price")] BandPrice bandPrice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bandPrice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BandId"] = new SelectList(_context.Bands, "PartnerId", "PartnerId", bandPrice.BandId);
            return View(bandPrice);
        }

        // GET: BandPrices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bandPrice = await _context.BandPrices.FindAsync(id);
            if (bandPrice == null)
            {
                return NotFound();
            }
            ViewData["BandId"] = new SelectList(_context.Bands, "PartnerId", "PartnerId", bandPrice.BandId);
            return View(bandPrice);
        }

        // POST: BandPrices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BandId,DayOfWeek,DurationHours,Price")] BandPrice bandPrice)
        {
            if (id != bandPrice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bandPrice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BandPriceExists(bandPrice.Id))
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
            ViewData["BandId"] = new SelectList(_context.Bands, "PartnerId", "PartnerId", bandPrice.BandId);
            return View(bandPrice);
        }

        // GET: BandPrices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bandPrice = await _context.BandPrices
                .Include(b => b.Band)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bandPrice == null)
            {
                return NotFound();
            }

            return View(bandPrice);
        }

        // POST: BandPrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bandPrice = await _context.BandPrices.FindAsync(id);
            if (bandPrice != null)
            {
                _context.BandPrices.Remove(bandPrice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BandPriceExists(int id)
        {
            return _context.BandPrices.Any(e => e.Id == id);
        }
    }
}
