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
                name: "first_time_flag",
                columns: table => new
                {
                    firsttimerunning = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_first_time_flag", x => x.firsttimerunning);
                });

            migrationBuilder.CreateTable(
                name: "purchase_history_models",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(nullable: true),
                    purchase_date = table.Column<string>(nullable: true),
                    total_price = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purchase_history_models", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "bucket_models",
                columns: table => new
                {
                    basket_id = table.Column<string>(nullable: false),
                    store_id = table.Column<string>(nullable: true),
                    Cart_modelID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bucket_models", x => x.basket_id);
                    table.ForeignKey(
                        name: "FK_bucket_models_cart_models_Cart_modelID",
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
                    address = table.Column<string>(nullable: true),
                    is_admin = table.Column<bool>(nullable: false),
                    hashed_password = table.Column<string>(nullable: true),
                    user_ID = table.Column<string>(nullable: true),
                    user_state = table.Column<string>(nullable: true)
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
                name: "bucket_history_models",
                columns: table => new
                {
                    basket_id = table.Column<string>(nullable: false),
                    store_id = table.Column<string>(nullable: true),
                    purchase_history_modelID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bucket_history_models", x => x.basket_id);
                    table.ForeignKey(
                        name: "FK_bucket_history_models_purchase_history_models_purchase_history_modelID",
                        column: x => x.purchase_history_modelID,
                        principalTable: "purchase_history_models",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "products_in_baskets_models",
                columns: table => new
                {
                    product_id = table.Column<string>(nullable: false),
                    basket_id = table.Column<string>(nullable: false),
                    quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products_in_baskets_models", x => new { x.product_id, x.basket_id });
                    table.ForeignKey(
                        name: "FK_products_in_baskets_models_bucket_models_basket_id",
                        column: x => x.basket_id,
                        principalTable: "bucket_models",
                        principalColumn: "basket_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product_in_basket_history_models",
                columns: table => new
                {
                    product_id = table.Column<string>(nullable: false),
                    quantity = table.Column<int>(nullable: false),
                    basket_history_id = table.Column<string>(nullable: true),
                    Bucket_model_historybasket_id = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_in_basket_history_models", x => x.product_id);
                    table.ForeignKey(
                        name: "FK_Product_in_basket_history_models_bucket_history_models_Bucket_model_historybasket_id",
                        column: x => x.Bucket_model_historybasket_id,
                        principalTable: "bucket_history_models",
                        principalColumn: "basket_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_bucket_history_models_purchase_history_modelID",
                table: "bucket_history_models",
                column: "purchase_history_modelID");

            migrationBuilder.CreateIndex(
                name: "IX_bucket_models_Cart_modelID",
                table: "bucket_models",
                column: "Cart_modelID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_in_basket_history_models_Bucket_model_historybasket_id",
                table: "Product_in_basket_history_models",
                column: "Bucket_model_historybasket_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_in_baskets_models_basket_id",
                table: "products_in_baskets_models",
                column: "basket_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_models_my_cartID",
                table: "user_models",
                column: "my_cartID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "first_time_flag");

            migrationBuilder.DropTable(
                name: "Product_in_basket_history_models");

            migrationBuilder.DropTable(
                name: "products_in_baskets_models");

            migrationBuilder.DropTable(
                name: "user_models");

            migrationBuilder.DropTable(
                name: "bucket_history_models");

            migrationBuilder.DropTable(
                name: "bucket_models");

            migrationBuilder.DropTable(
                name: "purchase_history_models");

            migrationBuilder.DropTable(
                name: "cart_models");
        }
    }
}
