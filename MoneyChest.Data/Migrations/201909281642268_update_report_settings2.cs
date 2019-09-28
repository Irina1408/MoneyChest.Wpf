namespace MoneyChest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_report_settings2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReportSetting", "IncludeActualTransactions", c => c.Boolean(nullable: false));
            AddColumn("dbo.ReportSetting", "IncludeFuturePlannedTransactions", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReportSetting", "IncludeFuturePlannedTransactions");
            DropColumn("dbo.ReportSetting", "IncludeActualTransactions");
        }
    }
}
