namespace MoneyChest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class save_ShowTemplates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DataFilters", "ShowTemplates", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DataFilters", "ShowTemplates");
        }
    }
}
