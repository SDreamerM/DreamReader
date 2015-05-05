namespace DreamReader.Database.Entities
{
    public class SectionRow
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public Section Section { get; set; }

        public SectionRow() { }

        public SectionRow(Section section) : this()
        {
            Section = section;
        }
    }
}
