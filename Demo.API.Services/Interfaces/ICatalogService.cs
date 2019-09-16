namespace Demo.API.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Demo.API.Data.Models;
    using Demo.API.Dtos;

    public interface ICatalogService
    {
        Task<CatalogDto> CreateAsync(CatalogDto model);

        Task DeleteAsync(Guid id);

        Task<CatalogDto> GetAsync(Guid id);

        Task<List<Catalog>> GetListAsync();

        Task<bool> IsNameExistAsync(CatalogDto catalog);

        Task<CatalogDto> UpdateAsync(CatalogDto model);
    }
}