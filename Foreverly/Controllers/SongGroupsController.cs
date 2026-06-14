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
    public class SongGroupsController : Controller
    {
        private readonly AppDbContext _context;

        public SongGroupsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: SongGroups
        public async Task<IActionResult> Index()
        {
            return View(await _context.SongGroups.ToListAsync());
        }

        // GET: SongGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var songGroup = await _context.SongGroups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (songGroup == null)
            {
                return NotFound();
            }

            return View(songGroup);
        }

        // GET: SongGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SongGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] SongGroup songGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(songGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(songGroup);
        }

        // GET: SongGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var songGroup = await _context.SongGroups.FindAsync(id);
            if (songGroup == null)
            {
                return NotFound();
            }
            return View(songGroup);
        }

        // POST: SongGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] SongGroup songGroup)
        {
            if (id != songGroup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(songGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SongGroupExists(songGroup.Id))
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
            return View(songGroup);
        }

        // GET: SongGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var songGroup = await _context.SongGroups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (songGroup == null)
            {
                return NotFound();
            }

            return View(songGroup);
        }

        // POST: SongGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var songGroup = await _context.SongGroups.FindAsync(id);
            if (songGroup != null)
            {
                _context.SongGroups.Remove(songGroup);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SongGroupExists(int id)
        {
            return _context.SongGroups.Any(e => e.Id == id);
        }
    }
}
