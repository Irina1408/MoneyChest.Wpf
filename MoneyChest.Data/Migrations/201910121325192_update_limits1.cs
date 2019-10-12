namespace MoneyChest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_limits1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CalendarSettings", "ShowAllLimits", c => c.Boolean(nullable: false));
            DropColumn("dbo.CalendarSettings", "ShowLimits");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CalendarSettings", "ShowLimits", c => c.Boolean(nullable: false));
            DropColumn("dbo.CalendarSettings", "ShowAllLimits");
        }
    }
}
