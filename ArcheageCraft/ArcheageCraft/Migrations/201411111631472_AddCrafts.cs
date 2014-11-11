namespace ArcheageCraft.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCrafts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Crafts",
                c => new
                    {
                        CraftId = c.Int(nullable: false, identity: true),
                        LaborCost = c.Int(nullable: false),
                        ProfessionId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CraftId)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .ForeignKey("dbo.Professions", t => t.ProfessionId, cascadeDelete: true)
                .Index(t => t.ProfessionId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ItemId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        MerchantCost = c.Int(nullable: false),
                        VocationBadgeCost = c.Int(nullable: false),
                        Profession_ProfessionId = c.Int(),
                    })
                .PrimaryKey(t => t.ItemId)
                .ForeignKey("dbo.Professions", t => t.Profession_ProfessionId)
                .Index(t => t.Profession_ProfessionId);
            
            CreateTable(
                "dbo.Prices",
                c => new
                    {
                        PriceId = c.Int(nullable: false, identity: true),
                        Value = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PriceId)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.Professions",
                c => new
                    {
                        ProfessionId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ProfessionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Crafts", "ProfessionId", "dbo.Professions");
            DropForeignKey("dbo.Crafts", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Items", "Profession_ProfessionId", "dbo.Professions");
            DropForeignKey("dbo.Prices", "ItemId", "dbo.Items");
            DropIndex("dbo.Prices", new[] { "ItemId" });
            DropIndex("dbo.Items", new[] { "Profession_ProfessionId" });
            DropIndex("dbo.Crafts", new[] { "ItemId" });
            DropIndex("dbo.Crafts", new[] { "ProfessionId" });
            DropTable("dbo.Professions");
            DropTable("dbo.Prices");
            DropTable("dbo.Items");
            DropTable("dbo.Crafts");
        }
    }
}
