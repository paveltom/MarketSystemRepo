﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Market_System.Migrations
{
    public partial class Initcreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cart_models",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    total_price = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cart_models", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "bucker_models",
                columns: table => new
                {
                    basket_id = table.Column<string>(nullable: false),
                    store_id = table.Column<string>(nullable: true),
                    Cart_modelID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bucker_models", x => x.basket_id);
                    table.ForeignKey(
                        name: "FK_bucker_models_cart_models_Cart_modelID",
                        column: x => x.Cart_modelID,
                        principalTable: "cart_models",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_models",
                columns: table => new
                {
                    username = table.Column<string>(nullable: false),
                    my_cartID = table.Column<int>(nullable: true),
                    address = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_models", x => x.username);
                    table.ForeignKey(
                        name: "FK_user_models_cart_models_my_cartID",
                        column: x => x.my_cartID,
                        principalTable: "cart_models",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Product_in_basket_model",
                columns: table => new
                {
                    product_id = table.Column<string>(nullable: false),
                    quantity = table.Column<int>(nullable: false),
                    Bucket_modelbasket_id = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_in_basket_model", x => x.product_id);
                    table.ForeignKey(
                        name: "FK_Product_in_basket_model_bucker_models_Bucket_modelbasket_id",
                        column: x => x.Bucket_modelbasket_id,
                        principalTable: "bucker_models",
                        principalColumn: "basket_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_bucker_models_Cart_modelID",
                table: "bucker_models",
                column: "Cart_modelID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_in_basket_model_Bucket_modelbasket_id",
                table: "Product_in_basket_model",
                column: "Bucket_modelbasket_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_models_my_cartID",
                table: "user_models",
                column: "my_cartID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product_in_basket_model");

            migrationBuilder.DropTable(
                name: "user_models");

            migrationBuilder.DropTable(
                name: "bucker_models");

            migrationBuilder.DropTable(
                name: "cart_models");
        }
    }
}
