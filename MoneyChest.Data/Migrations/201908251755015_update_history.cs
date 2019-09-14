namespace MoneyChest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_history : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MoneyTransferTemplateHistories", "Name", c => c.String(maxLength: 100));
            AddColumn("dbo.RecordTemplateHistories", "Name", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RecordTemplateHistories", "Name");
            DropColumn("dbo.MoneyTransferTemplateHistories", "Name");
        }
    }
}
