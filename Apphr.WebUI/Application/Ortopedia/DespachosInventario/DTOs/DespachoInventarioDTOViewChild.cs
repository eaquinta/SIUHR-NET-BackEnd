﻿using Apphr.Application.Ortopedia.ORTMovimientos.DTOs;
using System;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Ortopedia.DespachosInventario.DTOs
{
    public class DespachoInventarioDTOViewChild : MovimientoDTOBase
    {
        [Display(Name = "Registro médico")]
        public string PacienteRM { get; set; }

        [Display(Name = "Nombre Paciente")]
        public string PacienteNombreCompleto { get; set; }

        [Display(Name = "Bodega")]
        public string BodegaNombre { get; set; }

        [Display(Name = "Descripción")]                                         // BodegaDescripcion
        public string BodegaDescripcion { get; set; }

        [Display(Name = "Nombre Cirujano")]
        public string CirujanoNombre { get; set; }                      // CirujanoNombre

        [Display(Name = "Material Nombre")]
        public string MaterialNombre { get; set; }

        [Display(Name = "Código")]
        public string MaterialCodigo { get; set; }

        [Display(Name = "Precio")]
        public decimal MaterialPrecio { get; set; }

        [Display(Name = "Unidad de Medida")]
        public string UnidadMedida { get; set; }

        [Display(Name = "Orden Compra #")]
        public int NumeroOC { get; set; }

        [Display(Name = "Fecha O.C.")]
        [DataType(DataType.Date)]
        public DateTime FechaOC { get; set; }

        [Display(Name = "Año O.C.")]
        public int AnioOC { get; set; }

        [Display(Name = "Nit")]
        public string ProveedorNit { get; set; }

        [Display(Name = "Nombre")]                                      // ProveedorNombre
        public string ProveedorNombre { get; set; }
    }
}
