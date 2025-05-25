using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class MigracionOrdenesPagos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentEntity_Orders_ParentOrderId",
                table: "PaymentEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentEntity",
                table: "PaymentEntity");

            migrationBuilder.DropIndex(
                name: "IX_PaymentEntity_ParentOrderId",
                table: "PaymentEntity");

            migrationBuilder.DropColumn(
                name: "PaidAt",
                table: "PaymentEntity");

            migrationBuilder.DropColumn(
                name: "ParentOrderId",
                table: "PaymentEntity");

            migrationBuilder.RenameTable(
                name: "PaymentEntity",
                newName: "Payments");

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "OrderDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "MachineName",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValueSql: "HOST_NAME()",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AuditableDate",
                table: "Payments",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "IssuedAt",
                table: "Payments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payments",
                table: "Payments",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId",
                table: "Payments",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Orders_OrderId",
                table: "Payments",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Orders_OrderId",
                table: "Payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payments",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_OrderId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "IssuedAt",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Payments");

            migrationBuilder.RenameTable(
                name: "Payments",
                newName: "PaymentEntity");

            migrationBuilder.AlterColumn<string>(
                name: "MachineName",
                table: "PaymentEntity",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValueSql: "HOST_NAME()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AuditableDate",
                table: "PaymentEntity",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidAt",
                table: "PaymentEntity",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentOrderId",
                table: "PaymentEntity",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentEntity",
                table: "PaymentEntity",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentEntity_ParentOrderId",
                table: "PaymentEntity",
                column: "ParentOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentEntity_Orders_ParentOrderId",
                table: "PaymentEntity",
                column: "ParentOrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
