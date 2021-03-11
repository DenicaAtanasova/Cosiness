namespace Cosiness.Services.Data.Helpers
{
    using System;

    public static class ValidationExtensions
    {
        public static void ThrowIfNullOrEmpty(this object service, string param)
        {
            if (string.IsNullOrEmpty(param))
            {
                throw new ArgumentException($"{service.GetType().Name} - parameter cannot be null or empty!");
            }
        }

        public static void ThrowIfIncorrectId(this object service, object entity, string id)
        {
            if (entity is null)
            {
                throw new ArgumentException($"{service.GetType().Name} - incorrect id: {id}");
            }
        }
    }
}