namespace MoneyChest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change_datafilter : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.DataFilters", "IsSingleCategorySelection");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DataFilters", "IsSingleCategorySelection", c => c.Boolean(nullable: false));
        }
    }
}
