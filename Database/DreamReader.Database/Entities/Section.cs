using System.Collections.Generic;

namespace DreamReader.Database.Entities
{
    public class Section
    {
        public long Id { get; set; }
        public Book Book { get; set; }
        public ICollection<SectionRow> Rows { get; set; }

        public Section() { }

        public Section(Book book) : this()
        {
            Book = book;
        }
    }
}
