using App.Repositories.BaseEntites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace App.Repositories.Interceptors;

public class AuditDbContextInterceptor: SaveChangesInterceptor
{
    private static readonly Dictionary<EntityState, Action<DbContext, IAuditEntity>> Behaviors = new()
    {
        { EntityState.Added, AddBehavior},
        { EntityState.Modified, UpdateBehavior}
    };
    
    private static void AddBehavior(DbContext context, IAuditEntity auditEntity)
    {
        auditEntity.CreatedDate = DateTime.UtcNow;
        context.Entry(auditEntity).Property(x=>x.UpdatedDate).IsModified = false;
    }
    
    private static void UpdateBehavior(DbContext context, IAuditEntity auditEntity)
    {
        auditEntity.UpdatedDate = DateTime.UtcNow;
        context.Entry(auditEntity).Property(x => x.CreatedDate).IsModified = false;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entityEntry in eventData.Context!.ChangeTracker.Entries().ToList())
        {
            if (entityEntry.Entity is not IAuditEntity auditEntity) continue;

            #region First Way
            // switch (entityEntry.State)
            // {
            //     case EntityState.Added:
            //         AddBehavior(eventData.Context, auditEntity);
            //         break;
            //     case EntityState.Modified:
            //         UpdateBehavior(eventData.Context, auditEntity);
            //         break;
            // }

            #endregion
            if(Behaviors.TryGetValue(entityEntry.State, out var value))
                value(eventData.Context, auditEntity);
        }
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}