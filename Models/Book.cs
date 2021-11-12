using System.ComponentModel.DataAnnotations;

namespace BooksApi.Models
{
    public class Book
    {
        [Key] 
        public int ISBN { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public string Author { get; set; }
        public string PublishedYear { get; set; }
        public decimal Price { get; set; }
        public Currency Currency { get; set; }
    }

    public enum Currency
    {
        USD,
        GBP,
        INR
    }
}
