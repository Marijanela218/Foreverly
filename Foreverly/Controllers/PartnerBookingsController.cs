using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Foreverly.Data;
using Foreverly.Models;

namespace Foreverly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartnerBookingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PartnerBookingsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking(PartnerBooking booking)
        {
            bool collisionExists = await _context.PartnerBookings
                .AnyAsync(pb =>
                    pb.PartnerId == booking.PartnerId &&
                    booking.StartDateTime < pb.EndDateTime &&
                    booking.EndDateTime > pb.StartDateTime);

            if (collisionExists)
            {
                return BadRequest("Partner already booked.");
            }

            _context.PartnerBookings.Add(booking);

            await _context.SaveChangesAsync();

            return Ok(booking);
        }
    }
}