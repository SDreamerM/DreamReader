using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using DreamReader.Business.Definitions;
using DreamReader.Business.DTOs;
using DreamReader.Business.Hubs;
using DreamReader.Database;
using DreamReader.Database.Entities;
using FB2Library;

namespace DreamReader.Business
{
    public class BookManager : IBookManager
    {
        public void UploadBook(Stream stream, string userId)
        {
            var settings = new XmlReaderSettings {ValidationType = ValidationType.None};
            XDocument fb2Document;
            using (var reader = XmlReader.Create(stream, settings))
            {
                fb2Document = XDocument.Load(reader, LoadOptions.PreserveWhitespace);
                reader.Close();
            }
            var file = new FB2File();
            try
            {
                file.Load(fb2Document, false);
            }
            catch (Exception ex)
            {
                //Logger
                throw;
            }

            using (var dbContext = new DreamReaderContext())
            {
                var user = dbContext.Users.SingleOrDefault(x => x.Id == userId);
                if (user == null)
                    throw new Exception(string.Format("User#{0} not found in the system", userId));

                var book = new Book(user);
                book.Title = file.TitleInfo.BookTitle.ToString();
                book.Annotation = file.TitleInfo.Annotation.ToString();
                dbContext.Books.Add(book);
                dbContext.SaveChanges();

                var processedRows = 0;
                var processedSections = 0;
                var sections = file.MainBody.Sections.Count;
                var rows = file.MainBody.Sections.Sum(x => x.Content.Count);
                foreach (var sectionItem in file.MainBody.Sections)
                {
                    var section = new Section(book);
                    dbContext.Sections.Add(section);
                    dbContext.SaveChanges();

                    foreach (var row in sectionItem.Content)
                    {
                        var sectionRow = new SectionRow(section);
                        sectionRow.Content = row.ToString();

                        dbContext.SectionRows.Add(sectionRow);
                        dbContext.SaveChanges();

                        BookHub.BookSectionRowProcessed(string.Format("{0} of {1} book section rows processed", ++processedRows, rows), Math.Round((decimal) processedRows*100/rows));
                    }

                    BookHub.BookSectionProcessed(string.Format("{0} of {1} book sections processed", ++processedSections, sections), Math.Round((decimal) processedSections*100/sections));
                }
            }
        }

        public IEnumerable<BookDto> GetBooks(string userId)
        {
            using (var dbContext = new DreamReaderContext())
            {
                return dbContext.Books.Where(x => x.User.Id == userId).Select(x => new BookDto { Id = x.Id, Title = x.Title, Annotation = x.Annotation }).ToList();
            }
        }

        public BookDto GetBook(long bookId)
        {
            using (var dbContext = new DreamReaderContext())
            {
                var book = dbContext.Books.Include(x => x.Sections).Include(x => x.Sections.Select(s => s.Rows)).SingleOrDefault(x => x.Id == bookId);
                if (book == null)
                    throw new Exception(string.Format("Book#{0} not found in the system", bookId));
                var bookDto = new BookDto
                {
                    Id = book.Id,
                    Title = book.Title,
                    Annotation = book.Annotation,
                    Sections = book.Sections.Select(s => new SectionDto
                    {
                        Id = s.Id,
                        Rows = s.Rows.Select(r => new SectionRowDto
                        {
                            Id = r.Id,
                            Content = r.Content
                        }).ToList()
                    }).ToList()
                };
                return bookDto;
            }
        }

        public BookDto GetFullBook(long bookId)
        {
            using (var dbContext = new DreamReaderContext())
            {
                var book = dbContext.Books.Include(x => x.Sections).Include(x => x.Sections.Select(s => s.Rows)).SingleOrDefault(x => x.Id == bookId);
                if (book == null)
                    throw new Exception(string.Format("Book#{0} not found in the system", bookId));
                var bookDto = new BookDto
                {
                    Id = book.Id,
                    Title = book.Title,
                    Annotation = book.Annotation,
                    Sections = book.Sections.Select(s => new SectionDto
                    {
                        Id = s.Id,
                        Rows = s.Rows.Select(r => new SectionRowDto
                        {
                            Id = r.Id,
                            Content = r.Content
                        }).ToList()
                    }).ToList()
                };
                return bookDto;
            }
        }
    }
}
