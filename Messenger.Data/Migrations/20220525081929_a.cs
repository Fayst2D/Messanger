using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Messenger.Data.Migrations
{
    public partial class a : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Users_UserEntityId",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_UserEntityId",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "UserEntityId",
                table: "Chats");

            migrationBuilder.RenameColumn(
                name: "MemberCount",
                table: "Chats",
                newName: "MembersCount");

            migrationBuilder.CreateTable(
                name: "ChatEntityUserEntity",
                columns: table => new
                {
                    ChatUsersId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChatsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatEntityUserEntity", x => new { x.ChatUsersId, x.ChatsId });
                    table.ForeignKey(
                        name: "FK_ChatEntityUserEntity_Chats_ChatsId",
                        column: x => x.ChatsId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatEntityUserEntity_Users_ChatUsersId",
                        column: x => x.ChatUsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatEntityUserEntity_ChatsId",
                table: "ChatEntityUserEntity",
                column: "ChatsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatEntityUserEntity");

            migrationBuilder.RenameColumn(
                name: "MembersCount",
                table: "Chats",
                newName: "MemberCount");

            migrationBuilder.AddColumn<Guid>(
                name: "UserEntityId",
                table: "Chats",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chats_UserEntityId",
                table: "Chats",
                column: "UserEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Users_UserEntityId",
                table: "Chats",
                column: "UserEntityId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
