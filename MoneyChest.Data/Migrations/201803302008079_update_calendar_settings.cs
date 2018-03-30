namespace MoneyChest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_calendar_settings : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.DataFilterStorages", newName: "StorageDataFilters");
            RenameTable(name: "dbo.DataFilterCategories", newName: "CategoryDataFilters");
            DropForeignKey("dbo.StorageGroupCalendarSettings", "StorageGroup_Id", "dbo.StorageGroups");
            DropForeignKey("dbo.StorageGroupCalendarSettings", "CalendarSetting_UserId", "dbo.CalendarSettings");
            DropIndex("dbo.StorageGroupCalendarSettings", new[] { "StorageGroup_Id" });
            DropIndex("dbo.StorageGroupCalendarSettings", new[] { "CalendarSetting_UserId" });
            DropPrimaryKey("dbo.StorageDataFilters");
            DropPrimaryKey("dbo.CategoryDataFilters");
            AddColumn("dbo.CalendarSettings", "DataFilterId", c => c.Int(nullable: false));
            AddColumn("dbo.CalendarSettings", "PeriodFilterId", c => c.Int(nullable: false));
            AddColumn("dbo.CalendarSettings", "StorageGroup_Id", c => c.Int());
            AddPrimaryKey("dbo.StorageDataFilters", new[] { "Storage_Id", "DataFilter_Id" });
            AddPrimaryKey("dbo.CategoryDataFilters", new[] { "Category_Id", "DataFilter_Id" });
            CreateIndex("dbo.CalendarSettings", "DataFilterId");
            CreateIndex("dbo.CalendarSettings", "PeriodFilterId");
            CreateIndex("dbo.CalendarSettings", "StorageGroup_Id");
            AddForeignKey("dbo.CalendarSettings", "StorageGroup_Id", "dbo.StorageGroups", "Id");
            AddForeignKey("dbo.CalendarSettings", "DataFilterId", "dbo.DataFilters", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CalendarSettings", "PeriodFilterId", "dbo.PeriodFilters", "Id", cascadeDelete: true);
            DropColumn("dbo.CalendarSettings", "PeriodType");
            DropTable("dbo.StorageGroupCalendarSettings");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.StorageGroupCalendarSettings",
                c => new
                    {
                        StorageGroup_Id = c.Int(nullable: false),
                        CalendarSetting_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StorageGroup_Id, t.CalendarSetting_UserId });
            
            AddColumn("dbo.CalendarSettings", "PeriodType", c => c.Int(nullable: false));
            DropForeignKey("dbo.CalendarSettings", "PeriodFilterId", "dbo.PeriodFilters");
            DropForeignKey("dbo.CalendarSettings", "DataFilterId", "dbo.DataFilters");
            DropForeignKey("dbo.CalendarSettings", "StorageGroup_Id", "dbo.StorageGroups");
            DropIndex("dbo.CalendarSettings", new[] { "StorageGroup_Id" });
            DropIndex("dbo.CalendarSettings", new[] { "PeriodFilterId" });
            DropIndex("dbo.CalendarSettings", new[] { "DataFilterId" });
            DropPrimaryKey("dbo.CategoryDataFilters");
            DropPrimaryKey("dbo.StorageDataFilters");
            DropColumn("dbo.CalendarSettings", "StorageGroup_Id");
            DropColumn("dbo.CalendarSettings", "PeriodFilterId");
            DropColumn("dbo.CalendarSettings", "DataFilterId");
            AddPrimaryKey("dbo.CategoryDataFilters", new[] { "DataFilter_Id", "Category_Id" });
            AddPrimaryKey("dbo.StorageDataFilters", new[] { "DataFilter_Id", "Storage_Id" });
            CreateIndex("dbo.StorageGroupCalendarSettings", "CalendarSetting_UserId");
            CreateIndex("dbo.StorageGroupCalendarSettings", "StorageGroup_Id");
            AddForeignKey("dbo.StorageGroupCalendarSettings", "CalendarSetting_UserId", "dbo.CalendarSettings", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.StorageGroupCalendarSettings", "StorageGroup_Id", "dbo.StorageGroups", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.CategoryDataFilters", newName: "DataFilterCategories");
            RenameTable(name: "dbo.StorageDataFilters", newName: "DataFilterStorages");
        }
    }
}
