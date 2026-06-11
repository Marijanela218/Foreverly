using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Foreverly.Data;
using Foreverly.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Foreverly.Controllers
{
    public class WeddingsController : Controller
    {
        private readonly AppDbContext _context;

        public WeddingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Weddings
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Weddings.Include(w => w.Template);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Weddings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var wedding = await _context.Weddings
                .Include(w => w.WeddingServices)
                    .ThenInclude(ws => ws.Partner)
                .Include(w => w.Guests)
                .Include(w => w.Tables)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (wedding == null) return NotFound();

            ViewBag.TotalPrice = wedding.WeddingServices.Sum(s => s.TotalPrice);
            ViewBag.TotalCommission = wedding.WeddingServices.Sum(s => s.CommissionAmount);

            ViewBag.Partners = new SelectList(
                await _context.Partners
                    .OrderBy(p => p.Name)
                    .ToListAsync(),
                "Id",
                "Name"
            );

            ViewBag.Categories = new SelectList(
                await _context.PartnerCategories
                    .OrderBy(c => c.Name)
                    .ToListAsync(),
                "Id",
                "Name"
            );

            ViewBag.PartnerCommissions =
                System.Text.Json.JsonSerializer.Serialize(
                    await _context.Partners.ToDictionaryAsync(
                        p => p.Id.ToString(),
                        p => p.DefaultCommissionPercent
                    )
                );

            return View(wedding);
        }

        // GET: Weddings/Create
        public IActionResult Create()
        {
            ViewData["TemplateId"] = new SelectList(_context.Templates, "Id", "Id");
            return View();
        }

        // POST: Weddings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SuggestedDate,ConfirmedDate,Status,ClientName,ClientPhone,ClientEmail,ClientAddress,Notes,TemplateId")] Wedding wedding)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wedding);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TemplateId"] = new SelectList(_context.Templates, "Id", "Id", wedding.TemplateId);
            return View(wedding);
        }

        // GET: Weddings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wedding = await _context.Weddings.FindAsync(id);
            if (wedding == null)
            {
                return NotFound();
            }
            ViewData["TemplateId"] = new SelectList(_context.Templates, "Id", "Id", wedding.TemplateId);
            return View(wedding);
        }

        // POST: Weddings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SuggestedDate,ConfirmedDate,Status,ClientName,ClientPhone,ClientEmail,ClientAddress,Notes,TemplateId")] Wedding wedding)
        {
            if (id != wedding.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wedding);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WeddingExists(wedding.Id))
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
            ViewData["TemplateId"] = new SelectList(_context.Templates, "Id", "Id", wedding.TemplateId);
            return View(wedding);
        }

        // GET: Weddings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wedding = await _context.Weddings
                .Include(w => w.Template)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wedding == null)
            {
                return NotFound();
            }

            return View(wedding);
        }

        // POST: Weddings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wedding = await _context.Weddings.FindAsync(id);
            if (wedding != null)
            {
                _context.Weddings.Remove(wedding);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WeddingExists(int id)
        {
            return _context.Weddings.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<IActionResult> AddService(int weddingId)
        {
            var wedding = await _context.Weddings.FindAsync(weddingId);

            if (wedding == null)
                return NotFound();

            ViewBag.WeddingId = weddingId;

            ViewBag.Partners = new SelectList(
                await _context.Partners.ToListAsync(),
                "Id",
                "Name"
            );

            return View(new WeddingService
            {
                WeddingId = weddingId,
                Quantity = 1,
                CommissionPercent = 10
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddService(
            int WeddingId,
            int PartnerId,
            decimal CommissionPercent,
            bool Confirmed)
        {
            var partner = await _context.Partners
                .Include(p => p.Category)
                .Include(p => p.FloralArrangements)
                .Include(p => p.PastryItems)
                .Include(p => p.Band)
                    .ThenInclude(b => b.BandPrices)
                .Include(p => p.Restaurant)
                    .ThenInclude(r => r.Halls)
                .Include(p => p.Restaurant)
                    .ThenInclude(r => r.Menus)
                .FirstOrDefaultAsync(p => p.Id == PartnerId);

            if (partner == null)
                return NotFound();

            var wedding = await _context.Weddings.FindAsync(WeddingId);

            if (wedding == null)
                return NotFound();

            decimal unitPrice = 0;

            if (partner.Restaurant?.Halls.Any() == true)
            {
                unitPrice = partner.Restaurant.Halls.First().BasePrice;
            }
            else if (partner.Restaurant?.Menus.Any() == true)
            {
                unitPrice = partner.Restaurant.Menus.First().PricePerPerson;
            }
            else if (partner.Band?.BandPrices.Any() == true)
            {
                unitPrice = partner.Band.BandPrices.First().Price;
            }
            else if (partner.FloralArrangements.Any())
            {
                unitPrice = partner.FloralArrangements.First().BasePrice;
            }
            else if (partner.PastryItems.Any())
            {
                unitPrice = partner.PastryItems.First().BasePrice;
            }

            var totalPrice = unitPrice;

            var commissionAmount =
                totalPrice * CommissionPercent / 100;

            var weddingService = new WeddingService
            {
                WeddingId = WeddingId,
                PartnerId = PartnerId,

                ServiceType = partner.Category?.Name ?? "General",

                Quantity = 1,
                UnitPrice = unitPrice,
                TotalPrice = totalPrice,

                CommissionPercent = CommissionPercent,
                CommissionAmount = commissionAmount,

                Confirmed = Confirmed
            };

            _context.WeddingServices.Add(weddingService);

            if (wedding.ConfirmedDate != null)
            {
                _context.PartnerBookings.Add(new PartnerBooking
                {
                    PartnerId = PartnerId,
                    WeddingId = WeddingId,

                    StartDateTime = DateTime.SpecifyKind(
                        wedding.ConfirmedDate.Value.ToDateTime(TimeOnly.MinValue),
                        DateTimeKind.Utc),

                    EndDateTime = DateTime.SpecifyKind(
                        wedding.ConfirmedDate.Value.ToDateTime(TimeOnly.MaxValue),
                        DateTimeKind.Utc),

                    Status = "Booked"
                });
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = WeddingId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteService(int serviceId, int weddingId)
        {
            var service = await _context.WeddingServices.FindAsync(serviceId);

            if (service == null)
                return NotFound();

            _context.WeddingServices.Remove(service);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = weddingId });
        }
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var wedding = await _context.Weddings.FindAsync(id);

            if (wedding == null)
                return NotFound();

            wedding.Status = wedding.Status == "Planned"
                ? "Confirmed"
                : "Planned";

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}