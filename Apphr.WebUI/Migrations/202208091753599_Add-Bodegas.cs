namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBodegas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bodegas",
                c => new
                    {
                        BodegaId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Descripcion = c.String(),
                        Procedencia = c.String(),
                    })
                .PrimaryKey(t => t.BodegaId);
            
            AddColumn("dbo.Proveedor", "ProveedorId", c => c.Int(nullable: false));
            CreateIndex("dbo.Proveedor", "Nit", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Proveedor", new[] { "Nit" });
            DropColumn("dbo.Proveedor", "ProveedorId");
            DropTable("dbo.Bodegas");
        }
    }
}
