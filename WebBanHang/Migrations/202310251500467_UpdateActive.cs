namespace WebBanHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateActive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Category", "IsAdmin", c => c.Boolean(nullable: false));
            AddColumn("dbo.tb_News", "IsAdmin", c => c.Boolean(nullable: false));
            AddColumn("dbo.tb_Posts", "IsAdmin", c => c.Boolean(nullable: false));
            AddColumn("dbo.tb_Product", "IsAdmin", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Product", "IsAdmin");
            DropColumn("dbo.tb_Posts", "IsAdmin");
            DropColumn("dbo.tb_News", "IsAdmin");
            DropColumn("dbo.tb_Category", "IsAdmin");
        }
    }
}
