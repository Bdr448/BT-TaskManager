using Microsoft.AspNetCore.Mvc;
using BT_TaskManager.Data;
using System.Linq;

namespace BT_TaskManager.Controllers
{
    public class TestController : Controller
    {
        private readonly AppDbContext _context;

        public TestController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userCount = _context.BT_Users.Count();
            return Content("Database Connected Successfully. Total Users: " + userCount);
        }
    }
}
