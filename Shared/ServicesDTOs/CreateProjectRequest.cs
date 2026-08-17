using System.ComponentModel.DataAnnotations;

namespace Shared.ServicesDTOs
{
    public sealed record CreateProjectRequest(
        [Required] string Title,
        string Description
        );
}
