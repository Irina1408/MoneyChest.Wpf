namespace MoneyChest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class storagegrouphistory_fix : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.StorageGroupHistories", "Name", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StorageGroupHistories", "Name", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
