using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelDoc.Api.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TravelDocv4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "data_inclusao",
                table: "viagem_tb");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "data_inclusao",
                table: "viagem_tb",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
