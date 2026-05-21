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
    public class WeddingServicesController : Controller
    {
        private readonly AppDbContext _context;

        public WeddingServicesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: WeddingServices
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.WeddingServices.Include(w => w.Partner).Include(w => w.Wedding);
            return View(await appDbContext.ToListAsync());
        }

        // GET: WeddingServices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weddingService = await _context.WeddingServices
                .Include(w => w.Partner)
                .Include(w => w.Wedding)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (weddingService == null)
            {
                return NotFound();
            }

            return View(weddingService);
        }

        // GET: WeddingServices/Create
        public IActionResult Create()
        {
            ViewData["PartnerId"] = new SelectList(_context.Partners, "Id", "Id");
            ViewData["WeddingId"] = new SelectList(_context.Weddings, "Id", "Id");
            return View();
        }

        // POST: WeddingServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WeddingId,PartnerId,ServiceType,Description,Quantity,UnitPrice,TotalPrice,CommissionPercent,CommissionAmount,Confirmed,SpecialRequest,Notes")] WeddingService weddingService)
        {
            if (ModelState.IsValid)
            {
                _context.Add(weddingService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PartnerId"] = new SelectList(_context.Partners, "Id", "Id", weddingService.PartnerId);
            ViewData["WeddingId"] = new SelectList(_context.Weddings, "Id", "Id", weddingService.WeddingId);
            return View(weddingService);
        }

        // GET: WeddingServices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weddingService = await _context.WeddingServices.FindAsync(id);
            if (weddingService == null)
            {
                return NotFound();
            }
            ViewData["PartnerId"] = new SelectList(_context.Partners, "Id", "Id", weddingService.PartnerId);
            ViewData["WeddingId"] = new SelectList(_context.Weddings, "Id", "Id", weddingService.WeddingId);
            return View(weddingService);
        }

        // POST: WeddingServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,WeddingId,PartnerId,ServiceType,Description,Quantity,UnitPrice,TotalPrice,CommissionPercent,CommissionAmount,Confirmed,SpecialRequest,Notes")] WeddingService weddingService)
        {
            if (id != weddingService.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(weddingService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WeddingServiceExists(weddingService.Id))
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
            ViewData["PartnerId"] = new SelectList(_context.Partners, "Id", "Id", weddingService.PartnerId);
            ViewData["WeddingId"] = new SelectList(_context.Weddings, "Id", "Id", weddingService.WeddingId);
            return View(weddingService);
        }

        // GET: WeddingServices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weddingService = await _context.WeddingServices
                .Include(w => w.Partner)
                .Include(w => w.Wedding)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (weddingService == null)
            {
                return NotFound();
            }

            return View(weddingService);
        }

        // POST: WeddingServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var weddingService = await _context.WeddingServices.FindAsync(id);
            if (weddingService != null)
            {
                _context.WeddingServices.Remove(weddingService);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WeddingServiceExists(int id)
        {
            return _context.WeddingServices.Any(e => e.Id == id);
        }
    }
}
