using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SharpQuiz.Database;
using SharpQuiz.Domain.Entity;
using SharpQuiz.Models;

namespace SharpQuiz.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DatabaseContext _context;

    public HomeController(ILogger<HomeController> logger,
        DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        var maxId = _context.Clauses.Max(c => c.Id);
        var rnd = new Random();
        Clause? clause = null;

        while (clause == null)
        {
            var rndId  = rnd.Next(1, maxId + 1);  
            clause = _context.Clauses.FirstOrDefault(c => c.Id == rndId);
        }

        clause.Answer = clause.Answer.Replace("\r\n", "<br />");

        return View(clause);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}