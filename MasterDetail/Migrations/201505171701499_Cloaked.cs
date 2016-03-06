namespace MasterDetail.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cloaked : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "Cloaked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "Cloaked");
        }
    }
}
