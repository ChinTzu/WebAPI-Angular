using AutoMapper;
using SimpleNote.Api.Dtos;
using SimpleNote.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleNote.Api.Profiles
{
    public class NoteProfile : Profile
    {
        public NoteProfile()
        {
            CreateMap<Note, NoteDto>()
                .ForMember(dest => dest.UpdateTime, opt => opt.MapFrom(src => src.LastModified));

            CreateMap<NoteAddDto, Note>();
            CreateMap<Note, NoteUpdateDto>();
            CreateMap<NoteUpdateDto, Note>();
        }
    }
}
