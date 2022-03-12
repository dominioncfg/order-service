using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderService.Infrastructure.Migrations
{
    public partial class InitialOrderService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "core");

            migrationBuilder.CreateTable(
                name: "orders",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    buyer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    creation_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    status_name = table.Column<string>(type: "text", nullable: false),
                    address_country = table.Column<string>(type: "text", nullable: false),
                    address_city = table.Column<string>(type: "text", nullable: false),
                    address_street = table.Column<string>(type: "text", nullable: false),
                    address_number = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "order_items",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    sku = table.Column<string>(type: "text", nullable: false),
                    unit_price = table.Column<decimal>(type: "numeric", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_items_orders_order_id",
                        column: x => x.order_id,
                        principalSchema: "core",
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_order_items_order_id",
                schema: "core",
                table: "order_items",
                column: "order_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "order_items",
                schema: "core");

            migrationBuilder.DropTable(
                name: "orders",
                schema: "core");
        }
    }
}
