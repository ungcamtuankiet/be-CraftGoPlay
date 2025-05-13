using System.ComponentModel.DataAnnotations;

namespace CGP.Domain.Entities
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string RoleName { get; set; }
        public ICollection<ApplicationUser>? Users { get; set; }

    }
}
