namespace ArcheageCraft.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addproductiontocraft : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Crafts", "Production", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Crafts", "Production");
        }
    }
}
