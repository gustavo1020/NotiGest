using Microsoft.AspNetCore.Identity;

namespace Application.DTO
{
    public class UsuarioDto : IdentityUser<Guid>
    {
    }
    public class UserInsertDto
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
