using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agrosphere.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "USUARIOS_MONITORAMENTO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    TELEFONE = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIOS_MONITORAMENTO", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FAZENDAS_MONITORAMENTO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    LOCALIZACAO = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    DESCRICAO = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    USUARIO_ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAZENDAS_MONITORAMENTO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FAZENDAS_MONITORAMENTO_USUARIOS_MONITORAMENTO_USUARIO_ID",
                        column: x => x.USUARIO_ID,
                        principalTable: "USUARIOS_MONITORAMENTO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SENSORES_MONITORAMENTO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    TIPO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    LOCALIZACAO = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    DATA_INSTALACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    FAZENDA_ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SENSORES_MONITORAMENTO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SENSORES_MONITORAMENTO_FAZENDAS_MONITORAMENTO_FAZENDA_ID",
                        column: x => x.FAZENDA_ID,
                        principalTable: "FAZENDAS_MONITORAMENTO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ALERTAS_CLIMATICOS_SENSORES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    DESCRICAO = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    TIPO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    DATA_ALERTA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DATA_RESOLUCAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SENSOR_ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ALERTAS_CLIMATICOS_SENSORES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ALERTAS_CLIMATICOS_SENSORES_SENSORES_MONITORAMENTO_SENSOR_ID",
                        column: x => x.SENSOR_ID,
                        principalTable: "SENSORES_MONITORAMENTO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HISTORICO_LEITURAS_SENSORES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    DATA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DESCRICAO = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    TIPO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    SENSOR_ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HISTORICO_LEITURAS_SENSORES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HISTORICO_LEITURAS_SENSORES_SENSORES_MONITORAMENTO_SENSOR_ID",
                        column: x => x.SENSOR_ID,
                        principalTable: "SENSORES_MONITORAMENTO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LEITURAS_SENSORES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    DATA_HORA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    VALOR = table.Column<decimal>(type: "DECIMAL(10,2)", precision: 10, scale: 2, nullable: false),
                    UNIDADE = table.Column<string>(type: "NVARCHAR2(10)", maxLength: 10, nullable: false),
                    SENSOR_ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LEITURAS_SENSORES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LEITURAS_SENSORES_SENSORES_MONITORAMENTO_SENSOR_ID",
                        column: x => x.SENSOR_ID,
                        principalTable: "SENSORES_MONITORAMENTO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PREVISOES_SENSORES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    DESCRICAO = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    RECOMENDACAO = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    DATA_PREVISAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DATA_VIGENCIA_ATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SENSOR_ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PREVISOES_SENSORES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PREVISOES_SENSORES_SENSORES_MONITORAMENTO_SENSOR_ID",
                        column: x => x.SENSOR_ID,
                        principalTable: "SENSORES_MONITORAMENTO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ALERTAS_CLIMATICOS_SENSORES_SENSOR_ID",
                table: "ALERTAS_CLIMATICOS_SENSORES",
                column: "SENSOR_ID");

            migrationBuilder.CreateIndex(
                name: "IX_FAZENDAS_MONITORAMENTO_USUARIO_ID",
                table: "FAZENDAS_MONITORAMENTO",
                column: "USUARIO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HISTORICO_LEITURAS_SENSORES_SENSOR_ID",
                table: "HISTORICO_LEITURAS_SENSORES",
                column: "SENSOR_ID");

            migrationBuilder.CreateIndex(
                name: "IX_LEITURAS_SENSORES_SENSOR_ID",
                table: "LEITURAS_SENSORES",
                column: "SENSOR_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PREVISOES_SENSORES_SENSOR_ID",
                table: "PREVISOES_SENSORES",
                column: "SENSOR_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SENSORES_MONITORAMENTO_FAZENDA_ID",
                table: "SENSORES_MONITORAMENTO",
                column: "FAZENDA_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ALERTAS_CLIMATICOS_SENSORES");

            migrationBuilder.DropTable(
                name: "HISTORICO_LEITURAS_SENSORES");

            migrationBuilder.DropTable(
                name: "LEITURAS_SENSORES");

            migrationBuilder.DropTable(
                name: "PREVISOES_SENSORES");

            migrationBuilder.DropTable(
                name: "SENSORES_MONITORAMENTO");

            migrationBuilder.DropTable(
                name: "FAZENDAS_MONITORAMENTO");

            migrationBuilder.DropTable(
                name: "USUARIOS_MONITORAMENTO");
        }
    }
}
