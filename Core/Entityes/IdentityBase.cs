using System.ComponentModel;

namespace Core.Entityes
{
    public class IdentityBase
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        [DefaultValue(null)]
        public DateTime? FechaBaja { get; set; }
    }
}
