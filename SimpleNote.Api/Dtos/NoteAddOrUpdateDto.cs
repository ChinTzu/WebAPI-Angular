using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleNote.Api.Dtos
{
    public class NoteAddOrUpdateDto
    {
        [Required(ErrorMessage = "{0} must be filled")]
        public string Title { get; set; }

        [Required(ErrorMessage = "{0} must be filled")]
        public string Body { get; set; }

        public string Remark { get; set; }
    }
}
