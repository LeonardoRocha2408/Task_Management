using Enums.ServicesEnums;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementAPI.Entities
{
    [Table("Tasks")]
    public class TaskEntity
    {
        [Column("Id")]
        public Guid Id { get; set; }

        [Column("ProjectId")]
        public Guid ProjectId { get; set; }

        [Column("Title")]
        public string Title { get; set; } = string.Empty;

        [Column("Creator")]
        public Guid Creator { get; set;  }

        [Column("AssignedUser")]
        public Guid AssignedUser { get; set; }

        [Column("Status")]
        public StatusTasks Status {  get; set; }

        [Column("Priority")]
        public PriorityTasks Priority { get; set; }

        [Column("Description")]
        public string Description { get; set; } = string.Empty;

        [Column("DueDate")]
        public DateTime DueDate { get; set; }

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }

        [Column("UpdatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
