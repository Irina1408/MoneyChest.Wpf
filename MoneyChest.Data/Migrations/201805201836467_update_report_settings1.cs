namespace MoneyChest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_report_settings1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReportSetting", "ShowSettings", c => c.Boolean(nullable: false));
            AddColumn("dbo.ReportSetting", "ChartType", c => c.Int(nullable: false));
            AddColumn("dbo.ReportSetting", "Sorting", c => c.Int(nullable: false));
            AddColumn("dbo.ReportSetting", "ShowLegend", c => c.Boolean(nullable: false));
            AddColumn("dbo.ReportSetting", "ShowValue", c => c.Boolean(nullable: false));
            AddColumn("dbo.ReportSetting", "PieChartInnerRadius", c => c.Int(nullable: false));
            AddColumn("dbo.ReportSetting", "PieChartDetailsDepth", c => c.Int(nullable: false));
            AddColumn("dbo.ReportSetting", "BarChartView", c => c.Int(nullable: false));
            AddColumn("dbo.ReportSetting", "BarChartSection", c => c.Int(nullable: false));
            AddColumn("dbo.ReportSetting", "BarChartSectionPeriod", c => c.Int(nullable: false));
            AddColumn("dbo.ReportSetting", "BarChartDetail", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ReportSetting", "DataType", c => c.Int(nullable: false));
            DropColumn("dbo.ReportSetting", "ReportType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ReportSetting", "ReportType", c => c.Int(nullable: false));
            AlterColumn("dbo.ReportSetting", "DataType", c => c.Int());
            DropColumn("dbo.ReportSetting", "BarChartDetail");
            DropColumn("dbo.ReportSetting", "BarChartSectionPeriod");
            DropColumn("dbo.ReportSetting", "BarChartSection");
            DropColumn("dbo.ReportSetting", "BarChartView");
            DropColumn("dbo.ReportSetting", "PieChartDetailsDepth");
            DropColumn("dbo.ReportSetting", "PieChartInnerRadius");
            DropColumn("dbo.ReportSetting", "ShowValue");
            DropColumn("dbo.ReportSetting", "ShowLegend");
            DropColumn("dbo.ReportSetting", "Sorting");
            DropColumn("dbo.ReportSetting", "ChartType");
            DropColumn("dbo.ReportSetting", "ShowSettings");
        }
    }
}
