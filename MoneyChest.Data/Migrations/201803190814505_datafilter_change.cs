namespace MoneyChest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datafilter_change : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.DataFilters", "AllCategories");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DataFilters", "AllCategories", c => c.Boolean(nullable: false));
        }
    }
}
