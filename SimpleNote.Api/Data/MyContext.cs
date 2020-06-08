using Microsoft.EntityFrameworkCore;
using SimpleNote.Api.Entities;

namespace SimpleNote.Api.Data
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {

        }

        public DbSet<Note> Notes { get; set; }
    }
}
 