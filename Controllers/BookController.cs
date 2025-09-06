using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharpQuiz.Database;
using SharpQuiz.Domain.Entity;
using SharpQuiz.Models;

namespace SharpQuiz.Controllers;

[Authorize]
public class BookController : Controller
{
    private readonly DatabaseContext _context;
    
    public BookController(ILogger<BookController> logger,
        DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
    {
        var books = await _context.Books.OrderByDescending(p => p.Number)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var totalBooks = _context.Books.Count();

        var model = new PaginatedList<Book>(books, totalBooks, page, pageSize);

        return View(model);
    }
    
   public IActionResult Create()
   {
       return View();
   }

   [HttpPost]
   public async Task<IActionResult> Create(Book book)
   {
       if (ModelState.IsValid)
       {
           _context.Books.Add(book);
           await _context.SaveChangesAsync();
           
           return RedirectToAction("Index");
       }
       
       return View(book);
   }
   
   public IActionResult Edit(int id)
   {
       var book = _context.Books.Find(id);
       return View(book);
   }

   [HttpPost]
   public async Task<IActionResult> Edit(int id, Book book)
   {
       if (id != book.Id)
       {
           return NotFound();
       }

       if (ModelState.IsValid)
       {
           _context.Update(book);
           await _context.SaveChangesAsync();

       }
       return RedirectToAction("Index");
   }
   
   public IActionResult Delete(int id)
   {
       var book = _context.Books.Find(id);
       if (book != null) _context.Books.Remove(book);
       _context.SaveChanges();

       return RedirectToAction("Index");
   }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
