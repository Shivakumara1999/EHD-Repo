using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EHD.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ehd6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tickets_issues_IssueId",
                table: "tickets");

            migrationBuilder.AlterColumn<int>(
                name: "IssueId",
                table: "tickets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_tickets_issues_IssueId",
                table: "tickets",
                column: "IssueId",
                principalTable: "issues",
                principalColumn: "IssueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tickets_issues_IssueId",
                table: "tickets");

            migrationBuilder.AlterColumn<int>(
                name: "IssueId",
                table: "tickets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tickets_issues_IssueId",
                table: "tickets",
                column: "IssueId",
                principalTable: "issues",
                principalColumn: "IssueId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
