namespace Verifalia.Api.EmailValidations.Models
{
    /// <summary>
    /// Represents the output formats which Verifalia can accept while exporting email validation entries. 
    /// </summary>
    public enum ExportedEntriesFormat
    {
        /// <summary>
        /// Comma-separated values (.csv).
        /// </summary>
        Csv,

        /// <summary>
        /// Microsoft Excel 97-2003 Worksheet (.xls).
        /// </summary>
        ExcelXls,

        /// <summary>
        /// Microsoft Excel workbook (.xslx).
        /// </summary>
        ExcelXlsx
    }
}