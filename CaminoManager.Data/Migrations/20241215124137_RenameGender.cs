using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaminoManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameGender : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Gender",
                table: "People",
                newName: "GenderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GenderId",
                table: "People",
                newName: "Gender");
        }
    }
}
