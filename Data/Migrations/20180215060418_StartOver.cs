using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Data.Migrations
{
    public partial class StartOver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PropertyStatus",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PropertyTag",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyTag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PropertyType",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RentBits",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Location = table.Column<string>(maxLength: 70, nullable: true),
                    OneBedRoom = table.Column<int>(nullable: false),
                    ThreeOrMoreBedRoom = table.Column<int>(nullable: false),
                    TwoBedRoom = table.Column<int>(nullable: false),
                    Type = table.Column<string>(maxLength: 10, nullable: true),
                    Url = table.Column<string>(nullable: true),
                    Zip = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentBits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Property",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(maxLength: 150, nullable: true),
                    Address2 = table.Column<string>(maxLength: 50, nullable: true),
                    AnnualHOA = table.Column<int>(nullable: true),
                    AnnualTax = table.Column<int>(nullable: true),
                    AskingPrice = table.Column<int>(nullable: true),
                    Baths = table.Column<decimal>(type: "decimal(3,1)", nullable: true),
                    Beds = table.Column<int>(type: "int", nullable: true),
                    City = table.Column<string>(maxLength: 100, nullable: true),
                    Latitude = table.Column<string>(maxLength: 30, nullable: true),
                    Longitude = table.Column<string>(maxLength: 30, nullable: true),
                    LotSize = table.Column<int>(nullable: true),
                    PropertyStatusId = table.Column<byte>(nullable: true),
                    PropertyTypeId = table.Column<byte>(nullable: true),
                    RealtorListingId = table.Column<string>(maxLength: 25, nullable: true),
                    RealtorPropertyId = table.Column<string>(maxLength: 25, nullable: true),
                    RealtorUrl = table.Column<string>(maxLength: 550, nullable: true),
                    SourcePropertyId = table.Column<string>(maxLength: 20, nullable: true),
                    Sqft = table.Column<int>(nullable: true),
                    State = table.Column<string>(maxLength: 50, nullable: true),
                    Zip = table.Column<string>(maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Property", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Property_PropertyStatus_PropertyStatusId",
                        column: x => x.PropertyStatusId,
                        principalTable: "PropertyStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Property_PropertyType_PropertyTypeId",
                        column: x => x.PropertyTypeId,
                        principalTable: "PropertyType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PropertyTags",
                columns: table => new
                {
                    PropertyId = table.Column<int>(nullable: false),
                    PropertyTagId = table.Column<byte>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyTags", x => new { x.PropertyId, x.PropertyTagId });
                    table.ForeignKey(
                        name: "FK_PropertyTags_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Property",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropertyTags_PropertyTag_PropertyTagId",
                        column: x => x.PropertyTagId,
                        principalTable: "PropertyTag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Property_PropertyStatusId",
                table: "Property",
                column: "PropertyStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Property_PropertyTypeId",
                table: "Property",
                column: "PropertyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyTags_PropertyTagId",
                table: "PropertyTags",
                column: "PropertyTagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PropertyTags");

            migrationBuilder.DropTable(
                name: "RentBits");

            migrationBuilder.DropTable(
                name: "Property");

            migrationBuilder.DropTable(
                name: "PropertyTag");

            migrationBuilder.DropTable(
                name: "PropertyStatus");

            migrationBuilder.DropTable(
                name: "PropertyType");
        }
    }
}
