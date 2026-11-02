using BookWorldApp.Services;
using BookWorldApp.Data;
using BookWorldApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BookWorld.Data;

public sealed class DbInitializer(IDbContextFactory<BookStoreContext> factory)
{
    public async Task InitializeAsync()
    {
        await using var db = await factory.CreateDbContextAsync();
        await db.Database.MigrateAsync();
        if (await db.Shops.AnyAsync()) return;

        var center = new Shop { Title = "Центральный", Street = "Большая Московская", City = "Владимир", House = "10" };
        var north = new Shop { Title = "Северный", Street = "проспект Ленина", City = "Владимир", House = "45" };
        var south = new Shop { Title = "Южный", Street = "Горького", City = "Владимир", House = "77" };

        var eksmo = new PublishingHouse { Title = "Эксмо" };
        var ast = new PublishingHouse { Title = "АСТ" };
        var azbuka = new PublishingHouse { Title = "Азбука" };

        var classicSeries = new Series { Title = "Азбука-классика" };
        var foreignSeries = new Series { Title = "Зарубежная классика" };

        var classic = new Genre { Title = "Классика" };
        var novel = new Genre { Title = "Роман" };
        var fantasy = new Genre { Title = "Фантастика" };
        var dystopia = new Genre { Title = "Антиутопия" };
        var satire = new Genre { Title = "Сатира" };

        var tolstoy = new Author { Surname = "Толстой", Name = "Лев", Patronymic = "Николаевич" };
        var dostoevsky = new Author { Surname = "Достоевский", Name = "Фёдор", Patronymic = "Михайлович" };
        var bulgakov = new Author { Surname = "Булгаков", Name = "Михаил", Patronymic = "Афанасьевич" };
        var ilf = new Author { Surname = "Ильф", Name = "Илья", Patronymic = "Арнольдович" };
        var petrov = new Author { Surname = "Петров", Name = "Евгений", Patronymic = "Петрович" };
        var orwell = new Author { Surname = "Оруэлл", Name = "Джордж" };

        var golyshev = new Translator { Surname = "Голышев", Name = "Виктор", Patronymic = "Петрович" };

        db.Books.AddRange(
            NewBook("Война и мир", "978-5-00-000001-1", 890m, 12, azbuka, classicSeries,
                [tolstoy], [classic, novel], [], (center, 10), (south, 3)),
            NewBook("Преступление и наказание", "978-5-00-000002-2", 450.50m, 16, eksmo, null,
                [dostoevsky], [classic, novel], [], (center, 0), (north, 4)),
            NewBook("Мастер и Маргарита", "978-5-00-000003-3", 520m, 16, ast, null,
                [bulgakov], [classic, novel, fantasy], [], (center, 5), (north, 0), (south, 7)),
            NewBook("Двенадцать стульев", "978-5-00-000004-4", 399.90m, 16, ast, null,
                [ilf, petrov], [satire], [], (center, 2), (north, 6)),
            NewBook("1984", "978-5-00-000005-5", 610m, 16, ast, foreignSeries,
                [orwell], [fantasy, dystopia], [golyshev], (center, 8), (south, 1)),
            NewBook("Идиот", "978-5-00-000006-6", 475m, 16, eksmo, null,
                [dostoevsky], [classic, novel], []));

        db.Employees.AddRange(
            new Employee { Login = "admin", Password = "Admin#2026", Role = Roles.Admin },
            new Employee { Login = "cashier1", Password = "Cashier#2026", Role = Roles.Cashier, Shop = center },
            new Employee { Login = "cashier2", Password = "Cashier#2026", Role = Roles.Cashier, Shop = north });

        await db.SaveChangesAsync();
    }

    private static Book NewBook(
        string title, string isbn, decimal price, int ageLimit, PublishingHouse house, Series? series,
        Author[] authors, Genre[] genres, Translator[] translators, params (Shop Shop, int Copies)[] stock) => new()
        {
            Title = title,
            Isbn = isbn,
            Price = price,
            AgeLimit = ageLimit,
            PublishingHouse = house,
            Series = series,
            BookAuthors = authors.Select(a => new BookAuthor { Author = a }).ToList(),
            BookGenres = genres.Select(g => new BookGenre { Genre = g }).ToList(),
            BookTranslators = translators.Select(t => new BookTranslator { Translator = t }).ToList(),
            Remains = stock.Select(s => new Remains { Shop = s.Shop, NumberOfCopies = s.Copies }).ToList()
        };
}
