using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SmartQuery.Web.Data.Migrations
{
    public partial class AddAdjectives : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Adjectives",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adjectives", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdjectiveEntry",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AdjectiveId = table.Column<int>(type: "integer", nullable: false),
                    EntryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdjectiveEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdjectiveEntry_Adjectives_AdjectiveId",
                        column: x => x.AdjectiveId,
                        principalTable: "Adjectives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdjectiveEntry_Entries_EntryId",
                        column: x => x.EntryId,
                        principalTable: "Entries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdjectiveEntry_AdjectiveId_EntryId",
                table: "AdjectiveEntry",
                columns: new[] { "AdjectiveId", "EntryId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdjectiveEntry_EntryId",
                table: "AdjectiveEntry",
                column: "EntryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdjectiveEntry");

            migrationBuilder.DropTable(
                name: "Adjectives");
        }
    }
}
