using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleNote.Api.Dtos
{
    public class NoteDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Username { get; set; }
        public DateTime UpdateTime { get; set; }

        public string Remark { get; set; }
    }
}
