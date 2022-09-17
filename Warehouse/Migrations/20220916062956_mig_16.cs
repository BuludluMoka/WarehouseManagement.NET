using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse.Migrations
{
    public partial class mig_16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Ambars_sender_id",
                table: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "sender_id",
                table: "Transactions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Ambars_sender_id",
                table: "Transactions",
                column: "sender_id",
                principalTable: "Ambars",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Ambars_sender_id",
                table: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "sender_id",
                table: "Transactions",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Ambars_sender_id",
                table: "Transactions",
                column: "sender_id",
                principalTable: "Ambars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
