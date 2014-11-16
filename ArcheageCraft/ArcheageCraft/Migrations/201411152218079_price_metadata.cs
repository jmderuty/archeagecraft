namespace ArcheageCraft.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class price_metadata : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Prices", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.Prices", "UserName", c => c.String());
            AddColumn("dbo.Prices", "Comment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Prices", "Comment");
            DropColumn("dbo.Prices", "UserName");
            DropColumn("dbo.Prices", "UserId");
        }
    }
}
