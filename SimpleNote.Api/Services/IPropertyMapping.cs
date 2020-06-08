using System.Collections.Generic;

namespace SimpleNote.Api.Services
{
    public interface IPropertyMapping
    {
        Dictionary<string, List<MappedProperty>> MappingDictionary { get; }
    }
}
