namespace IndividualLoginExample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUserRoles : DbMigration
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
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true, name: "FK_UserRoles_Roles")
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true, name: "FK_UserRoles_Users")
                .Index(t => t.RoleId, name: "IX_UserRoles_RoleId")
                .Index(t => t.UserId, name: "IX_UserRoles_UserId");

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
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.UserRoleDelete");
            DropStoredProcedure("dbo.UserRoleUpdate");
            DropStoredProcedure("dbo.UserRoleInsert");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropTable("dbo.UserRoles");
        }
    }
}
