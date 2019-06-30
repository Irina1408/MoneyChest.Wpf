namespace MoneyChest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renaming : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Records", "IsAutoExecuted", c => c.Boolean(nullable: false));
            AddColumn("dbo.MoneyTransfers", "IsAutoExecuted", c => c.Boolean(nullable: false));
            AddColumn("dbo.RecordHistories", "IsAutoExecuted", c => c.Boolean(nullable: false));
            AddColumn("dbo.MoneyTransferHistories", "IsAutoExecuted", c => c.Boolean(nullable: false));
            DropColumn("dbo.Records", "IsAutomaticallyExecuted");
            DropColumn("dbo.MoneyTransfers", "IsAutomaticallyExecuted");
            DropColumn("dbo.RecordHistories", "IsAutomaticallyExecuted");
            DropColumn("dbo.MoneyTransferHistories", "IsAutomaticallyExecuted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MoneyTransferHistories", "IsAutomaticallyExecuted", c => c.Boolean(nullable: false));
            AddColumn("dbo.RecordHistories", "IsAutomaticallyExecuted", c => c.Boolean(nullable: false));
            AddColumn("dbo.MoneyTransfers", "IsAutomaticallyExecuted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Records", "IsAutomaticallyExecuted", c => c.Boolean(nullable: false));
            DropColumn("dbo.MoneyTransferHistories", "IsAutoExecuted");
            DropColumn("dbo.RecordHistories", "IsAutoExecuted");
            DropColumn("dbo.MoneyTransfers", "IsAutoExecuted");
            DropColumn("dbo.Records", "IsAutoExecuted");
        }
    }
}
