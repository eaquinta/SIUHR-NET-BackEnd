using Apphr.Application.Kardex.DTOs;
using Apphr.WebUI.Models.Entities;
using Apphr.WebUI.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Apphr.WebUI.Services
{
    public class KardexRepository
    {
        public List<KardexDTOrptKardex> GetCRptKardex(List<KardexDTOKardexMaterial> regs)
        {
            var res = new List<KardexDTOrptKardex>();
            foreach (var item in regs)
            {
                foreach (var det in item.Detalle)
                {
                    res.Add(new KardexDTOrptKardex()
                    {
                        BodegaId = item.BodegaId,
                        MaterialId = item.MaterialId,
                        MaterialCodigo = item.Material.Codigo,
                        MaterialDescripcion = item.Material.Descripcion,
                        RefPac_NumHCAntiguo = det.Paciente?.RefPac_NumHCAntiguo.ToString(),
                        PersonaName = det.Paciente?.Persona.Name,
                        Fecha = det.Fecha,
                        Cantidad = det.Cantidad,
                        Valor = det.Valor,
                        Documento = det.Documento,
                        DocumentoReferencia = det.DocumentoReferencia,
                        Line = det.Line,
                        TipoDocumento = det.TipoDocumento, //TipoMovimientoInventario.NombreCorto,
                        CantidadAcumulado = det.CantidadAcumulado,
                        ValorAcumulado = det.ValorAcumulado, 
                        CostoUnitario = (det.CantidadAcumulado == 0)?0:(det.ValorAcumulado / det.CantidadAcumulado),
                        CantidadAnterior = item.SaldoAnterior.Cantidad,
                        ValorAnterior = item.SaldoAnterior.Valor,
                        CostoUnitarioAnterior = item.SaldoAnterior.CostoUnitario,
                        CantidadActual = item.SaldoActual.Cantidad,
                        ValorActual = item.SaldoActual.Valor,
                        CostoUnitarioActual = item.SaldoActual.CostoUnitario,
                        //Destino = det.Destino
                        //CostoUnitario = det.Precio,
                        DestinoCodigo = det.Destino?.Codigo,
                        //CantidadAnterior = SaldoAnterior.Cantidad,
                        //ValorAnterior = SaldoAnterior.Valor
                    });
                }                
            }
            return res;
        }

        public List<KardexDTOKardexMaterial> GetKardexReportModel(int BodegaId, List<MovimientoInventario> regs)
        {   
            var res = new List<KardexDTOKardexMaterial>();
            //var SaldoAnterior = GetSaldoAnterior(gitem.BodegaId, gitem.MaterialId);
            foreach (var item in GetMaterialsInKardex(regs))
            {
                var Anterior = GetSaldoAnterior(BodegaId, item.MaterialId);
                var Detalle = GetMaterialMovimientos(item.MaterialId, regs, Anterior);
                var Actual = GetSaldoActual(Detalle);
                res.Add(new KardexDTOKardexMaterial() {
                    BodegaId = BodegaId,
                    MaterialId = item.MaterialId,
                    Material = item,
                    Detalle = Detalle,
                    SaldoAnterior = Anterior,
                    SaldoActual = Actual
                });
            }
            return res;
        }

        #region Private

        private KardexDTOSaldo GetSaldoActual(List<KardexDTOBase> regs)
        {            
            var ult = regs.Last();         
            return new KardexDTOSaldo() { 
                Cantidad = ult.CantidadAcumulado,
                Valor = ult.ValorAcumulado
            };
        }
        private List<KardexDTOBase> GetMaterialMovimientos(int MaterialId, List<MovimientoInventario> regs, KardexDTOSaldo Anterior)
        {
            var res = new List<KardexDTOBase>();
            decimal CantidadAcumulado = Anterior.Cantidad;
            decimal ValorAcumulado = Anterior.Valor;
            foreach (var item in regs.Where(x => x.Material.MaterialId == MaterialId))
            {
                // Calculo Acumulado
                CantidadAcumulado += (item.Cantidad * item.Efecto);
                ValorAcumulado += (item.Valor * item.Efecto);

                res.Add(new KardexDTOBase() {
                    MaterialId = item.MaterialId,
                    Material = item.Material,
                    Cantidad = item.Cantidad,
                    Valor = item.Valor,
                    BodegaId = item.BodegaId,
                    CantidadAcumulado = CantidadAcumulado,
                    ValorAcumulado = ValorAcumulado,                   
                    CostoUnitario = item.Precio,
                    Fecha = item.Fecha,
                    Documento = item.Documento,
                    DocumentoReferencia = item.DocumentoReferencia,
                    Line = item.Line,
                    TipoDocumento = item.TipoMovimientoInventario.NombreCorto,                   
                    Paciente = item.Paciente,
                    Destino = item.Destino
                    //MaterialCodigo = item.Material.Codigo,
                    //MaterialDescripcion = item.Material.Descripcion,
                    //RefPac_NumHCAntiguo = item.Paciente?.RefPac_NumHCAntiguo.ToString(),
                    //PersonaName = item.Paciente?.Persona.Name,
                    //DestinoCodigo = item.Destino?.Codigo,
                    //CantidadAnterior = SaldoAnterior.Cantidad,
                    //ValorAnterior = SaldoAnterior.Valor
                });
            }
            return res;
        }

        private List<Material> GetMaterialsInKardex(List<MovimientoInventario> regs)
        {
            //var res = new List<Material>();
            //foreach (var gitem in regs.GroupBy(x => x.Material.Codigo).Select(g => g.First()).ToList())
            //{                
            //    res.Add(gitem.Material);
            //}
            //return res;
            return regs.GroupBy(x => x.Material.Codigo).Select(g => g.First().Material).ToList();            
        }
        private KardexDTOSaldo GetSaldoAnterior(int BodegaId, int MaterialId)
        {
            DateTime now = DateTime.Now;
            now = now.AddDays(-now.Day);
            using (var db = new ApphrDbContext((HttpContext.Current.User != null) ? HttpContext.Current.User.Identity.GetUserId<int>() : -1)) //System.Web.HttpContext.Current.User.Identity.Name
            {
                var existencias = db.CierresInventario.Where(x => x.BodegaId == BodegaId && x.MaterialId == MaterialId && DbFunctions.TruncateTime(x.Fecha) <= now.Date).OrderByDescending(x => x.Fecha).FirstOrDefault();
                if (existencias != null)
                {
                    return new KardexDTOSaldo()
                    {
                        MaterialId = MaterialId,
                        Cantidad = existencias.Cantidad,
                        Valor = existencias.Valor
                    };
                }
                else
                {
                    return new KardexDTOSaldo()
                    {
                        MaterialId = MaterialId,
                        Cantidad = 0,
                        Valor = 0
                    };
                }
            }            
        }
        #endregion

    }
}