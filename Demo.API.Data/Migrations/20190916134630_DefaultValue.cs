namespace Demo.API.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class DefaultValue : Migration
    {
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                "Disabled",
                "Catalogs",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: true);
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                "Disabled",
                "Catalogs",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool));
        }
    }
}