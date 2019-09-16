namespace Demo.API.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Product : BaseEntity
    {
        [Required(ErrorMessage = "A catalog is required")]
        public Guid CatalogId { get; set; }

        [Required(ErrorMessage = "A name is required")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public int Order { get; set; }
    }
}