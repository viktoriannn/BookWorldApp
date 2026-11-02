using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookWorld.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Author",
                columns: table => new
                {
                    ID_Author = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Surname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Patronymic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Author", x => x.ID_Author);
                });

            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    ID_Genre = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre", x => x.ID_Genre);
                });

            migrationBuilder.CreateTable(
                name: "Publishing_house",
                columns: table => new
                {
                    ID_Publishing_house = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publishing_house", x => x.ID_Publishing_house);
                });

            migrationBuilder.CreateTable(
                name: "Series",
                columns: table => new
                {
                    ID_Series = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Series", x => x.ID_Series);
                });

            migrationBuilder.CreateTable(
                name: "Shop",
                columns: table => new
                {
                    ID_Shop = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    House = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Flat = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shop", x => x.ID_Shop);
                });

            migrationBuilder.CreateTable(
                name: "Translator",
                columns: table => new
                {
                    ID_Translator = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Surname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Patronymic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translator", x => x.ID_Translator);
                });

            migrationBuilder.CreateTable(
                name: "Book",
                columns: table => new
                {
                    ID_Book = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(17)", maxLength: 17, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Age_limit = table.Column<int>(type: "int", nullable: false),
                    ID_Publishing_house = table.Column<int>(type: "int", nullable: false),
                    ID_Series = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.ID_Book);
                    table.CheckConstraint("CK_Book_Price", "[Price] > 0");
                    table.ForeignKey(
                        name: "FK_Book_Publishing_house_ID_Publishing_house",
                        column: x => x.ID_Publishing_house,
                        principalTable: "Publishing_house",
                        principalColumn: "ID_Publishing_house",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Book_Series_ID_Series",
                        column: x => x.ID_Series,
                        principalTable: "Series",
                        principalColumn: "ID_Series");
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    ID_Employee = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ID_Shop = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.ID_Employee);
                    table.CheckConstraint("CK_Employee_Cashier_Shop", "[Role] <> 'Cashier' OR [ID_Shop] IS NOT NULL");
                    table.CheckConstraint("CK_Employee_Role", "[Role] IN ('Cashier', 'Admin')");
                    table.ForeignKey(
                        name: "FK_Employee_Shop_ID_Shop",
                        column: x => x.ID_Shop,
                        principalTable: "Shop",
                        principalColumn: "ID_Shop");
                });

            migrationBuilder.CreateTable(
                name: "Book_Author",
                columns: table => new
                {
                    ID_Book_Author = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Book = table.Column<int>(type: "int", nullable: false),
                    ID_Author = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book_Author", x => x.ID_Book_Author);
                    table.ForeignKey(
                        name: "FK_Book_Author_Author_ID_Author",
                        column: x => x.ID_Author,
                        principalTable: "Author",
                        principalColumn: "ID_Author",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Book_Author_Book_ID_Book",
                        column: x => x.ID_Book,
                        principalTable: "Book",
                        principalColumn: "ID_Book",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Book_Genre",
                columns: table => new
                {
                    ID_Book_Genre = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Book = table.Column<int>(type: "int", nullable: false),
                    ID_Genre = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book_Genre", x => x.ID_Book_Genre);
                    table.ForeignKey(
                        name: "FK_Book_Genre_Book_ID_Book",
                        column: x => x.ID_Book,
                        principalTable: "Book",
                        principalColumn: "ID_Book",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Book_Genre_Genre_ID_Genre",
                        column: x => x.ID_Genre,
                        principalTable: "Genre",
                        principalColumn: "ID_Genre",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Book_Translator",
                columns: table => new
                {
                    ID_Book_Translator = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Book = table.Column<int>(type: "int", nullable: false),
                    ID_Translator = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book_Translator", x => x.ID_Book_Translator);
                    table.ForeignKey(
                        name: "FK_Book_Translator_Book_ID_Book",
                        column: x => x.ID_Book,
                        principalTable: "Book",
                        principalColumn: "ID_Book",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Book_Translator_Translator_ID_Translator",
                        column: x => x.ID_Translator,
                        principalTable: "Translator",
                        principalColumn: "ID_Translator",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Remains",
                columns: table => new
                {
                    ID_Remains = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numberofcopies = table.Column<int>(name: "Number of copies", type: "int", nullable: false),
                    ID_Book = table.Column<int>(type: "int", nullable: false),
                    ID_Shop = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Remains", x => x.ID_Remains);
                    table.CheckConstraint("CK_Remains_Number_of_copies", "[Number of copies] >= 0");
                    table.ForeignKey(
                        name: "FK_Remains_Book_ID_Book",
                        column: x => x.ID_Book,
                        principalTable: "Book",
                        principalColumn: "ID_Book",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Remains_Shop_ID_Shop",
                        column: x => x.ID_Shop,
                        principalTable: "Shop",
                        principalColumn: "ID_Shop",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cheque",
                columns: table => new
                {
                    ID_Cheque = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Total_sum = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    ID_Employee = table.Column<int>(type: "int", nullable: false),
                    ID_Shop = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cheque", x => x.ID_Cheque);
                    table.ForeignKey(
                        name: "FK_Cheque_Employee_ID_Employee",
                        column: x => x.ID_Employee,
                        principalTable: "Employee",
                        principalColumn: "ID_Employee",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cheque_Shop_ID_Shop",
                        column: x => x.ID_Shop,
                        principalTable: "Shop",
                        principalColumn: "ID_Shop",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    ID_Order = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Sum = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    ID_Shop = table.Column<int>(type: "int", nullable: false),
                    ID_Employee = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.ID_Order);
                    table.ForeignKey(
                        name: "FK_Order_Employee_ID_Employee",
                        column: x => x.ID_Employee,
                        principalTable: "Employee",
                        principalColumn: "ID_Employee",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Shop_ID_Shop",
                        column: x => x.ID_Shop,
                        principalTable: "Shop",
                        principalColumn: "ID_Shop",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cheque_Book",
                columns: table => new
                {
                    ID_Cheque_Book = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Cheque = table.Column<int>(type: "int", nullable: false),
                    ID_Book = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cheque_Book", x => x.ID_Cheque_Book);
                    table.CheckConstraint("CK_Cheque_Book_Count", "[Count] > 0");
                    table.ForeignKey(
                        name: "FK_Cheque_Book_Book_ID_Book",
                        column: x => x.ID_Book,
                        principalTable: "Book",
                        principalColumn: "ID_Book",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cheque_Book_Cheque_ID_Cheque",
                        column: x => x.ID_Cheque,
                        principalTable: "Cheque",
                        principalColumn: "ID_Cheque",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order_Book",
                columns: table => new
                {
                    ID_Order_Book = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Order = table.Column<int>(type: "int", nullable: false),
                    ID_Book = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Book", x => x.ID_Order_Book);
                    table.CheckConstraint("CK_Order_Book_Count", "[Count] > 0");
                    table.ForeignKey(
                        name: "FK_Order_Book_Book_ID_Book",
                        column: x => x.ID_Book,
                        principalTable: "Book",
                        principalColumn: "ID_Book",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Book_Order_ID_Order",
                        column: x => x.ID_Order,
                        principalTable: "Order",
                        principalColumn: "ID_Order",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Book_ID_Publishing_house",
                table: "Book",
                column: "ID_Publishing_house");

            migrationBuilder.CreateIndex(
                name: "IX_Book_ID_Series",
                table: "Book",
                column: "ID_Series");

            migrationBuilder.CreateIndex(
                name: "IX_Book_ISBN",
                table: "Book",
                column: "ISBN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Book_Author_ID_Author",
                table: "Book_Author",
                column: "ID_Author");

            migrationBuilder.CreateIndex(
                name: "IX_Book_Author_ID_Book_ID_Author",
                table: "Book_Author",
                columns: new[] { "ID_Book", "ID_Author" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Book_Genre_ID_Book_ID_Genre",
                table: "Book_Genre",
                columns: new[] { "ID_Book", "ID_Genre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Book_Genre_ID_Genre",
                table: "Book_Genre",
                column: "ID_Genre");

            migrationBuilder.CreateIndex(
                name: "IX_Book_Translator_ID_Book_ID_Translator",
                table: "Book_Translator",
                columns: new[] { "ID_Book", "ID_Translator" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Book_Translator_ID_Translator",
                table: "Book_Translator",
                column: "ID_Translator");

            migrationBuilder.CreateIndex(
                name: "IX_Cheque_ID_Employee",
                table: "Cheque",
                column: "ID_Employee");

            migrationBuilder.CreateIndex(
                name: "IX_Cheque_ID_Shop",
                table: "Cheque",
                column: "ID_Shop");

            migrationBuilder.CreateIndex(
                name: "IX_Cheque_Book_ID_Book",
                table: "Cheque_Book",
                column: "ID_Book");

            migrationBuilder.CreateIndex(
                name: "IX_Cheque_Book_ID_Cheque_ID_Book",
                table: "Cheque_Book",
                columns: new[] { "ID_Cheque", "ID_Book" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_ID_Shop",
                table: "Employee",
                column: "ID_Shop");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_Login",
                table: "Employee",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_ID_Employee",
                table: "Order",
                column: "ID_Employee");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ID_Shop",
                table: "Order",
                column: "ID_Shop");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Book_ID_Book",
                table: "Order_Book",
                column: "ID_Book");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Book_ID_Order_ID_Book",
                table: "Order_Book",
                columns: new[] { "ID_Order", "ID_Book" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Remains_ID_Book_ID_Shop",
                table: "Remains",
                columns: new[] { "ID_Book", "ID_Shop" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Remains_ID_Shop",
                table: "Remains",
                column: "ID_Shop");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Book_Author");

            migrationBuilder.DropTable(
                name: "Book_Genre");

            migrationBuilder.DropTable(
                name: "Book_Translator");

            migrationBuilder.DropTable(
                name: "Cheque_Book");

            migrationBuilder.DropTable(
                name: "Order_Book");

            migrationBuilder.DropTable(
                name: "Remains");

            migrationBuilder.DropTable(
                name: "Author");

            migrationBuilder.DropTable(
                name: "Genre");

            migrationBuilder.DropTable(
                name: "Translator");

            migrationBuilder.DropTable(
                name: "Cheque");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Book");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Publishing_house");

            migrationBuilder.DropTable(
                name: "Series");

            migrationBuilder.DropTable(
                name: "Shop");
        }
    }
}
