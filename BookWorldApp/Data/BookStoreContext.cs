using BookWorldApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BookWorldApp.Data;

public class BookStoreContext : DbContext
{
    public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options)
    {
    }

    public DbSet<PublishingHouse> PublishingHouses => Set<PublishingHouse>();
    public DbSet<Series> SeriesSet => Set<Series>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Translator> Translators => Set<Translator>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<BookAuthor> BookAuthors => Set<BookAuthor>();
    public DbSet<BookGenre> BookGenres => Set<BookGenre>();
    public DbSet<BookTranslator> BookTranslators => Set<BookTranslator>();
    public DbSet<Shop> Shops => Set<Shop>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Remains> RemainsSet => Set<Remains>();
    public DbSet<Cheque> Cheques => Set<Cheque>();
    public DbSet<ChequeBook> ChequeBooks => Set<ChequeBook>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderBook> OrderBooks => Set<OrderBook>();

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        builder.Properties<decimal>().HavePrecision(10, 2);
    }

    protected override void OnModelCreating(ModelBuilder model)
    {
        model.Entity<PublishingHouse>(e =>
        {
            e.ToTable("Publishing_house");
            e.HasKey(x => x.IdPublishingHouse);
            e.Property(x => x.IdPublishingHouse).HasColumnName("ID_Publishing_house");
            e.Property(x => x.Title).HasMaxLength(200);
        });

        model.Entity<Series>(e =>
        {
            e.ToTable("Series");
            e.HasKey(x => x.IdSeries);
            e.Property(x => x.IdSeries).HasColumnName("ID_Series");
            e.Property(x => x.Title).HasMaxLength(200);
        });

        model.Entity<Author>(e =>
        {
            e.ToTable("Author");
            e.HasKey(x => x.IdAuthor);
            e.Property(x => x.IdAuthor).HasColumnName("ID_Author");
            e.Property(x => x.Surname).HasMaxLength(100);
            e.Property(x => x.Name).HasMaxLength(100);
            e.Property(x => x.Patronymic).HasMaxLength(100);
        });

        model.Entity<Genre>(e =>
        {
            e.ToTable("Genre");
            e.HasKey(x => x.IdGenre);
            e.Property(x => x.IdGenre).HasColumnName("ID_Genre");
            e.Property(x => x.Title).HasMaxLength(100);
        });

        model.Entity<Translator>(e =>
        {
            e.ToTable("Translator");
            e.HasKey(x => x.IdTranslator);
            e.Property(x => x.IdTranslator).HasColumnName("ID_Translator");
            e.Property(x => x.Surname).HasMaxLength(100);
            e.Property(x => x.Name).HasMaxLength(100);
            e.Property(x => x.Patronymic).HasMaxLength(100);
        });

        model.Entity<Book>(e =>
        {
            e.ToTable("Book", t => t.HasCheckConstraint("CK_Book_Price", "[Price] > 0"));
            e.HasKey(x => x.IdBook);
            e.Property(x => x.IdBook).HasColumnName("ID_Book");
            e.Property(x => x.Title).HasMaxLength(200);
            e.Property(x => x.Isbn).HasColumnName("ISBN").HasMaxLength(17);
            e.Property(x => x.AgeLimit).HasColumnName("Age_limit");
            e.Property(x => x.IdPublishingHouse).HasColumnName("ID_Publishing_house");
            e.Property(x => x.IdSeries).HasColumnName("ID_Series");
            e.HasIndex(x => x.Isbn).IsUnique();
            e.HasOne(x => x.PublishingHouse).WithMany(x => x.Books).HasForeignKey(x => x.IdPublishingHouse);
            e.HasOne(x => x.Series).WithMany(x => x.Books).HasForeignKey(x => x.IdSeries);
        });

        model.Entity<BookAuthor>(e =>
        {
            e.ToTable("Book_Author");
            e.HasKey(x => x.IdBookAuthor);
            e.Property(x => x.IdBookAuthor).HasColumnName("ID_Book_Author");
            e.Property(x => x.IdBook).HasColumnName("ID_Book");
            e.Property(x => x.IdAuthor).HasColumnName("ID_Author");
            e.HasIndex(x => new { x.IdBook, x.IdAuthor }).IsUnique();
            e.HasOne(x => x.Book).WithMany(x => x.BookAuthors).HasForeignKey(x => x.IdBook);
            e.HasOne(x => x.Author).WithMany(x => x.BookAuthors).HasForeignKey(x => x.IdAuthor);
        });

        model.Entity<BookGenre>(e =>
        {
            e.ToTable("Book_Genre");
            e.HasKey(x => x.IdBookGenre);
            e.Property(x => x.IdBookGenre).HasColumnName("ID_Book_Genre");
            e.Property(x => x.IdBook).HasColumnName("ID_Book");
            e.Property(x => x.IdGenre).HasColumnName("ID_Genre");
            e.HasIndex(x => new { x.IdBook, x.IdGenre }).IsUnique();
            e.HasOne(x => x.Book).WithMany(x => x.BookGenres).HasForeignKey(x => x.IdBook);
            e.HasOne(x => x.Genre).WithMany(x => x.BookGenres).HasForeignKey(x => x.IdGenre);
        });

        model.Entity<BookTranslator>(e =>
        {
            e.ToTable("Book_Translator");
            e.HasKey(x => x.IdBookTranslator);
            e.Property(x => x.IdBookTranslator).HasColumnName("ID_Book_Translator");
            e.Property(x => x.IdBook).HasColumnName("ID_Book");
            e.Property(x => x.IdTranslator).HasColumnName("ID_Translator");
            e.HasIndex(x => new { x.IdBook, x.IdTranslator }).IsUnique();
            e.HasOne(x => x.Book).WithMany(x => x.BookTranslators).HasForeignKey(x => x.IdBook);
            e.HasOne(x => x.Translator).WithMany(x => x.BookTranslators).HasForeignKey(x => x.IdTranslator);
        });

        model.Entity<Shop>(e =>
        {
            e.ToTable("Shop");
            e.HasKey(x => x.IdShop);
            e.Property(x => x.IdShop).HasColumnName("ID_Shop");
            e.Property(x => x.Title).HasMaxLength(200);
            e.Property(x => x.Street).HasMaxLength(200);
            e.Property(x => x.City).HasMaxLength(100);
            e.Property(x => x.House).HasMaxLength(10);
            e.Property(x => x.Flat).HasMaxLength(10);
        });

        model.Entity<Employee>(e =>
        {
            e.ToTable("Employee", t =>
            {
                t.HasCheckConstraint("CK_Employee_Role", "[Role] IN ('Cashier', 'Admin')");
                t.HasCheckConstraint("CK_Employee_Cashier_Shop", "[Role] <> 'Cashier' OR [ID_Shop] IS NOT NULL");
            });
            e.HasKey(x => x.IdEmployee);
            e.Property(x => x.IdEmployee).HasColumnName("ID_Employee");
            e.Property(x => x.IdShop).HasColumnName("ID_Shop");
            e.Property(x => x.Login).HasMaxLength(50);
            e.Property(x => x.Password).HasMaxLength(255);
            e.Property(x => x.Role).HasMaxLength(30);
            e.HasIndex(x => x.Login).IsUnique();
            e.HasOne(x => x.Shop).WithMany(x => x.Employees).HasForeignKey(x => x.IdShop);
        });

        model.Entity<Remains>(e =>
        {
            e.ToTable("Remains", t => t.HasCheckConstraint("CK_Remains_Number_of_copies", "[Number of copies] >= 0"));
            e.HasKey(x => x.IdRemains);
            e.Property(x => x.IdRemains).HasColumnName("ID_Remains");
            e.Property(x => x.NumberOfCopies).HasColumnName("Number of copies");
            e.Property(x => x.IdBook).HasColumnName("ID_Book");
            e.Property(x => x.IdShop).HasColumnName("ID_Shop");
            e.HasIndex(x => new { x.IdBook, x.IdShop }).IsUnique();
            e.HasOne(x => x.Book).WithMany(x => x.Remains).HasForeignKey(x => x.IdBook);
            e.HasOne(x => x.Shop).WithMany(x => x.Remains).HasForeignKey(x => x.IdShop);
        });

        model.Entity<Cheque>(e =>
        {
            e.ToTable("Cheque");
            e.HasKey(x => x.IdCheque);
            e.Property(x => x.IdCheque).HasColumnName("ID_Cheque");
            e.Property(x => x.TotalSum).HasColumnName("Total_sum");
            e.Property(x => x.IdEmployee).HasColumnName("ID_Employee");
            e.Property(x => x.IdShop).HasColumnName("ID_Shop");
            e.HasOne(x => x.Employee).WithMany().HasForeignKey(x => x.IdEmployee);
            e.HasOne(x => x.Shop).WithMany().HasForeignKey(x => x.IdShop);
        });

        model.Entity<ChequeBook>(e =>
        {
            e.ToTable("Cheque_Book", t => t.HasCheckConstraint("CK_Cheque_Book_Count", "[Count] > 0"));
            e.HasKey(x => x.IdChequeBook);
            e.Property(x => x.IdChequeBook).HasColumnName("ID_Cheque_Book");
            e.Property(x => x.IdCheque).HasColumnName("ID_Cheque");
            e.Property(x => x.IdBook).HasColumnName("ID_Book");
            e.HasIndex(x => new { x.IdCheque, x.IdBook }).IsUnique();
            e.HasOne(x => x.Cheque).WithMany(x => x.ChequeBooks).HasForeignKey(x => x.IdCheque);
            e.HasOne(x => x.Book).WithMany().HasForeignKey(x => x.IdBook);
        });

        model.Entity<Order>(e =>
        {
            e.ToTable("Order");
            e.HasKey(x => x.IdOrder);
            e.Property(x => x.IdOrder).HasColumnName("ID_Order");
            e.Property(x => x.IdShop).HasColumnName("ID_Shop");
            e.Property(x => x.IdEmployee).HasColumnName("ID_Employee");
            e.HasOne(x => x.Shop).WithMany().HasForeignKey(x => x.IdShop);
            e.HasOne(x => x.Employee).WithMany().HasForeignKey(x => x.IdEmployee);
        });

        model.Entity<OrderBook>(e =>
        {
            e.ToTable("Order_Book", t => t.HasCheckConstraint("CK_Order_Book_Count", "[Count] > 0"));
            e.HasKey(x => x.IdOrderBook);
            e.Property(x => x.IdOrderBook).HasColumnName("ID_Order_Book");
            e.Property(x => x.IdOrder).HasColumnName("ID_Order");
            e.Property(x => x.IdBook).HasColumnName("ID_Book");
            e.HasIndex(x => new { x.IdOrder, x.IdBook }).IsUnique();
            e.HasOne(x => x.Order).WithMany(x => x.OrderBooks).HasForeignKey(x => x.IdOrder);
            e.HasOne(x => x.Book).WithMany().HasForeignKey(x => x.IdBook);
        });
    }
}
