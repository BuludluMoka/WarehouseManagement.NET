using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse.Migrations
{
    public partial class init15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transacctions_Ambars_receiver_id",
                table: "Transacctions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transacctions_Ambars_sender_id",
                table: "Transacctions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transacctions_Products_product_id",
                table: "Transacctions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transacctions",
                table: "Transacctions");

            migrationBuilder.RenameTable(
                name: "Transacctions",
                newName: "Transactions");

            migrationBuilder.RenameIndex(
                name: "IX_Transacctions_sender_id",
                table: "Transactions",
                newName: "IX_Transactions_sender_id");

            migrationBuilder.RenameIndex(
                name: "IX_Transacctions_receiver_id",
                table: "Transactions",
                newName: "IX_Transactions_receiver_id");

            migrationBuilder.RenameIndex(
                name: "IX_Transacctions_product_id",
                table: "Transactions",
                newName: "IX_Transactions_product_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Ambars_receiver_id",
                table: "Transactions",
                column: "receiver_id",
                principalTable: "Ambars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Ambars_sender_id",
                table: "Transactions",
                column: "sender_id",
                principalTable: "Ambars",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Products_product_id",
                table: "Transactions",
                column: "product_id",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Ambars_receiver_id",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Ambars_sender_id",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Products_product_id",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.RenameTable(
                name: "Transactions",
                newName: "Transacctions");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_sender_id",
                table: "Transacctions",
                newName: "IX_Transacctions_sender_id");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_receiver_id",
                table: "Transacctions",
                newName: "IX_Transacctions_receiver_id");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_product_id",
                table: "Transacctions",
                newName: "IX_Transacctions_product_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transacctions",
                table: "Transacctions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transacctions_Ambars_receiver_id",
                table: "Transacctions",
                column: "receiver_id",
                principalTable: "Ambars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transacctions_Ambars_sender_id",
                table: "Transacctions",
                column: "sender_id",
                principalTable: "Ambars",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transacctions_Products_product_id",
                table: "Transacctions",
                column: "product_id",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
