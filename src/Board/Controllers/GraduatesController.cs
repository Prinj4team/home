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
    public class GraduatesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GraduatesController(ApplicationDbContext context)
        {
            _context = context;    
        }
        //-----------------------------------------------------------------------------------------------------
        public JsonResult GetData()
        {
            // создадим список данных
            List<Graduate> stations = new List<Graduate>();
            stations.Add(new Graduate()
            {
                BoardId = "Finf",
                key = 1,
                Town = "Канада - Альберта",
                Latitude = -115.77393,
                
                Longitude = 56.39376,
                Name = "Альберт Энштэйн",
               
            });
            stations.Add(new Graduate()
            {
                BoardId = "Finf",
                key = 2,
                Town = "США - Айова",
                Latitude = -92.17529,
                Longitude = 42.72028,
                Name = "Никола Тесла",
               
            }); 
            stations.Add(new Graduate()
            {
                BoardId = "Finf",
                key = 3,
                Town = "Австралия - Порт Хэдленд",
                Latitude = 119.08081,
               
                Longitude =-20.40288,
                Name = "Виктор Якубович",
               
            });

            stations.Add(new Graduate()
            {
                BoardId = "Finf",
                key = 3,
                Town = "Австралия - Порт Хэдленд",
                Latitude = 119.08082,

                Longitude = -20.40288,
                Name = "Виктор Якубович",

            });
            stations.Add(new Graduate()
            {
                BoardId = "Finf",
                key = 3,
                Town = "Аргентина - Санта-Роса",
                Latitude = -64.16016,

                Longitude = -38.22524,
                Name = "Николай Басков",

            });

            return Json(stations);  //, JsonRequestBehavior.AllowGet
        }
        //------------------------------------------------------------------------------------------------

        // GET: Graduates
        public async Task<IActionResult> Index()
        {
            return View(await _context.Graduates.ToListAsync());
        }

        // GET: Graduates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var graduate = await _context.Graduates.SingleOrDefaultAsync(m => m.key == id);
            if (graduate == null)
            {
                return NotFound();
            }

            return View(graduate);
        }

        // GET: Graduates/Create
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

        // POST: Graduates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("key,BoardId,FinishDate,Firm,History,Name,Town")] Graduate graduate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(graduate);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(graduate);
        }

        // GET: Graduates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var graduate = await _context.Graduates.SingleOrDefaultAsync(m => m.key == id);
            if (graduate == null)
            {
                return NotFound();
            }
            return View(graduate);
        }

        // POST: Graduates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("key,BoardId,FinishDate,Firm,History,Name,Town")] Graduate graduate)
        {
            if (id != graduate.key)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(graduate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GraduateExists(graduate.key))
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
            return View(graduate);
        }

        // GET: Graduates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var graduate = await _context.Graduates.SingleOrDefaultAsync(m => m.key == id);
            if (graduate == null)
            {
                return NotFound();
            }

            return View(graduate);
        }

        // POST: Graduates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var graduate = await _context.Graduates.SingleOrDefaultAsync(m => m.key == id);
            _context.Graduates.Remove(graduate);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool GraduateExists(int id)
        {
            return _context.Graduates.Any(e => e.key == id);
        }
    }
}
