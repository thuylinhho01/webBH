namespace WebBanHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateActive1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Category", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.tb_News", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.tb_Posts", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.tb_Product", "IsActive", c => c.Boolean(nullable: false));
            DropColumn("dbo.tb_Category", "IsAdmin");
            DropColumn("dbo.tb_News", "IsAdmin");
            DropColumn("dbo.tb_Posts", "IsAdmin");
            DropColumn("dbo.tb_Product", "IsAdmin");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tb_Product", "IsAdmin", c => c.Boolean(nullable: false));
            AddColumn("dbo.tb_Posts", "IsAdmin", c => c.Boolean(nullable: false));
            AddColumn("dbo.tb_News", "IsAdmin", c => c.Boolean(nullable: false));
            AddColumn("dbo.tb_Category", "IsAdmin", c => c.Boolean(nullable: false));
            DropColumn("dbo.tb_Product", "IsActive");
            DropColumn("dbo.tb_Posts", "IsActive");
            DropColumn("dbo.tb_News", "IsActive");
            DropColumn("dbo.tb_Category", "IsActive");
        }
    }
}
