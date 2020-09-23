using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class changespCreateComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          migrationBuilder.Sql(@"
CREATE OR
ALTER PROC CreateComment(@RequestId UNIQUEIDENTIFIER,
                         @Content NVARCHAR(300),
                         @ParentId UNIQUEIDENTIFIER=NULL,
                         @AuthorId UNIQUEIDENTIFIER)
AS
BEGIN
    DECLARE @NewId UNIQUEIDENTIFIER = NEWID();
    DECLARE @CreatedAt DATETIME = GETUTCDATE();
    INSERT INTO dbo.[Comment] (Id, CreatedBy, CreatedAt, Content, RequestId, ParentId, Status)
    VALUES (@NewId, @AuthorId, @CreatedAt, @Content, @RequestId, @ParentId, 1)

    DECLARE @avatarFileName NVARCHAR(MAX);
    DECLARE @avatarPath NVARCHAR(MAX);
    DECLARE @avatarExt NVARCHAR(MAX);
    SELECT @avatarFileName = FI.FileName, @avatarPath = FI.Path, @avatarExt = FI.Extension
    FROM [dbo].[User] u
             LEFT JOIN UserFileInstance UFI ON u.Id = UFI.UserId
             LEFT JOIN FileInstance FI ON FI.Id = UFI.FileInstanceId
    WHERE u.Id = @AuthorId;


    SELECT @NewId          AS Id,
           @CreatedAt      AS CreatedAt,
           @avatarExt      AS AvatarExtension,
           @avatarPath     AS AvatarPath,
           @avatarFileName AS AvatarFileName;
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
