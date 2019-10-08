namespace MoneyChest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_categories_filter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DataFilters", "AllCategories", c => c.Boolean(nullable: false));
            AddColumn("dbo.Limits", "AllCategories", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Limits", "AllCategories");
            DropColumn("dbo.DataFilters", "AllCategories");
        }
    }
}
