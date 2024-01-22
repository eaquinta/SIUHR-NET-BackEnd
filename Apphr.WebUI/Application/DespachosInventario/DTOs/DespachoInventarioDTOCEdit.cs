using Apphr.Application.Common.Validations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Apphr.Application.DespachosInventario.DTOs
{
    public class DespachoInventarioDTOCEdit : DespachoInventarioDTOBase
    {
        public DespachoInventarioDTOCEdit()
        {
            Child = new DespachoInventarioDetalleDTOCEdit();
        }
        


        public DespachoInventarioDetalleDTOCEdit Child { get; set; }
                
        
        [Display(Name = "Departamento/Servicio")]
        [Required]
        [Remote("JsCodigoExist", "Destinos", AdditionalFields = "mode", HttpMethod = "POST", ErrorMessage = "Este departamento no se ecuentra registrado")]
        public string DepartamentoCodigo { get; set; }



        [Display(Name = "Departamento/Servicio Nombre")]
        public string DepartamentoNombre { get; set; }

    }

    public class DespachoInventarioDetalleDTOCEdit : DespachoInventarioDetalleDTOBase
    {
        [Display(Name = "Código Material")]
        [Required]
        [Remote("JsMaterialCodigoExist", "Materiales",
            HttpMethod = "POST",
            ErrorMessageResourceType = typeof(Resources.Msg),
            ErrorMessageResourceName = "Material_NotExist")]
        public string MaterialCodigo { get; set; }
        
        
        [Display(Name = "Material Nombre")]
        public string MaterialNombre { get; set; }

        
        [Display(Name = "Unidad de Medida")]
        public string MaterialUM { get; set; }
        
        
        [Display(Name = "Precio")]
        public Decimal MaterialPrecio { get; set; }
        
        
        [Display(Name = "Nit")]
        [Remote("JsProveedorNitExistOpt", "Proveedores", HttpMethod = "POST", ErrorMessage = "Debe ser un Nit de Proveedor valido.")]
        public string ProveedorNit { get; set; }
        [Display(Name = "Nombre")]
        public string ProveedorNombre { get; set; }
                
        [Display(Name = "Registro médico")]
        [Remote("JsPacienteRMExist", "Pacientes", "Medica", HttpMethod = "POST", ErrorMessage = "Debe ser un RM Válido")]
        public string PacienteRM { get; set; }

        [Display(Name = "Nombre Paciente")]
        public string PacienteNombreCompleto { get; set; }
        // Solo por Referencia para Crear o Editar
        //public int BodegaId { get; set; }
        public DateTime Fecha { get; set; }
        public int? DestinoId { get; set; }

        [Display(Name = "Existencia")]
        public decimal MaterialExistencia { get; set; }

        [Display(Name = "Solicitado")]
        public decimal MaterialSolicitado { get; set; }

        [Display(Name = "Mínimo")]
        public decimal MaterialMinimo { get; set; }

        [NumericLessThan("MaterialExistencia", "Existencia", AllowEquality = true)]
        public override decimal Cantidad { get; set; }

        [Display(Name = "Bodega")]
        [Required]
        [Remote("JsNombreExist", "Bodegas",
            HttpMethod = "POST",
            ErrorMessageResourceType = typeof(Resources.Msg),
            ErrorMessageResourceName = "Bodega_NotExist")]
        public string BodegaNombre { get; set; }


        [Display(Name = "Descripción")]
        public string BodegaDescripcion { get; set; }

    }
}
