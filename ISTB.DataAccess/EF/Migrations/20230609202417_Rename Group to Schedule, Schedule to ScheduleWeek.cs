using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISTB.DataAccess.EF.Migrations
{
    /// <inheritdoc />
    public partial class RenameGrouptoScheduleScheduletoScheduleWeek : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleDays_Schedules_ScheduleId",
                table: "ScheduleDays");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Groups_GroupId",
                table: "Schedules");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_GroupId",
                table: "Schedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ScheduleDays",
                table: "ScheduleDays");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "Schedules");

            migrationBuilder.RenameTable(
                name: "Schedules",
                newName: "Schedule");

            migrationBuilder.RenameTable(
                name: "ScheduleDays",
                newName: "SchedulesDays");

            migrationBuilder.RenameColumn(
                name: "ScheduleId",
                table: "SchedulesDays",
                newName: "ScheduleWeekId");

            migrationBuilder.RenameColumn(
                name: "ImageFileName",
                table: "SchedulesDays",
                newName: "ImageFileUrl");

            migrationBuilder.RenameIndex(
                name: "IX_ScheduleDays_ScheduleId",
                table: "SchedulesDays",
                newName: "IX_SchedulesDays_ScheduleWeekId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Schedule",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Schedule",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schedule",
                table: "Schedule",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SchedulesDays",
                table: "SchedulesDays",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SchedulesWeeks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Position = table.Column<long>(type: "bigint", nullable: false),
                    ScheduleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchedulesWeeks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchedulesWeeks_Schedule_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedule",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_UserId",
                table: "Schedule",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SchedulesWeeks_ScheduleId",
                table: "SchedulesWeeks",
                column: "ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Users_UserId",
                table: "Schedule",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SchedulesDays_SchedulesWeeks_ScheduleWeekId",
                table: "SchedulesDays",
                column: "ScheduleWeekId",
                principalTable: "SchedulesWeeks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Users_UserId",
                table: "Schedule");

            migrationBuilder.DropForeignKey(
                name: "FK_SchedulesDays_SchedulesWeeks_ScheduleWeekId",
                table: "SchedulesDays");

            migrationBuilder.DropTable(
                name: "SchedulesWeeks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SchedulesDays",
                table: "SchedulesDays");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schedule",
                table: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_UserId",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Schedule");

            migrationBuilder.RenameTable(
                name: "SchedulesDays",
                newName: "ScheduleDays");

            migrationBuilder.RenameTable(
                name: "Schedule",
                newName: "Schedules");

            migrationBuilder.RenameColumn(
                name: "ScheduleWeekId",
                table: "ScheduleDays",
                newName: "ScheduleId");

            migrationBuilder.RenameColumn(
                name: "ImageFileUrl",
                table: "ScheduleDays",
                newName: "ImageFileName");

            migrationBuilder.RenameIndex(
                name: "IX_SchedulesDays_ScheduleWeekId",
                table: "ScheduleDays",
                newName: "IX_ScheduleDays_ScheduleId");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Schedules",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Position",
                table: "Schedules",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScheduleDays",
                table: "ScheduleDays",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_GroupId",
                table: "Schedules",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_UserId",
                table: "Groups",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleDays_Schedules_ScheduleId",
                table: "ScheduleDays",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Groups_GroupId",
                table: "Schedules",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }
    }
}
