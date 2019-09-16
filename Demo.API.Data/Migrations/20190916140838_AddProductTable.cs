namespace Demo.API.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddProductTable : Migration
    {
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("Products");

            migrationBuilder.AlterColumn<bool>(
                "Disabled",
                "Catalogs",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldDefaultValue: false);
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                "Disabled",
                "Catalogs",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldDefaultValue: true);

            migrationBuilder.CreateTable(
                "Products",
                table => new
                             {
                                 Id = table.Column<Guid>(),
                                 CreatedDate = table.Column<DateTime>(),
                                 Disabled = table.Column<bool>(),
                                 ModifiedDate = table.Column<DateTime>(),
                                 CatalogId = table.Column<Guid>(),
                                 Name = table.Column<string>(maxLength: 100),
                                 Order = table.Column<int>()
                             },
                constraints: table => { table.PrimaryKey("PK_Products", x => x.Id); });
        }
    }
}