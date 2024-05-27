using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagemetApi.Migrations
{
    /// <inheritdoc />
    public partial class changedreview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borroweds_Reservations_ReservationId",
                table: "Borroweds");

            migrationBuilder.DropIndex(
                name: "IX_Borroweds_ReservationId",
                table: "Borroweds");

            migrationBuilder.DropColumn(
                name: "ReservationId",
                table: "Borroweds");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Reviews");

            migrationBuilder.AddColumn<int>(
                name: "ReservationId",
                table: "Borroweds",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Borroweds_ReservationId",
                table: "Borroweds",
                column: "ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Borroweds_Reservations_ReservationId",
                table: "Borroweds",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id");
        }
    }
}
