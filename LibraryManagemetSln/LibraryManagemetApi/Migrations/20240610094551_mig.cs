using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagemetApi.Migrations
{
    /// <inheritdoc />
    public partial class mig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borroweds_Payments_PaymentId",
                table: "Borroweds");

            migrationBuilder.DropIndex(
                name: "IX_Borroweds_PaymentId",
                table: "Borroweds");

            migrationBuilder.DropColumn(
                name: "Fine",
                table: "Borroweds");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Borroweds");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Fine",
                table: "Borroweds",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "Borroweds",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Borroweds_PaymentId",
                table: "Borroweds",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Borroweds_Payments_PaymentId",
                table: "Borroweds",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id");
        }
    }
}
