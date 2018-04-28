namespace MoneyChest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_datafilter_settings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DataFilters", "IsFilterVisible", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.DataFilters", "IsCategoryBranchSelection", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DataFilters", "IsCategoryBranchSelection");
            DropColumn("dbo.DataFilters", "IsFilterVisible");
        }
    }
}
