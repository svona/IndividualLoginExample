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
                .PrimaryKey(t => t.Id);

            CreateStoredProcedure(
                "dbo.UserInsert",
                p => new
                {
                    UserName = p.String(maxLength: 100),
                    PasswordHash = p.String(maxLength: 68),
                    CreationDateUTC = p.DateTime(defaultValue: DateTime.UtcNow)
                },
                body:
                    @"
SET NOCOUNT ON;

INSERT [dbo].[Users]([UserName], [CreationDateUTC],PasswordHash)
VALUES (@UserName, @CreationDateUTC,@PasswordHash)

select CAST(SCOPE_IDENTITY() AS INT) as Id"
            );

            CreateStoredProcedure(
                "dbo.UserUpdate",
                p => new
                {
                    Id = p.Int(),
                    UserName = p.String(maxLength: 100),
                    PasswordHash = p.String(maxLength: 68),
                    CreationDateUTC = p.DateTime(defaultValue: DateTime.UtcNow)
                },
                body:
                    @"
SET NOCOUNT ON;

UPDATE [dbo].[Users]
SET [UserName] = @UserName,
PasswordHash=@PasswordHash,
[CreationDateUTC] = @CreationDateUTC
WHERE ([Id] = @Id)"
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

DELETE [dbo].[Users]
WHERE ([Id] = @Id)"
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
        }

        public override void Down()
        {
            DropStoredProcedure("GetUsers");
            DropStoredProcedure("dbo.UserDelete");
            DropStoredProcedure("dbo.UserUpdate");
            DropStoredProcedure("dbo.UserInsert");
            DropTable("dbo.Users");
        }
    }
}
