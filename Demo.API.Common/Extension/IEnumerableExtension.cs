namespace Demo.API.Common.Extension
{
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumerableExtension
    {
        public static bool AnyOrNotNull<T>(this IEnumerable<T> source)
        {
            return source != null && source.Any();
        }
    }
}