using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Enums.ServicesDTOs
{
    public sealed record CreateTaskRequest(
        [Required]
        string Title,

        [EmailAddress]
        string AssignedUser,

        DateTime DueDate,

        string Description);
}
