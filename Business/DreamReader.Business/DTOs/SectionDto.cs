using System.Collections.Generic;

namespace DreamReader.Business.DTOs
{
    public class SectionDto
    {
        public long Id { get; set; }
        public List<SectionRowDto> Rows { get; set; } 
    }
}
