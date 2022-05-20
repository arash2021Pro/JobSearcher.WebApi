using Microsoft.EntityFrameworkCore;

namespace JobSearcher.CoreDomains.ReposistoryPattern;

public interface IUnitOfWork
{
    DbSet<TEntity> Set<TEntity>() where TEntity:class;
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken  = new CancellationToken() );
}