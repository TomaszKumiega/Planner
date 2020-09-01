namespace ToDoList.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Days",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        EventType = c.Int(nullable: false),
                        EventDifficulty = c.Int(nullable: false),
                        Karma = c.Int(nullable: false),
                        DateTime = c.DateTime(),
                        Day_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Days", t => t.Day_Id)
                .Index(t => t.Day_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Karma = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "Day_Id", "dbo.Days");
            DropIndex("dbo.Events", new[] { "Day_Id" });
            DropTable("dbo.Users");
            DropTable("dbo.Events");
            DropTable("dbo.Days");
        }
    }
}
