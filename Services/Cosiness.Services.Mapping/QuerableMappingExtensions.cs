namespace Cosiness.Services.Mapping
{
    using AutoMapper.QueryableExtensions;

    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public static class QuerableMappingExtensions
    {
        public static IQueryable<TDestination> To<TDestination>(
            this IQueryable source,
            params Expression<Func<TDestination, object>>[] membersToExpand)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            //return source.ProjectTo(AutoMapperConfig.MapperInstance.ConfigurationProvider, null, membersToExpand);
            return source.ProjectTo(AutoMapperConfig.MapperInstance.ConfigurationProvider, membersToExpand);
        }

        public static IQueryable<TDestination> To<TDestination>(
            this IQueryable source,
            object parameters)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.ProjectTo<TDestination>(AutoMapperConfig.MapperInstance.ConfigurationProvider, parameters);
        }
    }
}