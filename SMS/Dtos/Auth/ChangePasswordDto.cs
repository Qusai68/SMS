using System.ComponentModel.DataAnnotations;

namespace SMS.Dtos.Auth
{
    public class ChangePasswordDto
    {
        [Required]
        public string OldPassword { get; set; }
        [Required , MinLength(8)]
        public string NewPassword { get; set; }
    }
}
