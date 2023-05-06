using MoneyManagment.DAL.Contexts;
using MoneyManagment.DAL.IRepositories;
using MoneyManagment.Domain.Entities;

namespace MoneyManagment.DAL.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly MoneyDbContext dbContext;

    public UnitOfWork(MoneyDbContext dbContext)
    {
        this.dbContext = dbContext;

        Users = new Repository<User>(dbContext);
        Transactions = new Repository<Transaction>(dbContext);
        TransactionCategories = new Repository<TransactionCategory>(dbContext);
    }

    public IRepository<User> Users { get; private set; }
    public IRepository<Transaction> Transactions { get; private set; }
    public IRepository<TransactionCategory> TransactionCategories { get; set; }


    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await dbContext.SaveChangesAsync() >= 0;
    }
}
