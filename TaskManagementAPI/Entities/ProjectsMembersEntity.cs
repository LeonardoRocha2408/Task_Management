using Enums.UserEnums;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementAPI.Entities
{
    [Table("ProjectsMembers")]
    public sealed class ProjectsMembersEntity
    {
        [Column("ProjectId")]
        public Guid ProjectId { get; set; }

        [Column("UserId")]
        public Guid UserId { get; set; }

        [Column("Role")]
        public Role Role { get; set; }

        [Column("JoinedAt")]
        public DateTime JoinedAt { get; set; }

        public ProjectEntity Project { get; set; } = new ProjectEntity();
    }
}
