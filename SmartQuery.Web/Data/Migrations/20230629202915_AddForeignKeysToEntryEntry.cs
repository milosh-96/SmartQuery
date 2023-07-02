using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartQuery.Web.Data.Migrations
{
    public partial class AddForeignKeysToEntryEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_EntryEntry_RelatedEntryId",
                table: "EntryEntry",
                column: "RelatedEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_EntryEntry_Entries_RelatedEntryId",
                table: "EntryEntry",
                column: "RelatedEntryId",
                principalTable: "Entries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntryEntry_Entries_RelatedEntryId",
                table: "EntryEntry");

            migrationBuilder.DropIndex(
                name: "IX_EntryEntry_RelatedEntryId",
                table: "EntryEntry");
        }
    }
}
