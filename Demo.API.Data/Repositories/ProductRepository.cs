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

    public class ProductRepository : IProductRepository
    {
        private readonly IDemoContext context;

        private readonly ILogger<ProductRepository> logger;

        public ProductRepository(IDemoContext context, ILogger<ProductRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<int> CreateAsync(Product model)
        {
            try
            {
                await this.context.Products.AddAsync(model);
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
                var entity = await this.context.Products.FirstOrDefaultAsync(e => e.Id == id);
                entity.Disabled = true;
                this.context.SaveChanges();
            }
            catch (Exception ex)
            {
                this.logger.LogError(100, ex, "error");
                throw;
            }
        }

        public async Task<Product> GetAsync(Guid id)
        {
            try
            {
                return await this.context.Products.FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception ex)
            {
                this.logger.LogError(100, ex, "error");
                throw;
            }
        }

        public async Task<List<Product>> GetListAsync()
        {
            try
            {
                return await this.context.Products.AsNoTracking().AsQueryable().OrderBy(x => x.Order).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(100, ex, "error");
                throw;
            }
        }

        public async Task<bool> IsCatalogExist(Product model)
        {
            try
            {
                return await this.context.Catalogs.AnyAsync(x => x.Id == model.CatalogId);
            }
            catch (Exception ex)
            {
                this.logger.LogError(100, ex, "error");
                throw;
            }
        }

        public async Task<bool> IsNameExistAsync(Product model)
        {
            try
            {
                return await this.context.Products.AnyAsync(
                           e => string.Equals(e.Name.ToLower(), model.Name.ToLower(), StringComparison.Ordinal)
                                && e.Id != model.Id);
            }
            catch (Exception ex)
            {
                this.logger.LogError(100, ex, "error");
                throw;
            }
        }

        public async Task<int> UpdateAsync(Product model)
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