using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagemetApi.Migrations
{
    /// <inheritdoc />
    public partial class minorchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borroweds_Payments_PaymentId",
                table: "Borroweds");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Reservations");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentId",
                table: "Borroweds",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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
                name: "FK_Borroweds_Payments_PaymentId",
                table: "Borroweds",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Borroweds_Reservations_ReservationId",
                table: "Borroweds",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borroweds_Payments_PaymentId",
                table: "Borroweds");

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
                name: "Status",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentId",
                table: "Borroweds",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Borroweds_Payments_PaymentId",
                table: "Borroweds",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
