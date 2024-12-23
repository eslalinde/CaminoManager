using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace CaminoManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class Partial1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "People",
                type: "tsvector",
                nullable: true,
                oldClrType: typeof(NpgsqlTsVector),
                oldType: "tsvector",
                oldNullable: true)
                .Annotation("Npgsql:TsVectorConfig", "spanish")
                .Annotation("Npgsql:TsVectorProperties", new[] { "PersonName" })
                .OldAnnotation("Npgsql:TsVectorConfig", "english")
                .OldAnnotation("Npgsql:TsVectorProperties", new[] { "PersonName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "People",
                type: "tsvector",
                nullable: true,
                oldClrType: typeof(NpgsqlTsVector),
                oldType: "tsvector",
                oldNullable: true)
                .Annotation("Npgsql:TsVectorConfig", "english")
                .Annotation("Npgsql:TsVectorProperties", new[] { "PersonName" })
                .OldAnnotation("Npgsql:TsVectorConfig", "spanish")
                .OldAnnotation("Npgsql:TsVectorProperties", new[] { "PersonName" });
        }
    }
}
