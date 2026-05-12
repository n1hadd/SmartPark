using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartPark.Migrations
{
    /// <inheritdoc />
    public partial class AddRegistrskaStevilkaToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LicensePlate",
                table: "AspNetUsers",
                newName: "RegistrskaStevilka");

            migrationBuilder.AddColumn<int>(
                name: "Stevilka",
                table: "ParkirnoMesto",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stevilka",
                table: "ParkirnoMesto");

            migrationBuilder.RenameColumn(
                name: "RegistrskaStevilka",
                table: "AspNetUsers",
                newName: "LicensePlate");
        }
    }
}
