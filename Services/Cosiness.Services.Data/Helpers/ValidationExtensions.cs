namespace Cosiness.Services.Data.Helpers
{
    using Cosiness.Models.Common;

    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class ValidationExtensions
    {
        public static void ThrowIfNullOrEmpty(this IValidator service, string param)
        {
            if (string.IsNullOrEmpty(param))
            {
                throw new ArgumentException($"{service.GetType().Name} - Parameter cannot be null or empty!");
            }
        }

        public static void ThrowIfIncorrectId<TEntity>(
            this IValidator service, 
            DbSet<TEntity> dbSet, string entityId)
            where TEntity : BaseEntity<string>
        {
            if (!dbSet.Any(x => x.Id == entityId))
            {
                throw new ArgumentException($"{service.GetType().Name} - Incorrect id!");
            }
        }

        public static void ThrowIfEmptyCollection(this IValidator service, IEnumerable<object> collection)
        {
            if (!collection.Any())
            {
                throw new InvalidOperationException($"{service.GetType().Name} - Collection is empty!");
            }
        }
    }
}