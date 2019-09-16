namespace Demo.API.Data.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Demo.API.Data.Models;

    public interface ICatalogRepository
    {
        Task<int> CreateAsync(Catalog model);

        Task DeleteAsync(Guid id);

        Task<Catalog> GetAsync(Guid id);

        Task<List<Catalog>> GetListAsync();

        Task<bool> IsNameExistAsync(Catalog catalog);

        Task<int> UpdateAsync(Catalog model);
    }
}