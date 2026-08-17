using System.ComponentModel.DataAnnotations;

namespace Shared.UserDTOs
{
    public sealed class MeResponse
    {
        [EmailAddress]
        [Required]
        public string Email { get; init; } = string.Empty;

        [Required]
        public string UserName {  get; init; } = string.Empty;

        public string PathProfilePicture {  get; init; } = string.Empty;
    }
}
