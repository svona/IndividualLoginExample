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

            #region User CRUD Stored Procedures
            CreateStoredProcedure("UserInsert",
                c => new
                {
                    UserName = c.String(maxLength: 100, unicode: true)
                },
                @"
SET NOCOUNT ON;

insert into users(UserName)
values(@UserName)

select CAST(SCOPE_IDENTITY() AS INT) as Id
");

            CreateStoredProcedure("UserUpdate", 
                c => new
                {
                    Id = c.Int(),
                    UserName = c.String(maxLength: 100, unicode: true)
                },
                @"
SET NOCOUNT ON;

update users
set userName = @UserName
where Id = @Id
");

            CreateStoredProcedure("UserDelete", 
                c => new
                {
                    Id = c.Int()
                },
                @"
SET NOCOUNT ON;

delete from users
where Id = @Id");

            CreateStoredProcedure("GetUsers", 
                c => new
                {
                    Id = c.Int(defaultValueSql: "NULL"),
                    UserName = c.String(maxLength: 100, unicode: true, defaultValueSql: "NULL")
                },
                @"
SET NOCOUNT ON;

select Id, UserName
from users
where (Id = @Id or @Id is null)
and (UserName = @UserName or @UserName is null)
");
            #endregion
        }

        public override void Down()
        {
            DropStoredProcedure("GetUsers");
            DropStoredProcedure("UserDelete");
            DropStoredProcedure("UserUpdate");
            DropStoredProcedure("UserInsert");
            DropTable("Users");
        }
    }
}
