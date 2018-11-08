﻿using System;
using System.Collections.Generic;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace AHK.ExcelResultsWriter
{
    public class XlsxResultsWriter : IDisposable
    {
        private readonly string resultsXlsxFileName;
        private readonly XSSFWorkbook wb;
        private readonly ISheet workSheet;

        private int nextRowIndex = 1;

        public XlsxResultsWriter(string resultsXlsxFileName)
        {
            this.resultsXlsxFileName = resultsXlsxFileName;
            this.wb = new XSSFWorkbook();
            this.workSheet = wb.CreateSheet();

            writeHeader();
        }

        private void writeHeader()
        {
            var headerRow = workSheet.CreateRow(0);
            headerRow.CreateCell(0, CellType.String).SetCellValue("Név");
            headerRow.CreateCell(1, CellType.String).SetCellValue("Neptun kód");
            headerRow.CreateCell(2, CellType.String).SetCellValue("Megjegyzés a hallgatónak");
            headerRow.CreateCell(3, CellType.String).SetCellValue("Eredmény");
        }

        public void Write(string studentId, Grader.GraderResult graderResult)
        {
            var row = workSheet.CreateRow(nextRowIndex++);
            row.CreateCell(0, CellType.String).SetCellValue("");
            row.CreateCell(1, CellType.String).SetCellValue(studentId);
            row.CreateCell(2, CellType.String).SetCellValue(formatIssuesForCell(graderResult.FailedTestNames));
            row.CreateCell(3, CellType.String).SetCellValue(graderResult.Grade);
        }

        private string formatIssuesForCell(IReadOnlyList<string> failedTestNames)
        {
            if (failedTestNames == null || failedTestNames.Count == 0)
                return string.Empty;
            else
                return "Az alábbi tesztek nem sikerültek:" + Environment.NewLine + string.Join(Environment.NewLine, failedTestNames);
        }

        public void Dispose()
        {
            using (var fs = File.Create(resultsXlsxFileName))
                wb.Write(fs);
        }
    }
}