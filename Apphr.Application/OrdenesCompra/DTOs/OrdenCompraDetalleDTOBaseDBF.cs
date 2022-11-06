namespace Apphr.Application.OrdenesCompra.DTOs
{
    public class OrdenCompraDetalleDTOBaseDBF
    {
        public string Orden { get; set; }
        public string Nit { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Valor { get; set; }
        public string MaterialCodigo { get; set; }
        public string Renglon { get; set; }
        public string UnidadMedida { get; set; }
        public string Descipcion { get; set; }        
    }
}
