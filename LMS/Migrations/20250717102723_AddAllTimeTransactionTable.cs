using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Migrations
{
    /// <inheritdoc />
    public partial class AddAllTimeTransactionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReturnedDate",
                table: "IssuedBooks");

            migrationBuilder.CreateTable(
                name: "AllTimeTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssuedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllTimeTransactions", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllTimeTransactions");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnedDate",
                table: "IssuedBooks",
                type: "datetime2",
                nullable: true);
        }
    }
}
