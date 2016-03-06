namespace MasterDetail.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CurrentWorker : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.WorkOrders", new[] { "CurrentWorkerId" });
            AlterColumn("dbo.WorkOrders", "CurrentWorkerId", c => c.String(maxLength: 128));
            CreateIndex("dbo.WorkOrders", "CurrentWorkerId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.WorkOrders", new[] { "CurrentWorkerId" });
            AlterColumn("dbo.WorkOrders", "CurrentWorkerId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.WorkOrders", "CurrentWorkerId");
        }
    }
}
