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
    public class PartnerBookingsController : Controller
    {
        private readonly AppDbContext _context;

        public PartnerBookingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PartnerBookings
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.PartnerBookings.Include(p => p.Partner).Include(p => p.Wedding);
            return View(await appDbContext.ToListAsync());
        }

        // GET: PartnerBookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partnerBooking = await _context.PartnerBookings
                .Include(p => p.Partner)
                .Include(p => p.Wedding)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (partnerBooking == null)
            {
                return NotFound();
            }

            return View(partnerBooking);
        }

        // GET: PartnerBookings/Create
        public IActionResult Create()
        {
            ViewData["PartnerId"] = new SelectList(_context.Partners, "Id", "Id");
            ViewData["WeddingId"] = new SelectList(_context.Weddings, "Id", "Id");
            return View();
        }

        // POST: PartnerBookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PartnerId,WeddingId,StartDateTime,EndDateTime,Status")] PartnerBooking partnerBooking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(partnerBooking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PartnerId"] = new SelectList(_context.Partners, "Id", "Id", partnerBooking.PartnerId);
            ViewData["WeddingId"] = new SelectList(_context.Weddings, "Id", "Id", partnerBooking.WeddingId);
            return View(partnerBooking);
        }

        // GET: PartnerBookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partnerBooking = await _context.PartnerBookings.FindAsync(id);
            if (partnerBooking == null)
            {
                return NotFound();
            }
            ViewData["PartnerId"] = new SelectList(_context.Partners, "Id", "Id", partnerBooking.PartnerId);
            ViewData["WeddingId"] = new SelectList(_context.Weddings, "Id", "Id", partnerBooking.WeddingId);
            return View(partnerBooking);
        }

        // POST: PartnerBookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PartnerId,WeddingId,StartDateTime,EndDateTime,Status")] PartnerBooking partnerBooking)
        {
            if (id != partnerBooking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(partnerBooking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartnerBookingExists(partnerBooking.Id))
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
            ViewData["PartnerId"] = new SelectList(_context.Partners, "Id", "Id", partnerBooking.PartnerId);
            ViewData["WeddingId"] = new SelectList(_context.Weddings, "Id", "Id", partnerBooking.WeddingId);
            return View(partnerBooking);
        }

        // GET: PartnerBookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partnerBooking = await _context.PartnerBookings
                .Include(p => p.Partner)
                .Include(p => p.Wedding)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (partnerBooking == null)
            {
                return NotFound();
            }

            return View(partnerBooking);
        }

        // POST: PartnerBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var partnerBooking = await _context.PartnerBookings.FindAsync(id);
            if (partnerBooking != null)
            {
                _context.PartnerBookings.Remove(partnerBooking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PartnerBookingExists(int id)
        {
            return _context.PartnerBookings.Any(e => e.Id == id);
        }
    }
}
