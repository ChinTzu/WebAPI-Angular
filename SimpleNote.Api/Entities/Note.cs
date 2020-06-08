using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleNote.Api.Entities
{
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Username { get; set; }
        public DateTime LastModified { get; set; }

        public string Remark { get; set; }
    }
}
