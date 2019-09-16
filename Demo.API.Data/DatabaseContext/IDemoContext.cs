namespace Demo.API.Data.DatabaseContext
{
    using System.Threading;
    using System.Threading.Tasks;

    using Demo.API.Data.Models;

    using Microsoft.EntityFrameworkCore;

    public interface IDemoContext
    {
        DbSet<Catalog> Catalogs { get; set; }

        DbSet<Product> Products { get; set; }

        void BeginTransaction();

        Task CommitTransactionAsync();

        void RollbackTransaction();

        int SaveChanges();

        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}