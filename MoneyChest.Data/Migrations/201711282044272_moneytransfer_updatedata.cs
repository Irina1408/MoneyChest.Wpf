namespace MoneyChest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class moneytransfer_updatedata : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "TakeCommissionFromReceiver", c => c.Boolean());
            AddColumn("dbo.EventHistories", "TakeCommissionFromReceiver", c => c.Boolean());
            AddColumn("dbo.MoneyTransfers", "TakeCommissionFromReceiver", c => c.Boolean(nullable: false));
            AddColumn("dbo.MoneyTransferHistories", "TakeCommissionFromReceiver", c => c.Boolean(nullable: false));
            AlterColumn("dbo.MoneyTransfers", "Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.Events", "TakeComissionFromReceiver");
            DropColumn("dbo.EventHistories", "TakeComissionFromReceiver");
            DropColumn("dbo.MoneyTransfers", "TakeComissionFromReceiver");
            DropColumn("dbo.MoneyTransferHistories", "TakeComissionFromReceiver");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MoneyTransferHistories", "TakeComissionFromReceiver", c => c.Boolean(nullable: false));
            AddColumn("dbo.MoneyTransfers", "TakeComissionFromReceiver", c => c.Boolean(nullable: false));
            AddColumn("dbo.EventHistories", "TakeComissionFromReceiver", c => c.Boolean());
            AddColumn("dbo.Events", "TakeComissionFromReceiver", c => c.Boolean());
            AlterColumn("dbo.MoneyTransfers", "Date", c => c.DateTime(nullable: false, storeType: "date"));
            DropColumn("dbo.MoneyTransferHistories", "TakeCommissionFromReceiver");
            DropColumn("dbo.MoneyTransfers", "TakeCommissionFromReceiver");
            DropColumn("dbo.EventHistories", "TakeCommissionFromReceiver");
            DropColumn("dbo.Events", "TakeCommissionFromReceiver");
        }
    }
}
