using LibraryManagemetApi.Models;
using Microsoft.EntityFrameworkCore;
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
            modelBuilder.Entity<Role>().HasData(new Role { Id = 1, Name = "User" },new Role { Id = 2, Name = "Admin" });
            modelBuilder.Entity<Location>().HasData(new Location { Id = 1, Floor=1, Shelf=1 },new Location { Id = 2, Floor=1 , Shelf=2 });
            modelBuilder.Entity<Category>().HasData(new Category { Id = 1, Name="Science Fiction" },new Category { Id = 2, Name="Fantasy" });
            modelBuilder.Entity<Publisher>().HasData(new Publisher { Id = 1, Name="Penguin" },new Publisher { Id = 2, Name="HarperCollins" });
            modelBuilder.Entity<Author>().HasData(new Author { Id = 1, Name="J.K. Rowling", Language="English" },new Author { Id = 2, Name="George Orwell", Language="English" });
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
