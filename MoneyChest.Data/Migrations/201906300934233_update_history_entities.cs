namespace MoneyChest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_history_entities : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RecordHistories", "IsAutoExecuted", c => c.Boolean(nullable: false));
            AddColumn("dbo.RecordHistories", "EventId", c => c.Int());
            AddColumn("dbo.MoneyTransferHistories", "IsAutoExecuted", c => c.Boolean(nullable: false));
            AddColumn("dbo.MoneyTransferHistories", "CategoryId", c => c.Int());
            AddColumn("dbo.MoneyTransferHistories", "EventId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MoneyTransferHistories", "EventId");
            DropColumn("dbo.MoneyTransferHistories", "CategoryId");
            DropColumn("dbo.MoneyTransferHistories", "IsAutoExecuted");
            DropColumn("dbo.RecordHistories", "EventId");
            DropColumn("dbo.RecordHistories", "IsAutoExecuted");
        }
    }
}
