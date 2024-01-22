namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario41 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ControlAbastecimiento", "Inicial", c => c.Decimal(precision: 18, scale: 2));
            CreateIndex("dbo.ControlAbastecimiento", "MaterialId");
            AddForeignKey("dbo.ControlAbastecimiento", "MaterialId", "dbo.Materiales", "MaterialId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ControlAbastecimiento", "MaterialId", "dbo.Materiales");
            DropIndex("dbo.ControlAbastecimiento", new[] { "MaterialId" });
            DropColumn("dbo.ControlAbastecimiento", "Inicial");
        }
    }
}
