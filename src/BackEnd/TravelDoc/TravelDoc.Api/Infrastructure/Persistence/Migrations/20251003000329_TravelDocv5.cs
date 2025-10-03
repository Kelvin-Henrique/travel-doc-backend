using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TravelDoc.Api.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TravelDocv5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "viagem_participante_tb",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    viagem_id = table.Column<int>(type: "integer", nullable: false),
                    participante_id = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    data_alteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    data_inclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_viagem_participante_tb", x => x.id);
                    table.ForeignKey(
                        name: "fk_viagem_participante_tb_usuario_tb",
                        column: x => x.participante_id,
                        principalTable: "usuario_tb",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_viagem_participante_tb_viagem_tb",
                        column: x => x.viagem_id,
                        principalTable: "viagem_tb",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_viagem_participante_tb_participante_id",
                table: "viagem_participante_tb",
                column: "participante_id");

            migrationBuilder.CreateIndex(
                name: "IX_viagem_participante_tb_viagem_id",
                table: "viagem_participante_tb",
                column: "viagem_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "viagem_participante_tb");
        }
    }
}
