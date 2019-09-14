namespace MoneyChest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_template_name : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MoneyTransferTemplates", "Name", c => c.String(maxLength: 100));
            AddColumn("dbo.RecordTemplates", "Name", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RecordTemplates", "Name");
            DropColumn("dbo.MoneyTransferTemplates", "Name");
        }
    }
}
