using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartQuery.Web.Data.Migrations
{
    public partial class AddUniqueForEntryEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EntryEntry_EntryId",
                table: "EntryEntry");

            migrationBuilder.CreateIndex(
                name: "IX_EntryEntry_EntryId_RelatedEntryId",
                table: "EntryEntry",
                columns: new[] { "EntryId", "RelatedEntryId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EntryEntry_EntryId_RelatedEntryId",
                table: "EntryEntry");

            migrationBuilder.CreateIndex(
                name: "IX_EntryEntry_EntryId",
                table: "EntryEntry",
                column: "EntryId");
        }
    }
}
