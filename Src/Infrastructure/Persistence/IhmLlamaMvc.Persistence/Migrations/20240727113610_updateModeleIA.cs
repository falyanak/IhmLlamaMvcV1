using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IhmLlamaMvc.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updateModeleIA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NomModeleApi",
                table: "IaModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NomModeleApi",
                table: "IaModels");
        }
    }
}
