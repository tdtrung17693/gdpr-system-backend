using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class spGetRequestNoPages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE OR ALTER PROC RequestGetNoPages
(
	@PageSize INT = 10,
	@NoPages INT OUT   
)
AS
BEGIN 
	SET @NoPages = CEILING((SELECT COUNT(id) FROM Request)/@PageSize)
	--SELECT @NoPages = SCOPE_IDENTITY()
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop proc RequestGetNoPages");
        }
    }
}
