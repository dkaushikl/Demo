namespace Demo.API.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Initial : Migration
    {
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("Catalogs");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Catalogs",
                table => new
                             {
                                 Id = table.Column<Guid>(),
                                 CreatedDate = table.Column<DateTime>(),
                                 Disabled = table.Column<bool>(),
                                 ModifiedDate = table.Column<DateTime>(),
                                 Name = table.Column<string>(maxLength: 100),
                                 Order = table.Column<int>()
                             },
                constraints: table => { table.PrimaryKey("PK_Catalogs", x => x.Id); });
        }
    }
}