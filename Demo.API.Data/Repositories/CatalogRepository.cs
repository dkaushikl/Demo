namespace Demo.API.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Demo.API.Data.DatabaseContext;
    using Demo.API.Data.Interfaces;
    using Demo.API.Data.Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class CatalogRepository : ICatalogRepository
    {
        private readonly IDemoContext context;

        private readonly ILogger<CatalogRepository> logger;

        public CatalogRepository(IDemoContext context, ILogger<CatalogRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<int> CreateAsync(Catalog model)
        {
            try
            {
                await this.context.Catalogs.AddAsync(model);
                return await this.context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(100, ex, "error");
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity = await this.context.Catalogs.FirstOrDefaultAsync(e => e.Id == id);
                entity.Disabled = true;
                this.context.SaveChanges();
            }
            catch (Exception ex)
            {
                this.logger.LogError(100, ex, "error");
                throw;
            }
        }

        public async Task<Catalog> GetAsync(Guid id)
        {
            try
            {
                return await this.context.Catalogs.FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception ex)
            {
                this.logger.LogError(100, ex, "error");
                throw;
            }
        }

        public async Task<List<Catalog>> GetListAsync()
        {
            try
            {
                return await this.context.Catalogs.AsNoTracking().AsQueryable().OrderBy(x => x.Order).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(100, ex, "error");
                throw;
            }
        }

        public async Task<bool> IsNameExistAsync(Catalog catalog)
        {
            try
            {
                return await this.context.Catalogs.AnyAsync(
                           e => string.Equals(e.Name.ToLower(), catalog.Name.ToLower(), StringComparison.Ordinal)
                                && e.Id != catalog.Id);
            }
            catch (Exception ex)
            {
                this.logger.LogError(100, ex, "error");
                throw;
            }
        }

        public async Task<int> UpdateAsync(Catalog model)
        {
            try
            {
                return await this.context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(100, ex, "error");
                throw;
            }
        }
    }
}