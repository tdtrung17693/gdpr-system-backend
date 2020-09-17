using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class spCreateRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE OR
    ALTER PROC CreateRequest(@CreatedBy UNIQUEIDENTIFIER,
                             @Title NVARCHAR(50),
                             @FromDate DATETIME,
                             @ToDate DATETIME,
                             @Server UNIQUEIDENTIFIER,
                             @Description NVARCHAR(100))
    AS
    BEGIN
        declare @NewId UNIQUEIDENTIFIER = newid();
        INSERT INTO [dbo].[Request]
        (Id,
         CreatedAt,
         CreatedBy,
         UpdatedAt,
         UpdatedBy,
         DeletedAt,
         DeletedBy,
         [Status],
         RequestStatus,
         Title,
         [Description],
         StartDate,
         EndDate,
         ServerId,
         Response,
         ApprovedBy)
        VALUES (@NewId,
                GETUTCDATE(),
                @CreatedBy,
                NULL,
                NULL,
                NULL,
                NULL,
                1,
                'New',
                @Title,
                @Description,
                @FromDate,
                @ToDate,
                @Server,
                NULL,
                NULL);

        select
               r.Id as RequestId, r.CreatedAt,
               concat(U.FirstName, ' ', U.LastName) as RequesterFullName,
               A.Username as RequesterUsername, U.Email as RequesterEmail, A.UserId as RequesterId,
               S.Name as ServerName, S.Id as ServerId
        from [dbo].Request r
        join [User] U ON r.CreatedBy = U.Id
        join Account A ON U.Id = A.UserId
        join Server S ON r.ServerId = S.Id
        WHERE r.Id = @NewId;
    END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop proc CreateRequest");
        }
    }
}
