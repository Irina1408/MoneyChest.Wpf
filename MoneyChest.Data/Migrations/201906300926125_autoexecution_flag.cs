namespace MoneyChest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class autoexecution_flag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Records", "IsAutoExecuted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Records", "EventId", c => c.Int());
            AddColumn("dbo.MoneyTransfers", "IsAutoExecuted", c => c.Boolean(nullable: false));
            AddColumn("dbo.MoneyTransfers", "EventId", c => c.Int());
            CreateIndex("dbo.Records", "EventId");
            CreateIndex("dbo.MoneyTransfers", "EventId");
            AddForeignKey("dbo.Records", "EventId", "dbo.Events", "Id");
            AddForeignKey("dbo.MoneyTransfers", "EventId", "dbo.Events", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MoneyTransfers", "EventId", "dbo.Events");
            DropForeignKey("dbo.Records", "EventId", "dbo.Events");
            DropIndex("dbo.MoneyTransfers", new[] { "EventId" });
            DropIndex("dbo.Records", new[] { "EventId" });
            DropColumn("dbo.MoneyTransfers", "EventId");
            DropColumn("dbo.MoneyTransfers", "IsAutoExecuted");
            DropColumn("dbo.Records", "EventId");
            DropColumn("dbo.Records", "IsAutoExecuted");
        }
    }
}
