using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookWorld.Migrations
{
    /// <inheritdoc />
    public partial class AddHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "History",
                columns: table => new
                {
                    ID_History = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Operation_Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ID_Shop_Source = table.Column<int>(type: "int", nullable: true),
                    ID_Shop = table.Column<int>(type: "int", nullable: false),
                    ID_Book = table.Column<int>(type: "int", nullable: false),
                    ID_Employee = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_History", x => x.ID_History);
                    table.CheckConstraint("CK_History_Operation_Type", "[Operation_Type] IN (N'Поступление', N'Перемещение', N'Списание')");
                    table.CheckConstraint("CK_History_Quantity", "[Quantity] > 0");
                    table.ForeignKey(
                        name: "FK_History_Book_ID_Book",
                        column: x => x.ID_Book,
                        principalTable: "Book",
                        principalColumn: "ID_Book",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_History_Employee_ID_Employee",
                        column: x => x.ID_Employee,
                        principalTable: "Employee",
                        principalColumn: "ID_Employee",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_History_Shop_ID_Shop",
                        column: x => x.ID_Shop,
                        principalTable: "Shop",
                        principalColumn: "ID_Shop");
                    table.ForeignKey(
                        name: "FK_History_Shop_ID_Shop_Source",
                        column: x => x.ID_Shop_Source,
                        principalTable: "Shop",
                        principalColumn: "ID_Shop");
                });

            migrationBuilder.CreateIndex(
                name: "IX_History_ID_Book",
                table: "History",
                column: "ID_Book");

            migrationBuilder.CreateIndex(
                name: "IX_History_ID_Employee",
                table: "History",
                column: "ID_Employee");

            migrationBuilder.CreateIndex(
                name: "IX_History_ID_Shop",
                table: "History",
                column: "ID_Shop");

            migrationBuilder.CreateIndex(
                name: "IX_History_ID_Shop_Source",
                table: "History",
                column: "ID_Shop_Source");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "History");
        }
    }
}
