namespace MasterDetail.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.CategoryId)
                .Index(t => t.CategoryName, unique: true, name: "AK_Category_CategoryName");
            
            CreateTable(
                "dbo.InventoryItems",
                c => new
                    {
                        InventoryItemId = c.Int(nullable: false, identity: true),
                        InventoryItemCode = c.String(nullable: false, maxLength: 15),
                        InventoryItemName = c.String(nullable: false, maxLength: 80),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.InventoryItemId)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.InventoryItemCode, unique: true, name: "AK_InventoryItem_InventoryItemCode")
                .Index(t => t.InventoryItemName, unique: true, name: "AK_InventoryItem_InventoryItemName")
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        AccountNumber = c.String(nullable: false, maxLength: 8),
                        CompanyName = c.String(nullable: false, maxLength: 30),
                        Address = c.String(nullable: false, maxLength: 30),
                        City = c.String(nullable: false, maxLength: 15),
                        State = c.String(nullable: false, maxLength: 2),
                        ZipCode = c.String(nullable: false, maxLength: 10),
                        Phone = c.String(maxLength: 15),
                    })
                .PrimaryKey(t => t.CustomerId)
                .Index(t => t.AccountNumber, unique: true, name: "AK_Customer_AccountNumber");
            
            CreateTable(
                "dbo.Labors",
                c => new
                    {
                        LaborId = c.Int(nullable: false, identity: true),
                        WorkOrderId = c.Int(nullable: false),
                        ServiceItemCode = c.String(nullable: false, maxLength: 15),
                        ServiceItemName = c.String(nullable: false, maxLength: 80),
                        LaborHours = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        //ExtendedPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Notes = c.String(maxLength: 140),
                    })
                .PrimaryKey(t => t.LaborId)
                .ForeignKey("dbo.WorkOrders", t => t.WorkOrderId, cascadeDelete: true)
                .Index(t => new { t.WorkOrderId, t.ServiceItemCode }, unique: true, name: "AK_Labor");

            Sql("ALTER TABLE dbo.Labors ADD ExtendedPrice AS CAST(LaborHours * Rate AS Decimal(18, 2))");
            
            CreateTable(
                "dbo.WorkOrders",
                c => new
                    {
                        WorkOrderId = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        OrderDateTime = c.DateTime(nullable: false, defaultValueSql: "GetDate()"),
                        TargetDateTime = c.DateTime(),
                        DropDeadDateTime = c.DateTime(),
                        Description = c.String(maxLength: 256),
                        WorkOrderStatus = c.Int(nullable: false),
                        //Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CertificationRequirements = c.String(maxLength: 120),
                    })
                .PrimaryKey(t => t.WorkOrderId)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);

            Sql(@"CREATE FUNCTION dbo.GetSumOfPartsAndLabor(@workOrderId INT)
                RETURNS DECIMAL(18, 2)
                AS
                BEGIN

                DECLARE @partsSum Decimal(18, 2);
                DECLARE @laborSum Decimal(18, 2);

                SELECT @partsSum = Sum(ExtendedPrice)
                FROM Parts
                WHERE WorkOrderId = @workOrderId;

                SELECT @laborSum = Sum(ExtendedPrice)
                FROM Labors
                WHERE WorkOrderId = @workOrderId;

                RETURN ISNULL(@partsSum, 0) + ISNULL(@laborSum, 0);
                END");

            Sql("ALTER TABLE dbo.WorkOrders ADD Total AS dbo.GetSumOfPartsAndLabor(WorkOrderId)");
            
            CreateTable(
                "dbo.Parts",
                c => new
                    {
                        PartId = c.Int(nullable: false, identity: true),
                        WorkOrderId = c.Int(nullable: false),
                        InventoryItemCode = c.String(nullable: false, maxLength: 15),
                        InventoryItemName = c.String(nullable: false, maxLength: 80),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        //ExtendedPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Notes = c.String(maxLength: 140),
                        IsInstalled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PartId)
                .ForeignKey("dbo.WorkOrders", t => t.WorkOrderId, cascadeDelete: true)
                .Index(t => new { t.WorkOrderId, t.InventoryItemCode }, unique: true, name: "AK_Part");

            Sql("ALTER TABLE dbo.Parts ADD ExtendedPrice AS CAST(Quantity * UnitPrice AS Decimal(18, 2))");
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.ServiceItems",
                c => new
                    {
                        ServiceItemId = c.Int(nullable: false, identity: true),
                        ServiceItemCode = c.String(nullable: false, maxLength: 15),
                        ServiceItemName = c.String(nullable: false, maxLength: 80),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ServiceItemId)
                .Index(t => t.ServiceItemCode, unique: true, name: "AK_ServiceItem_ServiceItemCode")
                .Index(t => t.ServiceItemName, unique: true, name: "AK_ServiceItem_ServiceItemName");
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Parts", "WorkOrderId", "dbo.WorkOrders");
            DropForeignKey("dbo.Labors", "WorkOrderId", "dbo.WorkOrders");
            DropForeignKey("dbo.WorkOrders", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.InventoryItems", "CategoryId", "dbo.Categories");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.ServiceItems", "AK_ServiceItem_ServiceItemName");
            DropIndex("dbo.ServiceItems", "AK_ServiceItem_ServiceItemCode");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Parts", "AK_Part");
            DropIndex("dbo.WorkOrders", new[] { "CustomerId" });
            DropIndex("dbo.Labors", "AK_Labor");
            DropIndex("dbo.Customers", "AK_Customer_AccountNumber");
            DropIndex("dbo.InventoryItems", new[] { "CategoryId" });
            DropIndex("dbo.InventoryItems", "AK_InventoryItem_InventoryItemName");
            DropIndex("dbo.InventoryItems", "AK_InventoryItem_InventoryItemCode");
            DropIndex("dbo.Categories", "AK_Category_CategoryName");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.ServiceItems");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Parts");
            DropTable("dbo.WorkOrders");
            DropTable("dbo.Labors");
            DropTable("dbo.Customers");
            DropTable("dbo.InventoryItems");
            DropTable("dbo.Categories");
        }
    }
}
