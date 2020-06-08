using Microsoft.EntityFrameworkCore;
using SimpleNote.Api.Data;
using SimpleNote.Api.Dtos;
using SimpleNote.Api.Entities;
using SimpleNote.Api.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleNote.Api.Services
{
    public class NoteRepository : INoteRepository
    {
        private readonly MyContext _myContext;
        private readonly IPropertyMappingContainer _propertyMappingContainer;

        public NoteRepository(MyContext myContext, IPropertyMappingContainer propertyMappingContainer)
        {
            _myContext = myContext;
            _propertyMappingContainer = propertyMappingContainer;
        }

        public void AddNote(Note note)
        {
            _myContext.Notes.Add(note);
        }

        public void Delete(Note note)
        {
            _myContext.Notes.Remove(note);
        }

        public async Task<PagedList<Note>> GetAllNotes(NoteDtoParameters parameters)
        {
            if (parameters == null) { throw new ArgumentNullException(nameof(parameters)); }

            var query = _myContext.Notes as IQueryable<Note>;

            //filter
            if (!string.IsNullOrWhiteSpace(parameters.Username))
            {
                parameters.Username = parameters.Username.Trim();
                query = query.Where(x => x.Username == parameters.Username);
            }

            //search
            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                parameters.Search = parameters.Search.Trim();
                query = query.Where(x => x.Title.Contains(parameters.Search) ||
                                                             x.Body.Contains(parameters.Search));
            }

            //var count = await query.CountAsync();
            //var data = await query.Skip(parameters.PageSize * (parameters.PageNumber - 1)).Take(parameters.PageSize).ToListAsync();
            //return new PagedList<Note>(data, count, parameters.PageNumber, parameters.PageSize);

            query = query.ApplySort(parameters.OrderBy, _propertyMappingContainer.Resolve<NoteDto, Note>());

            return await PagedList<Note>.CreateAsync(query, parameters.PageNumber, parameters.PageSize);

        }

        public async Task<Note> GetNoteById(int noteId)
        {
            return await _myContext.Notes.FirstOrDefaultAsync(x => x.Id == noteId);
        }

        public void Update(Note note)
        {
            _myContext.Entry(note).State = EntityState.Modified;
        }

        public async Task<bool> SaveAsync()
        {
            return await _myContext.SaveChangesAsync() >= 0;
        }
    }
}
