namespace MoneyChest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_calendar_settings1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CalendarSettings", "ShowSettings", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.CalendarSettings", "ShowAllTransactionsPerDay", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.CalendarSettings", "MaxTransactionsCountPerDay", c => c.Int(nullable: false, defaultValue: 3));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CalendarSettings", "MaxTransactionsCountPerDay");
            DropColumn("dbo.CalendarSettings", "ShowAllTransactionsPerDay");
            DropColumn("dbo.CalendarSettings", "ShowSettings");
        }
    }
}
