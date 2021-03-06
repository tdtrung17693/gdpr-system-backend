﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class spCreateUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
create or alter proc CreateUser
        (
          @FirstName NVARCHAR(20),
          @LastName NVARCHAR(20),
          @Email NVARCHAR(30),
          @Username NVARCHAR(30),
          @HashedPassword VARBINARY(256),
          @Salt NVARCHAR(20),
          @RoleId UNIQUEIDENTIFIER,
          @Creator UNIQUEIDENTIFIER = NULL,
          @Status Bit = 1
        )
        as
        begin
            SET NOCOUNT ON;
          begin transaction;
            declare @newId UNIQUEIDENTIFIER = NEWID();
            insert into dbo.[User] (Id, FirstName, LastName, Email, CreatedBy, RoleId, [Status]) VALUES (@newId, @FirstName, @LastName, @Email, @Creator, @RoleId, @Status);
            insert into dbo.[Account] (Id, Username, HashedPassword, Salt, UserId) VALUES (NEWID(), @Username, @HashedPassword, @Salt, @newId);
            select @newId as Id;
          commit;
        end");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop proc CreateUser");
        }
    }
}
