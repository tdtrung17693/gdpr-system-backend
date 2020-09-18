using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class spDeleteServer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"
            CREATE OR ALTER PROCEDURE DeleteServer (
	@Id UNIQUEIDENTIFIER
)
AS
BEGIN
	DELETE FROM dbo.[Server]
	WHERE Id = @Id;
END;
            ";


            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sp = @"drop procedure DeleteServer;";
            migrationBuilder.Sql(sp);
        }
    }
}
