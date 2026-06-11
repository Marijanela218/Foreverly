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
    public class PartnersController : Controller
    {
        private readonly AppDbContext _context;

        public PartnersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Partners
        public async Task<IActionResult> Index()
        {
            var partners = await _context.Partners
                .Include(p => p.Category)
                .ToListAsync();

            return View(partners);
        }

        // GET: Partners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partner = await _context.Partners
                .Include(p => p.Category)

                .Include(p => p.Band)
                    .ThenInclude(b => b.Playlists)

                .Include(p => p.Band)
                    .ThenInclude(b => b.BandPrices)

                .Include(p => p.Restaurant)
                    .ThenInclude(r => r.Halls)

                .Include(p => p.Restaurant)
                    .ThenInclude(r => r.Menus)
                        .ThenInclude(m => m.MenuItems)

                .Include(p => p.FloralArrangements)
                .Include(p => p.PastryItems)

                .FirstOrDefaultAsync(m => m.Id == id);

            if (partner == null)
            {
                return NotFound();
            }

            return View(partner);
        }

        // GET: Partners/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.PartnerCategories, "Id", "Name");
            return View();
        }

        // POST: Partners/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
    [Bind("CategoryId,Name,Address,Phone,Email,ContactPerson,DefaultCommissionPercent,Notes")]
    Partner partner)
        {
            if (ModelState.IsValid)
            {
                _context.Partners.Add(partner);
                await _context.SaveChangesAsync();

                var category = await _context.PartnerCategories
                    .FirstOrDefaultAsync(c => c.Id == partner.CategoryId);

                var categoryName = category?.Name ?? "";

                if (categoryName.Contains("Restoran") || categoryName.Contains("Dvorana"))
                {
                    var restaurant = new Restaurant
                    {
                        PartnerId = partner.Id,
                        HasWeddingHall = true,
                        OffersCatering = true
                    };

                    _context.Restaurants.Add(restaurant);
                    await _context.SaveChangesAsync();

                    _context.Halls.Add(new Hall
                    {
                        RestaurantId = partner.Id,
                        Name = "Wedding Hall",
                        Capacity = 150,
                        Address = partner.Address,
                        BasePrice = 2000
                    });

                    var menu = new Menu
                    {
                        RestaurantId = partner.Id,
                        Name = "Recommended Wedding Menu",
                        Description = "Standard wedding menu",
                        PricePerPerson = 55
                    };

                    _context.Menus.Add(menu);
                    await _context.SaveChangesAsync();

                    _context.MenuItems.AddRange(
                        new MenuItem { MenuId = menu.Id, Name = "Soup", Description = "Starter soup" },
                        new MenuItem { MenuId = menu.Id, Name = "Main dish", Description = "Meat, side dish and salad" },
                        new MenuItem { MenuId = menu.Id, Name = "Dessert", Description = "Cake or dessert" }
                    );
                }

                else if (categoryName.Contains("Slastičarnica"))
                {
                    _context.PastryItems.Add(new PastryItem
                    {
                        PartnerId = partner.Id,
                        Name = "Wedding Cake",
                        Type = "Cake",
                        Description = "Classic wedding cake",
                        BasePrice = 300
                    });
                }

                else if (categoryName.Contains("Cvjećara") || categoryName.Contains("Cvjećar"))
                {
                    _context.FloralArrangements.Add(new FloralArrangement
                    {
                        PartnerId = partner.Id,
                        Name = "Table Decoration",
                        Description = "Floral table arrangement",
                        BasePrice = 50
                    });
                }

                else if (categoryName.Contains("Bend") || categoryName.Contains("DJ"))
                {
                    var band = new Band
                    {
                        PartnerId = partner.Id,
                        Description = "Wedding music band / DJ"
                    };

                    _context.Bands.Add(band);
                    await _context.SaveChangesAsync();

                    _context.BandPrices.Add(new BandPrice
                    {
                        BandId = partner.Id,
                        DayOfWeek = "Saturday",
                        DurationHours = 8,
                        Price = 2500
                    });

                    _context.Playlists.Add(new Playlist
                    {
                        BandId = partner.Id,
                        Name = "Wedding Playlist",
                        Description = "Popular wedding songs"
                    });
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(
                _context.PartnerCategories,
                "Id",
                "Name",
                partner.CategoryId
            );

            return View(partner);
        }

        // GET: Partners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partner = await _context.Partners.FindAsync(id);
            if (partner == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.PartnerCategories, "Id", "Name", partner.CategoryId);
            return View(partner);
        }

        // POST: Partners/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryId,Name,Address,Phone,Email,ContactPerson,DefaultCommissionPercent,Notes")] Partner partner)
        {
            if (id != partner.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(partner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartnerExists(partner.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.PartnerCategories, "Id", "Name", partner.CategoryId);
            return View(partner);
        }

        // GET: Partners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partner = await _context.Partners
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (partner == null)
            {
                return NotFound();
            }

            return View(partner);
        }

        // POST: Partners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var partner = await _context.Partners.FindAsync(id);
            if (partner != null)
            {
                _context.Partners.Remove(partner);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PartnerExists(int id)
        {
            return _context.Partners.Any(e => e.Id == id);
        }
    }
}
