using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Board.Data;
using static Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Board.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Posts.Include(m => m.Tags).ToListAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.SingleOrDefaultAsync(m => m.key == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            List<SelectListItem> boards = new List<SelectListItem>();
            foreach (var c in _context.Boards.ToList())
            {
                boards.Add(new SelectListItem { Value = c.BoardId.ToString(), Text = c.BoardId });
            }
            ViewBag.boards = boards;

            List<SelectListItem> tags = new List<SelectListItem>();
            foreach (var c in _context.Tags.ToList())
            {
                tags.Add(new SelectListItem { Value = c.TagName, Text = c.TagName });
            }
            ViewBag.tags = tags;

            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("key,BoardId,DateBegin,DateEnd,HeaderText,Important,Text,Tags")] Post post)
        {
            if (ModelState.IsValid)
            {
                List<Tag> allTags = _context.Tags.ToListAsync().Result;
                var list = ModelState["Tags"];
                if (list != null && list.RawValue != null)
                {
                    var ar = list.RawValue as string[];
                    if (ar != null)
                    {
                        foreach (var tagname in ar)
                        {
                            foreach (var tag in allTags)
                            {
                                if (tagname == tag.TagName && tag.BoardId == post.BoardId)
                                {
                                    post.Tags.Add(tag);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        var str = list.RawValue as string;
                        if (str != null)
                        {
                            foreach (var tag in allTags)
                            {
                                if (str == tag.TagName && tag.BoardId == post.BoardId)
                                {
                                    post.Tags.Add(tag);
                                    break;
                                }
                            }
                        }
                    }
                }
                
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.SingleOrDefaultAsync(m => m.key == id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("key,BoardId,DateBegin,DateEnd,HeaderText,Important,Text")] Post post)
        {
            if (id != post.key)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.key))
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
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.SingleOrDefaultAsync(m => m.key == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.SingleOrDefaultAsync(m => m.key == id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.key == id);
        }
    }
}
