using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class spMarkAllAsRead : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"create or alter proc MarkAllAsRead
(
@UserId UNIQUEIDENTIFIER
)
as
begin
    update [dbo].[Notification] set IsRead = 1 where ToUserId = @UserId
end");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop proc MarkAllAsRead");
        }
    }
}
