using System;
using System.Collections.Generic;

namespace DreamReader.Database.Entities
{
    public class Book
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Annotation { get; set; }
        public DreamReaderUser User { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? LastUpdatedOn { get; set; }

        public virtual ICollection<Section> Sections { get; set; }

        public Book()
        {
            CreatedOn = DateTime.UtcNow;
        }

        public Book(DreamReaderUser user) : this()
        {
            User = user;
        }
    }
}
