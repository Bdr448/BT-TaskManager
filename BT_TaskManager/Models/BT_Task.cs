using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BT_TaskManager.Models
{
    public class BT_Task
    {
        [Key]
        public int TaskId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public DateTime? DueDate { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }

        [ForeignKey("BT_User")]
        public int UserId { get; set; }

        public BT_User BT_User { get; set; }
    }
}
