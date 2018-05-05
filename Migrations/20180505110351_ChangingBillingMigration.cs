using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BookCave.Migrations
{
    public partial class ChangingBillingMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HouseNumber",
                table: "Billings");

            migrationBuilder.RenameColumn(
                name: "StreetName",
                table: "Billings",
                newName: "StreetAddress");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StreetAddress",
                table: "Billings",
                newName: "StreetName");

            migrationBuilder.AddColumn<int>(
                name: "HouseNumber",
                table: "Billings",
                nullable: false,
                defaultValue: 0);
        }
    }
}
