namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario47 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Empleados", "EncargadoId", "dbo.Personas");
            DropForeignKey("dbo.Facilitadores", "FacilitadorId", "dbo.Personas");
            DropIndex("dbo.Empleados", new[] { "EncargadoId" });
            DropIndex("dbo.Facilitadores", new[] { "FacilitadorId" });
            CreateTable(
                "dbo.ORTAjusteInventarios",
                c => new
                    {
                        AjusteInventarioId = c.Long(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        Observacion = c.String(),
                        Lineas = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedByUser = c.Int(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedByUser = c.Int(),
                    })
                .PrimaryKey(t => t.AjusteInventarioId);
            
            CreateTable(
                "dbo.ORTCirujanos",
                c => new
                    {
                        CirujanoId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(maxLength: 100),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedByUser = c.Int(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedByUser = c.Int(),
                    })
                .PrimaryKey(t => t.CirujanoId);
            
            CreateTable(
                "dbo.ORTDespachoInventarios",
                c => new
                    {
                        DespachoInventarioId = c.Long(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        Observacion = c.String(),
                        Lineas = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedByUser = c.Int(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedByUser = c.Int(),
                    })
                .PrimaryKey(t => t.DespachoInventarioId);
            
            CreateTable(
                "dbo.ORTDevoluciones",
                c => new
                    {
                        DevolucionId = c.Long(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        Observacion = c.String(),
                        ProveedorId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedByUser = c.Int(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedByUser = c.Int(),
                    })
                .PrimaryKey(t => t.DevolucionId)
                .ForeignKey("dbo.Proveedores", t => t.ProveedorId, cascadeDelete: true)
                .Index(t => t.ProveedorId);
            
            CreateTable(
                "dbo.ORTHojasGasto",
                c => new
                    {
                        HojaGastoId = c.Long(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        FechaOperacion = c.DateTime(nullable: false),
                        HojaControlSala = c.String(maxLength: 10),
                        Observacion = c.String(),
                        AuxiliarEnfermeriaInstrumentista = c.String(maxLength: 100),
                        AuxiliarEnfermeriaCirculante = c.String(maxLength: 100),
                        ServicioId = c.Int(nullable: false),
                        CirujanoId = c.Int(nullable: false),
                        PacienteId = c.Long(),
                        Cama = c.String(maxLength: 25),
                        Lineas = c.Int(nullable: false),
                        DespachoInventarioId = c.Long(),
                        DespachoInventarioFecha = c.DateTime(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedByUser = c.Int(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedByUser = c.Int(),
                    })
                .PrimaryKey(t => t.HojaGastoId)
                .ForeignKey("dbo.ORTCirujanos", t => t.CirujanoId, cascadeDelete: true)
                .ForeignKey("dbo.ORTPacientes", t => t.PacienteId)
                .ForeignKey("dbo.Servicios", t => t.ServicioId, cascadeDelete: true)
                .Index(t => t.ServicioId)
                .Index(t => t.CirujanoId)
                .Index(t => t.PacienteId);
            
            CreateTable(
                "dbo.ORTPacientes",
                c => new
                    {
                        PacienteId = c.Long(nullable: false, identity: true),
                        Nombre = c.String(maxLength: 250),
                        FechaNacimiento = c.DateTime(nullable: false),
                        RefPac_NumHC = c.Long(),
                        RefPac_NumHCAntiguo = c.Long(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedByUser = c.Int(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedByUser = c.Int(),
                    })
                .PrimaryKey(t => t.PacienteId);
            
            CreateTable(
                "dbo.ORTIngresoInventarios",
                c => new
                    {
                        IngresoInventarioId = c.Long(nullable: false, identity: true),
                        OrdenCompraId = c.Long(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        Observacion = c.String(),
                        Lineas = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedByUser = c.Int(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedByUser = c.Int(),
                    })
                .PrimaryKey(t => t.IngresoInventarioId)
                .ForeignKey("dbo.ORTOrdenesCompra", t => t.OrdenCompraId, cascadeDelete: true)
                .Index(t => t.OrdenCompraId);
            
            CreateTable(
                "dbo.ORTOrdenesCompra",
                c => new
                    {
                        OrdenCompraId = c.Long(nullable: false, identity: true),
                        Numero = c.Int(nullable: false),
                        Anio = c.Int(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        Lineas = c.Int(nullable: false),
                        Observacion = c.String(),
                        SolicitudPedidoId = c.Long(),
                        ProveedorId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedByUser = c.Int(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedByUser = c.Int(),
                    })
                .PrimaryKey(t => t.OrdenCompraId)
                .ForeignKey("dbo.Proveedores", t => t.ProveedorId, cascadeDelete: true)
                .ForeignKey("dbo.ORTSolicitudesPedido", t => t.SolicitudPedidoId)
                .Index(t => new { t.Numero, t.Anio }, unique: true, name: "IX_Numero")
                .Index(t => t.SolicitudPedidoId)
                .Index(t => t.ProveedorId);
            
            CreateTable(
                "dbo.ORTSolicitudesPedido",
                c => new
                    {
                        SolicitudPedidoId = c.Long(nullable: false, identity: true),
                        OrdenCompraId = c.Long(),
                        NumeroOC = c.Int(),
                        FechaOC = c.DateTime(),
                        Numero = c.Int(nullable: false),
                        Anio = c.Int(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        Lineas = c.Int(nullable: false),
                        TipoPrioridad = c.String(maxLength: 3),
                        TipoEvento = c.String(maxLength: 2),
                        Solicito = c.String(maxLength: 100),
                        Elaboro = c.String(maxLength: 100),
                        JefeDepartamento = c.String(maxLength: 100),
                        Gerente = c.String(maxLength: 100),
                        Director = c.String(maxLength: 100),
                        Observacion = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedByUser = c.Int(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedByUser = c.Int(),
                    })
                .PrimaryKey(t => t.SolicitudPedidoId)
                .Index(t => new { t.Numero, t.Anio }, unique: true, name: "IX_Numero");
            
            CreateTable(
                "dbo.ORTMovimientos",
                c => new
                    {
                        MovimientoId = c.Long(nullable: false, identity: true),
                        Tipo = c.String(maxLength: 3),
                        Documento = c.Long(nullable: false),
                        DocumentoReferencia = c.String(maxLength: 25),
                        Linea = c.Int(nullable: false),
                        SolicitudPedidoId = c.Long(),
                        OrdenCompraId = c.Long(),
                        PacienteId = c.Long(),
                        CirujanoId = c.Int(),
                        MaterialId = c.Int(nullable: false),
                        ProveedorId = c.Int(),
                        BodegaId = c.Int(),
                        Cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Fecha = c.DateTime(nullable: false),
                        NoHojaControl = c.Int(),
                        DevolucionId = c.Long(),
                        HojaGastoId = c.Long(),
                        Devolver = c.Boolean(nullable: false),
                        Devuleto = c.Boolean(nullable: false),
                        ServicioId = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedByUser = c.Int(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedByUser = c.Int(),
                    })
                .PrimaryKey(t => t.MovimientoId)
                .ForeignKey("dbo.Bodegas", t => t.BodegaId)
                .ForeignKey("dbo.ORTCirujanos", t => t.CirujanoId)
                .ForeignKey("dbo.ORTDevoluciones", t => t.DevolucionId)
                .ForeignKey("dbo.ORTHojasGasto", t => t.HojaGastoId)
                .ForeignKey("dbo.Materiales", t => t.MaterialId, cascadeDelete: true)
                .ForeignKey("dbo.ORTOrdenesCompra", t => t.OrdenCompraId)
                .ForeignKey("dbo.ORTPacientes", t => t.PacienteId)
                .ForeignKey("dbo.Proveedores", t => t.ProveedorId)
                .ForeignKey("dbo.Servicios", t => t.ServicioId)
                .ForeignKey("dbo.ORTSolicitudesPedido", t => t.SolicitudPedidoId)
                .Index(t => t.SolicitudPedidoId)
                .Index(t => t.OrdenCompraId)
                .Index(t => t.PacienteId)
                .Index(t => t.CirujanoId)
                .Index(t => t.MaterialId)
                .Index(t => t.ProveedorId)
                .Index(t => t.BodegaId)
                .Index(t => t.DevolucionId)
                .Index(t => t.HojaGastoId)
                .Index(t => t.ServicioId);
            
            AddColumn("dbo.AccessRules", "CreatedByUser", c => c.Int(nullable: false));
            AddColumn("dbo.AccessRules", "LastModifiedByUser", c => c.Int());
            AddColumn("dbo.AjustesInventario", "CreatedByUser", c => c.Int(nullable: false));
            AddColumn("dbo.AjustesInventario", "LastModifiedByUser", c => c.Int());
            AddColumn("dbo.AjustesInventarioDetalle", "CreatedByUser", c => c.Int(nullable: false));
            AddColumn("dbo.AjustesInventarioDetalle", "LastModifiedByUser", c => c.Int());
            AddColumn("dbo.Bodegas", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Bodegas", "CreatedByUser", c => c.Int(nullable: false));
            AddColumn("dbo.Bodegas", "LastModifiedDate", c => c.DateTime());
            AddColumn("dbo.Bodegas", "LastModifiedByUser", c => c.Int());
            AddColumn("dbo.Materiales", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Materiales", "CreatedByUser", c => c.Int(nullable: false));
            AddColumn("dbo.Materiales", "LastModifiedDate", c => c.DateTime());
            AddColumn("dbo.Materiales", "LastModifiedByUser", c => c.Int());
            AddColumn("dbo.Personas", "CreatedByUser", c => c.Int(nullable: false));
            AddColumn("dbo.Personas", "LastModifiedByUser", c => c.Int());
            AddColumn("dbo.Proveedores", "CreatedByUser", c => c.Int(nullable: false));
            AddColumn("dbo.Proveedores", "LastModifiedByUser", c => c.Int());
            AddColumn("dbo.AppAuditLogs", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.AppAuditLogs", "Url", c => c.String());
            AddColumn("dbo.AppAuditLogs", "Method", c => c.String(maxLength: 10));
            AddColumn("dbo.AppEntityLogs", "CreatedByUser", c => c.Int(nullable: false));
            AddColumn("dbo.DespachosInventario", "CreatedByUser", c => c.Int(nullable: false));
            AddColumn("dbo.DespachosInventario", "LastModifiedByUser", c => c.Int());
            AddColumn("dbo.DespachosInventarioDetalle", "CreatedByUser", c => c.Int(nullable: false));
            AddColumn("dbo.DespachosInventarioDetalle", "LastModifiedByUser", c => c.Int());
            AddColumn("dbo.Encargados", "CreatedByUser", c => c.Int(nullable: false));
            AddColumn("dbo.Encargados", "LastModifiedByUser", c => c.Int());
            AddColumn("dbo.IngresosInventario", "CreatedByUser", c => c.Int(nullable: false));
            AddColumn("dbo.IngresosInventario", "LastModifiedByUser", c => c.Int());
            AddColumn("dbo.IngresosInvnetarioDetalle", "CreatedByUser", c => c.Int(nullable: false));
            AddColumn("dbo.IngresosInvnetarioDetalle", "LastModifiedByUser", c => c.Int());
            AddColumn("dbo.MovimientosAbastecimiento", "CreatedByUser", c => c.Int(nullable: false));
            AddColumn("dbo.MovimientosAbastecimiento", "LastModifiedByUser", c => c.Int());
            AddColumn("dbo.OrdenCompra", "CreatedByUser", c => c.Int(nullable: false));
            AddColumn("dbo.OrdenCompra", "LastModifiedByUser", c => c.Int());
            AddColumn("dbo.OrdenCompraDetalle", "CreatedByUser", c => c.Int(nullable: false));
            AddColumn("dbo.OrdenCompraDetalle", "LastModifiedByUser", c => c.Int());
            AddColumn("dbo.Servicios", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Servicios", "CreatedByUser", c => c.Int(nullable: false));
            AddColumn("dbo.Servicios", "LastModifiedDate", c => c.DateTime());
            AddColumn("dbo.Servicios", "LastModifiedByUser", c => c.Int());
            AddColumn("dbo.PacienteEventos", "CreatedByUser", c => c.Int(nullable: false));
            AddColumn("dbo.PacienteEventos", "LastModifiedByUser", c => c.Int());
            AddColumn("dbo.RegistrosMedicos", "CreatedByUser", c => c.Int(nullable: false));
            AddColumn("dbo.RegistrosMedicos", "LastModifiedByUser", c => c.Int());
            AddColumn("dbo.PacienteEventoTraslados", "CreatedByUser", c => c.Int(nullable: false));
            AddColumn("dbo.PacienteEventoTraslados", "LastModifiedByUser", c => c.Int());
            AddColumn("dbo.SHDExistenciaBodega", "CreatedByUser", c => c.Int(nullable: false));
            AddColumn("dbo.SHDExistenciaBodega", "LastModifiedByUser", c => c.Int());
            AddColumn("dbo.SHDExistenciasLote", "CreatedByUser", c => c.Int(nullable: false));
            AddColumn("dbo.SHDExistenciasLote", "LastModifiedByUser", c => c.Int());
            AddColumn("dbo.SHDExistenciaTotal", "CreatedByUser", c => c.Int(nullable: false));
            AddColumn("dbo.SHDExistenciaTotal", "LastModifiedByUser", c => c.Int());
            AddColumn("dbo.SHDMovimientosInventario", "CreatedByUser", c => c.Int(nullable: false));
            AddColumn("dbo.SHDMovimientosInventario", "LastModifiedByUser", c => c.Int());
            AddColumn("dbo.SolicitudDespacho", "CreatedByUser", c => c.Int(nullable: false));
            AddColumn("dbo.SolicitudDespacho", "LastModifiedByUser", c => c.Int());
            AddColumn("dbo.SolicitudDespachoDetalle", "CreatedByUser", c => c.Int(nullable: false));
            AddColumn("dbo.SolicitudDespachoDetalle", "LastModifiedByUser", c => c.Int());
            AddColumn("dbo.SolicitudPedido", "CreatedByUser", c => c.Int(nullable: false));
            AddColumn("dbo.SolicitudPedido", "LastModifiedByUser", c => c.Int());
            AddColumn("dbo.SolicitudPedidoDetalle", "CreatedByUser", c => c.Int(nullable: false));
            AddColumn("dbo.SolicitudPedidoDetalle", "LastModifiedByUser", c => c.Int());
            AddColumn("dbo.SolicitudMaterialesSala", "CreatedByUser", c => c.Int(nullable: false));
            AddColumn("dbo.SolicitudMaterialesSala", "LastModifiedByUser", c => c.Int());
            AddColumn("dbo.SolicitudMaterialesSalaDetalle", "CreatedByUser", c => c.Int(nullable: false));
            AddColumn("dbo.SolicitudMaterialesSalaDetalle", "LastModifiedByUser", c => c.Int());
            DropColumn("dbo.AccessRules", "CreatedBy");
            DropColumn("dbo.AccessRules", "LastModifiedBy");
            DropColumn("dbo.AjustesInventario", "CreatedBy");
            DropColumn("dbo.AjustesInventario", "LastModifiedBy");
            DropColumn("dbo.AjustesInventarioDetalle", "CreatedBy");
            DropColumn("dbo.AjustesInventarioDetalle", "LastModifiedBy");
            DropColumn("dbo.Personas", "CreatedBy");
            DropColumn("dbo.Personas", "LastModifiedBy");
            DropColumn("dbo.Proveedores", "CreatedBy");
            DropColumn("dbo.Proveedores", "LastModifiedBy");
            DropColumn("dbo.AppAuditLogs", "UserName");
            DropColumn("dbo.AppAuditLogs", "Client");
            DropColumn("dbo.AppEntityLogs", "Created_by");
            DropColumn("dbo.DespachosInventario", "CreatedBy");
            DropColumn("dbo.DespachosInventario", "LastModifiedBy");
            DropColumn("dbo.DespachosInventarioDetalle", "CreatedBy");
            DropColumn("dbo.DespachosInventarioDetalle", "LastModifiedBy");
            DropColumn("dbo.Encargados", "CreatedBy");
            DropColumn("dbo.Encargados", "LastModifiedBy");
            DropColumn("dbo.IngresosInventario", "CreatedBy");
            DropColumn("dbo.IngresosInventario", "LastModifiedBy");
            DropColumn("dbo.IngresosInvnetarioDetalle", "CreatedBy");
            DropColumn("dbo.IngresosInvnetarioDetalle", "LastModifiedBy");
            DropColumn("dbo.MovimientosAbastecimiento", "CreatedBy");
            DropColumn("dbo.MovimientosAbastecimiento", "LastModifiedBy");
            DropColumn("dbo.OrdenCompra", "CreatedBy");
            DropColumn("dbo.OrdenCompra", "LastModifiedBy");
            DropColumn("dbo.OrdenCompraDetalle", "CreatedBy");
            DropColumn("dbo.OrdenCompraDetalle", "LastModifiedBy");
            DropColumn("dbo.PacienteEventos", "CreatedBy");
            DropColumn("dbo.PacienteEventos", "LastModifiedBy");
            DropColumn("dbo.RegistrosMedicos", "CreatedBy");
            DropColumn("dbo.RegistrosMedicos", "LastModifiedBy");
            DropColumn("dbo.PacienteEventoTraslados", "CreatedBy");
            DropColumn("dbo.PacienteEventoTraslados", "LastModifiedBy");
            DropColumn("dbo.SHDExistenciaBodega", "CreatedBy");
            DropColumn("dbo.SHDExistenciaBodega", "LastModifiedBy");
            DropColumn("dbo.SHDExistenciasLote", "CreatedBy");
            DropColumn("dbo.SHDExistenciasLote", "LastModifiedBy");
            DropColumn("dbo.SHDExistenciaTotal", "CreatedBy");
            DropColumn("dbo.SHDExistenciaTotal", "LastModifiedBy");
            DropColumn("dbo.SHDMovimientosInventario", "CreatedBy");
            DropColumn("dbo.SHDMovimientosInventario", "LastModifiedBy");
            DropColumn("dbo.SolicitudDespacho", "CreatedBy");
            DropColumn("dbo.SolicitudDespacho", "LastModifiedBy");
            DropColumn("dbo.SolicitudDespachoDetalle", "CreatedBy");
            DropColumn("dbo.SolicitudDespachoDetalle", "LastModifiedBy");
            DropColumn("dbo.SolicitudPedido", "CreatedBy");
            DropColumn("dbo.SolicitudPedido", "LastModifiedBy");
            DropColumn("dbo.SolicitudPedidoDetalle", "CreatedBy");
            DropColumn("dbo.SolicitudPedidoDetalle", "LastModifiedBy");
            DropColumn("dbo.SolicitudMaterialesSala", "CreatedBy");
            DropColumn("dbo.SolicitudMaterialesSala", "LastModifiedBy");
            DropColumn("dbo.SolicitudMaterialesSalaDetalle", "CreatedBy");
            DropColumn("dbo.SolicitudMaterialesSalaDetalle", "LastModifiedBy");
            DropTable("dbo.Empleados");
            DropTable("dbo.Facilitadores");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Facilitadores",
                c => new
                    {
                        FacilitadorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FacilitadorId);
            
            CreateTable(
                "dbo.Empleados",
                c => new
                    {
                        EncargadoId = c.Int(nullable: false),
                        Codigo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EncargadoId);
            
            AddColumn("dbo.SolicitudMaterialesSalaDetalle", "LastModifiedBy", c => c.String());
            AddColumn("dbo.SolicitudMaterialesSalaDetalle", "CreatedBy", c => c.String());
            AddColumn("dbo.SolicitudMaterialesSala", "LastModifiedBy", c => c.String());
            AddColumn("dbo.SolicitudMaterialesSala", "CreatedBy", c => c.String());
            AddColumn("dbo.SolicitudPedidoDetalle", "LastModifiedBy", c => c.String());
            AddColumn("dbo.SolicitudPedidoDetalle", "CreatedBy", c => c.String());
            AddColumn("dbo.SolicitudPedido", "LastModifiedBy", c => c.String());
            AddColumn("dbo.SolicitudPedido", "CreatedBy", c => c.String());
            AddColumn("dbo.SolicitudDespachoDetalle", "LastModifiedBy", c => c.String());
            AddColumn("dbo.SolicitudDespachoDetalle", "CreatedBy", c => c.String());
            AddColumn("dbo.SolicitudDespacho", "LastModifiedBy", c => c.String());
            AddColumn("dbo.SolicitudDespacho", "CreatedBy", c => c.String());
            AddColumn("dbo.SHDMovimientosInventario", "LastModifiedBy", c => c.String());
            AddColumn("dbo.SHDMovimientosInventario", "CreatedBy", c => c.String());
            AddColumn("dbo.SHDExistenciaTotal", "LastModifiedBy", c => c.String());
            AddColumn("dbo.SHDExistenciaTotal", "CreatedBy", c => c.String());
            AddColumn("dbo.SHDExistenciasLote", "LastModifiedBy", c => c.String());
            AddColumn("dbo.SHDExistenciasLote", "CreatedBy", c => c.String());
            AddColumn("dbo.SHDExistenciaBodega", "LastModifiedBy", c => c.String());
            AddColumn("dbo.SHDExistenciaBodega", "CreatedBy", c => c.String());
            AddColumn("dbo.PacienteEventoTraslados", "LastModifiedBy", c => c.String());
            AddColumn("dbo.PacienteEventoTraslados", "CreatedBy", c => c.String());
            AddColumn("dbo.RegistrosMedicos", "LastModifiedBy", c => c.String());
            AddColumn("dbo.RegistrosMedicos", "CreatedBy", c => c.String());
            AddColumn("dbo.PacienteEventos", "LastModifiedBy", c => c.String());
            AddColumn("dbo.PacienteEventos", "CreatedBy", c => c.String());
            AddColumn("dbo.OrdenCompraDetalle", "LastModifiedBy", c => c.String());
            AddColumn("dbo.OrdenCompraDetalle", "CreatedBy", c => c.String());
            AddColumn("dbo.OrdenCompra", "LastModifiedBy", c => c.String());
            AddColumn("dbo.OrdenCompra", "CreatedBy", c => c.String());
            AddColumn("dbo.MovimientosAbastecimiento", "LastModifiedBy", c => c.String());
            AddColumn("dbo.MovimientosAbastecimiento", "CreatedBy", c => c.String());
            AddColumn("dbo.IngresosInvnetarioDetalle", "LastModifiedBy", c => c.String());
            AddColumn("dbo.IngresosInvnetarioDetalle", "CreatedBy", c => c.String());
            AddColumn("dbo.IngresosInventario", "LastModifiedBy", c => c.String());
            AddColumn("dbo.IngresosInventario", "CreatedBy", c => c.String());
            AddColumn("dbo.Encargados", "LastModifiedBy", c => c.String());
            AddColumn("dbo.Encargados", "CreatedBy", c => c.String());
            AddColumn("dbo.DespachosInventarioDetalle", "LastModifiedBy", c => c.String());
            AddColumn("dbo.DespachosInventarioDetalle", "CreatedBy", c => c.String());
            AddColumn("dbo.DespachosInventario", "LastModifiedBy", c => c.String());
            AddColumn("dbo.DespachosInventario", "CreatedBy", c => c.String());
            AddColumn("dbo.AppEntityLogs", "Created_by", c => c.String(nullable: false));
            AddColumn("dbo.AppAuditLogs", "Client", c => c.String());
            AddColumn("dbo.AppAuditLogs", "UserName", c => c.String(nullable: false));
            AddColumn("dbo.Proveedores", "LastModifiedBy", c => c.String());
            AddColumn("dbo.Proveedores", "CreatedBy", c => c.String());
            AddColumn("dbo.Personas", "LastModifiedBy", c => c.String());
            AddColumn("dbo.Personas", "CreatedBy", c => c.String());
            AddColumn("dbo.AjustesInventarioDetalle", "LastModifiedBy", c => c.String());
            AddColumn("dbo.AjustesInventarioDetalle", "CreatedBy", c => c.String());
            AddColumn("dbo.AjustesInventario", "LastModifiedBy", c => c.String());
            AddColumn("dbo.AjustesInventario", "CreatedBy", c => c.String());
            AddColumn("dbo.AccessRules", "LastModifiedBy", c => c.String());
            AddColumn("dbo.AccessRules", "CreatedBy", c => c.String());
            DropForeignKey("dbo.ORTMovimientos", "SolicitudPedidoId", "dbo.ORTSolicitudesPedido");
            DropForeignKey("dbo.ORTMovimientos", "ServicioId", "dbo.Servicios");
            DropForeignKey("dbo.ORTMovimientos", "ProveedorId", "dbo.Proveedores");
            DropForeignKey("dbo.ORTMovimientos", "PacienteId", "dbo.ORTPacientes");
            DropForeignKey("dbo.ORTMovimientos", "OrdenCompraId", "dbo.ORTOrdenesCompra");
            DropForeignKey("dbo.ORTMovimientos", "MaterialId", "dbo.Materiales");
            DropForeignKey("dbo.ORTMovimientos", "HojaGastoId", "dbo.ORTHojasGasto");
            DropForeignKey("dbo.ORTMovimientos", "DevolucionId", "dbo.ORTDevoluciones");
            DropForeignKey("dbo.ORTMovimientos", "CirujanoId", "dbo.ORTCirujanos");
            DropForeignKey("dbo.ORTMovimientos", "BodegaId", "dbo.Bodegas");
            DropForeignKey("dbo.ORTIngresoInventarios", "OrdenCompraId", "dbo.ORTOrdenesCompra");
            DropForeignKey("dbo.ORTOrdenesCompra", "SolicitudPedidoId", "dbo.ORTSolicitudesPedido");
            DropForeignKey("dbo.ORTOrdenesCompra", "ProveedorId", "dbo.Proveedores");
            DropForeignKey("dbo.ORTHojasGasto", "ServicioId", "dbo.Servicios");
            DropForeignKey("dbo.ORTHojasGasto", "PacienteId", "dbo.ORTPacientes");
            DropForeignKey("dbo.ORTHojasGasto", "CirujanoId", "dbo.ORTCirujanos");
            DropForeignKey("dbo.ORTDevoluciones", "ProveedorId", "dbo.Proveedores");
            DropIndex("dbo.ORTMovimientos", new[] { "ServicioId" });
            DropIndex("dbo.ORTMovimientos", new[] { "HojaGastoId" });
            DropIndex("dbo.ORTMovimientos", new[] { "DevolucionId" });
            DropIndex("dbo.ORTMovimientos", new[] { "BodegaId" });
            DropIndex("dbo.ORTMovimientos", new[] { "ProveedorId" });
            DropIndex("dbo.ORTMovimientos", new[] { "MaterialId" });
            DropIndex("dbo.ORTMovimientos", new[] { "CirujanoId" });
            DropIndex("dbo.ORTMovimientos", new[] { "PacienteId" });
            DropIndex("dbo.ORTMovimientos", new[] { "OrdenCompraId" });
            DropIndex("dbo.ORTMovimientos", new[] { "SolicitudPedidoId" });
            DropIndex("dbo.ORTSolicitudesPedido", "IX_Numero");
            DropIndex("dbo.ORTOrdenesCompra", new[] { "ProveedorId" });
            DropIndex("dbo.ORTOrdenesCompra", new[] { "SolicitudPedidoId" });
            DropIndex("dbo.ORTOrdenesCompra", "IX_Numero");
            DropIndex("dbo.ORTIngresoInventarios", new[] { "OrdenCompraId" });
            DropIndex("dbo.ORTHojasGasto", new[] { "PacienteId" });
            DropIndex("dbo.ORTHojasGasto", new[] { "CirujanoId" });
            DropIndex("dbo.ORTHojasGasto", new[] { "ServicioId" });
            DropIndex("dbo.ORTDevoluciones", new[] { "ProveedorId" });
            DropColumn("dbo.SolicitudMaterialesSalaDetalle", "LastModifiedByUser");
            DropColumn("dbo.SolicitudMaterialesSalaDetalle", "CreatedByUser");
            DropColumn("dbo.SolicitudMaterialesSala", "LastModifiedByUser");
            DropColumn("dbo.SolicitudMaterialesSala", "CreatedByUser");
            DropColumn("dbo.SolicitudPedidoDetalle", "LastModifiedByUser");
            DropColumn("dbo.SolicitudPedidoDetalle", "CreatedByUser");
            DropColumn("dbo.SolicitudPedido", "LastModifiedByUser");
            DropColumn("dbo.SolicitudPedido", "CreatedByUser");
            DropColumn("dbo.SolicitudDespachoDetalle", "LastModifiedByUser");
            DropColumn("dbo.SolicitudDespachoDetalle", "CreatedByUser");
            DropColumn("dbo.SolicitudDespacho", "LastModifiedByUser");
            DropColumn("dbo.SolicitudDespacho", "CreatedByUser");
            DropColumn("dbo.SHDMovimientosInventario", "LastModifiedByUser");
            DropColumn("dbo.SHDMovimientosInventario", "CreatedByUser");
            DropColumn("dbo.SHDExistenciaTotal", "LastModifiedByUser");
            DropColumn("dbo.SHDExistenciaTotal", "CreatedByUser");
            DropColumn("dbo.SHDExistenciasLote", "LastModifiedByUser");
            DropColumn("dbo.SHDExistenciasLote", "CreatedByUser");
            DropColumn("dbo.SHDExistenciaBodega", "LastModifiedByUser");
            DropColumn("dbo.SHDExistenciaBodega", "CreatedByUser");
            DropColumn("dbo.PacienteEventoTraslados", "LastModifiedByUser");
            DropColumn("dbo.PacienteEventoTraslados", "CreatedByUser");
            DropColumn("dbo.RegistrosMedicos", "LastModifiedByUser");
            DropColumn("dbo.RegistrosMedicos", "CreatedByUser");
            DropColumn("dbo.PacienteEventos", "LastModifiedByUser");
            DropColumn("dbo.PacienteEventos", "CreatedByUser");
            DropColumn("dbo.Servicios", "LastModifiedByUser");
            DropColumn("dbo.Servicios", "LastModifiedDate");
            DropColumn("dbo.Servicios", "CreatedByUser");
            DropColumn("dbo.Servicios", "CreatedDate");
            DropColumn("dbo.OrdenCompraDetalle", "LastModifiedByUser");
            DropColumn("dbo.OrdenCompraDetalle", "CreatedByUser");
            DropColumn("dbo.OrdenCompra", "LastModifiedByUser");
            DropColumn("dbo.OrdenCompra", "CreatedByUser");
            DropColumn("dbo.MovimientosAbastecimiento", "LastModifiedByUser");
            DropColumn("dbo.MovimientosAbastecimiento", "CreatedByUser");
            DropColumn("dbo.IngresosInvnetarioDetalle", "LastModifiedByUser");
            DropColumn("dbo.IngresosInvnetarioDetalle", "CreatedByUser");
            DropColumn("dbo.IngresosInventario", "LastModifiedByUser");
            DropColumn("dbo.IngresosInventario", "CreatedByUser");
            DropColumn("dbo.Encargados", "LastModifiedByUser");
            DropColumn("dbo.Encargados", "CreatedByUser");
            DropColumn("dbo.DespachosInventarioDetalle", "LastModifiedByUser");
            DropColumn("dbo.DespachosInventarioDetalle", "CreatedByUser");
            DropColumn("dbo.DespachosInventario", "LastModifiedByUser");
            DropColumn("dbo.DespachosInventario", "CreatedByUser");
            DropColumn("dbo.AppEntityLogs", "CreatedByUser");
            DropColumn("dbo.AppAuditLogs", "Method");
            DropColumn("dbo.AppAuditLogs", "Url");
            DropColumn("dbo.AppAuditLogs", "UserId");
            DropColumn("dbo.Proveedores", "LastModifiedByUser");
            DropColumn("dbo.Proveedores", "CreatedByUser");
            DropColumn("dbo.Personas", "LastModifiedByUser");
            DropColumn("dbo.Personas", "CreatedByUser");
            DropColumn("dbo.Materiales", "LastModifiedByUser");
            DropColumn("dbo.Materiales", "LastModifiedDate");
            DropColumn("dbo.Materiales", "CreatedByUser");
            DropColumn("dbo.Materiales", "CreatedDate");
            DropColumn("dbo.Bodegas", "LastModifiedByUser");
            DropColumn("dbo.Bodegas", "LastModifiedDate");
            DropColumn("dbo.Bodegas", "CreatedByUser");
            DropColumn("dbo.Bodegas", "CreatedDate");
            DropColumn("dbo.AjustesInventarioDetalle", "LastModifiedByUser");
            DropColumn("dbo.AjustesInventarioDetalle", "CreatedByUser");
            DropColumn("dbo.AjustesInventario", "LastModifiedByUser");
            DropColumn("dbo.AjustesInventario", "CreatedByUser");
            DropColumn("dbo.AccessRules", "LastModifiedByUser");
            DropColumn("dbo.AccessRules", "CreatedByUser");
            DropTable("dbo.ORTMovimientos");
            DropTable("dbo.ORTSolicitudesPedido");
            DropTable("dbo.ORTOrdenesCompra");
            DropTable("dbo.ORTIngresoInventarios");
            DropTable("dbo.ORTPacientes");
            DropTable("dbo.ORTHojasGasto");
            DropTable("dbo.ORTDevoluciones");
            DropTable("dbo.ORTDespachoInventarios");
            DropTable("dbo.ORTCirujanos");
            DropTable("dbo.ORTAjusteInventarios");
            CreateIndex("dbo.Facilitadores", "FacilitadorId");
            CreateIndex("dbo.Empleados", "EncargadoId");
            AddForeignKey("dbo.Facilitadores", "FacilitadorId", "dbo.Personas", "PersonId");
            AddForeignKey("dbo.Empleados", "EncargadoId", "dbo.Personas", "PersonId");
        }
    }
}
