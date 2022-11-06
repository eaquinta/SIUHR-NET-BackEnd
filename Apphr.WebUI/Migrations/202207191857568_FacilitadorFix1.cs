namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FacilitadorFix1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Facilitadors", "PersonId", "dbo.Personas");
            DropForeignKey("dbo.PacienteEventos", "FacilitadorId", "dbo.Facilitadors");
            DropIndex("dbo.Facilitadors", "IX_PersonaUnique");
            DropIndex("dbo.PacienteEventos", new[] { "FacilitadorId" });
            CreateTable(
                "dbo.Empleados",
                c => new
                    {
                        EncargadoId = c.Int(nullable: false),
                        Codigo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EncargadoId)
                .ForeignKey("dbo.Personas", t => t.EncargadoId)
                .Index(t => t.EncargadoId);
            
            DropTable("dbo.Facilitadors");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Facilitadors",
                c => new
                    {
                        FacilitadorId = c.Int(nullable: false, identity: true),
                        PersonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FacilitadorId);
            
            DropForeignKey("dbo.Empleados", "EncargadoId", "dbo.Personas");
            DropIndex("dbo.Empleados", new[] { "EncargadoId" });
            DropTable("dbo.Empleados");
            CreateIndex("dbo.PacienteEventos", "FacilitadorId");
            CreateIndex("dbo.Facilitadors", "PersonId", unique: true, name: "IX_PersonaUnique");
            AddForeignKey("dbo.PacienteEventos", "FacilitadorId", "dbo.Facilitadors", "FacilitadorId");
            AddForeignKey("dbo.Facilitadors", "PersonId", "dbo.Personas", "PersonId", cascadeDelete: true);
        }
    }
}
