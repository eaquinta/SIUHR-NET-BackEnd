using Apphr.Application.SiahadDespacho.DTOs;
using Apphr.Application.SiahadIngreso.DTOs;
using Apphr.WebUI.Models.Entities;
using System;

namespace Apphr.WebUI.Models.Repository
{
    public class SHDMovimientoInventarioRepository : GenericRepository<SHDMovimientoInventario>
    {
        public SHDMovimientoInventarioRepository(ApphrDbContext context) : base(context)
        {
        }

        public void AddIngreso(SiahadIngresoDTOCreate dto) 
        {
            // Actividad        (2) Sin Uso
            // Financiamiento   (5) Sin Uso
            // Space            (1)_
            // Nit              (10) "425115-6"
            // Space            (1)_
            // Orden            (10) Sin Uso
            // Space            (1)_
            // Renglon          (3) .Substring(0,3)
            // Space            (1)_
            // Factura          (10) 
            // Space            (1)_            
            string Actividad = new string(' ', 2);
            string Financiamiento = new string(' ', 5);
            string Nit = "425115-6".PadRight(10);
            string Orden = new string(' ', 10);
            string Renglon = dto.MaterialCodigo.Substring(0, 3);
            string Factura = dto.DocumentoIngreso;

            var reg = new SHDMovimientoInventario() { 
                Tipo = "ING",
                AdministracionId = dto.AdministracionId,
                BodegaId = dto.BodegaId,
                MaterialId = dto.MaterialId,
                Observacion = Actividad + Financiamiento + ' ' + Nit + ' ' + Orden + ' ' + Renglon + ' ' + Factura + ' ',
                BodegaCodigo = dto.BodegaCodigo,
                Documento = dto.Documento??0,
                Correlativo = dto.Correlativo??0,
                MaterialCodigo = dto.MaterialCodigo,
                MaterialCodigi = dto.MaterialCodigi,
                FechaMovimiento = dto.FechaMovimiento,
                Vence = dto.FechaVencimiento,
                Cantidad = dto.Cantidad,
                Valor = dto.Valor,
                FechaCreado = DateTime.Now.Date,
                // Datos especificos del movimiento
                DocumentoIngreso = dto.DocumentoIngreso,
                Renglon = Renglon,
                ProveedorNit = Nit
            };
            db.SHDMovimientosInventario.Add(reg);
            db.SaveChanges();
            //return dto;
        }

        public void AddDespacho(SiahadDespachoDTOCreate dto)
        {
            // DestinoCodigo    (5) 
            // Space            (1)_
            // Solicitud        (10)
            // Space            (1)_
            // s_observ         (28)
            string DestinoCodigo = dto.DestinoCodigo.PadRight(5);
            string Solicitud = dto.Solicitud.PadRight(10);
            string Observacion = dto.Observacion.PadRight(28);
            var reg = new SHDMovimientoInventario()
            {
                Tipo = "DES",
                AdministracionId = dto.AdministracionId,
                BodegaId = dto.BodegaId,
                MaterialId = dto.MaterialId,
                Observacion = DestinoCodigo + ' ' + Solicitud + ' ' + Observacion,
                BodegaCodigo = dto.BodegaCodigo,
                Documento = dto.Documento ?? 0,
                Correlativo = dto.Correlativo ?? 0,
                MaterialCodigo = dto.MaterialCodigo,
                MaterialCodigi = dto.MaterialCodigi,
                FechaMovimiento = dto.FechaMovimiento,
                Vence = dto.FechaVencimiento,
                Cantidad = dto.Cantidad,
                Valor = dto.Valor,
                FechaCreado = DateTime.Now.Date,
                // Datos especificos del movimiento
                DestinoCodigo = dto.DestinoCodigo,
                Solicitud = dto.Solicitud                
            };
            db.SHDMovimientosInventario.Add(reg);
            db.SaveChanges();
        }
    }
}