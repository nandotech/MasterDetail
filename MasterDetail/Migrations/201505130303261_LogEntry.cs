namespace MasterDetail.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LogEntry : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogEntries",
                c => new
                    {
                        LogEntryID = c.Long(nullable: false, identity: true),
                        LogDate = c.DateTime(nullable: false),
                        Logger = c.String(nullable: false, maxLength: 30),
                        LogLevel = c.String(nullable: false, maxLength: 5),
                        Thread = c.String(nullable: false, maxLength: 10),
                        EntityFormalNamePlural = c.String(nullable: false, maxLength: 30),
                        EntityKeyValue = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Message = c.String(nullable: false, maxLength: 256),
                        Exception = c.String(),
                    })
                .PrimaryKey(t => t.LogEntryID)
                .Index(t => new { t.EntityKeyValue, t.EntityFormalNamePlural }, name: "IDX_LogEntries_Entity");

            Sql("CREATE NONCLUSTERED INDEX IDX_LogEntries_LogDate ON LogEntries (LogDate DESC)");
        }
        
        public override void Down()
        {
            DropIndex("dbo.LogEntries", "IDX_LogEntries_Entity");
            DropTable("dbo.LogEntries");
        }
    }
}
