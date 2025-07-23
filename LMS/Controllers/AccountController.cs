using LMS.Data;
using LMS.Models;
using LMS.Services;
using LMS.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LMS.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly EmailService _emailService;
        public AccountController(AppDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // Admin credentials
        private const string AdminEmail = "admin@lms.com";
        private const string AdminPassword = "admin123";

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Member model)
        {
            if (_context.Members.Any(m => m.Email == model.Email))
            {
                ModelState.AddModelError("Email", "This email is already registered. ");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                _context.Members.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginModel
            {
                Email = string.Empty,
                Password = string.Empty
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Check for Admin login
            if (model.Email == AdminEmail && model.Password == AdminPassword)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            // Check for Member login
            var user = _context.Members
                .FirstOrDefault(m => m.Email == model.Email && m.Password == model.Password);

            if (user != null)
            {
                if (user.IsBlocked) // <-- Blocked user check
                {
                    ViewBag.Error = "Your account is blocked by admin.";
                    return View(model);
                }

                // Login success
                HttpContext.Session.SetString("MemberEmail", user.Email);
                HttpContext.Session.SetString("MemberName", user.Name);
                return RedirectToAction("Index", "MemberDashboard");
            }

            // Invalid login
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var member = _context.Members.FirstOrDefault(m => m.Email == email);

            if (member == null)
            {
                TempData["ForgotError"] = "This email ID is not registered. Please register yourself first.";
                TempData["OpenForgotModal"] = true;
                return RedirectToAction("Login");
            }

            // Generate dummy reset link (or use a real token system later)
            var resetLink = Url.Action("ResetPassword", "Account", new { email = member.Email }, Request.Scheme);
            //var resetLink = "http://localhost:5000/Account/ResetPassword?email=" + member.Email;


            string subject = "Reset Your LMS Password";
            string body = $"<p>Hi {member.Name},</p><p>Click the link below to reset your password:</p><p><a href='{resetLink}'>Reset Password</a></p>";

            await _emailService.SendEmailAsync(member.Email, subject, body);

            TempData["ForgotSuccess"] = "A password reset link has been sent to your email.";
            TempData["ShowSuccessPopup"] = true;
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult ResetPassword(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var member = _context.Members.FirstOrDefault(m => m.Email == model.Email);
            if (member == null)
            {
                TempData["ResetError"] = "Invalid email address.";
                return View(model);
            }

            member.Password = model.NewPassword;
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Your password has been reset successfully.";
            return RedirectToAction("Login");

        }

        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdminLogin(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Check if user exists (email + password match)
            var user = _context.Members.FirstOrDefault(m => m.Email == model.Email && m.Password == model.Password);

            if (user != null)
            {
                if (user.IsBlocked)
                {
                    ViewBag.Error = "Your account is blocked by admin.";
                    return View(model);
                }

                // Store login session
                HttpContext.Session.SetString("MemberEmail", user.Email);
                HttpContext.Session.SetString("MemberName", user.Name);

                // Redirect to Admin Dashboard
                TempData["SuccessMessage"] = "Logged in via Admin Panel";
                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.Error = "Invalid email or password.";
            return View(model);
        }






    }
}
