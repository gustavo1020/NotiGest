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
    public class UserGet
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? passwordHash { get; set; }
        public DateTime createdDate { get; set; }
        public string? phoneNumber { get; set; }
    }
}
