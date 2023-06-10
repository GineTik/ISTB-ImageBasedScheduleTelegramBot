using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISTB.DataAccess.EF.Migrations
{
    /// <inheritdoc />
    public partial class Updateproperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchedulesWeeks_Schedule_ScheduleId",
                table: "SchedulesWeeks");

            migrationBuilder.AlterColumn<int>(
                name: "ScheduleId",
                table: "SchedulesWeeks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Position",
                table: "SchedulesWeeks",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_SchedulesWeeks_Schedule_ScheduleId",
                table: "SchedulesWeeks",
                column: "ScheduleId",
                principalTable: "Schedule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchedulesWeeks_Schedule_ScheduleId",
                table: "SchedulesWeeks");

            migrationBuilder.AlterColumn<int>(
                name: "ScheduleId",
                table: "SchedulesWeeks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "Position",
                table: "SchedulesWeeks",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SchedulesWeeks_Schedule_ScheduleId",
                table: "SchedulesWeeks",
                column: "ScheduleId",
                principalTable: "Schedule",
                principalColumn: "Id");
        }
    }
}
