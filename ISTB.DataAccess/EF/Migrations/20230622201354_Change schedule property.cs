using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISTB.DataAccess.EF.Migrations
{
    /// <inheritdoc />
    public partial class Changescheduleproperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveWeek",
                table: "Schedule");

            migrationBuilder.AlterColumn<long>(
                name: "Position",
                table: "SchedulesWeeks",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ChosenAsCurrentWeekPosition",
                table: "Schedule",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeWhenWeekChosen",
                table: "Schedule",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChosenAsCurrentWeekPosition",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "DateTimeWhenWeekChosen",
                table: "Schedule");

            migrationBuilder.AlterColumn<long>(
                name: "Position",
                table: "SchedulesWeeks",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "ActiveWeek",
                table: "Schedule",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
