using Apphr.Application.Ortopedia.ORTMovimientos.DTOs;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Apphr.Application.Ortopedia.DespachosInventario.DTOs
{
    public class DespachoInventarioDTOCEditChild : MovimientoDTOBase
    {
        [Required]
        [Display(Name = "Registro médico")]
        [Remote("JsPacienteRMExist", "Paciente", "Ortopedia",
            HttpMethod = "POST",
            ErrorMessageResourceType = typeof(Resources.Msg),
            ErrorMessageResourceName = "PacienteRM_NotExist")]
        public string PacienteRM { get; set; }

        [Display(Name = "Nombre Paciente")]
        public string PacienteNombreCompleto { get; set; }
        //public Int64 PacienteId { get; set; }

        [Display(Name = "Bodega")]                                              // BodegaNombre
        [Required]
        [Remote("JsNombreExist", "Bodegas", "Inventario",
            HttpMethod = "POST",
            ErrorMessageResourceType = typeof(Resources.Msg),
            ErrorMessageResourceName = "Bodega_NotExist")]
        public string BodegaNombre { get; set; }


        [Display(Name = "Descripción")]                                         // BodegaDescripcion
        public string BodegaDescripcion { get; set; }

        [Display(Name = "Material Nombre")]
        public string MaterialNombre { get; set; }

        [Display(Name = "Código")]
        [Required]
        [Remote("JsMaterialCodigoExist", "Materiales", "Inventario",
            HttpMethod = "POST",
            ErrorMessageResourceType = typeof(Resources.Msg),
            ErrorMessageResourceName = "Material_NotExist")]
        public string MaterialCodigo { get; set; }

        [Display(Name = "Precio")]
        public decimal MaterialPrecio { get; set; }

        [Display(Name = "Unidad de Medida")]
        public string UnidadMedida { get; set; }

        public string Mode { get; set; }

        public int BodegaId { get; set; }

        [Display(Name = "Orden Compra #")]
        [Required]
        public int NumeroOC { get; set; }

        [Display(Name = "Fecha O.C.")]
        [DataType(DataType.Date)]
        public DateTime FechaOC { get; set; }

        [Display(Name = "Año O.C.")]
        public int AnioOC { get; set; }

        [Display(Name = "Nit")]                                         // Nit
        [Remote("JsProveedorNitExistOpt", "Proveedores", "Inventario",
            HttpMethod = "POST",
            ErrorMessage = "Debe ser un Nit de Proveedor valido.")]
        public string ProveedorNit { get; set; }

        [Display(Name = "Nombre")]                                      // ProveedorNombre
        public string ProveedorNombre { get; set; }

        public Int64 DespachoInventarioId { get; set; }                 // DespachoInventarioId

        [Display(Name = "Cirujano")]        
        [Required(ErrorMessage = "El campo Cirujano # es obligatorio.")]
        [UIHint("DropDownBtn")]
        public new int? CirujanoId { get; set; }                        // CirujanoId 

        [Display(Name = "Nombre Cirujano")]
        public string CirujanoNombre { get; set; }                      // CirujanoNombre

        [Display(Name = "Servicio")]
        [UIHint("DropDownBtn")]
        public int ServicioId { get; set; }                             // ServicioId

        public string ServicioNombre { get; set; }                      // ServicioNombre

        [Display(Name = "Orden Compra #")]
        [UIHint("DropDown")]
        public new Int64 OrdenCompraId { get; set; }                    // OrdenCompraId 
    }
}
