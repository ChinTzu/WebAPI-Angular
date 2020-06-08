using AutoMapper;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SimpleNote.Api.Dtos;
using SimpleNote.Api.Entities;
using SimpleNote.Api.Helpers;
using SimpleNote.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace SimpleNote.Api.Controllers
{
    [ApiController]
    [Route("api/notes")]
    public class NoteController : ControllerBase
    {
        private readonly INoteRepository _noteRepository;
        private readonly IMapper _mapper;
        private readonly IPropertyMappingContainer _propertyMappingContainer;

        public NoteController(INoteRepository noteRepository,
                                IMapper mapper,
                                IPropertyMappingContainer propertyMappingContainer)
        {
            _noteRepository = noteRepository;
            _mapper = mapper;
            _propertyMappingContainer = propertyMappingContainer;
        }

        //=================== Collection data ==================//
        [AllowAnonymous]
        [HttpGet(Name = nameof(GetAllNotes))]
        [HttpHead]
        public async Task<IActionResult> GetAllNotes([FromQuery] NoteDtoParameters parameters)
        {
            var notes = await _noteRepository.GetAllNotes(parameters);

            var previousPageLink = notes.HasPrevious ? CreateNotesResourceUri(parameters, ResourceUriType.PreviousPage) : null;
            var nextPageLink = notes.HasNext ? CreateNotesResourceUri(parameters, ResourceUriType.NextPage) : null;

            var pageData = new
            {
                totalCount = notes.TotalCount,
                pageSize = notes.PageSize,
                currentPage = notes.CurrentPage,
                totalPages = notes.TotalPages,
                previousPageLink,
                nextPageLink
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pageData,
                new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }));

            var noteDto = _mapper.Map<IEnumerable<NoteDto>>(notes);

            var shapedData = noteDto.ShapeData(parameters.Fields);

            var links = CreateLinksForNotes(parameters, notes.HasPrevious, notes.HasNext);

            return Ok(shapedData);
        }

        [AllowAnonymous]
        [HttpGet(Name = "GetAllNotes")]
        [SupportMediaType("Accept", "application/vnd.mycompany.hateoas+json")]
        public async Task<IActionResult> GetAllNotesWithHateoas([FromQuery] NoteDtoParameters parameters)
        {
            var notes = await _noteRepository.GetAllNotes(parameters);

            var previousPageLink = notes.HasPrevious ? CreateNotesResourceUri(parameters, ResourceUriType.PreviousPage) : null;
            var nextPageLink = notes.HasNext ? CreateNotesResourceUri(parameters, ResourceUriType.NextPage) : null;

            var pageData = new
            {
                totalCount = notes.TotalCount,
                pageSize = notes.PageSize,
                currentPage = notes.CurrentPage,
                totalPages = notes.TotalPages,
                previousPageLink,
                nextPageLink
            };

            var noteDto = _mapper.Map<IEnumerable<NoteDto>>(notes);

            var shapedData = noteDto.ShapeData(parameters.Fields);

            var shapedWithLinks = shapedData.Select(x =>
            {
                var dict = x as IDictionary<string, object>;
                var noteLinks = CreateLinksForNote((int)dict["Id"], parameters.Fields);
                dict.Add("links", noteLinks);
                return dict;
            });

            var links = CreateLinksForNotes(parameters, notes.HasPrevious, notes.HasNext);

            var result = new { value = shapedWithLinks, links };

            return Ok(result);
        }

        //=================== Single data ==================//

        [AllowAnonymous]
        [HttpGet(template: "{id}", Name = nameof(GetNote))]
        public async Task<IActionResult> GetNote(int id, string fields = null)
        {
            var note = await _noteRepository.GetNoteById(id);
            if (note == null) { return NotFound(); }

            var noteDto = _mapper.Map<NoteDto>(note);

            var shapedData = noteDto.ShapeData(fields);

            var links = CreateLinksForNote(id, fields);

            var result = shapedData as IDictionary<string, object>;
            result.Add("links", links);

            return Ok(result);
        }

        [HttpPost(Name = nameof(CreateNote))]
        [SupportMediaType("Content-Type", "application/json", "application/vnd.mycompany.note.create+json")]
        [Consumes("application/json", "application/vnd.mycompany.note.create+json")]
        public async Task<ActionResult<NoteDto>> CreateNote([FromBody] NoteAddDto noteAddDto)
        {
            if (noteAddDto == null) { return BadRequest(); }
            if (!ModelState.IsValid) { return UnprocessableEntity(ModelState); }

            var note = _mapper.Map<Note>(noteAddDto);

            note.Username = User.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.PreferredUserName)?.Value; 
            note.LastModified = DateTime.Now;

            _noteRepository.AddNote(note);

            await _noteRepository.SaveAsync();

            var ReturnDto = _mapper.Map<NoteDto>(note);

            var links = CreateLinksForNote(ReturnDto.Id, fields: null);
            var linkedDict = ReturnDto.ShapeData(fields: null) as IDictionary<string, object>;
            linkedDict.Add("links", links);

            return CreatedAtRoute(nameof(GetNote), new { id = ReturnDto.Id }, linkedDict);
        }

        [HttpDelete("{id}", Name = "DeleteNote")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var post = await _noteRepository.GetNoteById(id);

            if (post == null)
            {
                return NotFound();
            }

            _noteRepository.Delete(post);

            if (!await _noteRepository.SaveAsync())
            {
                throw new Exception($"Deleting note {id} failed when saving.");
            }

            return NoContent();
        }

        [HttpPut("{id}", Name = "UpdateNote")]
        [SupportMediaType("Content-Type", "application/vnd.mycompany.note.update+json")]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] NoteUpdateDto noteUpdate)
        {
            if (noteUpdate == null) { return BadRequest(); }
            if (!ModelState.IsValid) { return UnprocessableEntity(ModelState); }

            var note = await _noteRepository.GetNoteById(id);
            if (note == null) { return NotFound(); }

            note.LastModified = DateTime.Now;
            _mapper.Map(noteUpdate, note);

            await _noteRepository.SaveAsync();

            return NoContent();
        }

        [HttpPatch("{id}", Name = "PartiallyUpdateNote")]
        public async Task<IActionResult> PartiallyUpdatePost(int id, [FromBody] JsonPatchDocument<NoteUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var note = await _noteRepository.GetNoteById(id);

            if (note == null)
            {
                return NotFound();
            }

            var noteToPatch = _mapper.Map<Note, NoteUpdateDto>(note);

            patchDoc.ApplyTo(noteToPatch, ModelState);

            TryValidateModel(noteToPatch);

            if (!ModelState.IsValid)
            {
                { return UnprocessableEntity(ModelState); }
            }

            _mapper.Map(noteToPatch, note);
            note.LastModified = DateTime.Now;
            _noteRepository.Update(note);

            await _noteRepository.SaveAsync();

            return NoContent();
        }

        private string CreateNotesResourceUri(NoteDtoParameters parameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetAllNotes), new
                    {
                        fields = parameters.Fields,
                        orderBy = parameters.OrderBy,
                        pageNumber = parameters.PageNumber - 1,
                        pageSize = parameters.PageSize,
                        search = parameters.Search
                    });

                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetAllNotes), new
                    {
                        fields = parameters.Fields,
                        orderBy = parameters.OrderBy,
                        pageNumber = parameters.PageNumber + 1,
                        pageSize = parameters.PageSize,
                        search = parameters.Search
                    });

                case ResourceUriType.CurrentPage:
                default:
                    return Url.Link(nameof(GetAllNotes), new
                    {
                        fields = parameters.Fields,
                        orderBy = parameters.OrderBy,
                        pageNumber = parameters.PageNumber,
                        pageSize = parameters.PageSize,
                        search = parameters.Search
                    });
            }
        }

        private IEnumerable<LinkDto> CreateLinksForNotes(NoteDtoParameters parameters, bool hasPrevious, bool hasNext)
        {
            var links = new List<LinkDto>();

            links.Add(new LinkDto(CreateNotesResourceUri(parameters, ResourceUriType.CurrentPage), "self", "GET"));

            if (hasPrevious)
            {
                links.Add(new LinkDto(CreateNotesResourceUri(parameters, ResourceUriType.PreviousPage), "previous_page", "GET"));
            }

            if (hasNext)
            {
                links.Add(new LinkDto(CreateNotesResourceUri(parameters, ResourceUriType.NextPage), "next_page", "GET"));
            }

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForNote(int id, string fields = null)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(new LinkDto(href: Url.Link(nameof(GetNote), values: new { id }), rel: "self", method: "GET"));
            }
            else
            {
                links.Add(new LinkDto(href: Url.Link(nameof(GetNote), values: new { id, fields }), rel: "self", method: "GET"));
            }

            links.Add(new LinkDto(Url.Link(nameof(DeleteNote), new { id }), "delete_note", "DELETE"));

            return links;
        }
    }
}