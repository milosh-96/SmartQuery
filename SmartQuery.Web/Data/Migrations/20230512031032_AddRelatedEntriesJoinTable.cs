using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartQuery.Web.Data.Migrations
{
    public partial class AddRelatedEntriesJoinTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntryEntry",
                columns: table => new
                {
                    EntriesId = table.Column<int>(type: "integer", nullable: false),
                    RelatedToId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryEntry", x => new { x.EntriesId, x.RelatedToId });
                    table.ForeignKey(
                        name: "FK_EntryEntry_Entries_EntriesId",
                        column: x => x.EntriesId,
                        principalTable: "Entries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntryEntry_Entries_RelatedToId",
                        column: x => x.RelatedToId,
                        principalTable: "Entries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);

                    table.UniqueConstraint("UQ_EntryEntry_Relationship", r => new { r.EntriesId, r.RelatedToId });
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntryEntry_RelatedToId",
                table: "EntryEntry",
                column: "RelatedToId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntryEntry");
        }
    }
}
