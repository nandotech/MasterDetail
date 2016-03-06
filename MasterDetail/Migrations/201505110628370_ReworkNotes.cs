namespace MasterDetail.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReworkNotes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkOrders", "ReworkNotes", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkOrders", "ReworkNotes");
        }
    }
}
