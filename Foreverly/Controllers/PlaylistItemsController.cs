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
    public class PlaylistItemsController : Controller
    {
        private readonly AppDbContext _context;

        public PlaylistItemsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PlaylistItems
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.PlaylistItems.Include(p => p.Playlist).Include(p => p.Song).Include(p => p.SongGroup);
            return View(await appDbContext.ToListAsync());
        }

        // GET: PlaylistItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlistItem = await _context.PlaylistItems
                .Include(p => p.Playlist)
                .Include(p => p.Song)
                .Include(p => p.SongGroup)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (playlistItem == null)
            {
                return NotFound();
            }

            return View(playlistItem);
        }

        // GET: PlaylistItems/Create
        public IActionResult Create()
        {
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "Id");
            ViewData["SongId"] = new SelectList(_context.Songs, "Id", "Id");
            ViewData["SongGroupId"] = new SelectList(_context.SongGroups, "Id", "Id");
            return View();
        }

        // POST: PlaylistItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PlaylistId,SongId,SongGroupId")] PlaylistItem playlistItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(playlistItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "Id", playlistItem.PlaylistId);
            ViewData["SongId"] = new SelectList(_context.Songs, "Id", "Id", playlistItem.SongId);
            ViewData["SongGroupId"] = new SelectList(_context.SongGroups, "Id", "Id", playlistItem.SongGroupId);
            return View(playlistItem);
        }

        // GET: PlaylistItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlistItem = await _context.PlaylistItems.FindAsync(id);
            if (playlistItem == null)
            {
                return NotFound();
            }
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "Id", playlistItem.PlaylistId);
            ViewData["SongId"] = new SelectList(_context.Songs, "Id", "Id", playlistItem.SongId);
            ViewData["SongGroupId"] = new SelectList(_context.SongGroups, "Id", "Id", playlistItem.SongGroupId);
            return View(playlistItem);
        }

        // POST: PlaylistItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PlaylistId,SongId,SongGroupId")] PlaylistItem playlistItem)
        {
            if (id != playlistItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(playlistItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlaylistItemExists(playlistItem.Id))
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
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "Id", playlistItem.PlaylistId);
            ViewData["SongId"] = new SelectList(_context.Songs, "Id", "Id", playlistItem.SongId);
            ViewData["SongGroupId"] = new SelectList(_context.SongGroups, "Id", "Id", playlistItem.SongGroupId);
            return View(playlistItem);
        }

        // GET: PlaylistItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlistItem = await _context.PlaylistItems
                .Include(p => p.Playlist)
                .Include(p => p.Song)
                .Include(p => p.SongGroup)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (playlistItem == null)
            {
                return NotFound();
            }

            return View(playlistItem);
        }

        // POST: PlaylistItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var playlistItem = await _context.PlaylistItems.FindAsync(id);
            if (playlistItem != null)
            {
                _context.PlaylistItems.Remove(playlistItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlaylistItemExists(int id)
        {
            return _context.PlaylistItems.Any(e => e.Id == id);
        }
    }
}
