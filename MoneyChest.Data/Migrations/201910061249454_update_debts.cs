namespace MoneyChest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_debts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Debts", "OnlyInitialFee", c => c.Boolean(nullable: false));
            DropColumn("dbo.Debts", "TakeInitialFeeFromStorage");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Debts", "TakeInitialFeeFromStorage", c => c.Boolean(nullable: false));
            DropColumn("dbo.Debts", "OnlyInitialFee");
        }
    }
}
