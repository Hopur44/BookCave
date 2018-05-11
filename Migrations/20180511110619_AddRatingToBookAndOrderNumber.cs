using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BookCave.Migrations
{
    public partial class AddRatingToBookAndOrderNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "comment",
                table: "Reviews",
                newName: "Comment");

            migrationBuilder.AddColumn<string>(
                name: "OrderNumber",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Books",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "Comment",
                table: "Reviews",
                newName: "comment");
        }
    }
}
