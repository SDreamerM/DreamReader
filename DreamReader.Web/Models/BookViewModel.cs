using System.Collections.Generic;

namespace DreamReader.Web.Models
{
    public class BookViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Annotation { get; set; }
        public List<BookSectionViewModel> Sections { get; set; } 
    }

    public class BookSectionViewModel
    {
        public long Id { get; set; }
        public List<BookSectionRowViewModel> Rows { get; set; } 
    }

    public class BookSectionRowViewModel
    {
        public long Id { get; set; }
        public string Content { get; set; }
    }
}