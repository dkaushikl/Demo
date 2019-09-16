namespace Demo.API.Dtos
{
    using System.ComponentModel.DataAnnotations;

    public class CatalogDto : BaseEntityDto
    {
        [Required(ErrorMessage = "A name is required")]
        [MaxLength(100, ErrorMessage = "The name cannot exceed 100 characters")]
        public string Name { get; set; }
    }
}