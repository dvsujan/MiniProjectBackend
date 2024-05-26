﻿using System.ComponentModel.DataAnnotations;

namespace LibraryManagemetApi.Models
{
    public class Stock
    {
        [Key]
        public int Id { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }

        public Book Book { get; set; }
    }
}
