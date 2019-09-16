namespace Demo.API.Data.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Demo.API.Data.Models;

    public interface IProductRepository
    {
        Task<int> CreateAsync(Product model);

        Task DeleteAsync(Guid id);

        Task<Product> GetAsync(Guid id);

        Task<List<Product>> GetListAsync();

        Task<bool> IsCatalogExist(Product model);

        Task<bool> IsNameExistAsync(Product model);

        Task<int> UpdateAsync(Product model);
    }
}