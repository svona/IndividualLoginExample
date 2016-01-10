namespace IndividualLoginExample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 100),
                        PasswordHash = c.String(nullable: false, maxLength: 68),
                        CreationDateUTC = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                })
                .PrimaryKey(t => t.Id, "PK_Users");

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
                .Index(t => t.UserId);
            
            CreateStoredProcedure(
                "dbo.UserInsert",
                p => new
                    {
                        UserName = p.String(maxLength: 100),
                        PasswordHash = p.String(maxLength: 68),
                        CreationDateUTC = p.DateTime(),
                    },
                    body: @"
SET NOCOUNT ON;

declare @tempUsers table
(
	UserId int,
	CreationDateUTC datetime,
	PasswordHash nvarchar(68)
)

INSERT [dbo].[Users]([UserName], [CreationDateUTC],PasswordHash)
output inserted.id, inserted.CreationDateUTC, inserted.PasswordHash
into @tempUsers
VALUES (@UserName, @CreationDateUTC,@PasswordHash)
    
insert into UserPasswordHistory(userId, createdby, creationDateUTC, passwordHash)
select UserId, '', CreationDateUTC, PasswordHash
from @tempUsers

select CAST(SCOPE_IDENTITY() AS INT) as Id"
            );
            
            CreateStoredProcedure(
                "dbo.UserUpdate",
                p => new
                    {
                        Id = p.Int(),
                        UserName = p.String(maxLength: 100),
                        PasswordHash = p.String(maxLength: 68),
                        CreationDateUTC = p.DateTime(),
                    },
                body:
                    @"
SET NOCOUNT ON;

UPDATE Users
SET [UserName] = @UserName,
PasswordHash=@PasswordHash,
[CreationDateUTC] = @CreationDateUTC
WHERE [Id] = @Id

insert into UserPasswordHistory(userId, createdby, creationDateUTC, passwordHash)
select Id, '', CreationDateUTC, PasswordHash
from Users
WHERE Id = @Id"
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
                    UserName = c.String(maxLength: 100, unicode: true, defaultValueSql: "NULL")
                },
   @"
SET NOCOUNT ON;

select Id, UserName, PasswordHash,CreationDateUTC
from users
where (Id = @Id or @Id is null)
and (UserName = @UserName or @UserName is null)
");

            CreateStoredProcedure(
                "GetUserPasswordHistory",
                c => new
                {
                    Id = c.Int(defaultValueSql: "NULL"),
                    UserId = c.Int(defaultValueSql: "NULL")
                },
   @"
SET NOCOUNT ON;

SELECT Id,UserId,CreatedBy,CreationDateUTC
FROM UserPasswordHistory
where (Id = @Id or @Id is null)
and (UserId = @UserId or @UserId is null)
");
        }

        public override void Down()
        {
            DropStoredProcedure("dbo.GetUserPasswordHistory");
            DropStoredProcedure("dbo.GetUsers");
            DropStoredProcedure("dbo.UserDelete");
            DropStoredProcedure("dbo.UserUpdate");
            DropStoredProcedure("dbo.UserInsert");
            DropForeignKey("dbo.UserPasswordHistory", "UserId", "dbo.Users");
            DropIndex("dbo.UserPasswordHistory", new[] { "UserId" });
            DropTable("dbo.UserPasswordHistory");
            DropTable("dbo.Users");
        }
    }
}
