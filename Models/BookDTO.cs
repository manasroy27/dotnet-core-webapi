using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksApi.Models
{
    public class BookDTO
    {
        public int ISBN { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public string PublishedYear { get; set; }
        public decimal Price { get; set; }
        public Currency Currency { get; set; }
    }
}
