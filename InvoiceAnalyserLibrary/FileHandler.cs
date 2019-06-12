using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace InvoiceAnalyserLibrary
{
    public class FileHandler
    {
        private FileInfo[] _pdfFiles;
        public FileInfo[] PDFFiles
        {
            get { return _pdfFiles; }
            set { _pdfFiles = value; }
        }

        private FileInfo[] _renamingTargetInvoices;
        public FileInfo[] RenamingTargetInvoices
        {
            get { return _renamingTargetInvoices; }
            set { _renamingTargetInvoices = value; }
        }

        public DirectoryInfo SelectedDirectory;

        public DirectoryInfo WorkingInvoiceDirectory;

        public FileHandler(string _directoryPath)
        {
            SelectedDirectory = new DirectoryInfo(_directoryPath);
            WorkingInvoiceDirectory = new DirectoryInfo(_directoryPath + "/Deliveroo Invoices");
            PDFFiles = GetPDFFiles();
            RenamingTargetInvoices = GetRenamingTargetInvoices();
        }

        internal FileInfo[] GetPDFFiles()
        {
            var files = SelectedDirectory.GetFiles("*.pdf", SearchOption.AllDirectories);

            return files;
        }

        internal FileInfo[] GetRenamingTargetInvoices()
        {
            if (PDFFiles == null || PDFFiles.Length == 0)
                throw new Exception("No PDFs found.");

            List<FileInfo> output = new List<FileInfo>();
            foreach(FileInfo file in PDFFiles)
            {
                if (IsInvoiceRenamingTarget(file.Name))
                    output.Add(file);
            }

            return output.ToArray();
        }

        public bool IsInvoiceRenamingTarget(string fileName)
        {
            Regex regex = new Regex(@"^invoice_");

            return regex.IsMatch(fileName);
        }

        public void CreateInvoiceDirectory()
        {
            if (!WorkingInvoiceDirectory.Exists)
                WorkingInvoiceDirectory.Create();
        }

        public void OrganiseFiles()
        {
            CreateInvoiceDirectory();
            foreach(FileInfo invoice in RenamingTargetInvoices)
            {
                var date = AnalyserTools.GetDate(invoice);
                var TaxYear = AnalyserTools.IdentifyTaxYear(date);

                WorkingInvoiceDirectory.CreateSubdirectory(TaxYear);

                MoveFile(invoice, date, TaxYear, 0);
            }
        }

        public void MoveFile(FileInfo invoice, DateTime invoiceDate, string taxYear, int renamingAttempt)
        {
            var newFileName = CreateFileName(invoiceDate, renamingAttempt);
            try
            {
                File.Move(invoice.FullName, $"{WorkingInvoiceDirectory.FullName}\\{taxYear}\\{newFileName}.pdf");
            }
            catch (IOException)
            {
                MoveFile(invoice, invoiceDate, taxYear, renamingAttempt + 1);
            }
        }

        public string CreateFileName(DateTime dt, int attempt)
        {
            return attempt == 0 ? $"Invoice {dt.ToString("yyyy-MM-dd")}" : $"Invoice {dt.ToString("yyyy-MM-dd")} ({attempt})";
        }
    }
}
