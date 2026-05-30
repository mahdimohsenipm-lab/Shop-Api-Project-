using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class updateneworderandrequestpay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestPays_Orders_OrderId",
                table: "RequestPays");

            migrationBuilder.DropIndex(
                name: "IX_RequestPays_OrderId",
                table: "RequestPays");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "RequestPays",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_RequestPays_OrderId",
                table: "RequestPays",
                column: "OrderId",
                unique: true,
                filter: "[OrderId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestPays_Orders_OrderId",
                table: "RequestPays",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestPays_Orders_OrderId",
                table: "RequestPays");

            migrationBuilder.DropIndex(
                name: "IX_RequestPays_OrderId",
                table: "RequestPays");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "RequestPays",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestPays_OrderId",
                table: "RequestPays",
                column: "OrderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestPays_Orders_OrderId",
                table: "RequestPays",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
