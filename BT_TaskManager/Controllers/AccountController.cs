using Microsoft.AspNetCore.Mvc;
using BT_TaskManager.Data;
using BT_TaskManager.Models;
using System.Security.Cryptography;
using System.Text;

namespace BT_TaskManager.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(BT_User user)
        {
            if (ModelState.IsValid)
            {
                // Hash password
                user.PasswordHash = HashPassword(user.PasswordHash);
                user.CreatedAt = DateTime.Now;
                user.IsActive = true;

                _context.BT_Users.Add(user);
                _context.SaveChanges();

                return RedirectToAction("Login");
            }

            return View(user);
        }


        //new method added
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            string hashedPassword = HashPassword(password);

            var user = _context.BT_Users
                .FirstOrDefault(u => u.Email == email && u.PasswordHash == hashedPassword);

            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("UserName", user.FullName);

                return RedirectToAction("Index", "Task");
            }

            ViewBag.Error = "Invalid Email or Password";
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }


        private string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
