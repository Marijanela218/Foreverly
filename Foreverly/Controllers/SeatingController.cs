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

        public SeatingController(AppDbContext context, IHubContext<SeatingHub> hub)
        {
            _context = context;
            _hub = hub;
        }

        // ================================
        // PAGE
        // ================================
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

            return View(wedding);
        }

        // ================================
        // ASSIGN / MOVE GUEST
        // ================================
        [HttpPost]
        public async Task<IActionResult> AssignGuest([FromBody] AssignGuestDto dto)
        {
            if (dto == null)
                return Json(new { success = false });

            var table = await _context.WeddingTables
                .Include(t => t.SeatingAssignments)
                .FirstOrDefaultAsync(t => t.Id == dto.TableId);

            if (table == null)
                return Json(new { success = false, message = "Table not found" });

            // 🔴 CAPACITY CHECK
            if (table.SeatingAssignments.Count >= table.Capacity)
            {
                return Json(new
                {
                    success = false,
                    message = "Table is full"
                });
            }

            // 🔴 remove old assignment (MOVE logic)
            var existing = await _context.SeatingAssignments
                .FirstOrDefaultAsync(x => x.GuestId == dto.GuestId);

            if (existing != null)
                _context.SeatingAssignments.Remove(existing);

            // 🔴 seat number
            var seatNumber = table.SeatingAssignments.Count + 1;

            var assignment = new SeatingAssignment
            {
                GuestId = dto.GuestId,
                TableId = dto.TableId,
                SeatNumber = seatNumber
            };

            _context.SeatingAssignments.Add(assignment);

            await _context.SaveChangesAsync();

            // 🔴 REAL TIME UPDATE
            await _hub.Clients.All.SendAsync("RefreshSeating");

            return Json(new
            {
                success = true,
                tableId = dto.TableId,
                guestId = dto.GuestId,
                seatNumber = seatNumber
            });
        }
    }
}