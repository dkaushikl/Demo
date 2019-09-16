namespace Demo.API.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoMapper;

    using Demo.API.Data.Interfaces;
    using Demo.API.Data.Models;
    using Demo.API.Dtos;
    using Demo.API.Services.Exceptions;
    using Demo.API.Services.Interfaces;

    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<ProductDto> CreateAsync(ProductDto model)
        {
            var entity = Mapper.Map<Product>(model);
            await this.productRepository.CreateAsync(entity);

            var dto = Mapper.Map<ProductDto>(entity);
            return dto;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await this.productRepository.GetAsync(id);
            if (entity == null) throw new EntityNotFound($"Product with Id {id.ToString()} not found.");

            await this.productRepository.DeleteAsync(id);
        }

        public async Task<ProductDto> GetAsync(Guid id)
        {
            var entity = await this.productRepository.GetAsync(id);
            var product = Mapper.Map<ProductDto>(entity);

            if (product == null) throw new EntityNotFound($"product with Id {id.ToString()} not found.");

            return product;
        }

        public async Task<List<Product>> GetListAsync()
        {
            return await this.productRepository.GetListAsync();
        }

        public async Task<bool> IsCatalogExist(ProductDto model)
        {
            var entity = Mapper.Map<Product>(model);
            var result = await this.productRepository.IsCatalogExist(entity);
            return result;
        }

        public async Task<bool> IsNameExistAsync(ProductDto model)
        {
            var entity = Mapper.Map<Product>(model);
            var result = await this.productRepository.IsNameExistAsync(entity);
            return result;
        }

        public async Task<ProductDto> UpdateAsync(ProductDto model)
        {
            var entity = await this.productRepository.GetAsync(model.Id);
            if (entity == null) throw new EntityNotFound($"Product with id: {model.Id} not found");

            Mapper.Map(model, entity);
            await this.productRepository.UpdateAsync(entity);

            var dto = Mapper.Map<ProductDto>(entity);
            return dto;
        }
    }
}