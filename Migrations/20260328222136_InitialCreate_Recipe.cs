using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcLab1.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate_Recipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Cuisine = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PrepTime = table.Column<int>(type: "int", nullable: false),
                    CookTime = table.Column<int>(type: "int", nullable: false),
                    Difficulty = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Ingredients = table.Column<string>(type: "nvarchar(max)", maxLength: 1000, nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_Cuisine",
                table: "Recipes",
                column: "Cuisine");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_Difficulty",
                table: "Recipes",
                column: "Difficulty");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recipes");
        }
    }
}
