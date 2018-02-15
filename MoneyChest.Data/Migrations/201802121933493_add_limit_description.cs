namespace MoneyChest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_limit_description : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Limits", "Description", c => c.String(maxLength: 1000));
            AddColumn("dbo.LimitHistories", "Description", c => c.String(maxLength: 1000));
            DropColumn("dbo.Limits", "LimitState");
            DropColumn("dbo.LimitHistories", "LimitState");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LimitHistories", "LimitState", c => c.Int(nullable: false));
            AddColumn("dbo.Limits", "LimitState", c => c.Int(nullable: false));
            DropColumn("dbo.LimitHistories", "Description");
            DropColumn("dbo.Limits", "Description");
        }
    }
}
