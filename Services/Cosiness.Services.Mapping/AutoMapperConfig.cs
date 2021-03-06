﻿namespace Cosiness.Services.Mapping
{
    using AutoMapper;
    using AutoMapper.Configuration;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class AutoMapperConfig
    {
        private static bool initialized;

        public static IMapper MapperInstance { get; set; }

        public static void RegisterMappings(params Assembly[] assemblies)
        {
            if (initialized)
            {
                return;
            }

            var types = assemblies.SelectMany(a => a.GetExportedTypes()).ToList();
            var config = new MapperConfigurationExpression();

            config.CreateProfile(
                "ReflectionProfile",
                configuration =>
                {
                    //IMapFrom<>
                    foreach (var map in GetFromMaps(types))
                    {
                        configuration.CreateMap(map.Source, map.Destination);
                    }

                    //IMapTo<>
                    foreach (var map in GetToMaps(types))
                    {
                        configuration.CreateMap(map.Source, map.Destination);
                    }

                    //IMapExplicitly
                    foreach (var map in GetCustomMapping(types))
                    {
                        map.CreateMappings(configuration);
                    }
                });

            MapperInstance = new Mapper(new MapperConfiguration(config));
            initialized = true;
        }

        private static IEnumerable<TypesMap> GetFromMaps(IEnumerable<Type> types)
        {
            var fromMaps = from t in types
                           from i in t.GetTypeInfo().GetInterfaces()
                           where i.GetTypeInfo().IsGenericType &&
                                 i.GetGenericTypeDefinition() == typeof(IMapFrom<>) &&
                                 !t.GetTypeInfo().IsAbstract &&
                                 !t.GetTypeInfo().IsInterface
                           select new TypesMap
                           {
                               Source = i.GetTypeInfo().GetGenericArguments()[0],
                               Destination = t
                           };

            return fromMaps;
        }

        private static IEnumerable<TypesMap> GetToMaps(IEnumerable<Type> types)
        {
            var toMaps = from t in types
                         from i in t.GetTypeInfo().GetInterfaces()
                         where i.GetTypeInfo().IsGenericType &&
                               i.GetTypeInfo().GetGenericTypeDefinition() == typeof(IMapTo<>) &&
                               !t.GetTypeInfo().IsAbstract &&
                               !t.GetTypeInfo().IsInterface
                         select new TypesMap
                         {
                             Source = t,
                             Destination = i.GetTypeInfo().GetGenericArguments()[0],
                         };

            return toMaps;
        }

        private static IEnumerable<IMapExplicitly> GetCustomMapping(IEnumerable<Type> types)
        {
            var customMaps = from t in types
                             from i in t.GetTypeInfo().GetInterfaces()
                             where typeof(IMapExplicitly).GetTypeInfo().IsAssignableFrom(t) &&
                                   !t.GetTypeInfo().IsAbstract &&
                                   !t.GetTypeInfo().IsInterface
                             select (IMapExplicitly)Activator.CreateInstance(t);

            return customMaps;
        }

        private class TypesMap
        {
            public Type Source { get; set; }

            public Type Destination { get; set; }
        }
    }
}