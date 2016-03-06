namespace MasterDetail.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String(maxLength: 15));
            AddColumn("dbo.AspNetUsers", "Address", c => c.String(maxLength: 30));
            AddColumn("dbo.AspNetUsers", "City", c => c.String(maxLength: 20));
            AddColumn("dbo.AspNetUsers", "State", c => c.String(maxLength: 2));
            AddColumn("dbo.AspNetUsers", "ZipCode", c => c.String(maxLength: 10));
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String(maxLength: 15));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String(nullable: false, maxLength: 15));
            DropColumn("dbo.AspNetUsers", "ZipCode");
            DropColumn("dbo.AspNetUsers", "State");
            DropColumn("dbo.AspNetUsers", "City");
            DropColumn("dbo.AspNetUsers", "Address");
            DropColumn("dbo.AspNetUsers", "LastName");
        }
    }
}
