using Microsoft.EntityFrameworkCore;
using BT_TaskManager.Models;

namespace BT_TaskManager.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<BT_User> BT_Users { get; set; }
        public DbSet<BT_Task> BT_Tasks { get; set; }
    }
}
