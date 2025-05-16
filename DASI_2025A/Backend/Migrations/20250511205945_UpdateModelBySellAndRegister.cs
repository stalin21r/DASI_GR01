using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelBySellAndRegister : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Action = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    QuantityBefore = table.Column<long>(type: "bigint", nullable: false),
                    QuantityAfter = table.Column<long>(type: "bigint", nullable: false),
                    ProductFk = table.Column<int>(type: "int", nullable: false),
                    UserFk = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AuditableDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    MachineName = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValueSql: "HOST_NAME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductLogs_AspNetUsers_UserFk",
                        column: x => x.UserFk,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductLogs_Products_ProductFk",
                        column: x => x.ProductFk,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductLogs_ProductFk",
                table: "ProductLogs",
                column: "ProductFk");

            migrationBuilder.CreateIndex(
                name: "IX_ProductLogs_UserFk",
                table: "ProductLogs",
                column: "UserFk");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductLogs");
        }
    }
}
