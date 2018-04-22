namespace MoneyChest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class make_storage_required_in_record : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Records", new[] { "StorageId" });
            AlterColumn("dbo.Records", "StorageId", c => c.Int(nullable: false));
            CreateIndex("dbo.Records", "StorageId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Records", new[] { "StorageId" });
            AlterColumn("dbo.Records", "StorageId", c => c.Int());
            CreateIndex("dbo.Records", "StorageId");
        }
    }
}
