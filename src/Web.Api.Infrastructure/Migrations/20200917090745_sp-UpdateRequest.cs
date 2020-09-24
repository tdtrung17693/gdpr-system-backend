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
	@UpdatedBy UNIQUEIDENTIFIER,
	@UpdatedAt DATETIME
)
AS
BEGIN
    declare @oldTitle NVARCHAR(50)
    declare @oldStartDate DATETIME
    declare @oldEndDate DATETIME
    declare @oldServerid UNIQUEIDENTIFIER
    declare @oldDescription NVARCHAR(100)
    declare @updatedFields TABLE (UpdatedField nvarchar(max), PreviousState nvarchar(max), UpdatedState nvarchar(max));

    select @oldTitle = Title, @oldServerid = ServerId, @oldStartDate = StartDate, @oldEndDate = EndDate, @oldDescription = Description
    from [dbo].Request where Id = @Id;

    if @oldTitle != @Title
        begin
            insert into @updatedFields values ('Title', @oldTitle, @Title)
        end

    if @oldStartDate != @StartDate
        begin
            insert into @updatedFields values ('StartDate',
                                               convert(nvarchar(max), @oldStartDate),
                                               convert(nvarchar(max), @StartDate))
        end

    if @oldEndDate != @EndDate
        begin
            insert into @updatedFields values ('EndDate',
                                               convert(nvarchar(max), @oldEndDate),
                                               convert(nvarchar(max), @EndDate))
        end

    if @oldServerid != @ServerId
        begin
            insert into @updatedFields values ('ServerId',
                                               convert(nvarchar(max), @oldServerid),
                                               convert(nvarchar(max), @ServerId))
        end

    if @oldDescription != @Description
        begin
            insert into @updatedFields values ('Description', @oldDescription, @Description)
        end

    UPDATE [dbo].[Request]
	SET			
				 Title = @Title,
				 StartDate = @StartDate,
				 EndDate = @EndDate,
				 ServerId = @ServerId,
				 [Description] = @Description,
				 UpdatedAt = GETDATE(),
				 UpdatedBy = @UpdatedBy
				
	WHERE @Id = Request.Id

    select UpdatedField, PreviousState, UpdatedState from @updatedFields
END");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(" drop proc UpdateRequest");
        }
    }
}
