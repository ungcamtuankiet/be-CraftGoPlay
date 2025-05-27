using CGP.Domain.Enums;

namespace CGP.Domain.Entities
{
    public class Category : BaseEntity
    {
        public Category()
        {
            SubCategories = new HashSet<SubCategory>();
        }
        public string CategoryName { get; set; }
        public CategoryStatusEnum CategoryStatus { get; set; }
        public virtual ICollection<SubCategory> SubCategories { get; set; }
    }
}
