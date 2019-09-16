namespace Demo.API.Services.Exceptions
{
    using System;

    public class EntityNotFound : Exception
    {
        public EntityNotFound(string message)
            : base(message)
        {
        }
    }
}