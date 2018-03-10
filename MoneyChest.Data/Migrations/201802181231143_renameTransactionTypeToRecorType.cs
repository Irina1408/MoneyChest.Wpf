namespace MoneyChest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renameTransactionTypeToRecorType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "RecordType", c => c.Int());
            AddColumn("dbo.CategoryHistories", "RecordType", c => c.Int());
            AddColumn("dbo.Events", "RecordType", c => c.Int());
            AddColumn("dbo.EventHistories", "RecordType", c => c.Int());
            AddColumn("dbo.Records", "RecordType", c => c.Int(nullable: false));
            AddColumn("dbo.RecordHistories", "RecordType", c => c.Int(nullable: false));
            AddColumn("dbo.RecordsViewFilter", "RecordType", c => c.Int());
            DropColumn("dbo.Categories", "TransactionType");
            DropColumn("dbo.CategoryHistories", "TransactionType");
            DropColumn("dbo.Events", "TransactionType");
            DropColumn("dbo.EventHistories", "TransactionType");
            DropColumn("dbo.Records", "TransactionType");
            DropColumn("dbo.RecordHistories", "TransactionType");
            DropColumn("dbo.RecordsViewFilter", "TransactionType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RecordsViewFilter", "TransactionType", c => c.Int());
            AddColumn("dbo.RecordHistories", "TransactionType", c => c.Int(nullable: false));
            AddColumn("dbo.Records", "TransactionType", c => c.Int(nullable: false));
            AddColumn("dbo.EventHistories", "TransactionType", c => c.Int());
            AddColumn("dbo.Events", "TransactionType", c => c.Int());
            AddColumn("dbo.CategoryHistories", "TransactionType", c => c.Int());
            AddColumn("dbo.Categories", "TransactionType", c => c.Int());
            DropColumn("dbo.RecordsViewFilter", "RecordType");
            DropColumn("dbo.RecordHistories", "RecordType");
            DropColumn("dbo.Records", "RecordType");
            DropColumn("dbo.EventHistories", "RecordType");
            DropColumn("dbo.Events", "RecordType");
            DropColumn("dbo.CategoryHistories", "RecordType");
            DropColumn("dbo.Categories", "RecordType");
        }
    }
}
