using System.Collections.Generic;
using System.IO;
using DreamReader.Business.DTOs;

namespace DreamReader.Business.Definitions
{
    public interface IBookManager
    {
        BookDto GetFullBook(long bookId);
        IEnumerable<BookDto> GetBooks(string userId);
        void UploadBook(Stream stream, string userId);
    }
}
