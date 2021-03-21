namespace Cosiness.Services.Mapping
{
    using AutoMapper;

    public interface IMapExplicitly
    {
        void CreateMappings(IProfileExpression configuration);
    }
}
