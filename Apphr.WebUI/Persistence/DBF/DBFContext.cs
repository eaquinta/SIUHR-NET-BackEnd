using Apphr.Domain.DTOs;
using Apphr.Domain.EntitiesDBF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;

namespace Apphr.Infrastructure.Persistence.DBF
{
    public class DBFContext : DBFEngine
    {        
        public IEnumerable<ReporteExistenciasDBF> GetExistencias(string bodega, DateTime afec)
        {
            try
            {
                using (DataTable dt = GetDataSQL($"SELECT mt.Codigo, mt.Unimed, mi.Codigi, mi.CanMov, mi.Valmov, mt.Descri FROM (SELECT Codmate, Codigi, SUM(IIF(INLIST(TIPO, 'APE', 'ING', 'DEV', 'TR+', 'AJU'),1,-1) * Canmov) AS Canmov, SUM(IIF(INLIST(TIPO, 'APE', 'ING', 'DEV', 'TR+', 'AJU'),1,-1)* Valmov) AS Valmov FROM MOVINVEN.DBF WHERE Codbode = '{bodega}' AND ((anomov*10000)+(mesmov*100)+(diamov*1) <= ({afec.Year}*10000)+({afec.Month}*100)+({afec.Day}*1)) ORDER BY Codmate,Codigi GROUP BY Codmate, Codigi) AS mi LEFT JOIN material.dbf mt ON mi.Codmate = mt.Codigo AND mi.codigi = mt.codigi"))
                {
                    return (from DataRow dr in dt.Rows
                            select ReporteExistenciasDBF.MapDataRow(dr));
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<ReporteExistenciasDBF> GetExistenciasDetalle(string bodega, DateTime afec)
        {
            try
            {
                using (DataTable dt = GetDataSQL($"SELECT mt.Codigo, mi.Vence, mt.Unimed, mi.Codigi, mi.CanMov, mi.Valmov, mt.Descri FROM (SELECT Codmate, Vence, Codigi, SUM(IIF(INLIST(TIPO, 'APE', 'ING', 'DEV', 'TR+', 'AJU'), 1, -1) * Canmov) AS Canmov, SUM(IIF(INLIST(TIPO, 'APE', 'ING', 'DEV', 'TR+', 'AJU'), 1, -1) * Valmov) AS Valmov FROM MOVINVEN.DBF WHERE Codbode = '{bodega}' AND ((anomov*10000)+(mesmov*100)+(diamov*1) <= ({afec.Year}*10000)+({afec.Month}*100)+({afec.Day}*1)) ORDER BY Codmate, Codigi, Vence GROUP BY Codmate, Codigi, Vence) AS mi LEFT JOIN material.dbf mt ON mi.Codmate = mt.Codigo AND mi.codigi = mt.codigi"))            
                {
                    return (from DataRow dr in dt.Rows
                            select ReporteExistenciasDBF.MapDataRow(dr));
                }
            }
            catch (Exception)
            {
                return null;
            }
        }


        public IEnumerable<SolicitudPedidoDBF> GetSolicitudesPedido()
        {
            try
            {
                using (DataTable dt = GetDataSQL($"SELECT * FROM SOLENC.dbf ORDER BY NUMSOL"))
                {
                    return (from DataRow dr in dt.Rows
                            select SolicitudPedidoDBF.MapDataRow(dr));
                }
            }
            catch (Exception)
            {
                return null;              
            }            
        }
        public SolicitudPedidoDBF GetSolicitudPedido(string id)
        {
            try
            {
                using (DataTable dt = GetDataSQL($"SELECT * FROM SOLENC.dbf WHERE NUMSOL = '{id}'"))
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
            using (DataTable dt = GetDataSQL($"SELECT * FROM SOLDET.dbf WHERE NUMSOL = '{id}'"))
            {
                return (from DataRow dr in dt.Rows
                        select SolicitudPedidoDetalleDBF.MapDataRow(dr)).ToList();
            }
        }
        
        
        public IEnumerable<DTOMovimientosInventarioDBF> GetMovimientos(string CodigoMaterial, string CodigoBodega, DateTime? DFec, DateTime? AFec, string searchValue, string sortColumnName, string sortColumnDir) 
        {
            try
            {
                //var isFirst = false;
                var hasLike = !string.IsNullOrEmpty(searchValue);
                var sqlLike = hasLike ? "AND m.OBSMOV LIKE ? " : "";
                var hasCodigoMaterial = !string.IsNullOrEmpty(CodigoMaterial);
                var sqlCodigoMaterial = hasCodigoMaterial ? $"AND mt.CODIGO = ? " : "";
                var hasCodigoBodega = !string.IsNullOrEmpty(CodigoBodega);
                var sqlCodigoBodega = hasCodigoBodega ? $"AND b.CODIGO = ? " : "";
                // CTOD ("MM/DD/YYyy")
                var sql = $"SELECT * FROM (SELECT RECNO() AS SECUENCIA, b.CODIGO AS CODIGOBODEGA, mt.CODIGO AS CODIGOMATERIAL, CTOD(SUBSTR(VENCE,5,2)+'/'+SUBSTR(VENCE,7,2)+'/'+SUBSTR(VENCE,1,4)) AS VENCE, CTOD(STR(m.MESMOV)+'/'+ STR(m.DIAMOV) +'/'+STR(m.ANOMOV)) AS FECMOV, m.TIPO, m.DOCUME, m.ANOMOV, m.MESMOV, m.DIAMOV, mt.UNIMED, m.OBSMOV, m.CANMOV, m.VALMOV FROM MOVINVEN.DBF AS m " +
                    $"LEFT JOIN MATERIAL.DBF AS mt ON m.CODMATE = mt.CODIGO LEFT JOIN BODEGAS.DBF AS b ON b.CODIGO = m.CODBODE WHERE " +
                    $"    (((anomov*10000)+(mesmov*100)+(diamov*1) >= ? ) OR ? = 0) " +  // DFec
                    $"AND (((anomov*10000)+(mesmov*100)+(diamov*1) <= ? ) OR ? = 0) " +  // AFec
                    sqlCodigoMaterial +  // CodigoMaterial
                    sqlCodigoBodega +  // CodigoBodega
                    sqlLike + $") AS t ORDER BY {sortColumnName} {sortColumnDir.ToUpper()}";

                OleDbCommand cmd = new OleDbCommand(sql);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("@DFec1", OleDbType.Integer);
                cmd.Parameters.Add("@DFec2", OleDbType.Integer);

                cmd.Parameters.Add("@AFec1", OleDbType.Integer);
                cmd.Parameters.Add("@AFec2", OleDbType.Integer);
                
                if (hasCodigoMaterial)
                    cmd.Parameters.Add("@CodigoMaterial1", OleDbType.VarChar);

                if (hasCodigoBodega)
                    cmd.Parameters.Add("@CodigoBodega1", OleDbType.VarChar);

                if (hasLike)
                    cmd.Parameters.Add("@Like1", OleDbType.VarChar);
                                

                cmd.Parameters["@DFec1"].Value = DFec.HasValue ? (DFec.Value.Year * 10000 + DFec.Value.Month * 100 + DFec.Value.Day) : 0;
                cmd.Parameters["@DFec2"].Value = DFec.HasValue ? (DFec.Value.Year * 10000 + DFec.Value.Month * 100 + DFec.Value.Day) : 0;

                cmd.Parameters["@AFec1"].Value = AFec.HasValue ? (AFec.Value.Year * 10000 + AFec.Value.Month * 100 + AFec.Value.Day) : 0;
                cmd.Parameters["@AFec2"].Value = AFec.HasValue ? (AFec.Value.Year * 10000 + AFec.Value.Month * 100 + AFec.Value.Day) : 0;

                if (hasCodigoMaterial)
                    cmd.Parameters["@CodigoMaterial1"].Value = CodigoMaterial ?? (object)DBNull.Value;

                if (hasCodigoBodega)
                    cmd.Parameters["@CodigoBodega1"].Value = CodigoBodega ?? (object)DBNull.Value;

                if (hasLike)
                    cmd.Parameters["@Like1"].Value = (searchValue == null )? null :"%"+searchValue+"%";

                using (DataTable dt = GetDataSQL(cmd))
                {
                    return (from DataRow dr in dt.Rows
                            select DTOMovimientosInventarioDBF.MapDataRow(dr));
                }
            }
            catch (Exception)
            {
                return null;
            }
        }


        public List<MaterialDBF> GetMateriales()
        {
            try
            {
                using (DataTable dt = GetDataSQL($"SELECT * FROM material.dbf"))
                {
                    return (from DataRow dr in dt.Rows
                            select MaterialDBF.MapDataRow(dr)).ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public IEnumerable<MaterialDBF> GetMateriales(string f)
        {
            try
            {
                f = f.ToUpper();
                using (DataTable dt = GetDataSQL($"SELECT TOP 50 * FROM material.dbf WHERE UPPER(CODIGO) LIKE '%{f}%' OR UPPER(DESCRI) LIKE '%{f}%' ORDER BY CODIGO"))
                {
                    return (from DataRow dr in dt.Rows
                            select MaterialDBF.MapDataRow(dr)).ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<MaterialDBF> GetMaterial(string id)
        {
            using (DataTable dt = GetDataSQL($"SELECT * FROM material.dbf WHERE CODIGO = '{id}'"))
            {
                return (from DataRow dr in dt.Rows
                        select MaterialDBF.MapDataRow(dr)).ToList();
            }
        }



        public List<DestinoDBF> GetDestinos()
        {
            try
            {
                using (DataTable dt = GetDataSQL($"SELECT * FROM destinos.dbf"))
                {
                    return (from DataRow dr in dt.Rows
                            select DestinoDBF.MapDataRow(dr)).ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<DestinoDBF> GetDestino(string id)
        {
            try
            {
                using (DataTable dt = GetDataSQL($"SELECT * FROM destinos.dbf WHERE CODIGO = '{id}'"))
                {
                    return (from DataRow dr in dt.Rows
                            select DestinoDBF.MapDataRow(dr)).ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }

        }



        public IEnumerable<BodegaDBF> GetBodegas()
        {
            try
            {
                using (DataTable dt = GetDataSQL($"SELECT * FROM bodegas.dbf"))
                {
                    return (from DataRow dr in dt.Rows
                            select BodegaDBF.MapDataRow(dr)).ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<BodegaDBF> GetBodega(string id)
        {
            try
            {
                using (DataTable dt = GetDataSQL($"SELECT * FROM bodegas.dbf WHERE CODIGO = '{id}'"))
                {
                    return (from DataRow dr in dt.Rows
                            select BodegaDBF.MapDataRow(dr)).ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public BodegaDBF AddBodega(BodegaDBF reg)
        {
            try
            {
                var No = ExecNoQuery(reg.CmdInsert());
                var res = GetBodega(reg.CODIGO).FirstOrDefault();
                return res;
            }
            catch (Exception)
            {
                return null;
            }
             
        }
        public BodegaDBF UpdBodega(BodegaDBF reg)
        {
            try
            {
                var res = ExecNoQuery(reg.CmdUpdate());
                return GetBodega(reg.CODIGO).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public void DltBodega(BodegaDBF reg)
        {
            try
            {
                var res  = ExecNoQuery(reg.CmdDelete());
            }
            catch (Exception)
            {
                //return null;
            }
        }



        public List<ProveedorDBF> GetProveedores()
        {
            try
            {
                using (DataTable dt = GetDataSQL($"SELECT * FROM provee.dbf"))
                {
                    return (from DataRow dr in dt.Rows
                            select ProveedorDBF.MapDataRow(dr)).ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<ProveedorDBF> GetProveedor(string id)
        {
            try
            {
                using (DataTable dt = GetDataSQL($"SELECT * FROM provee.dbf WHERE NIT = '{id}'"))
                {
                    return (from DataRow dr in dt.Rows
                            select ProveedorDBF.MapDataRow(dr)).ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }


        public IEnumerable<OrdenCompraDBF> GetOrdenesCompra()
        {
            try
            {
                using (DataTable dt = GetDataSQL($"SELECT ORDEN, ANOORD, MESORD, DIAORD, NIT FROM ORDEN.dbf ORDER BY ORDEN"))
                {
                    return (from DataRow dr in dt.Rows
                            select OrdenCompraDBF.MapDataRow(dr));
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public OrdenCompraDBF GetOrdenCompra(string id)
        {
            try
            {
                using (DataTable dt = GetDataSQL($"SELECT * FROM ORDEN.dbf WHERE ORDEN = '{id}'"))
                {
                    return OrdenCompraDBF.MapDataRow(dt.Rows[0]);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<OrdenCompraDetalleDBF> GetOrdenCompraDetalle(string id)
        {
            using (DataTable dt = GetDataSQL($"SELECT * FROM ORDDET.dbf WHERE ORDEN = '{id}'"))
            {
                return (from DataRow dr in dt.Rows
                        select OrdenCompraDetalleDBF.MapDataRow(dr)).ToList();
            }
        }



        public IEnumerable<SolicitudDespachoDBF> GetSolicitudesDespacho()
        {
            try
            {
                using (DataTable dt = GetDataSQL($"SELECT NUMDES, ANODES, MESDES, DIADES, DEPDES, TIPDES, GERDES, JEFDES, SOLDES, OBSDES FROM DESENC.dbf ORDER BY NUMDES"))
                {
                    return (from DataRow dr in dt.Rows
                            select SolicitudDespachoDBF.MapDataRow(dr));
                }
            }
            catch (Exception)
            {
                return null;
            }            
        }
        public SolicitudDespachoDBF GetSolicitudDespacho(string id)
        {
            using (DataTable dt = GetDataSQL($"SELECT * FROM DESENC.dbf WHERE NUMDES = '{id}'"))
            {
                return SolicitudDespachoDBF.MapDataRow(dt.Rows[0]);
            }
        }
        public List<SolicitudDespachoDetalleDBF> GetSolicitudDespachoDetalle(string id, string mes)
        {
            using (DataTable dt = GetDataSQL($"SELECT * FROM DES{mes}D.dbf WHERE NUMDES = '{id}'"))
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
            using (DataTable dt = GetDataSQL($"SELECT * FROM EXISTOT.dbf WHERE CODIGO = '{codigo}' AND CODIGI = '{codigi}'"))
            {
                if (dt.Rows.Count > 0)
                    return ExistenciaTotalDBF.MapDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }
        public IEnumerable<ExistenciaTotalDBF> GetExistenciasTotales()
        {
            using (DataTable dt = GetDataSQL($"SELECT * FROM EXISTOT.dbf ORDER BY CODIGO"))
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
            using (DataTable dt = GetDataSQL($"SELECT * FROM EXISBOD.dbf WHERE CODIGO = '{codigo}' AND CODIGI = '{codigi}' AND BODEGA = '{bodega}'"))
            {
                if (dt.Rows.Count > 0)
                    return ExistenciaTotalDBF.MapDataRow(dt.Rows[0]);
                else
                    return null;
            }
        }
    }
}