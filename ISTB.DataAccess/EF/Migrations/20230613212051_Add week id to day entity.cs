using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISTB.DataAccess.EF.Migrations
{
    /// <inheritdoc />
    public partial class Addweekidtodayentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchedulesDays_SchedulesWeeks_ScheduleWeekId",
                table: "SchedulesDays");

            migrationBuilder.AlterColumn<int>(
                name: "ScheduleWeekId",
                table: "SchedulesDays",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SchedulesDays_SchedulesWeeks_ScheduleWeekId",
                table: "SchedulesDays",
                column: "ScheduleWeekId",
                principalTable: "SchedulesWeeks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchedulesDays_SchedulesWeeks_ScheduleWeekId",
                table: "SchedulesDays");

            migrationBuilder.AlterColumn<int>(
                name: "ScheduleWeekId",
                table: "SchedulesDays",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_SchedulesDays_SchedulesWeeks_ScheduleWeekId",
                table: "SchedulesDays",
                column: "ScheduleWeekId",
                principalTable: "SchedulesWeeks",
                principalColumn: "Id");
        }
    }
}
