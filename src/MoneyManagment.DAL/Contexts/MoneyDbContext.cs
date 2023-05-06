using Microsoft.EntityFrameworkCore;
using MoneyManagment.Domain.Entities;

namespace MoneyManagment.DAL.Contexts;

public class MoneyDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<TransactionCategory> TransactionCategories { get; set; }

    public MoneyDbContext(DbContextOptions<MoneyDbContext> options) : base(options)
    {

    }
}
