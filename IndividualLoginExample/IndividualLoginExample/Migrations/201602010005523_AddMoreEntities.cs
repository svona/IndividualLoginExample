namespace IndividualLoginExample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMoreEntities : DbMigration
    {
        public override void Up()
        {
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

            CreateStoredProcedure(
                "dbo.UserRoleInsert",
                p => new
                    {
                        RoleId = p.Int(),
                        UserId = p.Int(),
                    },
                body:
                    @"
SET NOCOUNT ON;

INSERT [dbo].[UserRoles]([RoleId], [UserId])
VALUES (@RoleId, @UserId)"
            );

            CreateStoredProcedure(
                "dbo.UserRoleUpdate",
                p => new
                    {
                        RoleId = p.Int(),
                        UserId = p.Int(),
                    },
                body:
                    @"
SET NOCOUNT ON;
RETURN"
            );

            CreateStoredProcedure(
                "dbo.UserRoleDelete",
                p => new
                    {
                        RoleId = p.Int(),
                        UserId = p.Int(),
                    },
                body:
                    @"
SET NOCOUNT ON;

DELETE [dbo].[UserRoles]
WHERE [RoleId] = @RoleId AND [UserId] = @UserId"
            );

            CreateStoredProcedure(
                "dbo.BizObjectInsert",
                p => new
                    {
                        Name = p.String(maxLength: 255),
                    },
                body:
                    @"
SET NOCOUNT ON;
INSERT [dbo].[BizObjects]([Name])
output inserted.Id
VALUES (@Name)"
            );

            CreateStoredProcedure(
                "dbo.BizObjectUpdate",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 255),
                    },
                body:
                    @"
SET NOCOUNT ON;

UPDATE [dbo].[BizObjects]
SET [Name] = @Name
WHERE ([Id] = @Id)

select Id
from BizObjects
where Id = @Id
"
            );

            CreateStoredProcedure(
                "dbo.BizObjectDelete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"
SET NOCOUNT ON;

DELETE [dbo].[BizObjects]
WHERE ([Id] = @Id)"
            );

        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.BizObjectDelete");
            DropStoredProcedure("dbo.BizObjectUpdate");
            DropStoredProcedure("dbo.BizObjectInsert");
            DropStoredProcedure("dbo.UserRoleDelete");
            DropStoredProcedure("dbo.UserRoleUpdate");
            DropStoredProcedure("dbo.UserRoleInsert");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropTable("dbo.BizObjects");
            DropTable("dbo.UserRoles");
        }
    }
}
