namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario17 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.DespachosInventario", name: "DestinoId", newName: "DepartamentoId");
            RenameIndex(table: "dbo.DespachosInventario", name: "IX_DestinoId", newName: "IX_DepartamentoId");
            AddColumn("dbo.DespachosInventarioDetalle", "PacienteId", c => c.Int());
            AddColumn("dbo.Pacientes", "RefPac_NumHC", c => c.Decimal(precision: 18, scale: 0));
            AddColumn("dbo.Pacientes", "RefPac_NumHCAntiguo", c => c.Decimal(precision: 18, scale: 0));
            CreateIndex("dbo.DespachosInventarioDetalle", "PacienteId");
            AddForeignKey("dbo.DespachosInventarioDetalle", "PacienteId", "dbo.Pacientes", "PacienteId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DespachosInventarioDetalle", "PacienteId", "dbo.Pacientes");
            DropIndex("dbo.DespachosInventarioDetalle", new[] { "PacienteId" });
            DropColumn("dbo.Pacientes", "RefPac_NumHCAntiguo");
            DropColumn("dbo.Pacientes", "RefPac_NumHC");
            DropColumn("dbo.DespachosInventarioDetalle", "PacienteId");
            RenameIndex(table: "dbo.DespachosInventario", name: "IX_DepartamentoId", newName: "IX_DestinoId");
            RenameColumn(table: "dbo.DespachosInventario", name: "DepartamentoId", newName: "DestinoId");
        }
    }
}
