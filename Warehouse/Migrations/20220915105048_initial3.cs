using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse.Migrations
{
    public partial class initial3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transacctions_Ambars_AmbarId",
                table: "Transacctions");

            migrationBuilder.RenameColumn(
                name: "AmbarId",
                table: "Transacctions",
                newName: "sender_id");

            migrationBuilder.RenameIndex(
                name: "IX_Transacctions_AmbarId",
                table: "Transacctions",
                newName: "IX_Transacctions_sender_id");

            migrationBuilder.AddColumn<int>(
                name: "receiver_id",
                table: "Transacctions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transacctions_receiver_id",
                table: "Transacctions",
                column: "receiver_id");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transacctions_Ambars_receiver_id",
                table: "Transacctions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transacctions_Ambars_sender_id",
                table: "Transacctions");

            migrationBuilder.DropIndex(
                name: "IX_Transacctions_receiver_id",
                table: "Transacctions");

            migrationBuilder.DropColumn(
                name: "receiver_id",
                table: "Transacctions");

            migrationBuilder.RenameColumn(
                name: "sender_id",
                table: "Transacctions",
                newName: "AmbarId");

            migrationBuilder.RenameIndex(
                name: "IX_Transacctions_sender_id",
                table: "Transacctions",
                newName: "IX_Transacctions_AmbarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transacctions_Ambars_AmbarId",
                table: "Transacctions",
                column: "AmbarId",
                principalTable: "Ambars",
                principalColumn: "Id");
        }
    }
}
