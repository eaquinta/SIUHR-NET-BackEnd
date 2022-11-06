namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario02 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Proveedores",
                c => new
                    {
                        ProveedorId = c.Int(nullable: false, identity: true),
                        Nit = c.String(nullable: false, maxLength: 10),
                        Descripcion = c.String(nullable: false, maxLength: 40),
                        Direccion = c.String(nullable: false, maxLength: 40),
                        Telefono = c.String(maxLength: 19),
                        Contacto = c.String(maxLength: 30),
                        DiasCredito = c.Int(),
                        Email = c.String(maxLength: 25),
                        Banco1 = c.String(maxLength: 20),
                        Cuenta1 = c.String(maxLength: 20),
                        Banco2 = c.String(maxLength: 20),
                        Cuenta2 = c.String(maxLength: 20),
                        Banco3 = c.String(maxLength: 20),
                        Cuenta3 = c.String(maxLength: 20),
                        PersonId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.ProveedorId)
                .ForeignKey("dbo.Personas", t => t.PersonId)
                .Index(t => t.Nit, unique: true)
                .Index(t => t.PersonId);
            
            CreateIndex("dbo.MovimientoInventarios", "ProveedorId");
            AddForeignKey("dbo.MovimientoInventarios", "ProveedorId", "dbo.Proveedores", "ProveedorId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MovimientoInventarios", "ProveedorId", "dbo.Proveedores");
            DropForeignKey("dbo.Proveedores", "PersonId", "dbo.Personas");
            DropIndex("dbo.Proveedores", new[] { "PersonId" });
            DropIndex("dbo.Proveedores", new[] { "Nit" });
            DropIndex("dbo.MovimientoInventarios", new[] { "ProveedorId" });
            DropTable("dbo.Proveedores");
        }
    }
}
