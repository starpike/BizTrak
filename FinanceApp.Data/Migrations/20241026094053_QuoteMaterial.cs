using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class QuoteMaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuoteMaterial_Quotes_QuoteId",
                table: "QuoteMaterial");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuoteMaterial",
                table: "QuoteMaterial");

            migrationBuilder.RenameTable(
                name: "QuoteMaterial",
                newName: "QuoteMaterials");

            migrationBuilder.RenameIndex(
                name: "IX_QuoteMaterial_QuoteId",
                table: "QuoteMaterials",
                newName: "IX_QuoteMaterials_QuoteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuoteMaterials",
                table: "QuoteMaterials",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuoteMaterials_Quotes_QuoteId",
                table: "QuoteMaterials",
                column: "QuoteId",
                principalTable: "Quotes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuoteMaterials_Quotes_QuoteId",
                table: "QuoteMaterials");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuoteMaterials",
                table: "QuoteMaterials");

            migrationBuilder.RenameTable(
                name: "QuoteMaterials",
                newName: "QuoteMaterial");

            migrationBuilder.RenameIndex(
                name: "IX_QuoteMaterials_QuoteId",
                table: "QuoteMaterial",
                newName: "IX_QuoteMaterial_QuoteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuoteMaterial",
                table: "QuoteMaterial",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuoteMaterial_Quotes_QuoteId",
                table: "QuoteMaterial",
                column: "QuoteId",
                principalTable: "Quotes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
