using Apphr.Application.Bodegas.DTOs;
using Apphr.Application.Destinos.DTOs;
using Apphr.WebUI.Models.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Apphr.Application.DespachosInventario.DTOs
{
    public class DespachoInventarioDTOBase
    {
        [Display(Name = "Despacho")] 
        public string DespachoInventarioId { get; set; }
        [Display(Name = "Fecha Egreso")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }
        
        [Display(Name = "No. Documento Ref.")]
        [MaxLength(20)]
        [Required]
        [Remote("JsValidateDocumentRef", "DespachosInventario",
            HttpMethod = "POST",
            AdditionalFields = "Mode, TipoDocumentoReferencia",
            ErrorMessageResourceType = typeof(Resources.Msg),
            ErrorMessageResourceName = "DocumentRef_NotValid")]
        public string DocumentoReferencia { get; set; }

        [MaxLength(10)]       
        [Display(Name = "Tipo Documento Ref.")]
        [UIHint("DropDown")]
        public string TipoDocumentoReferencia { get; set; }
        
        [Display(Name = "Fecha Emisión Documento")]
        [DataType(DataType.Date)]
        public DateTime FechaDocumentoReferencia { get; set; }

        public int DepartamentoId { get; set; }
        public DestinoDTOBase Departamento { get; set; }
       
        public int Lineas { get; set; }
    }

    public class DespachoInventarioDetalleDTOBase
    {
        public int DespachoInventarioDetalleId { get; set; }

        [MaxLength(15)]
        public string DespachoInventarioId { get; set; }
        [Display(Name = "Documento Ingreso")]
        public string DocumentoReferencia { get; set; }        
        [Display(Name = "Fecha Emision")]
        public DateTime FechaDocumentoReferencia { get; set; }
        public int MaterialId { get; set; }
        public Material Material { get; set; }
        public int? ProveedorId { get; set; }
        public Proveedor Proveedor { get; set; }
        [MaxLength(20)]
        public string Lote { get; set; }
        [Display(Name = "Fecha Vencimiento")]
        [DataType(DataType.Date)]
        public DateTime? FechaVencimiento { get; set; }

        public int? PacienteId { get; set; }
        public Paciente Paciente { get; set; }

        public virtual decimal Cantidad { get; set; }
        public Decimal Precio { get; set; }
        public Decimal Valor { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Observación")]
        public string Observacion { get; set; }

        public int BodegaId { get; set; }
        public BodegaDTOBase Bodega { get; set; }


        public int Linea { get; set; }

    }
}
