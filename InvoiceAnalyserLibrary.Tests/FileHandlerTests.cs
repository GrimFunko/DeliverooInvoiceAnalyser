using System;
using Xunit;

using InvoiceAnalyserLibrary;
using System.IO;

namespace InvoiceAnalyserLibrary.Tests
{
    public class FileHandlerTests
    {
        FileHandler fh = new FileHandler(@"C:\Users\luke\Desktop\TestFolder\TestDos");

        [Fact]
        public void CorrectFileCount()
        {
            int expected = 11;

            int actual = fh.PDFFiles.Length;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetsCorrectFileNames()
        {
            FileHandler handler = new FileHandler(@"C:\Users\luke\Desktop\TestFolder\AnalyserTests");

            string expected = @"invoice_ac19e198_0a4f_41c4_bad0_83b4b737da54_44_1559044034.pdf";

            string actual = handler.PDFFiles[0].Name;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CorrectInvoiceRegex()
        {
            string exampleFileName = "Invoice 2019-01-01";
            string exampleFileName2 = "invoice_asldjfaljsdflkajsdf_aksdjfs_123123123";
            string exampleFileName3 = "Invoice_aslkjdf_9345_9382_lakjsdlfj";
            string exampleFileName4 = "asdf a invoice_slkjdf_9345_9382_lakjsdlfj";
            

            Assert.False(fh.IsInvoiceRenamingTarget(exampleFileName));
            Assert.True(fh.IsInvoiceRenamingTarget(exampleFileName2));
            Assert.False(fh.IsInvoiceRenamingTarget(exampleFileName3));
            Assert.False(fh.IsInvoiceRenamingTarget(exampleFileName4));
        }

        [Fact]
        public void CorrectTargetInvoiceCount()
        {
            int expected = 8;

            int actual = fh.RenamingTargetInvoices.Length;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SuccessfullyCreateInvoiceDirectory()
        {
            fh.CreateInvoiceDirectory();

            var TopDirectoryExists = Directory.Exists(fh.SelectedDirectory.FullName + "/Deliveroo Invoices");

            Assert.True(TopDirectoryExists);
        } 

        [Fact]
        public void MoveIntoCorrectTaxYear()
        {
            int expected_1718 = 2;
            int expected_1920 = 4;
            int expected_1819 = 2;

            fh.CreateInvoiceDirectory();
            fh.OrganiseFiles();

            DirectoryInfo tax_1718 = new DirectoryInfo(fh.WorkingInvoiceDirectory.FullName + "/2017-18 Tax Year");
            DirectoryInfo tax_1920 = new DirectoryInfo(fh.WorkingInvoiceDirectory.FullName + "/2019-20 Tax Year");
            DirectoryInfo tax_1819 = new DirectoryInfo(fh.WorkingInvoiceDirectory.FullName + "/2018-19 Tax Year");

            var actual_1718 = tax_1718.GetFiles().Length;
            var actual_1920 = tax_1920.GetFiles().Length;
            var actual_1819 = tax_1819.GetFiles().Length;

            Assert.Equal(expected_1718, actual_1718);
            Assert.Equal(expected_1819, actual_1819);
            Assert.Equal(expected_1920, actual_1920);
        }

        [Fact]
        public void CorrectFileRename()
        {
            FileHandler newFileHandler = new FileHandler(@"C:\Users\luke\Desktop\TestFolder\AnalyserTests");
            int attempt = 0;
            DateTime dt = AnalyserTools.GetDate(newFileHandler.RenamingTargetInvoices[0]);

            string expected = "Invoice 2019-05-28";

            string actualFileName = fh.CreateFileName(dt, attempt);

            Assert.Equal(expected, actualFileName);
        }
    }
}
