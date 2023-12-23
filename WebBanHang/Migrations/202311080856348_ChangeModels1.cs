namespace WebBanHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeModels1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Product", "CreatedBy", c => c.String());
            AddColumn("dbo.tb_Product", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.tb_Product", "ModifiedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.tb_Product", "Modifiedby", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Product", "Modifiedby");
            DropColumn("dbo.tb_Product", "ModifiedDate");
            DropColumn("dbo.tb_Product", "CreatedDate");
            DropColumn("dbo.tb_Product", "CreatedBy");
        }
    }
}
