namespace MasterDetail.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Widget : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Widgets",
                c => new
                    {
                        WidgetId = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 256),
                        MainBusCode = c.String(maxLength: 12),
                        TestPassDateTime = c.DateTime(),
                        WidgetStatus = c.Int(nullable: false),
                        CurrentWorkerId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.WidgetId)
                .ForeignKey("dbo.AspNetUsers", t => t.CurrentWorkerId)
                .Index(t => t.CurrentWorkerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Widgets", "CurrentWorkerId", "dbo.AspNetUsers");
            DropIndex("dbo.Widgets", new[] { "CurrentWorkerId" });
            DropTable("dbo.Widgets");
        }
    }
}
