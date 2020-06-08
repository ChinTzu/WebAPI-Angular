using SimpleNote.Api.Entities;
using System.Collections.Generic;

namespace SimpleNote.Api.Services
{
    public abstract class PropertyMapping<TSource, TDestination> : IPropertyMapping
    {
        public Dictionary<string, List<MappedProperty>> MappingDictionary { get; }

        protected PropertyMapping(Dictionary<string, List<MappedProperty>> mappingDictionary)
        {
            MappingDictionary = mappingDictionary;
            MappingDictionary[nameof(Note.Id)] = new List<MappedProperty>
            {
                new MappedProperty { Name = nameof(Note.Id), Revert = false}
            };
        }
    }
}
