using System.Data.Entity;
using DreamReader.Database.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DreamReader.Database
{
    public class DreamReaderContext : IdentityDbContext<DreamReaderUser>
    {
        public DreamReaderContext() : base("Main") { }

        public static DreamReaderContext Create()
        {
            return new DreamReaderContext();
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<SectionRow> SectionRows { get; set; }
    }
}
