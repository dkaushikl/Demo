namespace Demo.API.Dtos
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ProductDto : BaseEntityDto
    {
        [Required(ErrorMessage = "A catalog is required")]
        public Guid CatalogId { get; set; }

        [Required(ErrorMessage = "A name is required")]
        [MaxLength(100, ErrorMessage = "The name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required]
        public int Order { get; set; }
    }
}