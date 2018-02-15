using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Data.Migrations
{
    public partial class MoreFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ListedAddress",
                table: "Property");

            migrationBuilder.AlterColumn<int>(
                name: "Beds",
                table: "Property",
                type: "decimal(3,1)",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Baths",
                table: "Property",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnnualHOA",
                table: "Property",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnnualTax",
                table: "Property",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Latitude",
                table: "Property",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Longitude",
                table: "Property",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourcePropertyId",
                table: "Property",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Sqft",
                table: "Property",
                nullable: true);

            migrationBuilder.CreateIndex("ListingIdAndPropertyId", "Property", new string[] { "RealtorListingId", "RealtorPropertyId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnnualHOA",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "AnnualTax",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "SourcePropertyId",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "Sqft",
                table: "Property");

            migrationBuilder.AlterColumn<int>(
                name: "Beds",
                table: "Property",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "decimal(3,1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Baths",
                table: "Property",
                nullable: true,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ListedAddress",
                table: "Property",
                maxLength: 500,
                nullable: true);
        }
    }
}
