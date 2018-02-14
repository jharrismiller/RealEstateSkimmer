using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Data.Migrations
{
    public partial class AdditionalPropertyInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ListedAddress",
                table: "Property",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RealtorListingId",
                table: "Property",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RealtorPropertyId",
                table: "Property",
                maxLength: 25,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ListedAddress",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "RealtorListingId",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "RealtorPropertyId",
                table: "Property");
        }
    }
}
