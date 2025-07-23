using LMS.Data;
using LMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;

namespace LMS.Controllers
{
    public class MemberDashboardController : Controller
    {
        private readonly AppDbContext _context;

        public MemberDashboardController(AppDbContext context)
        {
            _context = context;
        }

        // Show all books and their issued status
        public IActionResult Index()
        {
            var memberName = HttpContext.Session.GetString("MemberName");
            if (string.IsNullOrEmpty(memberName)) memberName = "Unknown";

            var books = _context.Books.ToList();

            // Only get book IDs issued by this member
            var issuedBookIds = _context.IssuedBooks
                .Where(i => i.MemberName == memberName)
                .Select(i => i.BookId)
                .ToList();

            ViewBag.IssuedBookIds = issuedBookIds;
            return View(books);
        }

        // Issue a book (updates IssuedBooks table + status)
        public IActionResult Issue(int id)
        {
            string? memberName = HttpContext.Session.GetString("MemberName");
            if (string.IsNullOrEmpty(memberName)) return RedirectToAction("Login", "Account");

            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();

            bool alreadyIssued = _context.IssuedBooks.Any(i => i.BookId == id);
            if (!alreadyIssued)
            {
                var issued = new IssuedBook
                {
                    BookId = book.Id,
                    MemberName = memberName,
                    IssuedDate = DateTime.Now
                };

                _context.IssuedBooks.Add(issued);

                // Update book status
                book.Status = "Issued";
                _context.Books.Update(book);

                _context.SaveChanges();


                // Step 4: After book issue is saved
                // or however you're getting the book
                // Adjust according to your auth logic

                var transaction = new AllTimeTransaction
                {
                    BookTitle = book.Title,
                    MemberName = memberName,
                    IssuedDate = DateTime.Now,
                    ReturnedDate = null
                };

                _context.AllTimeTransactions.Add(transaction);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // Return a book (removes from IssuedBooks, adds to ReturnedBooks, updates status)
        [HttpPost]
        public IActionResult Return(int id)
        {
            var issue = _context.IssuedBooks.FirstOrDefault(i => i.BookId == id);
            if (issue != null)
            {
                _context.IssuedBooks.Remove(issue);

                var returnBook = new ReturnedBook
                {
                    BookId = id,
                    MemberName = issue.MemberName,
                    IssuedDate = issue.IssuedDate,
                    ReturnedDate = DateTime.Now
                };

                _context.ReturnedBooks.Add(returnBook);

                var book = _context.Books.FirstOrDefault(b => b.Id == id);
                if (book != null)
                {
                    book.Status = "Available";
                    _context.Books.Update(book);
                }


                _context.SaveChanges(); // Save changes
            

            // Step 5: Update AllTimeTransaction table
            // adjust as needed
            // use member name or email depending on your logic

            var transaction = _context.AllTimeTransactions
                .FirstOrDefault(t => t.BookTitle == book.Title &&
                                     t.MemberName == issue.MemberName &&
                                     t.ReturnedDate == null);

            if (transaction != null)
            {
                transaction.ReturnedDate = DateTime.Now;
                _context.SaveChanges();
            }

        }

            return RedirectToAction("Index");
        }


        // Show issued books by member
        public IActionResult Issued()
        {
            string? memberName = HttpContext.Session.GetString("MemberName");
            if (string.IsNullOrEmpty(memberName)) return RedirectToAction("Login", "Account");
            //var issuedBooks = _context.IssuedBooks
            //    .Include(i => i.Book)
            //    .Where(i => i.MemberName == "Shivam") // Replace with session
            //    .ToList();

            var issuedBooks = _context.IssuedBooks
                .Include(i => i.Book)
                .Where(i => i.MemberName == memberName) //filter per logged-in member
                .ToList();

            return View(issuedBooks);
        }

        // Show returned books by member
        public IActionResult Returned()
        {
            string? memberName = HttpContext.Session.GetString("MemberName");
            if (string.IsNullOrEmpty(memberName)) return RedirectToAction("Login", "Account");
            //var returnedBooks = _context.ReturnedBooks
            //    .Include(rb => rb.Book)
            //    .Where(rb => rb.MemberName == "Shivam") // Replace with actual member session
            //    .OrderByDescending(rb => rb.ReturnedDate)
            //    .ToList();
            var returnedBooks = _context.ReturnedBooks
                .Include(rb => rb.Book)
                .Where(rb => rb.MemberName == memberName) //filter by logged-in member
                .OrderByDescending(rb => rb.ReturnedDate)
                .ToList();

            return View(returnedBooks);
        }

public IActionResult ChangePassword()
    {
        return View();
    }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string? email = HttpContext.Session.GetString("MemberEmail");
            var member = _context.Members.FirstOrDefault(m => m.Email == email);

            if (member == null)
            {
                ModelState.AddModelError("", "Member not found.");
                return View(model);
            }

            // Plain password check (since no hashing used)
            if (member.Password != model.CurrentPassword)
            {
                ModelState.AddModelError("CurrentPassword", "Current password is incorrect.");
                return View(model);
            }

            // Update the password
            member.Password = model.NewPassword;
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Password changed successfully!";
            return RedirectToAction("Index");
        }

    }
}
