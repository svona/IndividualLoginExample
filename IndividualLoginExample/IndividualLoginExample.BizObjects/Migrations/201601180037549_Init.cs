namespace IndividualLoginExample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            #region Create Tables
            CreateTable(
                "dbo.Users",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserName = c.String(nullable: false, maxLength: 100),
                    PasswordHash = c.String(nullable: false, maxLength: 68),
                    CreationDateUTC = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LockoutEndDateUtc = c.DateTime(),
                    AccessFailedCount = c.Int(nullable: false),
                    LockoutEnabled = c.Boolean(nullable: false),
                    TwoFactorEnabled = c.Boolean(nullable: false),
                    Email = c.String(nullable: false, maxLength: 100),
                    EmailConfirmed = c.Boolean(nullable: false),
                    SecurityStamp = c.String(nullable: false),
                })
                .PrimaryKey(t => t.Id, name: "PK_Users");

            CreateTable(
                "dbo.UserPasswordHistory",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserId = c.Int(nullable: false),
                    PasswordHash = c.String(nullable: false, maxLength: 68),
                    CreatedBy = c.String(nullable: false, maxLength: 100),
                    CreationDateUTC = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                })
                .PrimaryKey(t => t.Id, "PK_UserPasswordHistory")
                .ForeignKey("dbo.Users", t => t.UserId, false, "FK_Users_UserPasswordHistory")
                .Index(t => t.UserId, name: "IX_UserPasswordHistory_UserId");

            CreateTable(
                "dbo.Roles",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 100),
                    CreationDateUTC = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                })
                .PrimaryKey(t => t.Id, name: "PK_Roles");

            CreateTable(
                "dbo.UserRoles",
                c => new
                {
                    RoleId = c.Int(nullable: false),
                    UserId = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.RoleId, t.UserId }, name: "PK_UserRoles")
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: false, name: "FK_UserRoles_Roles")
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false, name: "FK_UserRoles_Users")
                .Index(t => t.RoleId, name: "IX_UserRoles_RoleId")
                .Index(t => t.UserId, name: "IX_UserRoles_UserId");

            CreateTable(
                "dbo.BizObjects",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 255),
                })
                .PrimaryKey(t => t.Id, name: "PK_BizObjects");

            #endregion

            #region User SPs
            CreateStoredProcedure(
                "dbo.UserInsert",
                p => new
                {
                    UserName = p.String(maxLength: 100),
                    PasswordHash = p.String(maxLength: 68),
                    CreationDateUTC = p.DateTime(),
                    LockoutEndDateUtc = p.DateTime(),
                    AccessFailedCount = p.Int(),
                    LockoutEnabled = p.Boolean(),
                    TwoFactorEnabled = p.Boolean(),
                    Email = p.String(maxLength: 100),
                    EmailConfirmed = p.Boolean(),
                    SecurityStamp = p.String(),
                },
                    body: @"
SET NOCOUNT ON;

declare @tempUsers table
(
	UserId int,
	CreationDateUTC datetime,
	PasswordHash nvarchar(68)
)

INSERT [dbo].[Users]([UserName], [PasswordHash], [CreationDateUTC], [LockoutEndDateUtc], [AccessFailedCount], [LockoutEnabled], [TwoFactorEnabled], [Email], [EmailConfirmed], [SecurityStamp])
output inserted.id, inserted.CreationDateUTC, inserted.PasswordHash
into @tempUsers
VALUES (@UserName, @PasswordHash, @CreationDateUTC, @LockoutEndDateUtc, @AccessFailedCount, @LockoutEnabled, @TwoFactorEnabled, @Email, @EmailConfirmed, @SecurityStamp)

declare @userId int

select @userId = CAST(SCOPE_IDENTITY() AS INT)

insert into UserPasswordHistory(userId, createdby, creationDateUTC, passwordHash)
select UserId, '', CreationDateUTC, PasswordHash
from @tempUsers

select @userId as Id"
            );

            CreateStoredProcedure(
                "dbo.UserUpdate",
                p => new
                {
                    Id = p.Int(),
                    UserName = p.String(maxLength: 100),
                    PasswordHash = p.String(maxLength: 68),
                    CreationDateUTC = p.DateTime(),
                    LockoutEndDateUtc = p.DateTime(),
                    AccessFailedCount = p.Int(),
                    LockoutEnabled = p.Boolean(),
                    TwoFactorEnabled = p.Boolean(),
                    Email = p.String(maxLength: 100),
                    EmailConfirmed = p.Boolean(),
                    SecurityStamp = p.String(),
                },
                body:
                    @"
SET NOCOUNT ON;
UPDATE [dbo].[Users]
SET [UserName] = @UserName, 
[PasswordHash] = @PasswordHash, 
[CreationDateUTC] = @CreationDateUTC, 
[LockoutEndDateUtc] = @LockoutEndDateUtc, 
[AccessFailedCount] = @AccessFailedCount, 
[LockoutEnabled] = @LockoutEnabled, 
[TwoFactorEnabled] = @TwoFactorEnabled, 
[Email] = @Email, [EmailConfirmed] = @EmailConfirmed, 
[SecurityStamp] = @SecurityStamp
WHERE [Id] = @Id                      

insert into UserPasswordHistory(userId, createdby, creationDateUTC, passwordHash)
select Id, '', CreationDateUTC, PasswordHash
from Users
WHERE Id = @Id

select Id
from Users
where Id = @Id
"
            );

            CreateStoredProcedure(
                "dbo.UserDelete",
                p => new
                {
                    Id = p.Int(),
                },
                body:
                    @"
SET NOCOUNT ON;

DELETE dbo.Users
WHERE [Id] = @Id"
            );
            CreateStoredProcedure(
                "GetUsers",
                c => new
                {
                    Id = c.Int(defaultValueSql: "NULL"),
                    UserName = c.String(maxLength: 100, unicode: true, defaultValueSql: "NULL"),
                    Email = c.String(maxLength: 100, unicode: true, defaultValueSql: "NULL")
                },
   @"
SET NOCOUNT ON;

select Id, UserName, PasswordHash,CreationDateUTC,LockoutEndDateUtc,AccessFailedCount,LockoutEnabled,TwoFactorEnabled,Email, EmailConfirmed,SecurityStamp
from users
where (Id = @Id or @Id is null)
and (UserName = @UserName or @UserName is null)
and (Email = @Email or @Email is null)
");

            #endregion

            #region Role SPs
            CreateStoredProcedure(
    "dbo.RoleInsert",
    p => new
    {
        Name = p.String(maxLength: 100),
        CreationDateUTC = p.DateTime(),
    },
    body:
        @"
SET NOCOUNT ON;
INSERT [dbo].[Roles]([Name], CreationDateUTC)
output inserted.Id
VALUES (@Name, @CreationDateUTC)"
);

            CreateStoredProcedure(
                "dbo.RoleUpdate",
                p => new
                {
                    Id = p.Int(),
                    Name = p.String(maxLength: 100),
                    CreationDateUTC = p.DateTime(),
                },
                body:
                    @"
SET NOCOUNT ON;

UPDATE [dbo].[Roles]
SET [Name] = @Name
WHERE [Id] = @Id

select Id
from Roles
where Id = @Id"
            );

            CreateStoredProcedure(
                "dbo.RoleDelete",
                p => new
                {
                    Id = p.Int(),
                },
                body:
                    @"
SET NOCOUNT ON;

DELETE [dbo].[Roles]
WHERE ([Id] = @Id)"
            );

            CreateStoredProcedure(
                "GetRoles",
                c => new
                {
                    Id = c.Int(defaultValueSql: "NULL"),
                    RoleName = c.String(maxLength: 100, unicode: true, defaultValueSql: "NULL"),
                    NameContains = c.String(maxLength: 100, unicode: true, defaultValueSql: "NULL"),
                    RowsToSkip = c.Int(defaultValueSql: "10"),
                    RowsToTake = c.Int(defaultValueSql: "10")
                },
                @"
SET NOCOUNT ON;

WITH CTE AS (
    select Id, Name, CreationDateUTC
    from Roles
    where (Id = @Id or @Id is null)
    and (Name = @RoleName or @RoleName is null)
    and (Name like '%' + @NameContains + '%' or @NameContains is null)
), RecordsCount AS (SELECT Count(*) AS TotalRecords FROM CTE)

SELECT *
FROM CTE, RecordsCount
ORDER BY Name
OFFSET @RowsToSkip ROWS FETCH NEXT @RowsToTake ROWS ONLY
");
            #endregion

            CreateStoredProcedure(
                "GetUserPasswordHistory",
                c => new
                {
                    Id = c.Int(defaultValueSql: "NULL"),
                    UserId = c.Int(defaultValueSql: "NULL")
                },
   @"
SET NOCOUNT ON;

SELECT Id,UserId,PasswordHash,CreatedBy,CreationDateUTC
FROM UserPasswordHistory
where (Id = @Id or @Id is null)
and (UserId = @UserId or @UserId is null)
");
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.GetUserPasswordHistory");

            DropStoredProcedure("dbo.GetRoles");
            DropStoredProcedure("dbo.RoleDelete");
            DropStoredProcedure("dbo.RoleUpdate");
            DropStoredProcedure("dbo.RoleInsert");

            DropStoredProcedure("dbo.GetUsers");
            DropStoredProcedure("dbo.UserDelete");
            DropStoredProcedure("dbo.UserUpdate");
            DropStoredProcedure("dbo.UserInsert");

            DropForeignKey("dbo.UserPasswordHistory", "UserId", "dbo.Users");
            DropIndex("dbo.UserPasswordHistory", new[] { "UserId" });
            DropTable("dbo.Roles");
            DropTable("dbo.UserPasswordHistory");
            DropTable("dbo.Users");
        }
    }
}
