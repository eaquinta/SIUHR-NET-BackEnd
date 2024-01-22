namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario46 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Materiales", new[] { "Codigo" });
            AddColumn("dbo.Materiales", "Codigi", c => c.String(maxLength: 5));
            AddColumn("dbo.Materiales", "Ojo", c => c.String(maxLength: 1));
            AddColumn("dbo.Materiales", "AlternoDe", c => c.String(maxLength: 14));
            AddColumn("dbo.Materiales", "Bres", c => c.String(maxLength: 1));
            AddColumn("dbo.Materiales", "FechaCreacion", c => c.DateTime(nullable: false));
            AddColumn("dbo.Materiales", "Usuario", c => c.String(maxLength: 3));
            AddColumn("dbo.Materiales", "UsoBodega", c => c.String(maxLength: 5));
            AddColumn("dbo.Materiales", "UsoServicio", c => c.String(maxLength: 5));
            AlterColumn("dbo.Materiales", "RenglonCodigo", c => c.String(maxLength: 20));
            AlterColumn("dbo.Materiales", "GrupoCodigo", c => c.String(maxLength: 20));
            AlterColumn("dbo.Materiales", "Codigo", c => c.String(maxLength: 14));
            AlterColumn("dbo.Materiales", "UnidadMedida", c => c.String(maxLength: 10));
            AlterColumn("dbo.Materiales", "Descripcion", c => c.String(maxLength: 254));
            AlterColumn("dbo.Materiales", "Producto", c => c.String(maxLength: 30));
            AlterColumn("dbo.Materiales", "Precio", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Materiales", "Estatus", c => c.String(maxLength: 1));
            AlterColumn("dbo.Materiales", "SigesCodigo", c => c.String(maxLength: 20));
            CreateIndex("dbo.Materiales", new[] { "Codigo", "Codigi" }, unique: true, name: "IX_Material");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Materiales", "IX_Material");
            AlterColumn("dbo.Materiales", "SigesCodigo", c => c.String());
            AlterColumn("dbo.Materiales", "Estatus", c => c.String());
            AlterColumn("dbo.Materiales", "Precio", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Materiales", "Producto", c => c.String());
            AlterColumn("dbo.Materiales", "Descripcion", c => c.String());
            AlterColumn("dbo.Materiales", "UnidadMedida", c => c.String());
            AlterColumn("dbo.Materiales", "Codigo", c => c.String(maxLength: 15));
            AlterColumn("dbo.Materiales", "GrupoCodigo", c => c.String());
            AlterColumn("dbo.Materiales", "RenglonCodigo", c => c.String());
            DropColumn("dbo.Materiales", "UsoServicio");
            DropColumn("dbo.Materiales", "UsoBodega");
            DropColumn("dbo.Materiales", "Usuario");
            DropColumn("dbo.Materiales", "FechaCreacion");
            DropColumn("dbo.Materiales", "Bres");
            DropColumn("dbo.Materiales", "AlternoDe");
            DropColumn("dbo.Materiales", "Ojo");
            DropColumn("dbo.Materiales", "Codigi");
            CreateIndex("dbo.Materiales", "Codigo", unique: true);
        }
    }
}
