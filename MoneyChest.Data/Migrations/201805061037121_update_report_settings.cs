namespace MoneyChest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_report_settings : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ReportSettingCategories", "ReportSetting_UserId", "dbo.ReportSetting");
            DropForeignKey("dbo.ReportSettingCategories", "Category_Id", "dbo.Categories");
            DropIndex("dbo.ReportSettingCategories", new[] { "ReportSetting_UserId" });
            DropIndex("dbo.ReportSettingCategories", new[] { "Category_Id" });
            AddColumn("dbo.ReportSetting", "DataFilterId", c => c.Int(nullable: false));
            AddColumn("dbo.ReportSetting", "PeriodFilterId", c => c.Int(nullable: false));
            AddColumn("dbo.ReportSetting", "Category_Id", c => c.Int());
            CreateIndex("dbo.ReportSetting", "DataFilterId");
            CreateIndex("dbo.ReportSetting", "PeriodFilterId");
            CreateIndex("dbo.ReportSetting", "Category_Id");
            AddForeignKey("dbo.ReportSetting", "DataFilterId", "dbo.DataFilters", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ReportSetting", "PeriodFilterId", "dbo.PeriodFilters", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ReportSetting", "Category_Id", "dbo.Categories", "Id");
            DropColumn("dbo.ReportSetting", "IncludeRecordsWithoutCategory");
            DropColumn("dbo.ReportSetting", "AllCategories");
            DropColumn("dbo.ReportSetting", "PeriodFilterType");
            DropColumn("dbo.ReportSetting", "DateFrom");
            DropColumn("dbo.ReportSetting", "DateUntil");
            DropTable("dbo.ReportSettingCategories");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ReportSettingCategories",
                c => new
                    {
                        ReportSetting_UserId = c.Int(nullable: false),
                        Category_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ReportSetting_UserId, t.Category_Id });
            
            AddColumn("dbo.ReportSetting", "DateUntil", c => c.DateTime(storeType: "date"));
            AddColumn("dbo.ReportSetting", "DateFrom", c => c.DateTime(storeType: "date"));
            AddColumn("dbo.ReportSetting", "PeriodFilterType", c => c.Int(nullable: false));
            AddColumn("dbo.ReportSetting", "AllCategories", c => c.Boolean(nullable: false));
            AddColumn("dbo.ReportSetting", "IncludeRecordsWithoutCategory", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.ReportSetting", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.ReportSetting", "PeriodFilterId", "dbo.PeriodFilters");
            DropForeignKey("dbo.ReportSetting", "DataFilterId", "dbo.DataFilters");
            DropIndex("dbo.ReportSetting", new[] { "Category_Id" });
            DropIndex("dbo.ReportSetting", new[] { "PeriodFilterId" });
            DropIndex("dbo.ReportSetting", new[] { "DataFilterId" });
            DropColumn("dbo.ReportSetting", "Category_Id");
            DropColumn("dbo.ReportSetting", "PeriodFilterId");
            DropColumn("dbo.ReportSetting", "DataFilterId");
            CreateIndex("dbo.ReportSettingCategories", "Category_Id");
            CreateIndex("dbo.ReportSettingCategories", "ReportSetting_UserId");
            AddForeignKey("dbo.ReportSettingCategories", "Category_Id", "dbo.Categories", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ReportSettingCategories", "ReportSetting_UserId", "dbo.ReportSetting", "UserId", cascadeDelete: true);
        }
    }
}
