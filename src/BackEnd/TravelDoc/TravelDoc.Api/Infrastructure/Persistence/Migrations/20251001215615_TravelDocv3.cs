using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelDoc.Api.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TravelDocv3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_viagem_tb_usuario_tb_criador_id",
                table: "viagem_tb");

            migrationBuilder.DropTable(
                name: "event");

            migrationBuilder.AlterColumn<string>(
                name: "nome_viagem",
                table: "viagem_tb",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "descricao",
                table: "viagem_tb",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<DateTime>(
                name: "data_alteracao",
                table: "viagem_tb",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "data_inclusao",
                table: "viagem_tb",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "fk_viagem_tb_usuario_tb",
                table: "viagem_tb",
                column: "criador_id",
                principalTable: "usuario_tb",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_viagem_tb_usuario_tb",
                table: "viagem_tb");

            migrationBuilder.DropColumn(
                name: "data_alteracao",
                table: "viagem_tb");

            migrationBuilder.DropColumn(
                name: "data_inclusao",
                table: "viagem_tb");

            migrationBuilder.AlterColumn<string>(
                name: "nome_viagem",
                table: "viagem_tb",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "descricao",
                table: "viagem_tb",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "event",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    creation_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    viagem_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_event", x => x.id);
                    table.ForeignKey(
                        name: "fk_event_viagem_tb_viagem_id",
                        column: x => x.viagem_id,
                        principalTable: "viagem_tb",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_event_viagem_id",
                table: "event",
                column: "viagem_id");

            migrationBuilder.AddForeignKey(
                name: "fk_viagem_tb_usuario_tb_criador_id",
                table: "viagem_tb",
                column: "criador_id",
                principalTable: "usuario_tb",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
