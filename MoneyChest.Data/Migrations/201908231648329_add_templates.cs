namespace MoneyChest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_templates : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MoneyTransferTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .ForeignKey("dbo.Storages", t => t.StorageFromId)
                .ForeignKey("dbo.Storages", t => t.StorageToId)
                .Index(t => t.StorageFromId)
                .Index(t => t.StorageToId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.RecordTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
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
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .ForeignKey("dbo.Currencies", t => t.CurrencyId)
                .ForeignKey("dbo.Debts", t => t.DebtId)
                .ForeignKey("dbo.Storages", t => t.StorageId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.CurrencyId)
                .Index(t => t.StorageId)
                .Index(t => t.DebtId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RecordTemplates", "UserId", "dbo.Users");
            DropForeignKey("dbo.RecordTemplates", "StorageId", "dbo.Storages");
            DropForeignKey("dbo.RecordTemplates", "DebtId", "dbo.Debts");
            DropForeignKey("dbo.RecordTemplates", "CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.RecordTemplates", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.MoneyTransferTemplates", "StorageToId", "dbo.Storages");
            DropForeignKey("dbo.MoneyTransferTemplates", "StorageFromId", "dbo.Storages");
            DropForeignKey("dbo.MoneyTransferTemplates", "CategoryId", "dbo.Categories");
            DropIndex("dbo.RecordTemplates", new[] { "UserId" });
            DropIndex("dbo.RecordTemplates", new[] { "DebtId" });
            DropIndex("dbo.RecordTemplates", new[] { "StorageId" });
            DropIndex("dbo.RecordTemplates", new[] { "CurrencyId" });
            DropIndex("dbo.RecordTemplates", new[] { "CategoryId" });
            DropIndex("dbo.MoneyTransferTemplates", new[] { "CategoryId" });
            DropIndex("dbo.MoneyTransferTemplates", new[] { "StorageToId" });
            DropIndex("dbo.MoneyTransferTemplates", new[] { "StorageFromId" });
            DropTable("dbo.RecordTemplates");
            DropTable("dbo.MoneyTransferTemplates");
        }
    }
}
