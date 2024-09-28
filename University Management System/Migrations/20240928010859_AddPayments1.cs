using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class AddPayments1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoutingNumber",
                table: "Payments");

            migrationBuilder.AddColumn<string>(
                name: "IFSCCode",
                table: "Payments",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IFSCCode",
                table: "Payments");

            migrationBuilder.AddColumn<string>(
                name: "RoutingNumber",
                table: "Payments",
                type: "nvarchar(9)",
                maxLength: 9,
                nullable: true);
        }
    }
}
