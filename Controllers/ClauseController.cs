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
public class ClauseController : Controller
{
    
    private readonly DatabaseContext _context;

    public ClauseController(ILogger<ClauseController> logger,
        DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string search = "*")
    {
        var clause = await _context.Clauses
            .Where(x => (search.Equals("*") 
            || x.Question.ToLower().Contains(search.ToLower()) 
            || x.Answer.ToLower().Contains(search.ToLower())))
            .OrderByDescending(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        clause.ForEach(x =>
        {
            x.Answer = x.Answer.Replace("\r\n", "<br />");
            if (x.FullAnswer != null) x.FullAnswer = x.FullAnswer.Replace("\r\n", "<br />");
        });

        var totalClause = _context.Clauses.Count();

        var model = new PaginatedList<Clause>(clause, totalClause, page, pageSize);
        ViewData["CurrentFilter"] = search;

        return View(model);
    }
    
   public async Task<IActionResult> Create()
   {
       var lastChapter = await _context.Chapters
           .OrderByDescending(p => p.Id)
           .Take(1)
           .FirstOrDefaultAsync();
       
       var lastClause = await _context.Clauses
                                             .Include(x => x.Chapter)
                                             .Where(p => lastChapter != null && p.Chapter.BookId == lastChapter.BookId)
                                             .OrderByDescending(p => p.Id)
                                             .Take(1)
                                             .FirstOrDefaultAsync();
       


       var clause = new Clause();
       var defaultDdLs = await PrepareDefaultDropDownList();
       // ViewBag.Books = defaultDdLs.BooksDDL;

       if (lastClause == null)
       {
           clause.Number = 1;
           ViewBag.Chapters = defaultDdLs.ChaptersDDL;
       }
       else
       {
           ViewBag.Chapters = await PrepareDropDownList(lastClause);
           clause.Number = lastClause.Number + 1;
            clause.ChapterId = lastClause.ChapterId;
           // clause.Chapter = new Chapter()
           // {
           //     BookId = lastClause.Chapter.BookId
           // };
       }
       
       return View(clause);
   }

   [HttpPost]
   public async Task<IActionResult> Create(Clause clause)
   {
       if (ModelState.IsValid)
       {
           _context.Clauses.Add(clause);
           await _context.SaveChangesAsync();
           
           return RedirectToAction("Index");
       }
       
       return View(clause);
   }
   
   public async Task<IActionResult> Edit(int id)
   {
       var clause = await _context.Clauses
           .Include(x => x.Chapter)
           .FirstOrDefaultAsync(x => x.Id == id);

       if (clause == null)
           return NotFound();

       ViewBag.Chapters = await PrepareDropDownList(clause);
       
       return View(clause);
   }

   private async Task<List<SelectListItem>> PrepareDropDownList(Clause clause)
   {
       var selectedChapter = clause.Chapter;
       var chapterItems = await _context.Chapters
           .OrderBy(x => x.Number)
           .Where(p => p.BookId == selectedChapter!.BookId)
           .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
           .ToListAsync();
       
       // chapterItems.First(x => x.Value == selectedChapter!.Id.ToString()).Selected = true;
       return chapterItems;
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
   public async Task<IActionResult> Edit(int id, Clause clause)
   {
       if (id != clause.Id)
       {
           return NotFound();
       }

       if (ModelState.IsValid)
       {
           _context.Update(clause);
           await _context.SaveChangesAsync();

       }
       
       return RedirectToAction("Index");
   }
   
   public IActionResult Delete(int id)
   {
       var clause = _context.Clauses.Find(id);
       if (clause != null) _context.Clauses.Remove(clause);
       _context.SaveChanges();

       return RedirectToAction("Index");
   }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
