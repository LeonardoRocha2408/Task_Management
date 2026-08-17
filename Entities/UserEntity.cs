using Enums.UserEnums;
using System.ComponentModel.DataAnnotations.Schema;


namespace TaskManagementAPI.Entities
{
    [Table("Users")]
    public sealed class UserEntity
    {
        [Column("Id")]
        public Guid Id { get; set; }

        [Column("Role")]
        public Role Role { get; set; }

        [Column("Email")]
        public string Email { get; set; } = string.Empty;

        [Column("UserName")]
        public string UserName { get; set; } = string.Empty;

        [Column("PasswordHash")]
        public string PasswordHash { get; set; } = string.Empty;

        [Column("ProfilePicture")]
        public string ProfilePicture {  get; set; } = string.Empty;

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }

    }
}
