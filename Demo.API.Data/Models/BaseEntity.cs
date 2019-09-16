namespace Demo.API.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class BaseEntity
    {
        [Required(ErrorMessage = "A created date is required")]
        public DateTime CreatedDate { get; set; }

        [Required(ErrorMessage = "A disabled is required")]
        public bool Disabled { get; set; }

        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "A modified date is required")]
        public DateTime ModifiedDate { get; set; }
    }
}