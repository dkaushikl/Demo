namespace Demo.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Demo.API.Common.Extension;
    using Demo.API.Dtos;
    using Demo.API.Services.Interfaces;
    using Demo.Utility;

    using global::AutoMapper;

    using Microsoft.AspNetCore.Mvc;

    [Produces("application/json")]
    [Route("api/catalog")]
    [ApiController]
    public class CatalogController : Controller
    {
        private readonly ICatalogService catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            this.catalogService = catalogService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CatalogDto catalogDto)
        {
            if (!this.ModelState.IsValid)
            {
                var errors = string.Join(
                    " | ",
                    this.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, errors, this.ModelState));
            }

            if (await this.catalogService.IsNameExistAsync(catalogDto))
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Catalog already exist", string.Empty));

            var newCatalog = await this.catalogService.CreateAsync(catalogDto);

            return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Catalog added successfully", newCatalog));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            if (!Guid.TryParse(id, out var catalogId))
                return this.Ok(
                    ApiResponse.SetResponse(ApiResponseStatus.Error, $"{id} is not a valid Guid", string.Empty));

            var getCatalog = await this.catalogService.GetAsync(catalogId);

            if (getCatalog == null)
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Catalog not found", string.Empty));

            await this.catalogService.DeleteAsync(catalogId);

            return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Catalog deleted successfully", true));
        }

        [HttpGet]
        public async Task<IActionResult> GetCatalogAsync()
        {
            var catalogs = await this.catalogService.GetListAsync();
            var catalogDtos = Mapper.Map<List<CatalogDto>>(catalogs);
            if (!catalogDtos.AnyOrNotNull())
                return this.Ok(
                    ApiResponse.SetResponse(ApiResponseStatus.Ok, "Get catalog list successfully", catalogDtos));

            return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Get catalog list successfully", catalogDtos));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(string id, [FromBody] CatalogDto catalogDto)
        {
            if (!this.ModelState.IsValid)
            {
                var errors = string.Join(
                    " | ",
                    this.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, errors, this.ModelState));
            }

            if (!Guid.TryParse(id, out var catalogId))
                return this.Ok(
                    ApiResponse.SetResponse(ApiResponseStatus.Error, $"{id} is not a valid Guid", string.Empty));

            var updateCatalog = await this.catalogService.GetAsync(catalogId);

            if (updateCatalog == null)
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Catalog not found", string.Empty));

            if (await this.catalogService.IsNameExistAsync(catalogDto))
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Catalog already exist", string.Empty));

            await this.catalogService.UpdateAsync(catalogDto);

            return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Catalog updated successfully", catalogDto));
        }
    }
}