using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Messanger.DataAccess.Migrations
{
    public partial class AddChatTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserEntityId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChatType",
                table: "Chats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserEntityId",
                table: "Users",
                column: "UserEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_UserEntityId",
                table: "Users",
                column: "UserEntityId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_UserEntityId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserEntityId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserEntityId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ChatType",
                table: "Chats");
        }
    }
}
