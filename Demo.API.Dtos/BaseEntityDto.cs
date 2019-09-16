namespace Demo.API.Dtos
{
    using System;

    public class BaseEntityDto
    {
        public DateTime CreatedDate { get; set; }

        public bool Disabled { get; set; }

        public Guid Id { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}