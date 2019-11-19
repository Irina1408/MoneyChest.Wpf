namespace MoneyChest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class currency_exchange_rate_change : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CurrencyExchangeRates", "IsFromSecond", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CurrencyExchangeRates", "IsFromSecond");
        }
    }
}
