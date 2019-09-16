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
    [Route("api/product")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ProductDto productDto)
        {
            if (!this.ModelState.IsValid)
            {
                var errors = string.Join(
                    " | ",
                    this.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, errors, this.ModelState));
            }

            if (await this.productService.IsNameExistAsync(productDto))
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Product already exist", string.Empty));

            if (!await this.productService.IsCatalogExist(productDto))
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Catalog not found", string.Empty));

            var newProduct = await this.productService.CreateAsync(productDto);

            return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Product added successfully", newProduct));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            if (!Guid.TryParse(id, out var productId))
                return this.Ok(
                    ApiResponse.SetResponse(ApiResponseStatus.Error, $"{id} is not a valid Guid", string.Empty));

            var getProduct = await this.productService.GetAsync(productId);

            if (getProduct == null)
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Product not found", string.Empty));

            await this.productService.DeleteAsync(productId);

            return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Product deleted successfully", true));
        }

        [HttpGet]
        public async Task<IActionResult> GetProductAsync()
        {
            var products = await this.productService.GetListAsync();
            var productDtos = Mapper.Map<List<ProductDto>>(products);
            if (!productDtos.AnyOrNotNull())
                return this.Ok(
                    ApiResponse.SetResponse(ApiResponseStatus.Ok, "Get product list successfully", productDtos));

            return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Get product list successfully", productDtos));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(string id, [FromBody] ProductDto productDto)
        {
            if (!this.ModelState.IsValid)
            {
                var errors = string.Join(
                    " | ",
                    this.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, errors, this.ModelState));
            }

            if (!Guid.TryParse(id, out var productId))
                return this.Ok(
                    ApiResponse.SetResponse(ApiResponseStatus.Error, $"{id} is not a valid Guid", string.Empty));

            var updateProduct = await this.productService.GetAsync(productId);

            if (updateProduct == null)
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Product not found", string.Empty));

            if (!await this.productService.IsCatalogExist(productDto))
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Catalog not found", string.Empty));

            if (await this.productService.IsNameExistAsync(productDto))
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Product already exist", string.Empty));

            await this.productService.UpdateAsync(productDto);

            return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Product updated successfully", productDto));
        }
    }
}