using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

using System.Threading.Tasks;
using NLog;
using Utilities.LogHelper;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices.ComTypes;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.ExtendedProperties;



//using Microsoft.WindowsAzure.Storage;
//using Microsoft.WindowsAzure.Storage.Blob;


namespace Utilities.FileHelper
{
    public class FileHelper
    {
        Logger log = LogManager.GetCurrentClassLogger();
        const string dateTimeFormat = "yyyy/MM/dd_hh:mm:ss ";


        public WorkbookPart ImportExcel(string fileName)
        {
            try
            {
                string path = @"your path to excel document";

                using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    MemoryStream m_ms = new MemoryStream();
                    fs.CopyTo(m_ms);

                    SpreadsheetDocument m_Doc = SpreadsheetDocument.Open(m_ms, false);

                    return m_Doc.WorkbookPart;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message + ex.StackTrace);
            }
            return null;
        }

        public static DataTable ReadAsDataTable(string fileName)
        {
            DataTable dataTable = new DataTable();

            using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
                IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                string relationshipId = sheets.First().Id.Value;
                WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
                Worksheet workSheet = worksheetPart.Worksheet;
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                IEnumerable<Row> rows = sheetData.Descendants<Row>();

                foreach (Cell cell in rows.ElementAt(0))
                {
                    dataTable.Columns.Add(GetCellValue(spreadSheetDocument, cell));
                }

                foreach (Row row in rows)
                {
                    DataRow dataRow = dataTable.NewRow();
                    for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
                    {
                        dataRow[i] = GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(i));
                    }
                    dataTable.Rows.Add(dataRow);
                }

            }
            dataTable.Rows.RemoveAt(0);

            return dataTable;
        }

        private static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            string value = cell.CellValue.InnerXml;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
            }
            else
            {
                return value;
            }
        }



        private static int? GetColumnIndex(string cellReference)
        {
            if (string.IsNullOrEmpty(cellReference))
            {
                return null;
            }

            string columnReference = Regex.Replace(cellReference.ToUpper(), @"[\d]", string.Empty);

            int columnNumber = -1;
            int mulitplier = 1;

            foreach (char c in columnReference.ToCharArray().Reverse())
            {
                columnNumber += mulitplier * ((int)c - 64);

                mulitplier = mulitplier * 26;
            }

            return columnNumber + 1;
        }
        public static void UpdateCell(string fileName,   string outcome, string ScenarioName, string output)
        {
            DataTable dataTable = new DataTable();
            UInt32 rowIndex=0;
            using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(fileName, true))
            {
                WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
                IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                string relationshipId = sheets.First().Id.Value;
                WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
                Worksheet workSheet = worksheetPart.Worksheet;
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                IEnumerable<Row> rows = sheetData.Descendants<Row>();


                bool found=false;
                foreach (Row row in rows)
                {
                   
                    for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
                    {
                        //dataRow[i] = GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(i));
                         var value = GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(i));
                        found = value.Trim().ToLower().Contains(ScenarioName.ToLower());
                        if (found)
                            { 
                            rowIndex =  row.RowIndex;
                            break; 
                          }
                    }
                    if (found)
                    {
                     
                        break;
                    }

                }
                Cell cell1 = GetCell(worksheetPart, "D" + rowIndex.ToString());
                Cell outputcell = GetCell(worksheetPart, "E" + rowIndex.ToString());
                // Update the cell value
                cell1.CellValue = new CellValue(outcome);
                outputcell.CellValue = new CellValue(output);
                cell1.DataType = new EnumValue<CellValues>(CellValues.String);
                outputcell.DataType = new EnumValue<CellValues>(CellValues.String);

                // Save changes
                worksheetPart.Worksheet.Save();
            }
            
                
            
        }

        // Helper function to get the cell
        private static Cell GetCell(WorksheetPart worksheetPart, string cellAddress)
        {
            var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
            
            foreach (Row row in sheetData.Elements<Row>())
            {
                foreach (Cell cell in row.Elements<Cell>())
                {
                    if (cell.CellReference.Value.Contains(cellAddress))
                    {
                        return cell;
                    }
                }
            }
            return null;
        }


    }

}
