namespace MasterDetail.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PercentComplete : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Labors", "PercentComplete", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Labors", "PercentComplete");
        }
    }
}
