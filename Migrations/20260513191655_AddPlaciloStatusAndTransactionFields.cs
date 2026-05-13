using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartPark.Migrations
{
    /// <inheritdoc />
    public partial class AddPlaciloStatusAndTransactionFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FailureReason",
                table: "Placilo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Placilo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "Placilo",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FailureReason",
                table: "Placilo");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Placilo");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Placilo");
        }
    }
}
