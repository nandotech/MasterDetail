namespace MasterDetail.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cascading : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.InventoryItems", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.WorkOrders", "CurrentWorkerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.WorkOrders", "CustomerId", "dbo.Customers");
            AddForeignKey("dbo.InventoryItems", "CategoryId", "dbo.Categories", "CategoryId");
            AddForeignKey("dbo.WorkOrders", "CurrentWorkerId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.WorkOrders", "CustomerId", "dbo.Customers", "CustomerId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkOrders", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.WorkOrders", "CurrentWorkerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.InventoryItems", "CategoryId", "dbo.Categories");
            AddForeignKey("dbo.WorkOrders", "CustomerId", "dbo.Customers", "CustomerId", cascadeDelete: true);
            AddForeignKey("dbo.WorkOrders", "CurrentWorkerId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.InventoryItems", "CategoryId", "dbo.Categories", "CategoryId", cascadeDelete: true);
        }
    }
}
