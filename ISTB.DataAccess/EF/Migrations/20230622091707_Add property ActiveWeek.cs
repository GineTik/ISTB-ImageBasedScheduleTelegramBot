using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISTB.DataAccess.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddpropertyActiveWeek : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActiveWeek",
                table: "Schedule",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveWeek",
                table: "Schedule");
        }
    }
}
