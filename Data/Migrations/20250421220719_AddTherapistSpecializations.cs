using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mentalHealth.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTherapistSpecializations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LicenseNumber",
                table: "Therapists");

            migrationBuilder.DropColumn(
                name: "YearsOfExperience",
                table: "Therapists");

            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "Therapists",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AverageRating",
                table: "Therapists",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Education",
                table: "Therapists",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Therapists",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfileImage",
                table: "Therapists",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ReviewCount",
                table: "Therapists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Specializations",
                table: "Therapists",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "Therapists");

            migrationBuilder.DropColumn(
                name: "Education",
                table: "Therapists");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Therapists");

            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "Therapists");

            migrationBuilder.DropColumn(
                name: "ReviewCount",
                table: "Therapists");

            migrationBuilder.DropColumn(
                name: "Specializations",
                table: "Therapists");

            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "Therapists",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "LicenseNumber",
                table: "Therapists",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "YearsOfExperience",
                table: "Therapists",
                type: "int",
                nullable: true);
        }
    }
}
