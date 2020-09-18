using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class GetAdminEmailListToSendEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE OR ALTER PROCEDURE GetAdminEmailListToSendEmail
AS
BEGIN
    SELECT dbo.[User].Email
    FROM dbo.[User] JOIN dbo.[Role] ON dbo.[User].RoleId = dbo.[Role].Id
    WHERE dbo.[Role].[Name] = 'Administrator'
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop proc GetAdminEmailListToSendEmail");
        }
    }
}
