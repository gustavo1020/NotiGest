using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace Core.Entityes
{
    public class Rol : IdentityRole<Guid>
    {
        public Rol() { }

        public Rol(string roleName) : this()
        {
            Name = roleName;
        }
        public DateTime CreatedDate { get; set; }

        public DateTime LastUpdateDate { get; set; }

        [DefaultValue(null)]
        public DateTime? FechaBaja { get; set; }
    }
}
