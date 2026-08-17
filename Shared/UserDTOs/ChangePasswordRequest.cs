using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.UserDTOs
{
    public sealed record ChangePasswordRequest(
        [Required][EmailAddress] 
        string Email,

        [Required] 
        string Password,

        [Required] 
        string NewPassword);
}
