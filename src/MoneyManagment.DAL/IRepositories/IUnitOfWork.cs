using MoneyManagment.Domain.Entities;

namespace MoneyManagment.DAL.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<Transaction> Transactions { get; }
        IRepository<TransactionCategory> TransactionCategories { get; set; }

        Task<bool> SaveChangesAsync();
    }
}
