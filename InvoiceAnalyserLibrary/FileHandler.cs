using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace InvoiceAnalyserLibrary
{
    public class FileHandler
    {
        public DirectoryInfo SelectedDirectory;

        public DirectoryInfo WorkingInvoiceDirectory;

        public FileHandler(string _directoryPath)
        {
            SelectedDirectory = new DirectoryInfo(_directoryPath);
            WorkingInvoiceDirectory = new DirectoryInfo(_directoryPath + "/Deliveroo Invoices");
        }

        public FileInfo[] PDFFiles()
        {
            var files = SelectedDirectory.GetFiles("*.pdf", SearchOption.AllDirectories);

            return files;
        }

        static FileInfo[] PDFFiles(DirectoryInfo directory)
        {
            var files = directory.GetFiles("*.pdf", SearchOption.AllDirectories);

            return files;
        }

        public FileInfo[] InvoiceRenamingTargets()
        {
            var pdfs = PDFFiles();
            if (pdfs == null || pdfs.Length == 0)
                throw new Exception("No PDFs found.");

            List<FileInfo> output = new List<FileInfo>();
            foreach(FileInfo file in pdfs)
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
            foreach(FileInfo invoice in InvoiceRenamingTargets())
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

        public FileInfo[] InvoiceFiles()
        {
            var pdfs = PDFFiles(WorkingInvoiceDirectory);
            if (pdfs == null || pdfs.Length == 0)
                throw new Exception("No PDFs found.");

            List<FileInfo> output = new List<FileInfo>();
            foreach (FileInfo file in pdfs)
            {
                if (IsInvoice(file.Name))
                    output.Add(file);
            }

            return output.ToArray();
        }

        public bool IsInvoice(string fileName)
        {
            Regex reg = new Regex(@"^Invoice \d{4}-\d{2}-\d{2}\.pdf$");

            return reg.IsMatch(fileName);
        }
    }
}
