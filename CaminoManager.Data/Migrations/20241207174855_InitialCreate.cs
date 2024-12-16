using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaminoManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Communities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Number = table.Column<string>(type: "text", nullable: false),
                    BornDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ParishId = table.Column<Guid>(type: "uuid", nullable: false),
                    BornBrothers = table.Column<int>(type: "integer", nullable: false),
                    ActualBrothers = table.Column<int>(type: "integer", nullable: false),
                    StepWayId = table.Column<Guid>(type: "uuid", nullable: false),
                    StepWayDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CatechistTeamId = table.Column<Guid>(type: "uuid", nullable: true),
                    OldCatechist = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Communities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PersonName = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Mobile = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PersonTypeId = table.Column<int>(type: "integer", nullable: false),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    SpouseId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                    table.ForeignKey(
                        name: "FK_People_People_SpouseId",
                        column: x => x.SpouseId,
                        principalTable: "People",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Brothers",
                columns: table => new
                {
                    PersonId = table.Column<Guid>(type: "uuid", nullable: false),
                    CommunityId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brothers", x => new { x.PersonId, x.CommunityId });
                    table.ForeignKey(
                        name: "FK_Brothers_Communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Communities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Brothers_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Brothers_CommunityId",
                table: "Brothers",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_People_SpouseId",
                table: "People",
                column: "SpouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Brothers");

            migrationBuilder.DropTable(
                name: "Communities");

            migrationBuilder.DropTable(
                name: "People");
        }
    }
}
