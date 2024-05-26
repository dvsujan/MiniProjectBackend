using System.Reflection;

namespace LibraryManagemetApi.Models.DTO
{
    public class ReturnBookDTO
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set;  }
        public DateTime publishedDate { get; set; }
        public string Category { get; set;  }
        public int Quantity { get; set;  }
    }
}
