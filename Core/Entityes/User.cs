using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace Core.Entityes
{
    public class User : IdentityUser<Guid>
    {
        public DateTime CreatedDate { get; set; }

        public DateTime LastUpdateDate { get; set; }
        [DefaultValue(null)]
        public DateTime? FechaBaja { get; set; }
    }
}
