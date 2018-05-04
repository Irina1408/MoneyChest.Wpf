namespace MoneyChest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_settings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CalendarSettings", "ShowAllStorages", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.GeneralSettings", "AccentColor", c => c.String());
            AddColumn("dbo.GeneralSettings", "ThemeColor", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GeneralSettings", "ThemeColor");
            DropColumn("dbo.GeneralSettings", "AccentColor");
            DropColumn("dbo.CalendarSettings", "ShowAllStorages");
        }
    }
}
