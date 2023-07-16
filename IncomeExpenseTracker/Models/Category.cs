namespace IncomeExpenseTracker.Models
{
    public class Category
    {

        public Guid id { get; set; }
        public string Name { get; set; }
        public string? ParentCategoryId { get; set; }
        public virtual Category? Categories { get; set; }
        public virtual ICollection<Category>? Subcategories { get; set; }
        public virtual ICollection<Transaction>? Transactions { get; set; }
    }
}
