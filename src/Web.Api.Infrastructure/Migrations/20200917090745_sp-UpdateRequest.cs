using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class spUpdateRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"
CREATE OR ALTER PROC UpdateRequest
(
	@Id uniqueidentifier,
	@Title NVARCHAR(50),
	@StartDate DATETIME,
	@EndDate DATETIME,
	@ServerId UNIQUEIDENTIFIER,
	@Description NVARCHAR(100),
	@RequestStatus NVARCHAR(MAX),
	@Response NVARCHAR(200),
	@ApprovedBy UNIQUEIDENTIFIER,
	@UpdatedBy UNIQUEIDENTIFIER,
	@UpdatedAt DATETIME
	
)
AS
BEGIN
	UPDATE [dbo].[Request] 
	SET			
				 Title = @Title,
				 StartDate = @StartDate,
				 EndDate = @EndDate,
				 ServerId = @ServerId,
				 [Description] = @Description,
				 RequestStatus = @RequestStatus,
				 Response = @Response,
				 ApprovedBy = @ApprovedBy,
				 UpdatedAt = GETDATE(),
				 UpdatedBy = @UpdatedBy
				
	WHERE @Id = Request.Id
END");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql("UpdateRequest");
        }
    }
}
