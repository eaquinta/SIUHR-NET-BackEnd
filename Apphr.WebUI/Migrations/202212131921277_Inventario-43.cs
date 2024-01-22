namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario43 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ControlAbastecimiento", "MaterialId", "dbo.Materiales");
            DropPrimaryKey("dbo.ControlAbastecimiento");
            AddPrimaryKey("dbo.ControlAbastecimiento", "MaterialId");
            AddForeignKey("dbo.ControlAbastecimiento", "MaterialId", "dbo.Materiales", "MaterialId");
            DropColumn("dbo.ControlAbastecimiento", "DestinoId");
            DropColumn("dbo.ControlAbastecimiento", "Inicial");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ControlAbastecimiento", "Inicial", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.ControlAbastecimiento", "DestinoId", c => c.Int(nullable: false));
            DropForeignKey("dbo.ControlAbastecimiento", "MaterialId", "dbo.Materiales");
            DropPrimaryKey("dbo.ControlAbastecimiento");
            AddPrimaryKey("dbo.ControlAbastecimiento", new[] { "DestinoId", "MaterialId" });
            AddForeignKey("dbo.ControlAbastecimiento", "MaterialId", "dbo.Materiales", "MaterialId", cascadeDelete: false);
        }
    }
}
