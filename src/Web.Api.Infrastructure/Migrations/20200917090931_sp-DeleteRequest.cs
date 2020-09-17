using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class spDeleteRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE OR ALTER PROC DeleteRequest
(
	@Id UNIQUEIDENTIFIER
)
AS
BEGIN
	DELETE FROM [dbo].[Request] 
	WHERE @Id = Request.Id
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop proc DeleteRequest");
        }
    }
}
