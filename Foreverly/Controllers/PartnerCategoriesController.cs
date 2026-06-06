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
    public class PartnerCategoriesController : Controller
    {
        private readonly AppDbContext _context;

        public PartnerCategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PartnerCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.PartnerCategories.ToListAsync());
        }

        // GET: PartnerCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partnerCategory = await _context.PartnerCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (partnerCategory == null)
            {
                return NotFound();
            }

            return View(partnerCategory);
        }

        // GET: PartnerCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PartnerCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] PartnerCategory partnerCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(partnerCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(partnerCategory);
        }

        // GET: PartnerCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partnerCategory = await _context.PartnerCategories.FindAsync(id);
            if (partnerCategory == null)
            {
                return NotFound();
            }
            return View(partnerCategory);
        }

        // POST: PartnerCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] PartnerCategory partnerCategory)
        {
            if (id != partnerCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(partnerCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartnerCategoryExists(partnerCategory.Id))
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
            return View(partnerCategory);
        }

        // GET: PartnerCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partnerCategory = await _context.PartnerCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (partnerCategory == null)
            {
                return NotFound();
            }

            return View(partnerCategory);
        }

        // POST: PartnerCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var partnerCategory = await _context.PartnerCategories.FindAsync(id);
            if (partnerCategory != null)
            {
                _context.PartnerCategories.Remove(partnerCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PartnerCategoryExists(int id)
        {
            return _context.PartnerCategories.Any(e => e.Id == id);
        }
    }
}
