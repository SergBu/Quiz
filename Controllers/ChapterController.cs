using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SharpQuiz.Database;
using SharpQuiz.Domain.Entity;
using SharpQuiz.Models;

namespace SharpQuiz.Controllers;

[Authorize]
public class ChapterController : Controller
{
    private readonly DatabaseContext _context;
    
    public ChapterController(ILogger<ChapterController> logger,
        DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
    {
        var chapters = await _context.Chapters.OrderByDescending(p => p.Number)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var totalChapters = _context.Chapters.Count();

        var model = new PaginatedList<Chapter>(chapters, totalChapters, page, pageSize);

        return View(model);
    }
    
   public async Task<IActionResult> Create()
   {
       var lastBook = await _context.Books
           .OrderByDescending(p => p.Id)
           .Take(1)
           .FirstOrDefaultAsync();
       
       var lastChapter = await _context.Chapters
           .Where(p => lastBook != null && p.BookId == lastBook.Id)
           .OrderByDescending(p => p.Id)
           .Take(1)
           .FirstOrDefaultAsync();
       


       var chapter = new Chapter
       {
           Number = 1
       };
       var defaultDdLs = await PrepareDefaultDropDownList();
       ViewBag.Books = defaultDdLs.BooksDDL;

       if (lastChapter != null)
       {
           chapter.Number = lastChapter.Number + 1;
           chapter.BookId = lastChapter.BookId;
       }
       
       return View(chapter);
   }
   
   private async Task<(List<SelectListItem> BooksDDL, List<SelectListItem> ChaptersDDL)> PrepareDefaultDropDownList()
   {
       var bookItems = await _context.Books
           .OrderBy(x => x.Number)
           .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
           .ToListAsync();
       
       var chapterItems = await _context.Chapters
           .OrderBy(x => x.Number)
           .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
           .ToListAsync();
       
       return (bookItems, chapterItems);
   }

   [HttpPost]
   public async Task<IActionResult> Create(Chapter chapter)
   {
       if (ModelState.IsValid)
       {
           _context.Chapters.Add(chapter);
           await _context.SaveChangesAsync();
           
           return RedirectToAction("Index");
       }
       
       return View(chapter);
   }
   
   public async Task<IActionResult> Edit(int id)
   {
       var chapter = await _context.Chapters
           .FirstOrDefaultAsync(x => x.Id == id);

       if (chapter == null)
           return NotFound();

       var defaultDdLs = await PrepareDefaultDropDownList();
       ViewBag.Books = defaultDdLs.BooksDDL;
       
       return View(chapter);
   }

   [HttpPost]
   public async Task<IActionResult> Edit(int id, Chapter chapter)
   {
       if (id != chapter.Id)
       {
           return NotFound();
       }

       if (ModelState.IsValid)
       {
           _context.Update(chapter);
           await _context.SaveChangesAsync();

       }
       return RedirectToAction("Index");
   }
   
   public IActionResult Delete(int id)
   {
       var chapter = _context.Chapters.Find(id);
       if (chapter != null) _context.Chapters.Remove(chapter);
       _context.SaveChanges();

       return RedirectToAction("Index");
   }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
