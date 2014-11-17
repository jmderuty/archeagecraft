namespace ArcheageCraft.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ItemCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "Category", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "Category");
        }
    }
}
