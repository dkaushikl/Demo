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

    public class CatalogService : ICatalogService
    {
        private readonly ICatalogRepository catalogRepository;

        public CatalogService(ICatalogRepository catalogRepository)
        {
            this.catalogRepository = catalogRepository;
        }

        public async Task<CatalogDto> CreateAsync(CatalogDto model)
        {
            var entity = Mapper.Map<Catalog>(model);
            await this.catalogRepository.CreateAsync(entity);

            var dto = Mapper.Map<CatalogDto>(entity);
            return dto;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await this.catalogRepository.GetAsync(id);
            if (entity == null) throw new EntityNotFound($"Catalog with Id {id.ToString()} not found.");

            await this.catalogRepository.DeleteAsync(id);
        }

        public async Task<CatalogDto> GetAsync(Guid id)
        {
            var entity = await this.catalogRepository.GetAsync(id);
            var catalog = Mapper.Map<CatalogDto>(entity);

            if (catalog == null) throw new EntityNotFound($"catalog with Id {id.ToString()} not found.");

            return catalog;
        }

        public async Task<List<Catalog>> GetListAsync()
        {
            return await this.catalogRepository.GetListAsync();
        }

        public async Task<bool> IsNameExistAsync(CatalogDto objCatalogDto)
        {
            var entity = Mapper.Map<Catalog>(objCatalogDto);
            var result = await this.catalogRepository.IsNameExistAsync(entity);
            return result;
        }

        public async Task<CatalogDto> UpdateAsync(CatalogDto model)
        {
            var entity = await this.catalogRepository.GetAsync(model.Id);
            if (entity == null) throw new EntityNotFound($"Catalog with id: {model.Id} not found");

            Mapper.Map(model, entity);
            await this.catalogRepository.UpdateAsync(entity);

            var dto = Mapper.Map<CatalogDto>(entity);
            return dto;
        }
    }
}