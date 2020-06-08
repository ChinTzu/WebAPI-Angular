using SimpleNote.Api.Dtos;
using SimpleNote.Api.Entities;
using SimpleNote.Api.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleNote.Api.Services
{
    public interface INoteRepository
    {
        Task<PagedList<Note>> GetAllNotes(NoteDtoParameters parameters);
        Task<Note> GetNoteById(int id);

        void AddNote(Note post);
        void Delete(Note post);
        void Update(Note post);
        Task<bool> SaveAsync();
        
    }
}
