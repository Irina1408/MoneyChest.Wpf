namespace MoneyChest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_limits : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Limits", new[] { "CategoryId" });
            CreateTable(
                "dbo.LimitCategories",
                c => new
                    {
                        LimitId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LimitId, t.CategoryId })
                .ForeignKey("dbo.Limits", t => t.LimitId, cascadeDelete: true)
                .Index(t => t.LimitId)
                .Index(t => t.CategoryId);

            DropForeignKey("dbo.Limits", "CategoryId", "dbo.Categories");
            DropColumn("dbo.Limits", "CategoryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Limits", "CategoryId", c => c.Int());
            DropForeignKey("dbo.LimitCategories", "LimitId", "dbo.Limits");
            DropIndex("dbo.LimitCategories", new[] { "CategoryId" });
            DropIndex("dbo.LimitCategories", new[] { "LimitId" });
            DropTable("dbo.LimitCategories");
            CreateIndex("dbo.Limits", "CategoryId");
        }
    }
}
