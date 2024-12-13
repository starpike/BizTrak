using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BizTrak.Data.Migrations
{
    /// <inheritdoc />
    public partial class Job_IsScheduled_Column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsScheduled",
                table: "Jobs",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsScheduled",
                table: "Jobs");
        }
    }
}
