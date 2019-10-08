namespace MoneyChest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class minor_settings_changes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Limits", "IncludeWithoutCategory", c => c.Boolean(nullable: false));
            AddColumn("dbo.TransactionsSettings", "ShowTemplates", c => c.Boolean(nullable: false));
            DropColumn("dbo.DataFilters", "ShowTemplates");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DataFilters", "ShowTemplates", c => c.Boolean(nullable: false));
            DropColumn("dbo.TransactionsSettings", "ShowTemplates");
            DropColumn("dbo.Limits", "IncludeWithoutCategory");
        }
    }
}
