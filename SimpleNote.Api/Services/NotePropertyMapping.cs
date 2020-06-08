using SimpleNote.Api.Dtos;
using SimpleNote.Api.Entities;
using System;
using System.Collections.Generic;


namespace SimpleNote.Api.Services
{
    //mapping from PostResource to post
    public class NotePropertyMapping : PropertyMapping<NoteDto, Note>
    {
        public NotePropertyMapping() : base(
            new Dictionary<string, List<MappedProperty>>
                (StringComparer.OrdinalIgnoreCase)
            {
                [nameof(NoteDto.Title)] = new List<MappedProperty>
                    {
                        new MappedProperty{ Name = nameof(Note.Title), Revert = false}
                    },
                [nameof(NoteDto.Body)] = new List<MappedProperty>
                    {
                        new MappedProperty{ Name = nameof(Note.Body), Revert = false}
                    },
                [nameof(NoteDto.Username)] = new List<MappedProperty>
                    {
                        new MappedProperty{ Name = nameof(Note.Username), Revert = false}
                    }
            })
        {
        }
    }
}
