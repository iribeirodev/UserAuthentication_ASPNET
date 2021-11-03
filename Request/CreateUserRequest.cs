using System.ComponentModel.DataAnnotations;
using ShopAutenticacao.Models.Validation;

namespace ShopAutenticacao.Request
{
    public class CreateUserRequest
    {
        [StringLength(12, MinimumLength = 5,
        ErrorMessage = "Username deve ter entre 10 e 15 caracteres")]
        public string Username { get; set; }

        [StringLength(12, MinimumLength = 5,
        ErrorMessage = "Password deve ter entre 12 e 36 caracteres")]
        public string Password { get; set; }

        [Required]
        [RoleAttribute]
        public string Role { get; set; }
    }
}