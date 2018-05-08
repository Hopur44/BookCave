using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BookCave.Migrations
{
    public partial class PaymentInfoInBilling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ZipCode",
                table: "Billings",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "CardNumber",
                table: "Billings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardOwner",
                table: "Billings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CvCode",
                table: "Billings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExpireDate",
                table: "Billings",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Finished",
                table: "Billings",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardNumber",
                table: "Billings");

            migrationBuilder.DropColumn(
                name: "CardOwner",
                table: "Billings");

            migrationBuilder.DropColumn(
                name: "CvCode",
                table: "Billings");

            migrationBuilder.DropColumn(
                name: "ExpireDate",
                table: "Billings");

            migrationBuilder.DropColumn(
                name: "Finished",
                table: "Billings");

            migrationBuilder.AlterColumn<int>(
                name: "ZipCode",
                table: "Billings",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
