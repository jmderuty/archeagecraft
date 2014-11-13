namespace ArcheageCraft.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Items", "Profession_ProfessionId", "dbo.Professions");
            DropIndex("dbo.Items", new[] { "Profession_ProfessionId" });
            CreateTable(
                "dbo.CraftItems",
                c => new
                    {
                        CraftItemId = c.Int(nullable: false, identity: true),
                        CraftId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CraftItemId)
                .ForeignKey("dbo.Crafts", t => t.CraftId, cascadeDelete: false)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: false)
                .Index(t => t.CraftId)
                .Index(t => t.ItemId);
            
            DropColumn("dbo.Items", "Profession_ProfessionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Items", "Profession_ProfessionId", c => c.Int());
            DropForeignKey("dbo.CraftItems", "ItemId", "dbo.Items");
            DropForeignKey("dbo.CraftItems", "CraftId", "dbo.Crafts");
            DropIndex("dbo.CraftItems", new[] { "ItemId" });
            DropIndex("dbo.CraftItems", new[] { "CraftId" });
            DropTable("dbo.CraftItems");
            CreateIndex("dbo.Items", "Profession_ProfessionId");
            AddForeignKey("dbo.Items", "Profession_ProfessionId", "dbo.Professions", "ProfessionId");
        }
    }
}
