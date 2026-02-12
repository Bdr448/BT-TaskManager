using Microsoft.AspNetCore.Mvc;
using BT_TaskManager.Data;
using BT_TaskManager.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace BT_TaskManager.Controllers
{
    public class TaskController : Controller
    {
        private readonly AppDbContext _context;

        public TaskController(AppDbContext context)
        {
            _context = context;
        }

        // LIST TASKS
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var tasks = _context.BT_Tasks
                .Where(t => t.UserId == userId && t.IsDeleted == false)
                .ToList();

            return View(tasks);
        }

        // EDIT (GET)
        public IActionResult Edit(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var task = _context.BT_Tasks
                .FirstOrDefault(t => t.TaskId == id && t.UserId == userId);

            if (task == null)
                return NotFound();

            return View(task);
        }

        // EDIT (POST)
        [HttpPost]
        public IActionResult Edit(BT_Task task)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var existingTask = _context.BT_Tasks
                .FirstOrDefault(t => t.TaskId == task.TaskId && t.UserId == userId);

            if (existingTask == null)
                return NotFound();

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.DueDate = task.DueDate;
            existingTask.Status = task.Status;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // DELETE (Soft Delete)
        public IActionResult Delete(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var task = _context.BT_Tasks
                .FirstOrDefault(t => t.TaskId == id && t.UserId == userId);

            if (task == null)
                return NotFound();

            task.IsDeleted = true;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // CREATE (GET)
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
                return RedirectToAction("Login", "Account");

            return View();
        }

        // CREATE (POST)
        [HttpPost]
        public IActionResult Create(BT_Task task)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            task.UserId = userId.Value;
            task.CreatedAt = DateTime.Now;
            task.IsDeleted = false;
            task.Status = "Pending";

            _context.BT_Tasks.Add(task);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
