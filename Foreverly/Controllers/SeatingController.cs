using Foreverly.Data;
using Foreverly.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Foreverly.Hubs;

namespace Foreverly.Controllers
{
    public class SeatingController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<SeatingHub> _hub;

        private const int CapacityPerTable = 10;

        public SeatingController(AppDbContext context, IHubContext<SeatingHub> hub)
        {
            _context = context;
            _hub = hub;
        }

        public async Task<IActionResult> Index(int id)
        {
            var wedding = await _context.Weddings
                .Include(w => w.Guests)
                .Include(w => w.Tables)
                    .ThenInclude(t => t.SeatingAssignments)
                        .ThenInclude(sa => sa.Guest)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (wedding == null)
                return NotFound();

            int neededTables =
                (int)Math.Ceiling(wedding.Guests.Count / (double)CapacityPerTable);

            int existingTables = wedding.Tables.Count;

            if (existingTables < neededTables)
            {
                for (int i = existingTables + 1; i <= neededTables; i++)
                {
                    _context.WeddingTables.Add(new WeddingTable
                    {
                        WeddingId = wedding.Id,
                        TableName = $"Table {i}",
                        Capacity = CapacityPerTable
                    });
                }

                await _context.SaveChangesAsync();

                wedding = await _context.Weddings
                    .Include(w => w.Guests)
                    .Include(w => w.Tables)
                        .ThenInclude(t => t.SeatingAssignments)
                            .ThenInclude(sa => sa.Guest)
                    .FirstOrDefaultAsync(w => w.Id == id);
            }

            return View(wedding);
        }

        [HttpPost]
        public async Task<IActionResult> AddGuest([FromBody] AddGuestDto dto)
        {
            var guest = new Guest
            {
                WeddingId = dto.WeddingId,
                FullName = dto.FullName,
                Side = ""
            };

            _context.Guests.Add(guest);

            await _context.SaveChangesAsync();

            await _hub.Clients.All.SendAsync("RefreshSeating");

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteGuest(int id)
        {
            var guest = await _context.Guests
                .FirstOrDefaultAsync(g => g.Id == id);

            if (guest == null)
                return Json(new { success = false });

            var assignment = await _context.SeatingAssignments
                .FirstOrDefaultAsync(x => x.GuestId == id);

            if (assignment != null)
                _context.SeatingAssignments.Remove(assignment);

            _context.Guests.Remove(guest);

            await _context.SaveChangesAsync();

            await _hub.Clients.All.SendAsync("RefreshSeating");

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> AssignGuest([FromBody] AssignGuestDto dto)
        {
            if (dto == null)
                return Json(new { success = false });

            var table = await _context.WeddingTables
                .Include(t => t.SeatingAssignments)
                .FirstOrDefaultAsync(t => t.Id == dto.TableId);

            if (table == null)
                return Json(new { success = false });

            if (table.SeatingAssignments.Count >= table.Capacity)
            {
                return Json(new
                {
                    success = false,
                    message = "Table is full"
                });
            }

            var existing = await _context.SeatingAssignments
                .FirstOrDefaultAsync(x => x.GuestId == dto.GuestId);

            if (existing != null)
                _context.SeatingAssignments.Remove(existing);

            var assignment = new SeatingAssignment
            {
                GuestId = dto.GuestId,
                TableId = dto.TableId,
                SeatNumber = table.SeatingAssignments.Count + 1
            };

            _context.SeatingAssignments.Add(assignment);

            await _context.SaveChangesAsync();

            await _hub.Clients.All.SendAsync("RefreshSeating");

            return Json(new { success = true });
        }
    }
}