using LibraryManagemetApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibraryManagemetApi.Contexts
{

    public class LibraryManagementContext : DbContext
    {
      
        public LibraryManagementContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Borrowed> Borroweds { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Card> Cards { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(new Role { Id = 1, Name = "User" }, new Role { Id = 2, Name = "Admin" });
            modelBuilder.Entity<Location>().HasData(new Location { Id = 1, Floor = 1, Shelf = 1 }, new Location { Id = 2, Floor = 1, Shelf = 2 });
            modelBuilder.Entity<Category>().HasData(new Category { Id = 1, Name = "Science Fiction" }, new Category { Id = 2, Name = "Fantasy" });
            modelBuilder.Entity<Author>().HasData(new Author { Id = 1, Name = "J.K. Rowling", Language = "English" }, new Author { Id = 2, Name = "George Orwell", Language = "English" });
            modelBuilder.Entity<Publisher>().HasData(new Publisher { Id = 1, Name = "j&k publishers", Address = "5th Ave NY" });

            modelBuilder.Entity<Book>().HasData(new Book
            {
                Id = 1,
                Title = "Title",
                ISBN = "1234567890",
                PublishedDate = DateTime.Now,
                CategoryId = 1,
                LocationId = 1,
                AuthorId = 1,
                PublisherId = 1,
            },
            new Book
            {
                Id = 2,
                Title = "Title2",
                ISBN = "1234567891",
                PublishedDate = DateTime.Now,
                CategoryId = 1,
                LocationId = 1,
                AuthorId = 1,
                PublisherId = 1,
            },
            new Book
            {
                Id = 3,
                Title = "Title3",
                ISBN = "1234567892",
                PublishedDate = DateTime.Now,
                CategoryId = 1,
                LocationId = 1,
                AuthorId = 1,
                PublisherId = 1,
            }, 
            new Book
            {
                Id =4, 
                Title = "jamesbond777",
                ISBN = "1234567893",
                PublishedDate = DateTime.Now,
                CategoryId = 1,
                LocationId = 1,
                AuthorId = 1,
                PublisherId = 1,
            });

            modelBuilder.Entity<Stock>().HasData(new Stock
            {
                Id = 1,
                BookId = 1,
                Quantity = 10
            },
            new Stock
            {
                Id = 2,
                BookId = 2,
                Quantity = 10
            },
            new Stock
            {
                Id = 3,
                BookId = 3,
                Quantity = 10
            }, 
            new Stock
            {
                Id = 4,
                BookId = 4,
                Quantity = 0
            });

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 101,
                Username = "ramu",
                Email = "ramu@gmail.com",
                HashKey = Encoding.ASCII.GetBytes("0x498A2FEABB2EB2ABD1F1AFEC39500BA3367C72E6E2FD20A2BCC5679E9E4B8FABE22AAC8E78D22D3FED9F3D10E45F0231E4292A5836656B5B3B21DCBACB34E598BD476B30C763A7D8C3B70C1F536AA72F09AE9F9C9BDDAC467AF729906BCE5257E43CC674E391B9B68C050BF883C00C69745FF0EC959771035904E06E025F23CD"),
                Password = Encoding.ASCII.GetBytes("0x3D04A5EA151414FD9F495FC081F1E292A897592A7C620675032AD7557A2060F82899A6F04265F3488610B4FED82E26803AB9FA3AFA89FDA004CCD82C50873C3A"),
                Active = true,
                RoleId = 1
            }, 
            new User
            {
                Id = 102,
                Username = "samu",
                Email = "samu@gmail.com",
                HashKey = Encoding.ASCII.GetBytes("0x498A2FEABB2EB2ABD1F1AFEC39500BA3367C72E6E2FD20A2BCC5679E9E4B8FABE22AAC8E78D22D3FED9F3D10E45F0231E4292A5836656B5B3B21DCBACB34E598BD476B30C763A7D8C3B70C1F536AA72F09AE9F9C9BDDAC467AF729906BCE5257E43CC674E391B9B68C050BF883C00C69745FF0EC959771035904E06E025F23CD"),
                Password = Encoding.ASCII.GetBytes("0x3D04A5EA151414FD9F495FC081F1E292A897592A7C620675032AD7557A2060F82899A6F04265F3488610B4FED82E26803AB9FA3AFA89FDA004CCD82C50873C3A"),
                Active = true,
                RoleId = 1
            });

            //new User
            //{
            //    Id = 2,
            //    Username = "bimu",
            //    Email = "bimu@gmail.com",
            //    HashKey = Encoding.ASCII.GetBytes("0x498A2FEABB2EB2ABD1F1AFEC39500BA3367C72E6E2FD20A2BCC5679E9E4B8FABE22AAC8E78D22D3FED9F3D10E45F0231E4292A5836656B5B3B21DCBACB34E598BD476B30C763A7D8C3B70C1F536AA72F09AE9F9C9BDDAC467AF729906BCE5257E43CC674E391B9B68C050BF883C00C69745FF0EC959771035904E06E025F23CF"),
            //    Password = Encoding.ASCII.GetBytes("0x3D04A5EA151414FD9F495FC081F1E292A897592A7C620675032AD7557A2060F82899A6F04265F3488610B4FED82E26803AB9FA3AFA89FDA004CCD82C50873C3B"),
            //    Active = true,
            //    RoleId = 2,
            //},
            //new User
            //{
            //    Id = 3,
            //    Username = "bimbum",
            //    Email = "bimbum1971@gmail.com",
            //    HashKey = Encoding.ASCII.GetBytes("0x498A2FEABB2EB2ABD1F1AFEC39500BA3367C72E6E2FD20A2BCC5679E9E4B8FABE22AAC8E78D22D3FED9F3D10E45F0231E4292A5836656B5B3B21DCBACB34E598BD476B30C763A7D8C3B70C1F536AA72F09AE9F9C9BDDAC467AF729906BCE5257E43CC674E391B9B68C050BF883C00C69745FF0EC959771035904E06E025F23CC"),
            //    Password = Encoding.ASCII.GetBytes("0x3D04A5EA151414FD9F495FC081F1E292A897592A7C620675032AD7557A2060F82899A6F04265F3488610B4FED82E26803AB9FA3AFA89FDA004CCD82C50873C3B"),
            //    Active = true,
            //    RoleId = 2,
            //}
            //);

            modelBuilder.Entity<Card>().HasData(new Card
            {
                Id = 1,
                CardNumber = "123456789",
                ExpDate = DateTime.Now,
                CVV = 123,
                UserId = 2
            },new Card
            {
                Id = 2,
                CardNumber = "123456789",
                ExpDate = DateTime.Now,
                CVV = 123,
                UserId = 101
            });
            modelBuilder.Entity<Borrowed>().HasData(new Borrowed
            {
                Id = 1,
                UserId = 2,
                BookId = 1,
                BorrowedDate = DateTime.Now,
                DueDate = new DateTime(2024, 5, 1),
                ReturnDate = null,
                PaymentId = 1
            },
            new Borrowed
            {
                Id = 2,
                UserId = 101,
                BookId = 1,
                BorrowedDate = DateTime.Now,
                DueDate = new DateTime(2024, 5, 1),
                ReturnDate = null,
                PaymentId = 1
            },
            new Borrowed
            {
                Id=3, 
                UserId=101 , 
                BookId=2 , 
                BorrowedDate=DateTime.Now,
                DueDate = new DateTime(2024, 5, 1),
                ReturnDate = null,
                PaymentId = 1
            }
            );

            modelBuilder.Entity<Payment>().HasData(new Payment
            {
                Id = 1,
                UserId = 101,
                CardId = 1,
                BorrowedId = 3,
                Amount = 100,
                PaymentDate = DateTime.Now
            });
            modelBuilder.Entity<Reservation>().HasData(new Reservation
            {
                Id = 1,
                UserId = 101,
                BookId = 4,
                ReservationDate = DateTime.Now
            },new Reservation
            {
                Id=2 , 
                UserId=102 , 
                BookId=3 ,
                ReservationDate = DateTime.Now
            });

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId);
            
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany()
                .HasForeignKey(b => b.AuthorId);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Publisher)
                .WithMany()
                .HasForeignKey(b => b.PublisherId);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany()
                .HasForeignKey(b => b.CategoryId);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Location)
                .WithMany()
                .HasForeignKey(b => b.LocationId);
           
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Book)
                .WithMany()
                .HasForeignKey(r => r.BookId);
     
            modelBuilder.Entity<Borrowed>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<Borrowed>()
                .HasOne(b => b.Book)
                .WithMany()
                .HasForeignKey(b => b.BookId);

            modelBuilder.Entity<Borrowed>()
                .HasOne(b => b.Payment)
                .WithMany()
                .HasForeignKey(b => b.PaymentId);

            
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Book)
                .WithMany()
                .HasForeignKey(r => r.BookId);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Card)
                .WithMany()
                .HasForeignKey(p => p.CardId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Borrowed)
                .WithMany()
                .HasForeignKey(p => p.BorrowedId)
                .OnDelete(DeleteBehavior.Restrict); 
            
            modelBuilder.Entity<Card>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict); 
           
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            modelBuilder.Entity<Book>()
                .HasIndex(b => b.ISBN)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
