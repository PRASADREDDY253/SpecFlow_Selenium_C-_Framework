using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace BDD_AMN.Utils
{
    public class ExcelHelper
    {

        static string path = GetDirectoryPath() + @"\Data\ExcelFiles\ExcelData.xlsx";


        public static Dictionary<string, string> ExtractRowDataAsDictionary(string sheetName, int rowNum)
        {
            XSSFWorkbook wb = new XSSFWorkbook(File.Open(path, FileMode.Open));
            var sheet = wb.GetSheet(sheetName);
            Dictionary<string, string> RowDataMap = new Dictionary<string, string>();
            List<string> KeyList = new List<string>();
            List<string> ValueList = new List<string>();

            var row = sheet.GetRow(0);
            var dataRow = sheet.GetRow(rowNum);

            for (int i = 0; i < row.LastCellNum; i++)
            {
                KeyList.Add(row.GetCell(i).StringCellValue);
                ValueList.Add(dataRow.GetCell(i).StringCellValue);
            }

            for (int j = 0; j < KeyList.Count; j++)
            {
                RowDataMap.Add(KeyList[j], ValueList[j]);
            }
            return RowDataMap;
        }

        public static List<int> GetRowIndexInfoFromExcel(string sheetName, string columnName)
        {
            List<int> DataIndices = new List<int>();
            XSSFWorkbook wb = new XSSFWorkbook(File.Open(path, FileMode.Open));
            var sheet = wb.GetSheet(sheetName);
            int colIndex = 0;
            var row = sheet.GetRow(0);
            for (int i = 0; i < row.LastCellNum; i++)
            {
                if (row.GetCell(i).StringCellValue.Equals(columnName))
                {
                    colIndex = i;
                    break;
                }
            }

            foreach (XSSFRow row1 in sheet)
            {
                XSSFCell cell = (XSSFCell)row1.GetCell(colIndex);
                if (cell.StringCellValue.Equals(columnName))
                {
                    DataIndices.Add(row1.RowNum);
                }

            }

            return DataIndices;
        }

        /// <summary>
        /// Method to read data from Excel based on scenario name
        /// </summary>
        /// <param name="Sheetname"></param>
        /// <param name="Scenarioname"></param>
        /// <returns>Dictionary</returns>
        public static Dictionary<string,string> ReadDataFromExcel(string Sheetname, string Scenarioname)
        {
            List<int> Indices = GetDataIndex(Sheetname, Scenarioname);
            return ExtractRowDataAsDictionary(Sheetname, Indices[0]);          
        }

        public static List<int> GetDataIndex(string Sheetname, string Scenarioname)
        {
            return GetRowIndexInfoFromExcel(Sheetname, Scenarioname);
           
        }

        /// <summary>
        /// This will return directory path 
        /// </summary>
        public static string GetDirectoryPath()
        {
            string directoryPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            return directoryPath;
        }
    }
}
