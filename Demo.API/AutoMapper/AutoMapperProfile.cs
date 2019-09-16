namespace Demo.AutoMapper
{
    using Demo.API.Data.Models;
    using Demo.API.Dtos;

    using global::AutoMapper;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            this.CreateMap<Catalog, CatalogDto>();
            this.CreateMap<CatalogDto, Catalog>();

            this.CreateMap<Product, ProductDto>();
            this.CreateMap<ProductDto, Product>();
        }
    }
}