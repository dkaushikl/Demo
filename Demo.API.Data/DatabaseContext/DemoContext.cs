namespace Demo.API.Data.DatabaseContext
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Demo.API.Data.Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;

    public class DemoContext : DbContext, IDemoContext
    {
        private IDbContextTransaction currentTransaction;

        public DemoContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Catalog> Catalogs { get; set; }

        public DbSet<Product> Products { get; set; }

        public virtual void BeginTransaction()
        {
            if (this.currentTransaction != null) return;

            this.currentTransaction = this.Database.BeginTransaction();
        }

        public virtual async Task CommitTransactionAsync()
        {
            try
            {
                await this.SaveChangesAsync();

                this.currentTransaction?.Commit();
            }
            catch
            {
                this.RollbackTransaction();
                throw;
            }
            finally
            {
                if (this.currentTransaction != null)
                {
                    this.currentTransaction.Dispose();
                    this.currentTransaction = null;
                }
            }
        }

        public virtual void RollbackTransaction()
        {
            try
            {
                this.currentTransaction?.Rollback();
            }
            finally
            {
                if (this.currentTransaction != null)
                {
                    this.currentTransaction.Dispose();
                    this.currentTransaction = null;
                }
            }
        }

        public override int SaveChanges()
        {
            this.AddTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            this.ValidateCreatedAndUpdatedEntities();
            this.AddTimestamps();
            var updates = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            return updates;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.ValidateCreatedAndUpdatedEntities();
            this.AddTimestamps();
            var updates = await base.SaveChangesAsync(cancellationToken);
            return updates;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Catalog>(entity => entity.Property(e => e.Disabled).HasDefaultValue(false));
            modelBuilder.Entity<Product>(entity => entity.Property(e => e.Disabled).HasDefaultValue(false));
        }

        private void AddTimestamps()
        {
            var entities = this.ChangeTracker.Entries().Where(
                x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added) ((BaseEntity)entity.Entity).CreatedDate = DateTime.UtcNow;
                ((BaseEntity)entity.Entity).ModifiedDate = DateTime.UtcNow;
            }
        }
    }
}