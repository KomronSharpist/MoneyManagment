using Microsoft.EntityFrameworkCore;
using MoneyManagment.DAL.Contexts;

namespace MoneyManagment.Api.Extensions;

public static class DataExtensions
{
    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<MoneyDbContext>();

        db.Database.Migrate();
    }
}
