using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefundManager.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    budget = table.Column<decimal>(type: "numeric", nullable: false),
                    name = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("category_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "employee",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    surname = table.Column<string>(type: "character varying", nullable: false),
                    mail = table.Column<string>(type: "character varying", nullable: false),
                    phone = table.Column<string>(type: "character varying", nullable: false),
                    role = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("employee_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "outlay",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    category = table.Column<Guid>(type: "uuid", nullable: false),
                    employee = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    receipt = table.Column<bool>(type: "boolean", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    description = table.Column<string>(type: "character varying", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("outlay_pkey", x => x.id);
                    table.ForeignKey(
                        name: "category_id",
                        column: x => x.category,
                        principalTable: "category",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "employee_id",
                        column: x => x.employee,
                        principalTable: "employee",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "refund",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    outlay = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    approved = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("refund_pk", x => x.id);
                    table.ForeignKey(
                        name: "outlay_id",
                        column: x => x.outlay,
                        principalTable: "outlay",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_outlay_category",
                table: "outlay",
                column: "category");

            migrationBuilder.CreateIndex(
                name: "IX_outlay_employee",
                table: "outlay",
                column: "employee");

            migrationBuilder.CreateIndex(
                name: "outlay_uq",
                table: "refund",
                column: "outlay",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "refund");

            migrationBuilder.DropTable(
                name: "outlay");

            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropTable(
                name: "employee");
        }
    }
}
