namespace Demo.API.Data.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;

    public static class DbContextValidationExtensions
    {
        public static void ValidateCreatedAndUpdatedEntities(this DbContext context)
        {
            var createdAndUpdatedEntities = context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).Select(e => e.Entity);

            foreach (var entity in createdAndUpdatedEntities)
            {
                var validationContext = new ValidationContext(entity, null, null);
                var results = new List<ValidationResult>();

                var valid = Validator.TryValidateObject(entity, validationContext, results, true);
                if (!valid) throw new InvalidOperationException(results.First().ErrorMessage);

                if (entity is IValidatable validatable) validatable.Validate();
            }
        }
    }
}