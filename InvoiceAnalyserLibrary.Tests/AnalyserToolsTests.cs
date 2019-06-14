using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using InvoiceAnalyserLibrary;

using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;

namespace InvoiceAnalyserLibrary.Tests
{
    public class AnalyserToolsTests
    {
        FileHandler fh = new FileHandler(@"C:\Users\luke\Desktop\TestFolder\AnalyserTests");
        
        [Fact]
        public void FindCorrectDateStringRegex()
        {
            string exampleData = "Roofoods Limited " +
                "1 Cousin Lane London, United Kingdom EC4R 3TE Company Number: 08167130" +
                "Payment for Services Rendered: 24 May 2019 - 25 May 2019" +
                "Services Rendered: Restaurant Food and Beverage Delivery" +
                "Pay to: Luke Glasgow" +
                "Invoice Date: 28 May 2019" +
                "Services provided - 24 May 2019 - 25 May 2019" +
                "Day Date Time In Time Out Hours Worked Orders Delivered Total" +
                "Friday 24 May 2019 19:11 22:05 2.9h 4: £18.55 £18.55" +
                "Saturday 25 May 2019 19:23 20:40 1.3h 1: £3.93 £3.93" +
                "Summary" +
                "Drop Fees £22.48" +
                "Tips £3.00" +
                "Total £25.48" +
                "1";

            DateTime expected = new DateTime(2019,05,28);

            DateTime actual = AnalyserTools.ExtractDate(exampleData);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReturnCorrectInvoiceDateTime()
        {
            DateTime expected = new DateTime(2019, 5, 28);

            DateTime actual = AnalyserTools.GetDate(fh.InvoiceRenamingTargets()[0]);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReturnCorrectTaxYear()
        {
            string expected1 = "2019-20 Tax Year";

            DateTime invDate1 = new DateTime(2019, 5, 28);
            string actual1 = AnalyserTools.IdentifyTaxYear(invDate1);

            string expected2 = "2019-20 Tax Year";

            DateTime invDate2 = new DateTime(2019, 4, 6);
            string actual2 = AnalyserTools.IdentifyTaxYear(invDate2);

            string expected3 = "2018-19 Tax Year";

            DateTime invDate3 = new DateTime(2019, 4, 4);
            string actual3 = AnalyserTools.IdentifyTaxYear(invDate3);

            Assert.Equal(expected1, actual1);
            Assert.Equal(expected2, actual2);
            Assert.Equal(expected3, actual3);
        }

        [Fact]
        public void ReturnCorrectInvoiceTotal()
        {
            decimal expected = 25.48m;
            decimal? actual = AnalyserTools.GetTotal(fh.InvoiceRenamingTargets()[0]);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReturnCorrectDropFees()
        {
            FileHandler newFH = new FileHandler(@"C:\Users\luke\Desktop\TestFolder\AnalyserTests\AdjustmentsOnly");

            decimal expected = 22.48m;
            decimal actual = AnalyserTools.GetDropFees(fh.InvoiceRenamingTargets()[0]);

            decimal expected1 = 0;
            decimal actual1 = AnalyserTools.GetDropFees(newFH.PDFFiles()[0]);

            Assert.Equal(expected1, actual1);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReturnCorrectAdjustments()
        {
            FileHandler newFH = new FileHandler(@"C:\Users\luke\Desktop\TestFolder\AnalyserTests\AdjustmentsOnly");

            decimal expected1 = 0;
            decimal actual1 = AnalyserTools.GetAdjustments(fh.InvoiceRenamingTargets()[0]);

            decimal expected = 25m;
            decimal actual = AnalyserTools.GetAdjustments(newFH.PDFFiles()[0]);

            Assert.Equal(expected1, actual1);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReturnCorrectTipTotals()
        {
            FileHandler newFH = new FileHandler(@"C:\Users\luke\Desktop\TestFolder\AnalyserTests\AdjustmentsOnly");

            decimal expected1 = 3m;
            decimal actual1 = AnalyserTools.GetTips(fh.InvoiceRenamingTargets()[0]);

            decimal expected = 0;
            decimal actual = AnalyserTools.GetTips(newFH.PDFFiles()[0]);
            
            Assert.Equal(expected1, actual1);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReturnCorrectHoursWorkedTotal()
        {
            FileHandler newFH = new FileHandler(@"C:\Users\luke\Desktop\TestFolder\AnalyserTests\AdjustmentsOnly");

            double expected1 = 4.2;
            double actual1 = AnalyserTools.GetHoursWorked(fh.InvoiceRenamingTargets()[0]);

            double expected = 0;
            double actual = AnalyserTools.GetHoursWorked(newFH.PDFFiles()[0]);

            Assert.Equal(expected1, actual1);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReturnCorrectOrdersDelivered()
        {
            FileHandler newFH = new FileHandler(@"C:\Users\luke\Desktop\TestFolder\AnalyserTests\AdjustmentsOnly");

            int expected1 = 5;
            int actual1 = AnalyserTools.GetOrdersDelivered(fh.InvoiceRenamingTargets()[0]);

            int expected = 0;
            int actual = AnalyserTools.GetOrdersDelivered(newFH.PDFFiles()[0]);

            Assert.Equal(expected1, actual1);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SuccessullyCreateInvoiceObject()
        {
            IInvoice expected = new InvoiceModel()
            {
                Total = 25.48m,
                DropFees = 22.48m,
                Tips = 3m,
                Adjustments = 0,
                Date = new DateTime(2019, 05, 28),
                HoursWorked = 4.2,
                OrdersDelivered = 5
            };

            IInvoice actual = AnalyserTools.GetInvoiceModel(fh.InvoiceRenamingTargets()[0]);

            Assert.Equal(expected.Date, actual.Date);
            Assert.Equal(expected.Total, actual.Total);
            Assert.Equal(expected.Adjustments, actual.Adjustments);
            Assert.Equal(expected.Tips, actual.Tips);
            Assert.Equal(expected.OrdersDelivered, actual.OrdersDelivered);
            Assert.Equal(expected.HoursWorked, actual.HoursWorked);
            Assert.Equal(expected.DropFees, actual.DropFees);
        }

    }
}
