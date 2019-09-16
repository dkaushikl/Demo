namespace Demo.API.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Demo.API.Data.Models;
    using Demo.API.Dtos;

    public interface IProductService
    {
        Task<ProductDto> CreateAsync(ProductDto model);

        Task DeleteAsync(Guid id);

        Task<ProductDto> GetAsync(Guid id);

        Task<List<Product>> GetListAsync();

        Task<bool> IsCatalogExist(ProductDto model);

        Task<bool> IsNameExistAsync(ProductDto model);

        Task<ProductDto> UpdateAsync(ProductDto model);
    }
}