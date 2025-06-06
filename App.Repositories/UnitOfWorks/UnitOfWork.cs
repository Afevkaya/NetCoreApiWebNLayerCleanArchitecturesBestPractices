using App.Repositories.Context;

namespace App.Repositories.UnitOfWorks;

public class UnitOfWork(AppDbContext context): IUnitOfWork
{
    public Task<int> CommitAsync() => context.SaveChangesAsync();
}