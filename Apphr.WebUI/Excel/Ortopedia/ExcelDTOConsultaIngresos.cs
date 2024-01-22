using Apphr.WebUI.Models.Entities.Ortopedia;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.Generic;

namespace Apphr.WebUI.Excel.Ortopedia
{
    public class ExcelDTOConsultaIngresos
    {
        int rowIndex = 2;
        ExcelRange cell;
        //ExcelFill fill;
        //Border border;
        public byte[] GenerateExcel(List<ORTMovimiento> Listado)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "Ortopedia WEB";
                excelPackage.Workbook.Properties.Title = "Listado de Ingresos";
                var sheet = excelPackage.Workbook.Worksheets.Add("Listado");
                sheet.Name = "Listado de Ingresos";
                sheet.Column(2).Width = 10;//SL;
                sheet.Column(3).Width = 30;//Name;
                sheet.Column(4).Width = 20;//Roll;

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, 4].Merge = true;
                cell = sheet.Cells[rowIndex, 2];
                cell.Value = "Listado de Ingresos";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                //sheet.Cells[rowIndex, 2, rowIndex, 4].Merge = true;

                foreach (var item in Listado)
                {
                    cell = sheet.Cells[rowIndex, 2];
                    cell.Value = item.Material.Codigo;
                    //cell.Style.Font.Bold = true;
                    //cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    rowIndex = rowIndex + 1;
                }
                
                #endregion
                return excelPackage.GetAsByteArray();
            }
        }
    }
}