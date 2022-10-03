using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse.Migrations
{
    public partial class mig1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Anbars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Place = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anbars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AnbarId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Anbars_AnbarId",
                        column: x => x.AnbarId,
                        principalTable: "Anbars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    buyPrice = table.Column<float>(type: "real", nullable: false),
                    sellPrice = table.Column<float>(type: "real", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sender_id = table.Column<int>(type: "int", nullable: true),
                    receiver_id = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Anbars_receiver_id",
                        column: x => x.receiver_id,
                        principalTable: "Anbars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_Anbars_sender_id",
                        column: x => x.sender_id,
                        principalTable: "Anbars",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transactions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transactions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Anbars",
                columns: new[] { "Id", "CreatedDate", "Name", "Phone", "Place", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 10, 3, 12, 13, 9, 359, DateTimeKind.Local).AddTicks(6400), "Yasamal", "55623415", "Baki,Yasamal,Dalan4", null },
                    { 2, new DateTime(2022, 10, 3, 12, 13, 9, 359, DateTimeKind.Local).AddTicks(6410), "Seki", "55623415", "Seki,Xan Sarayi,Dalan4", null },
                    { 3, new DateTime(2022, 10, 3, 12, 13, 9, 359, DateTimeKind.Local).AddTicks(6412), "Qebele", "55623415", "Qebele,Dalan4", null },
                    { 4, new DateTime(2022, 10, 3, 12, 13, 9, 359, DateTimeKind.Local).AddTicks(6413), "Nerimanov", "55623415", "Baki,Nerimanov,Dalan4", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a18be9c0-aa65-4af8-bd17-00bd9344e575", "a8fba4ed-4e13-4547-bdea-ad2ca91531c3", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedDate", "Name", "ParentId", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 10, 3, 12, 13, 9, 359, DateTimeKind.Local).AddTicks(6433), "Electronics", null, null },
                    { 2, new DateTime(2022, 10, 3, 12, 13, 9, 359, DateTimeKind.Local).AddTicks(6434), "Medicine", null, null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "Address", "AnbarId", "Email", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "SecurityStamp", "Status", "UserName" },
                values: new object[] { "a18be9c0-aa65-4af8-bd17-00bd9344e575", "WarehouseHome", 1, "buludlumoka@gmail.com", "BULUDLUMOKA@GMAIL.COM", "ADMIN", "AQAAAAEAACcQAAAAEDTzlRF8WPYm3wcqBEma8t9n1XHD0gC8diz9BvO6fBNgqVhz3HvItjVVK27ickvRuw==", "055557623415", "", true, "Admin" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedDate", "Name", "ParentId", "UpdatedDate" },
                values: new object[,]
                {
                    { 3, new DateTime(2022, 10, 3, 12, 13, 9, 359, DateTimeKind.Local).AddTicks(6435), "Laptops", 1, null },
                    { 4, new DateTime(2022, 10, 3, 12, 13, 9, 359, DateTimeKind.Local).AddTicks(6437), "Mouse & Keyboards", 1, null },
                    { 5, new DateTime(2022, 10, 3, 12, 13, 9, 359, DateTimeKind.Local).AddTicks(6438), "Computer Components", 1, null },
                    { 6, new DateTime(2022, 10, 3, 12, 13, 9, 359, DateTimeKind.Local).AddTicks(6439), "Accessories", 1, null },
                    { 7, new DateTime(2022, 10, 3, 12, 13, 9, 359, DateTimeKind.Local).AddTicks(6440), "Electronic Medical Equipment", 2, null },
                    { 8, new DateTime(2022, 10, 3, 12, 13, 9, 359, DateTimeKind.Local).AddTicks(6441), "Diagnostic Medical Equipment", 2, null },
                    { 9, new DateTime(2022, 10, 3, 12, 13, 9, 359, DateTimeKind.Local).AddTicks(6442), "Durable Medical Equipment", 2, null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "a18be9c0-aa65-4af8-bd17-00bd9344e575", "a18be9c0-aa65-4af8-bd17-00bd9344e575" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedDate", "Description", "Name", "UpdatedDate", "buyPrice", "sellPrice" },
                values: new object[,]
                {
                    { 1, 3, new DateTime(2022, 10, 3, 12, 13, 9, 359, DateTimeKind.Local).AddTicks(6466), null, "Xiaomi RedmiBook Pro 15 Laptop 15.6 Inch 3.2K 90Hz Super Retina Screen AMD R5 5600H 16GB 512GB AMD Radeon Graphics Card Notebook", null, 1554.64f, 1660.55f },
                    { 2, 3, new DateTime(2022, 10, 3, 12, 13, 9, 359, DateTimeKind.Local).AddTicks(6468), null, "Dere V9 MAX Laptop 15.6',Intel Core i7-1165G7, 16GB RAM + 1TB SSD, 2.5K IPS Screen, Computer Office Windows 11 Notebook", null, 1111.34f, 1300.56f },
                    { 3, 5, new DateTime(2022, 10, 3, 12, 13, 9, 359, DateTimeKind.Local).AddTicks(6469), null, "AMD RX 580 8G Computer Graphics Card,RX580 8G For GDDR5 GPU mining Video Card", null, 185.5f, 200f },
                    { 4, 5, new DateTime(2022, 10, 3, 12, 13, 9, 359, DateTimeKind.Local).AddTicks(6470), null, "AMD Ryzen 9 5900X R9 5900X 3.7 GHz Twelve-Core 24-Thread CPU Processor", null, 777.6f, 956.78f },
                    { 5, 6, new DateTime(2022, 10, 3, 12, 13, 9, 359, DateTimeKind.Local).AddTicks(6471), null, "Domiso Mutil-use Laptop Sleeve With Handle For 14' 15.6' 17' Inch Notebook Computer Bag", null, 61f, 74.6f },
                    { 6, 6, new DateTime(2022, 10, 3, 12, 13, 9, 359, DateTimeKind.Local).AddTicks(6472), null, "Fan For Computer PC Laptop Notebook", null, 3f, 3.6f },
                    { 7, 7, new DateTime(2022, 10, 3, 12, 13, 9, 359, DateTimeKind.Local).AddTicks(6474), null, "Heart Rate Monitors", null, 800.6f, 996.78f },
                    { 8, 7, new DateTime(2022, 10, 3, 12, 13, 9, 359, DateTimeKind.Local).AddTicks(6475), null, "Blood Pressure Monitors", null, 14000.6f, 15560.78f },
                    { 9, 7, new DateTime(2022, 10, 3, 12, 13, 9, 359, DateTimeKind.Local).AddTicks(6476), null, "Ultrasound", null, 23000.6f, 35000.78f },
                    { 10, 8, new DateTime(2022, 10, 3, 12, 13, 9, 359, DateTimeKind.Local).AddTicks(6477), null, "MRI Scans", null, 12000.6f, 18000.78f },
                    { 11, 8, new DateTime(2022, 10, 3, 12, 13, 9, 359, DateTimeKind.Local).AddTicks(6478), null, "X-Rays", null, 4600.6f, 5000.78f },
                    { 12, 9, new DateTime(2022, 10, 3, 12, 13, 9, 359, DateTimeKind.Local).AddTicks(6480), null, "Hospital beds", null, 700.6f, 956.78f },
                    { 13, 9, new DateTime(2022, 10, 3, 12, 13, 9, 359, DateTimeKind.Local).AddTicks(6481), null, "Ventilators", null, 80.6f, 95.78f }
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AnbarId",
                table: "AspNetUsers",
                column: "AnbarId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentId",
                table: "Categories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ProductId",
                table: "Transactions",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_receiver_id",
                table: "Transactions",
                column: "receiver_id");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_sender_id",
                table: "Transactions",
                column: "sender_id");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserId",
                table: "Transactions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Anbars");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
