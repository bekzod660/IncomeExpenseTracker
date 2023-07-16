using Microsoft.AspNetCore.Identity;

namespace IncomeExpenseTracker.Models
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public string Type { get; set; }
        public string CategoryId { get; set; }
        public Category? Category { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
        public string UserId { get; set; }  // Foreign key for IdentityUser
        public IdentityUser? User { get; set; }

    }
}
