using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Net14Web.Migrations
{
    /// <inheritdoc />
    public partial class PC8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PCModel_CpuModel_CPUId",
                table: "PCModel");

            migrationBuilder.DropIndex(
                name: "IX_PCModel_CPUId",
                table: "PCModel");

            migrationBuilder.DropColumn(
                name: "CPUId",
                table: "PCModel");

            migrationBuilder.AddColumn<string>(
                name: "ModelFromManufacturer",
                table: "PCModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PCModelUser",
                columns: table => new
                {
                    PCModelsId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PCModelUser", x => new { x.PCModelsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_PCModelUser_PCModel_PCModelsId",
                        column: x => x.PCModelsId,
                        principalTable: "PCModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PCModelUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PCs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PCModelId = table.Column<int>(type: "int", nullable: false),
                    CPUId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PCs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PCs_CpuModel_CPUId",
                        column: x => x.CPUId,
                        principalTable: "CpuModel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PCs_PCModel_PCModelId",
                        column: x => x.PCModelId,
                        principalTable: "PCModel",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PCModelUser_UsersId",
                table: "PCModelUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_PCs_CPUId",
                table: "PCs",
                column: "CPUId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PCs_PCModelId",
                table: "PCs",
                column: "PCModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PCModelUser");

            migrationBuilder.DropTable(
                name: "PCs");

            migrationBuilder.DropColumn(
                name: "ModelFromManufacturer",
                table: "PCModel");

            migrationBuilder.AddColumn<int>(
                name: "CPUId",
                table: "PCModel",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PCModel_CPUId",
                table: "PCModel",
                column: "CPUId");

            migrationBuilder.AddForeignKey(
                name: "FK_PCModel_CpuModel_CPUId",
                table: "PCModel",
                column: "CPUId",
                principalTable: "CpuModel",
                principalColumn: "Id");
        }
    }
}
