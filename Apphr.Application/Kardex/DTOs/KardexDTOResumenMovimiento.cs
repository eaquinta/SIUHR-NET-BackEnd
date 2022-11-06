namespace Apphr.Application.Kardex.DTOs
{
    //Apphr.Application.Kardex.DTOs.KardexDTOResumenMovimiento
    public class KardexDTOResumenMovimiento
    {
        public int BodegaId { get; set; }
        public int MaterialId { get; set; }
        public string TipoMovimiento { get; set; }
        public int CantidadDocumentos { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Valor { get; set; }
    }
}
