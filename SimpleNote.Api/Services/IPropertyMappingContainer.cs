namespace SimpleNote.Api.Services
{
    public interface IPropertyMappingContainer
    {
        void Register<T>() where T : IPropertyMapping, new();
        IPropertyMapping Resolve<TSource, TDestination>();
        bool ValidateMappingExistsFor<TSource, TDestination>(string fields);
    }
}