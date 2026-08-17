using System.ComponentModel.DataAnnotations;

namespace Shared.UserDTOs
{
    public sealed record DeleteAccountRequest(
        [Required][EmailAddress]
        string Email,

        [Required] string Password);
}
