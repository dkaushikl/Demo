namespace Demo.API.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Catalog : BaseEntity
    {
        [Required(ErrorMessage = "A name is required")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public int Order { get; set; }
    }
}