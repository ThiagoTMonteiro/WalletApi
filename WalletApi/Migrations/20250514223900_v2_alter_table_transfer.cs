using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WalletApi.Migrations
{
    /// <inheritdoc />
    public partial class v2_alter_table_transfer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ToUserId",
                table: "Transfers",
                newName: "SenderUserId");

            migrationBuilder.RenameColumn(
                name: "FromUserId",
                table: "Transfers",
                newName: "ReceiverUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SenderUserId",
                table: "Transfers",
                newName: "ToUserId");

            migrationBuilder.RenameColumn(
                name: "ReceiverUserId",
                table: "Transfers",
                newName: "FromUserId");
        }
    }
}
