using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class spDeleteComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE OR ALTER PROCEDURE DeleteComment (
	@CommentId UNIQUEIDENTIFIER
)
AS
BEGIN
	DELETE FROM dbo.Comment
	WHERE Comment.Id = @CommentId OR Comment.ParentId = @CommentId;
END");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Drop proc DeleteComment");
        }
    }
}
