using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicCollection.BusinessLogic.Migrations
{
    /// <inheritdoc />
    public partial class Queues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QueueContextStorage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContextId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueueContextStorage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QueueListStorage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false),
                    TrackId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueueListStorage", x => new { x.Id, x.Position });
                });

            migrationBuilder.CreateTable(
                name: "QueuePointerStorage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Current = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueuePointerStorage", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QueueContextStorage");

            migrationBuilder.DropTable(
                name: "QueueListStorage");

            migrationBuilder.DropTable(
                name: "QueuePointerStorage");
        }
    }
}
