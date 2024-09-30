using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalorieCount.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SecondeMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "DailyMeals");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "DailyMeals",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Meals");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "DailyMeals");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "DailyMeals",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        }
    }
}
