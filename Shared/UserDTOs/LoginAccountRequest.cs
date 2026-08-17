using System.ComponentModel.DataAnnotations;

namespace Shared.UserDTOs
{
    public sealed record LoginAccountRequest(
        [Required][EmailAddress]
        string Email,

        [Required]
        string Password);
}
