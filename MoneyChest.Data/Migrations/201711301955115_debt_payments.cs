namespace MoneyChest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class debt_payments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DebtPenaltyHistories",
                c => new
                    {
                        ActionId = c.Int(nullable: false, identity: true),
                        ActionDateTime = c.DateTime(nullable: false),
                        ActionType = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Description = c.String(),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DebtId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ActionId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.DebtPenalties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Description = c.String(),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DebtId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Debts", t => t.DebtId, cascadeDelete: true)
                .Index(t => t.DebtId);
            
            AddColumn("dbo.Debts", "CurrencyExchangeRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Debts", "InitialFee", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Debts", "TakeInitialFeeFromStorage", c => c.Boolean(nullable: false));
            AddColumn("dbo.Debts", "PaymentType", c => c.Int(nullable: false));
            AddColumn("dbo.Debts", "FixedAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Debts", "InterestRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Debts", "MonthCount", c => c.Int(nullable: false));
            AddColumn("dbo.DebtHistories", "CurrencyExchangeRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.DebtHistories", "InitialFee", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.DebtHistories", "PaymentType", c => c.Int(nullable: false));
            AddColumn("dbo.DebtHistories", "FixedAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.DebtHistories", "InterestRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.DebtHistories", "MonthCount", c => c.Int(nullable: false));
            AddColumn("dbo.DebtHistories", "CategoryId", c => c.Int());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DebtPenalties", "DebtId", "dbo.Debts");
            DropForeignKey("dbo.DebtPenaltyHistories", "UserId", "dbo.Users");
            DropIndex("dbo.DebtPenalties", new[] { "DebtId" });
            DropIndex("dbo.DebtPenaltyHistories", new[] { "UserId" });
            DropColumn("dbo.DebtHistories", "CategoryId");
            DropColumn("dbo.DebtHistories", "MonthCount");
            DropColumn("dbo.DebtHistories", "InterestRate");
            DropColumn("dbo.DebtHistories", "FixedAmount");
            DropColumn("dbo.DebtHistories", "PaymentType");
            DropColumn("dbo.DebtHistories", "InitialFee");
            DropColumn("dbo.DebtHistories", "CurrencyExchangeRate");
            DropColumn("dbo.Debts", "MonthCount");
            DropColumn("dbo.Debts", "InterestRate");
            DropColumn("dbo.Debts", "FixedAmount");
            DropColumn("dbo.Debts", "PaymentType");
            DropColumn("dbo.Debts", "TakeInitialFeeFromStorage");
            DropColumn("dbo.Debts", "InitialFee");
            DropColumn("dbo.Debts", "CurrencyExchangeRate");
            DropTable("dbo.DebtPenalties");
            DropTable("dbo.DebtPenaltyHistories");
        }
    }
}
