namespace ArcheageCraft.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class prices : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Prices", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Prices", "UserId", c => c.Int(nullable: false));
        }
    }
}
