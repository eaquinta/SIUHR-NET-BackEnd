using Apphr.Domain.Entities;
using System;

//Apphr.Application.Kardex.DTOs.KardexDTOBase
namespace Apphr.Application.Kardex.DTOs
{
    public class KardexDTOBase
    {
        public int BodegaId { get; set; }
        public int MaterialId { get; set; }
        public Material Material { get; set; }
        public string TipoDocumento { get; set; }
        public string Documento { get; set; }
        public string DocumentoReferencia { get; set; }
        public int Line { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Valor { get; set; }
        public decimal CantidadAcumulado { get; set; }
        public decimal ValorAcumulado { get; set; }
        public decimal CostoUnitario { get; set; }
        public int PacienteId { get; set; }
        public Paciente Paciente { get; set; }
        public int DestinoId { get; set; }
        public Destino Destino { get; set; }
    }
}
