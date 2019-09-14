namespace MoneyChest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_history_for_templates : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MoneyTransferTemplateHistories",
                c => new
                    {
                        ActionId = c.Int(nullable: false, identity: true),
                        ActionDateTime = c.DateTime(nullable: false),
                        ActionType = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                        Description = c.String(maxLength: 1000),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrencyExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Commission = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CommissionType = c.Int(nullable: false),
                        TakeCommissionFromReceiver = c.Boolean(nullable: false),
                        Remark = c.String(maxLength: 4000),
                        StorageFromId = c.Int(nullable: false),
                        StorageToId = c.Int(nullable: false),
                        CategoryId = c.Int(),
                    })
                .PrimaryKey(t => t.ActionId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.RecordTemplateHistories",
                c => new
                    {
                        ActionId = c.Int(nullable: false, identity: true),
                        ActionDateTime = c.DateTime(nullable: false),
                        ActionType = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                        Description = c.String(maxLength: 1000),
                        RecordType = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrencyExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Commission = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CommissionType = c.Int(nullable: false),
                        Remark = c.String(maxLength: 4000),
                        CategoryId = c.Int(),
                        CurrencyId = c.Int(nullable: false),
                        StorageId = c.Int(nullable: false),
                        DebtId = c.Int(),
                    })
                .PrimaryKey(t => t.ActionId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RecordTemplateHistories", "UserId", "dbo.Users");
            DropForeignKey("dbo.MoneyTransferTemplateHistories", "UserId", "dbo.Users");
            DropIndex("dbo.RecordTemplateHistories", new[] { "UserId" });
            DropIndex("dbo.MoneyTransferTemplateHistories", new[] { "UserId" });
            DropTable("dbo.RecordTemplateHistories");
            DropTable("dbo.MoneyTransferTemplateHistories");
        }
    }
}
