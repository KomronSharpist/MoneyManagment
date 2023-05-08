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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Fluent API relations

        modelBuilder.Entity<Transaction>()
            .HasOne(i => i.User)
            .WithMany(p => p.Transactions)
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Transaction>()
            .HasOne(i => i.TransactionCategory)
            .WithMany(p => p.Transactions)
            .HasForeignKey(i => i.TransactionCategoryId)
            .OnDelete(DeleteBehavior.NoAction);

        #endregion

        #region Seed data
        
        modelBuilder.Entity<User>().HasData(
                new User() { Id = 1, FirstName = "admin", LastName = "admin", Email = "admin@gmail.com", Password = "$2a$11$L8TlL9xtC4YQu/fbHv6jzOCwIgp2tvAPgyPb4EWZvyhsJ8ERp7CYG", CreatedAt = DateTime.UtcNow, DeletedBy = null, ImagePath = "wwwroot\\uploads\\images\\1c6aee695d58469a8fa431333374f906.png", IsVerify = true, Role = Domain.Enums.Roles.Admin, Salt = "b24ed81a-6853-4bed-910d-a0cf432a335f", IsDeleted = false, UpdatedAt = null, UpdatedBy = null },
                new User() { Id = 2, FirstName = "user", LastName = "user", Email = "user@gmail.com", Password = "$2a$11$Vhb626LK0A0vS4C5BAhMuO3ybHR5g1mhcoVp0mo4cCF05dq22F6f2", CreatedAt = DateTime.UtcNow, DeletedBy = null, ImagePath = "wwwroot\\\\uploads\\\\images\\\\3d89777ec7f74a00b77e97397fce10e6.jpg", IsVerify = true, Role = Domain.Enums.Roles.Admin, Salt = "27ffbd0d-569d-4e1c-bb6b-48b846463982", IsDeleted = false, UpdatedAt = null, UpdatedBy = null }); ; ;
        
        #endregion
    }
}