using System.ComponentModel.DataAnnotations;
using Enums.ServicesEnums;

namespace Shared.ServicesDTOs
{
    public sealed class ResponseTasks
    {
        [Required]
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public Guid AssignedUser {  get; set; }

        public StatusTasks Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
