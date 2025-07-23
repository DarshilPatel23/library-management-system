using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Migrations
{
    /// <inheritdoc />
    public partial class AddReturnedBooksTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "IssuedDate",
                table: "ReturnedBooks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IssuedDate",
                table: "ReturnedBooks");
        }
    }
}
