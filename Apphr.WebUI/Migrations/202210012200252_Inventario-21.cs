namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario21 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DespachosInventario", "DocumentoReferencia", c => c.String());
            AddColumn("dbo.EgresosInventario", "DocumentoReferencia", c => c.String());
            AddColumn("dbo.IngresosInventario", "DocumentoReferencia", c => c.String());
            DropColumn("dbo.DespachosInventario", "DocumentoRefenrecia");
            DropColumn("dbo.EgresosInventario", "DocumentoRefenrecia");
            DropColumn("dbo.IngresosInventario", "DocumentoRefenrecia");
        }
        
        public override void Down()
        {
            AddColumn("dbo.IngresosInventario", "DocumentoRefenrecia", c => c.String());
            AddColumn("dbo.EgresosInventario", "DocumentoRefenrecia", c => c.String());
            AddColumn("dbo.DespachosInventario", "DocumentoRefenrecia", c => c.String());
            DropColumn("dbo.IngresosInventario", "DocumentoReferencia");
            DropColumn("dbo.EgresosInventario", "DocumentoReferencia");
            DropColumn("dbo.DespachosInventario", "DocumentoReferencia");
        }
    }
}
