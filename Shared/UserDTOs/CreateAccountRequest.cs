using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Shared.UserDTOs
{
    public sealed record CreateAccountRequest(
        [EmailAddress]
        [Required]
        string Email,

        [Required]
        string UserName,

        [Required]
        string Password);
}
