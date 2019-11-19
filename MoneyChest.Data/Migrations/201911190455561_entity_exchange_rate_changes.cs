namespace MoneyChest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class entity_exchange_rate_changes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Debts", "SwappedCurrenciesRate", c => c.Boolean(nullable: false));
            AddColumn("dbo.CurrencyExchangeRates", "SwappedCurrencies", c => c.Boolean(nullable: false));
            AddColumn("dbo.DebtHistories", "SwappedCurrenciesRate", c => c.Boolean(nullable: false));
            AddColumn("dbo.Events", "SwappedCurrenciesRate", c => c.Boolean(nullable: false));
            AddColumn("dbo.Records", "SwappedCurrenciesRate", c => c.Boolean(nullable: false));
            AddColumn("dbo.MoneyTransfers", "SwappedCurrenciesRate", c => c.Boolean(nullable: false));
            AddColumn("dbo.EventHistories", "SwappedCurrenciesRate", c => c.Boolean(nullable: false));
            AddColumn("dbo.RecordHistories", "CurrencyExchangeRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.RecordHistories", "SwappedCurrenciesRate", c => c.Boolean(nullable: false));
            AddColumn("dbo.RecordHistories", "Commission", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.RecordHistories", "CommissionType", c => c.Int(nullable: false));
            AddColumn("dbo.MoneyTransferHistories", "SwappedCurrenciesRate", c => c.Boolean(nullable: false));
            AddColumn("dbo.MoneyTransferTemplates", "SwappedCurrenciesRate", c => c.Boolean(nullable: false));
            AddColumn("dbo.MoneyTransferTemplateHistories", "SwappedCurrenciesRate", c => c.Boolean(nullable: false));
            AddColumn("dbo.RecordTemplates", "SwappedCurrenciesRate", c => c.Boolean(nullable: false));
            AddColumn("dbo.RecordTemplateHistories", "SwappedCurrenciesRate", c => c.Boolean(nullable: false));
            AlterColumn("dbo.RecordHistories", "Date", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RecordHistories", "CategoryId", c => c.Int());
            AlterColumn("dbo.RecordHistories", "StorageId", c => c.Int(nullable: false));
            DropColumn("dbo.CurrencyExchangeRates", "IsFromSecond");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CurrencyExchangeRates", "IsFromSecond", c => c.Boolean(nullable: false));
            AlterColumn("dbo.RecordHistories", "StorageId", c => c.Int());
            AlterColumn("dbo.RecordHistories", "CategoryId", c => c.Int(nullable: false));
            AlterColumn("dbo.RecordHistories", "Date", c => c.DateTime(nullable: false, storeType: "date"));
            DropColumn("dbo.RecordTemplateHistories", "SwappedCurrenciesRate");
            DropColumn("dbo.RecordTemplates", "SwappedCurrenciesRate");
            DropColumn("dbo.MoneyTransferTemplateHistories", "SwappedCurrenciesRate");
            DropColumn("dbo.MoneyTransferTemplates", "SwappedCurrenciesRate");
            DropColumn("dbo.MoneyTransferHistories", "SwappedCurrenciesRate");
            DropColumn("dbo.RecordHistories", "CommissionType");
            DropColumn("dbo.RecordHistories", "Commission");
            DropColumn("dbo.RecordHistories", "SwappedCurrenciesRate");
            DropColumn("dbo.RecordHistories", "CurrencyExchangeRate");
            DropColumn("dbo.EventHistories", "SwappedCurrenciesRate");
            DropColumn("dbo.MoneyTransfers", "SwappedCurrenciesRate");
            DropColumn("dbo.Records", "SwappedCurrenciesRate");
            DropColumn("dbo.Events", "SwappedCurrenciesRate");
            DropColumn("dbo.DebtHistories", "SwappedCurrenciesRate");
            DropColumn("dbo.CurrencyExchangeRates", "SwappedCurrencies");
            DropColumn("dbo.Debts", "SwappedCurrenciesRate");
        }
    }
}
