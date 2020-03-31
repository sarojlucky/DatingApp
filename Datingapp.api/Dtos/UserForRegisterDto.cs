using System.ComponentModel.DataAnnotations;

namespace Datingapp.api.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        [StringLength(8,MinimumLength = 4, ErrorMessage ="Enter password between 4-8 chars")]
        public string Password { get; set; }
    }
}