using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BookCave.Migrations
{
    public partial class ChangedModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "houseNumber",
                table: "Billings",
                newName: "HouseNumber");

            migrationBuilder.RenameColumn(
                name: "country",
                table: "Billings",
                newName: "Country");

            migrationBuilder.AlterColumn<string>(
                name: "PublishDate",
                table: "Books",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Billings",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HouseNumber",
                table: "Billings",
                newName: "houseNumber");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "Billings",
                newName: "country");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishDate",
                table: "Books",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "City",
                table: "Billings",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
