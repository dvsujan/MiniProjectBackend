using System.Reflection;

namespace LibraryManagemetApi.Models.DTO
{
    public class ReturnBookDTO
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public DateTime publishedDate { get; set; }
        public string Category { get; set;  }
        public int Quantity { get; set;  }
        public string ISBN { get; set; }
        public float rating { get; set; } = 0;
        public int noOfRatings { get; set; } = 0;
        public int floorNo { get; set; }
        public int shelfNo { get; set; }
    }
}
