using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class spCreateMultipleLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                create type NewLogs as table
                (
                    UpdatedField nvarchar(max),
                    UpdatedState nvarchar(max),
                    PreviousState nvarchar(max),
                    Message nvarchar(max)
                )");
            migrationBuilder.Sql(@"
            CREATE OR ALTER PROC CreateMultipleLogs (
                @NewLogs NewLogs READONLY,
                @RequestId UNIQUEIDENTIFIER,
                @CreatedBy UNIQUEIDENTIFIER
            )
            AS
            BEGIN
                INSERT INTO dbo.[HistoryLog](Id, RequestId, CreatedAt, CreatedBy, UpdatedField, UpdatedState, PreviousState, [Message])
                    SELECT newid(), @RequestId, GETUTCDATE(), @CreatedBy, UpdatedField, UpdatedState, PreviousState, Message from @NewLogs
            END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop proc CreateMultipleLogs");
            migrationBuilder.Sql("drop type NewLogs;");
        }
    }
}
