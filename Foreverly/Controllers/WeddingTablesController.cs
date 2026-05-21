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
    public class WeddingTablesController : Controller
    {
        private readonly AppDbContext _context;

        public WeddingTablesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: WeddingTables
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.WeddingTables.Include(w => w.Wedding);
            return View(await appDbContext.ToListAsync());
        }

        // GET: WeddingTables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weddingTable = await _context.WeddingTables
                .Include(w => w.Wedding)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (weddingTable == null)
            {
                return NotFound();
            }

            return View(weddingTable);
        }

        // GET: WeddingTables/Create
        public IActionResult Create()
        {
            ViewData["WeddingId"] = new SelectList(_context.Weddings, "Id", "Id");
            return View();
        }

        // POST: WeddingTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WeddingId,TableName,Capacity,PositionNote")] WeddingTable weddingTable)
        {
            if (ModelState.IsValid)
            {
                _context.Add(weddingTable);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["WeddingId"] = new SelectList(_context.Weddings, "Id", "Id", weddingTable.WeddingId);
            return View(weddingTable);
        }

        // GET: WeddingTables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weddingTable = await _context.WeddingTables.FindAsync(id);
            if (weddingTable == null)
            {
                return NotFound();
            }
            ViewData["WeddingId"] = new SelectList(_context.Weddings, "Id", "Id", weddingTable.WeddingId);
            return View(weddingTable);
        }

        // POST: WeddingTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,WeddingId,TableName,Capacity,PositionNote")] WeddingTable weddingTable)
        {
            if (id != weddingTable.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(weddingTable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WeddingTableExists(weddingTable.Id))
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
            ViewData["WeddingId"] = new SelectList(_context.Weddings, "Id", "Id", weddingTable.WeddingId);
            return View(weddingTable);
        }

        // GET: WeddingTables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weddingTable = await _context.WeddingTables
                .Include(w => w.Wedding)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (weddingTable == null)
            {
                return NotFound();
            }

            return View(weddingTable);
        }

        // POST: WeddingTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var weddingTable = await _context.WeddingTables.FindAsync(id);
            if (weddingTable != null)
            {
                _context.WeddingTables.Remove(weddingTable);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WeddingTableExists(int id)
        {
            return _context.WeddingTables.Any(e => e.Id == id);
        }
    }
}
