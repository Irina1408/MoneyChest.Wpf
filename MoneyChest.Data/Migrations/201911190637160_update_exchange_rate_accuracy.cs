namespace MoneyChest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_exchange_rate_accuracy : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Debts", "CurrencyExchangeRate", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.CurrencyExchangeRates", "Rate", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.DebtHistories", "CurrencyExchangeRate", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.Events", "CurrencyExchangeRate", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.Records", "CurrencyExchangeRate", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.MoneyTransfers", "CurrencyExchangeRate", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.EventHistories", "CurrencyExchangeRate", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.RecordHistories", "CurrencyExchangeRate", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.CurrencyExchangeRateHistories", "Rate", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.MoneyTransferHistories", "CurrencyExchangeRate", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.MoneyTransferTemplates", "CurrencyExchangeRate", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.MoneyTransferTemplateHistories", "CurrencyExchangeRate", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.RecordTemplates", "CurrencyExchangeRate", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.RecordTemplateHistories", "CurrencyExchangeRate", c => c.Decimal(nullable: false, precision: 20, scale: 4));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RecordTemplateHistories", "CurrencyExchangeRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.RecordTemplates", "CurrencyExchangeRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MoneyTransferTemplateHistories", "CurrencyExchangeRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MoneyTransferTemplates", "CurrencyExchangeRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MoneyTransferHistories", "CurrencyExchangeRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.CurrencyExchangeRateHistories", "Rate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.RecordHistories", "CurrencyExchangeRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.EventHistories", "CurrencyExchangeRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MoneyTransfers", "CurrencyExchangeRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Records", "CurrencyExchangeRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Events", "CurrencyExchangeRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.DebtHistories", "CurrencyExchangeRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.CurrencyExchangeRates", "Rate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Debts", "CurrencyExchangeRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
