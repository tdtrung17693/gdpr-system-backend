using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class spCreateComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE OR ALTER PROC CreateComment
(
@RequestId UNIQUEIDENTIFIER,
@Content NVARCHAR(300),
@ParentId UNIQUEIDENTIFIER=NULL,
@AuthorId UNIQUEIDENTIFIER
)
AS
BEGIN
    DECLARE @NewId UNIQUEIDENTIFIER = NEWID();
    DECLARE @CreatedAt DATETIME = GETUTCDATE();
    INSERT INTO dbo.[Comment] (Id, CreatedBy, CreatedAt, Content, RequestId, ParentId, Status) VALUES
        (@NewId, @AuthorId, @CreatedAt, @Content, @RequestId, @ParentId, 1)
    SELECT @NewId AS Id, @CreatedAt AS CreatedAt;
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop proc CreateComment");
        }
    }
}