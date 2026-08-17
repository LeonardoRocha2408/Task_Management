using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementAPI.Entities
{
    [Table("Projects")]
    public sealed class ProjectEntity
    {
        [Column("Id")]
        public Guid Id { get; set; }

        [Column("OwnerId")]
        public Guid OwnerId { get; set; }

        [Column("Title")]
        public string Title { get; set; } = string.Empty;

        [Column("Description")]
        public string Description { get; set; } = string.Empty;

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }

        [Column("UpdatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
