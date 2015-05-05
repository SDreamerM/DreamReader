using System.Collections.Generic;

namespace DreamReader.Business.DTOs
{
    public class BookDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Annotation { get; set; }

        public List<SectionDto> Sections { get; set; } 
    }
}
