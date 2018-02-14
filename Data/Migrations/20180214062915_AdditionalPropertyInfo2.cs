using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Data.Migrations
{
    public partial class AdditionalPropertyInfo2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AskingPrice",
                table: "Property",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LotSize",
                table: "Property",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AskingPrice",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "LotSize",
                table: "Property");
        }
    }
}
