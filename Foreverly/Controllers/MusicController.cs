using Foreverly.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Foreverly.Controllers
{
    public class MusicController : Controller
    {
        private readonly AppDbContext _context;

        public MusicController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var bandsQuery = _context.Bands
                .Include(b => b.Partner)
                .Include(b => b.Playlists)
                    .ThenInclude(p => p.PlaylistItems)
                        .ThenInclude(pi => pi.Song)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                bandsQuery = bandsQuery.Where(b =>
                    b.Partner.Name.Contains(search) ||
                    b.Playlists.Any(p =>
                        p.Name.Contains(search) ||
                        p.PlaylistItems.Any(pi =>
                            pi.Song != null &&
                            (
                                pi.Song.Title.Contains(search) ||
                                pi.Song.Artist.Contains(search) ||
                                pi.Song.Genre.Contains(search)
                            )
                        )
                    )
                );
            }

            ViewBag.Search = search;
            ViewBag.TotalBands = await _context.Bands.CountAsync();
            ViewBag.TotalSongs = await _context.Songs.CountAsync();
            ViewBag.TotalPlaylists = await _context.Playlists.CountAsync();

            var bands = await bandsQuery.ToListAsync();

            return View(bands);
        }

        public async Task<IActionResult> Details(int id)
        {
            var band = await _context.Bands
                .Include(b => b.Partner)
                .Include(b => b.BandPrices)
                .Include(b => b.Playlists)
                    .ThenInclude(p => p.PlaylistItems)
                        .ThenInclude(pi => pi.Song)
                .Include(b => b.Playlists)
                    .ThenInclude(p => p.PlaylistItems)
                        .ThenInclude(pi => pi.SongGroup)
                .FirstOrDefaultAsync(b => b.PartnerId == id);

            if (band == null)
                return NotFound();

            return View(band);
        }
    }
}