using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoWebApp.DAL.Migrations;

public partial class AddPictureUrlColumn : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "PictureUrl",
            table: "Categories",
            type: "nvarchar(max)",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "PictureUrl",
            table: "Categories");
    }
}
