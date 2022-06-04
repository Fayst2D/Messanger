using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Messanger.DataAccess.Migrations
{
    public partial class RemoveKeyFromUserChat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatEntityUserEntity");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
