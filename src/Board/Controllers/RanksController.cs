using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Board.Data;

namespace Board.Controllers
{
    public class RanksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RanksController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Ranks
        public async Task<IActionResult> Index()
        {
            return View(await _context.Ranks.ToListAsync());
        }

        // GET: Ranks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rank = await _context.Ranks.SingleOrDefaultAsync(m => m.Key == id);
            if (rank == null)
            {
                return NotFound();
            }

            return View(rank);
        }

        // GET: Ranks/Create
        public IActionResult Create()
        {
            List<SelectListItem> boards = new List<SelectListItem>();
            foreach (var c in _context.Boards.ToList())
            {
                boards.Add(new SelectListItem { Value = c.BoardId.ToString(), Text = c.BoardId });
            }
            ViewBag.boards = boards;
            return View();
        }

        // POST: Ranks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Key,BoardId,Image,Position,Text")] Rank rank)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rank);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(rank);
        }

        // GET: Ranks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rank = await _context.Ranks.SingleOrDefaultAsync(m => m.Key == id);
            if (rank == null)
            {
                return NotFound();
            }
            return View(rank);
        }

        // POST: Ranks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Key,BoardId,Image,Position,Text")] Rank rank)
        {
            if (id != rank.Key)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rank);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RankExists(rank.Key))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(rank);
        }

        // GET: Ranks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rank = await _context.Ranks.SingleOrDefaultAsync(m => m.Key == id);
            if (rank == null)
            {
                return NotFound();
            }

            return View(rank);
        }

        // POST: Ranks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rank = await _context.Ranks.SingleOrDefaultAsync(m => m.Key == id);
            _context.Ranks.Remove(rank);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool RankExists(int id)
        {
            return _context.Ranks.Any(e => e.Key == id);
        }
    }
}
