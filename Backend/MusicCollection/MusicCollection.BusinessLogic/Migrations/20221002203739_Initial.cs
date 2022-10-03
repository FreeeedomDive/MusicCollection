using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicCollection.BusinessLogic.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NodesStorage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Path = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NodesStorage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RootsStorage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Path = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RootsStorage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersStorage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Login = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersStorage", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NodesStorage_Id_Path",
                table: "NodesStorage",
                columns: new[] { "Id", "Path" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NodesStorage");

            migrationBuilder.DropTable(
                name: "RootsStorage");

            migrationBuilder.DropTable(
                name: "UsersStorage");
        }
    }
}
