using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieLibraryOO.Migrations
{
    public partial class InsertUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sqlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Migrations", "Data", @"4-InsertUsers.sql");  //inserting info from the file in data folder
            migrationBuilder.Sql(File.ReadAllText(sqlFile));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from users");
        }
    }
}
