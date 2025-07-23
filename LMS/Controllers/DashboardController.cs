using LMS.Data;
using LMS.Models;
using LMS.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class DashboardController : Controller
{
    private readonly AppDbContext _context;

    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        ViewBag.TotalBooks = _context.Books.Count();
        ViewBag.TotalIssued = _context.IssuedBooks.Count();
        ViewBag.TotalMembers = _context.Members.Count();
        ViewBag.TotalReturned = _context.ReturnedBooks.Count();
        return View();
    }
    public IActionResult IssuedBooks()
    {
        var issuedBooks = _context.IssuedBooks
            .Include(i => i.Book)
            .OrderByDescending(i => i.IssuedDate)
            .ToList();

        return View(issuedBooks);
    }
    public IActionResult Returned()
    {
        var returnedBooks = _context.ReturnedBooks
            .Include(r => r.Book)
            .OrderByDescending(r => r.ReturnedDate)
            .ToList();

        return View(returnedBooks);
    }


    public IActionResult Members()
    {
        var members = _context.Members.ToList();
        return View(members);
    }

    [HttpPost]
    public IActionResult ToggleBlock(int id)
    {
        var member = _context.Members.FirstOrDefault(m => m.Id == id);
        if (member != null)
        {
            member.IsBlocked = !member.IsBlocked;
            _context.SaveChanges();
        }
        return RedirectToAction("Members");
    }



    public IActionResult Transactions()
    {
        var issued = _context.IssuedBooks
            .Include(i => i.Book)
            .ToList();

        var returned = _context.ReturnedBooks
            .ToList();

        var data = issued.Select(i => new TransactionViewModel
        {
            BookTitle = i.Book.Title,
            MemberName = i.MemberName,
            IssuedDate = i.IssuedDate,
            ReturnedDate = returned
                .FirstOrDefault(r =>
                    r.BookId == i.BookId &&
                    r.MemberName == i.MemberName &&
                    r.IssuedDate == i.IssuedDate
                )?.ReturnedDate
        }).ToList();

        return View(data);
    }

    public IActionResult Records()
    {
        var returnedBooks = _context.ReturnedBooks
            .Include(r => r.Book)
            .ToList();

        var transactions = returnedBooks.Select(r => new TransactionViewModel
        {
            BookTitle = r.Book?.Title ?? "Unknown Title",
            MemberName = r.MemberName,
            IssuedDate = r.IssuedDate,
            ReturnedDate = r.ReturnedDate
        }).ToList();

        return View(transactions);
    }

    public IActionResult AllTimeTransactions(string filter = "Not Returned")
    {
        var query = _context.AllTimeTransactions.AsQueryable();

        if (filter == "Returned")
        {
            query = query.Where(t => t.ReturnedDate != null);
        }
        else if (filter == "Not Returned")
        {
            query = query.Where(t => t.ReturnedDate == null);
        }

        var model = new TransactionFilterViewModel
        {
            Filter = filter,
            Transactions = query
                .OrderByDescending(t => t.IssuedDate)
                .ToList()
        };

        return View(model);
    }






}
