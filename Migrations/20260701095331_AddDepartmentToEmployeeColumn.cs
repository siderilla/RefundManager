using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefundManager.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartmentToEmployeeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "department",
                table: "employee",
                type: "character varying",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "department",
                table: "employee");
        }
    }
}
