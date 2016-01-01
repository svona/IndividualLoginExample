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
                        UserName = c.String(maxLength: 100),
                        CreationDateUTC = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateStoredProcedure(
                "dbo.UserInsert",
                p => new
                {
                    UserName = p.String(maxLength: 100),
                    CreationDateUTC = p.DateTime(defaultValue: DateTime.UtcNow, defaultValueSql: "GETUTCNOW()")
                },
                body:
                    @"
SET NOCOUNT ON;

INSERT [dbo].[Users]([UserName], [CreationDateUTC])
VALUES (@UserName, @CreationDateUTC)

select CAST(SCOPE_IDENTITY() AS INT) as Id"
            );
            
            CreateStoredProcedure(
                "dbo.UserUpdate",
                p => new
                {
                    Id = p.Int(),
                    UserName = p.String(maxLength: 100),
                    CreationDateUTC = p.DateTime(defaultValue: DateTime.UtcNow, defaultValueSql: "GETUTCNOW()")
                },
                body:
                    @"
SET NOCOUNT ON;

UPDATE [dbo].[Users]
SET [UserName] = @UserName,
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

select Id, UserName, CreationDateUTC
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
