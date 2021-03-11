namespace Cosiness.Services.Data.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class ValidationExtensions
    {
        public static void ThrowIfNullOrEmpty(this IValidatable service, string param)
        {
            if (string.IsNullOrEmpty(param))
            {
                throw new ArgumentException($"{service.GetType().Name} - Parameter cannot be null or empty!");
            }
        }

        public static void ThrowIfIncorrectId(this IValidatable service, object entity, string id)
        {
            if (entity is null)
            {
                throw new ArgumentException($"{service.GetType().Name} - Incorrect id: {id}!");
            }
        }

        public static void ThrowIfEmptyCollection(this IValidatable service, IEnumerable<object> collection)
        {
            if (!collection.Any())
            {
                throw new InvalidOperationException($"{service.GetType().Name} - Collection is empty!");
            }
        }
    }
}