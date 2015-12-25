namespace IndividualLoginExample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id, "PK_Users");

//            CreateStoredProcedure("UserInsert",
//                c => new
//                {
//                    UserName = c.String(maxLength:100, unicode:true)
//                },
//                @"
//SET NOCOUNT ON;

//insert into users(UserName)
//values(@UserName)

//select CAST(SCOPE_IDENTITY() AS INT) as Id
//");
        }
        
        public override void Down()
        {
            // DropStoredProcedure("UserInsert");
            DropTable("Users");
        }
    }
}
