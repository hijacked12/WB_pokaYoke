using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WB.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class newDbSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaterialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComponentNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComponentDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    ChangeNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories1", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "change_state",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    state = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_change_state", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ScanLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Line = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaterialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComponentNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Supplier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductionDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BatchNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScanDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ScanTime = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScanLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "scanner",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ScannerDTOid = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    scan_value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pn_no = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    plc_address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    supplier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    prodDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    batchNo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scanner", x => x.id);
                    table.ForeignKey(
                        name: "FK_scanner_scanner_ScannerDTOid",
                        column: x => x.ScannerDTOid,
                        principalTable: "scanner",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_scanner_ScannerDTOid",
                table: "scanner",
                column: "ScannerDTOid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories1");

            migrationBuilder.DropTable(
                name: "change_state");

            migrationBuilder.DropTable(
                name: "ScanLog");

            migrationBuilder.DropTable(
                name: "scanner");
        }
    }
}
