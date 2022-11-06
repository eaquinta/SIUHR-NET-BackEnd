using Apphr.Domain.EntitiesDBF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Infrastructure.Persistence.DBF
{
    public class DBFContext : DBFEngine
    {
        public DBFContext(string filePath) : base(filePath)
        {
        }

        public IEnumerable<SolicitudPedidoDBF> GetSolicitudesPedido()
        {
            using (DataTable dt = this.GetDataSQL($"SELECT * FROM SOLENC.dbf ORDER BY NUMSOL"))
            {
                return (from DataRow dr in dt.Rows
                        select SolicitudPedidoDBF.MapDataRow(dr));
            }
        }
        public SolicitudPedidoDBF GetSolicitudPedido(string id)
        {
            try
            {
                using (DataTable dt = this.GetDataSQL($"SELECT * FROM SOLENC.dbf WHERE NUMSOL = '{id}'"))
                {
                    return SolicitudPedidoDBF.MapDataRow(dt.Rows[0]);
                }
            }
            catch (Exception)
            {
                return null;
            }            
        }
        public List<SolicitudPedidoDetalleDBF> GetSolicitudPedidoDetalle(string id)
        {
            using (DataTable dt = this.GetDataSQL($"SELECT * FROM SOLDET.dbf WHERE NUMSOL = '{id}'"))
            {
                return (from DataRow dr in dt.Rows
                        select SolicitudPedidoDetalleDBF.MapDataRow(dr)).ToList();
            }
        }



        public List<MaterialDBF> GetMateriales()
        {
            using (DataTable dt = this.GetDataSQL($"SELECT * FROM material.dbf"))
            {
                return (from DataRow dr in dt.Rows
                        select MaterialDBF.MapDataRow(dr)).ToList();
            }
        }
        public List<MaterialDBF> GetMaterial(string id)
        {
            using (DataTable dt = this.GetDataSQL($"SELECT * FROM material.dbf WHERE CODIGO = '{id}'"))
            {
                return (from DataRow dr in dt.Rows
                        select MaterialDBF.MapDataRow(dr)).ToList();
            }
        }



        public List<DestinoDBF> GetDestinos()
        {
            using (DataTable dt = this.GetDataSQL($"SELECT * FROM destinos.dbf"))
            {
                return (from DataRow dr in dt.Rows
                        select DestinoDBF.MapDataRow(dr)).ToList();
            }
        }
        public List<DestinoDBF> GetDestino(string id)
        {
            using (DataTable dt = GetDataSQL($"SELECT * FROM destinos.dbf WHERE CODIGO = '{id}'"))
            {
                return (from DataRow dr in dt.Rows
                        select DestinoDBF.MapDataRow(dr)).ToList();
            }

        }



        public List<BodegaDBF> GetBodegas()
        {
            using (DataTable dt = this.GetDataSQL($"SELECT * FROM bodegas.dbf"))
            {
                return (from DataRow dr in dt.Rows
                        select BodegaDBF.MapDataRow(dr)).ToList();
            }
        }
        public List<BodegaDBF> GetBodega(string id)
        {
            using (DataTable dt = GetDataSQL($"SELECT * FROM bodegas.dbf WHERE CODIGO = '{id}'"))
            {
                return (from DataRow dr in dt.Rows
                        select BodegaDBF.MapDataRow(dr)).ToList();
            }

        }



        public List<ProveedorDBF> GetProveedores()
        {
            using (DataTable dt = this.GetDataSQL($"SELECT * FROM provee.dbf"))
            {
                return (from DataRow dr in dt.Rows
                        select ProveedorDBF.MapDataRow(dr)).ToList();
            }
        }
        public List<ProveedorDBF> GetProveedor(string id)
        {
            using (DataTable dt = GetDataSQL($"SELECT * FROM provee.dbf WHERE NIT = '{id}'"))
            {
                return (from DataRow dr in dt.Rows
                        select ProveedorDBF.MapDataRow(dr)).ToList();
            }

        }


        public IEnumerable<OrdenCompraDBF> GetOrdenesCompra()
        {
            using (DataTable dt = this.GetDataSQL($"SELECT ORDEN, ANOORD, MESORD, DIAORD, NIT FROM ORDEN.dbf ORDER BY ORDEN"))
            {
                return (from DataRow dr in dt.Rows
                        select OrdenCompraDBF.MapDataRow(dr));
            }
        }
        public OrdenCompraDBF GetOrdenCompra(string id)
        {
            using (DataTable dt = this.GetDataSQL($"SELECT * FROM ORDEN.dbf WHERE ORDEN = '{id}'"))
            {
                return OrdenCompraDBF.MapDataRow(dt.Rows[0]);
            }
        }
        public List<OrdenCompraDetalleDBF> GetOrdenCompraDetalle(string id)
        {
            using (DataTable dt = this.GetDataSQL($"SELECT * FROM ORDDET.dbf WHERE ORDEN = '{id}'"))
            {
                return (from DataRow dr in dt.Rows
                        select OrdenCompraDetalleDBF.MapDataRow(dr)).ToList();
            }
        }



        public IEnumerable<SolicitudDespachoDBF> GetSolicitudesDespacho()
        {
            using (DataTable dt = this.GetDataSQL($"SELECT NUMDES, ANODES, MESDES, DIADES, DEPDES, TIPDES, GERDES, JEFDES, SOLDES, OBSDES FROM DESENC.dbf ORDER BY NUMDES"))
            {
                return (from DataRow dr in dt.Rows
                        select SolicitudDespachoDBF.MapDataRow(dr));
            }
        }
        public SolicitudDespachoDBF GetSolicitudDespacho(string id)
        {
            using (DataTable dt = this.GetDataSQL($"SELECT * FROM DESENC.dbf WHERE NUMDES = '{id}'"))
            {
                return SolicitudDespachoDBF.MapDataRow(dt.Rows[0]);
            }
        }
        public List<SolicitudDespachoDetalleDBF> GetSolicitudDespachoDetalle(string id, string mes)
        {
            using (DataTable dt = this.GetDataSQL($"SELECT * FROM DES{mes}D.dbf WHERE NUMDES = '{id}'"))
            {
                return (from DataRow dr in dt.Rows
                        select SolicitudDespachoDetalleDBF.MapDataRow(dr)).ToList();
            }
        }



        public ExistenciaTotalDBF GetExistenciaTotal(string codigo, string codigi, bool fixKey)
        {
            if (fixKey)
            {
                codigi = codigi.PadLeft(5);codigi = codigi.Substring(codigi.Length - 5);
                codigo = codigo.PadRight(14); codigo = codigo.Substring(0, 14);
            }
            using (DataTable dt = this.GetDataSQL($"SELECT * FROM EXISTOT.dbf WHERE CODIGO = '{codigo}' AND CODIGI = '{codigi}'"))
            {
                if (dt.Rows.Count > 0)
                    return ExistenciaTotalDBF.MapDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }
        public IEnumerable<ExistenciaTotalDBF> GetExistenciasTotales()
        {
            using (DataTable dt = this.GetDataSQL($"SELECT * FROM EXISTOT.dbf ORDER BY CODIGO"))
            {
                return (from DataRow dr in dt.Rows
                        select ExistenciaTotalDBF.MapDataRow(dr));
            }
        }



        public ExistenciaTotalDBF GetExistenciaBodega(string codigo, string codigi, string bodega, bool fixKey)
        {
            if (fixKey)
            {
                bodega = bodega.PadLeft(5); bodega = bodega.Substring(bodega.Length - 5);
                codigi = codigi.PadLeft(5); codigi = codigi.Substring(codigi.Length - 5);
                codigo = codigo.PadRight(14); codigo = codigo.Substring(0, 14);
            }
            using (DataTable dt = this.GetDataSQL($"SELECT * FROM EXISBOD.dbf WHERE CODIGO = '{codigo}' AND CODIGI = '{codigi}' AND BODEGA = '{bodega}'"))
            {
                if (dt.Rows.Count > 0)
                    return ExistenciaTotalDBF.MapDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }
    }
}