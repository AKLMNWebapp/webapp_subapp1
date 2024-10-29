using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mvc.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_allergyProducts_Allergies_AllergyCode",
                table: "allergyProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_allergyProducts_Products_ProductId",
                table: "allergyProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Collections_Products_ProductId",
                table: "Collections");

            migrationBuilder.DropIndex(
                name: "IX_Collections_ProductId",
                table: "Collections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_allergyProducts",
                table: "allergyProducts");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Collections");

            migrationBuilder.RenameTable(
                name: "allergyProducts",
                newName: "AllergyProducts");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AllergyProducts",
                newName: "AllergyProductId");

            migrationBuilder.RenameIndex(
                name: "IX_allergyProducts_ProductId",
                table: "AllergyProducts",
                newName: "IX_AllergyProducts_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_allergyProducts_AllergyCode",
                table: "AllergyProducts",
                newName: "IX_AllergyProducts_AllergyCode");

            migrationBuilder.AddColumn<int>(
                name: "UserRole",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CollectionId",
                table: "Products",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Products",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Collections",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AllergyProducts",
                table: "AllergyProducts",
                column: "AllergyProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CollectionId",
                table: "Products",
                column: "CollectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AllergyProducts_Allergies_AllergyCode",
                table: "AllergyProducts",
                column: "AllergyCode",
                principalTable: "Allergies",
                principalColumn: "AllergyCode",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AllergyProducts_Products_ProductId",
                table: "AllergyProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Collections_CollectionId",
                table: "Products",
                column: "CollectionId",
                principalTable: "Collections",
                principalColumn: "CollectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AllergyProducts_Allergies_AllergyCode",
                table: "AllergyProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_AllergyProducts_Products_ProductId",
                table: "AllergyProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Collections_CollectionId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CollectionId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AllergyProducts",
                table: "AllergyProducts");

            migrationBuilder.DropColumn(
                name: "UserRole",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CollectionId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Collections");

            migrationBuilder.RenameTable(
                name: "AllergyProducts",
                newName: "allergyProducts");

            migrationBuilder.RenameColumn(
                name: "AllergyProductId",
                table: "allergyProducts",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_AllergyProducts_ProductId",
                table: "allergyProducts",
                newName: "IX_allergyProducts_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_AllergyProducts_AllergyCode",
                table: "allergyProducts",
                newName: "IX_allergyProducts_AllergyCode");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Collections",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_allergyProducts",
                table: "allergyProducts",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_Collections_ProductId",
                table: "Collections",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_allergyProducts_Allergies_AllergyCode",
                table: "allergyProducts",
                column: "AllergyCode",
                principalTable: "Allergies",
                principalColumn: "AllergyCode");

            migrationBuilder.AddForeignKey(
                name: "FK_allergyProducts_Products_ProductId",
                table: "allergyProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Collections_Products_ProductId",
                table: "Collections",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
