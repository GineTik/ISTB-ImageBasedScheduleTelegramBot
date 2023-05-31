using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISTB.DataAccess.EF.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserNametoTelegramUserIdandrenamepropertyFileNametoImageFileName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "ScheduleDays",
                newName: "ImageFileName");

            migrationBuilder.AddColumn<long>(
                name: "TelegramUserId",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TelegramUserId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "ImageFileName",
                table: "ScheduleDays",
                newName: "FileName");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
