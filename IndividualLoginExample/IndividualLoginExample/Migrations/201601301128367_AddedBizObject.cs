namespace IndividualLoginExample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedBizObject : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BizObjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id, name:"PK_BizObjects");
            
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
            DropTable("dbo.BizObjects");
        }
    }
}
