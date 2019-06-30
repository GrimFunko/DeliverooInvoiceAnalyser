using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using InvoiceAnalyserLibrary;

using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using System.IO;
using System.Text.RegularExpressions;

namespace InvoiceAnalyserLibrary.Tests
{
    public class AnalyserToolsTests
    {
        FileHandler fh = new FileHandler(@"C:\Users\luke\Desktop\TestFolder\AnalyserTests\overall");
        string ExampleData = "Roofoods Limited " +
            "1 Cousin Lane " +
            "London, United Kingdom EC4R 3TE" +
            "Company Number: 08167130" +
            "Payment for Services Rendered: 04 July 2018 - 15 July 2018" +
            "Services Rendered: Restaurant Food and Beverage Delivery" +
            "Pay to: Luke Glasgow" +
            "Invoice Date: 17 July 2018" +
            "Services provided - 04 July 2018 - 15 July 2018" +
            "Day Date Time In Time Out Hours Worked Orders Delivered Total" +
            "Wednesday 04 July 2018 19:03 21:23 2.3h 5: £20.00 £20.00" +
            "Friday 06 July 2018 18:16 21:32 3.3h 7: £39.41 £39.41" +
            "Saturday 07 July 2018 18:10 22:15 4.1h 9: £42.24 £42.24" +
            "Sunday 08 July 2018 18:07 20:03 1.9h 5: £21.95 £21.95" +
            "Friday 13 July 2018 18:29 21:15 2.8h 5: £24.25 £24.25" +
            "Saturday 14 July 2018 18:29 22:37 4.1h 7: £31.56 £31.56" +
            "Sunday 15 July 2018 17:52 21:55 4.1h 10: £42.68 £42.68" +
            "Summary" +
            "Drop Fees £222.09" +
            "Tips £7.00" +
            "Total £229.09" +
            "1";

        [Fact]
        public void FindCorrectDateStringRegex()
        {

            DateTime expected = new DateTime(2018,07,17);

            DateTime actual = AnalyserTools.ExtractDate(ExampleData);

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
        public void ReturnCorrectTransactionFees()
        {
            FileHandler newFH = new FileHandler(@"C:\Users\luke\Desktop\TestFolder\TestAnalysis");

            decimal expected1 = -0.5m;
            FileInfo invoice = new FileInfo(@"C:\Users\luke\Desktop\TestFolder\TestAnalysis\Deliveroo Invoices\2019-20 Tax Year\Invoice 2019-05-30.pdf");
            decimal actual1 = AnalyserTools.GetTransactionFees(invoice);

            decimal expected = 0m;
            decimal actual = AnalyserTools.GetTransactionFees(newFH.PDFFiles()[0]);

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
        public void ReturnCorrectOrdersDeliveredByDay()
        {
            FileHandler newFH = new FileHandler(@"C:\Users\luke\Desktop\TestFolder\AnalyserTests\DateTests");

            Dictionary<DayOfWeek, int> expected = new Dictionary<DayOfWeek, int>();
            expected.Add(DayOfWeek.Monday, 0);
            expected.Add(DayOfWeek.Tuesday, 0);
            expected.Add(DayOfWeek.Wednesday, 5);
            expected.Add(DayOfWeek.Thursday, 0);
            expected.Add(DayOfWeek.Friday, 12);
            expected.Add(DayOfWeek.Saturday, 16);
            expected.Add(DayOfWeek.Sunday, 15);

            Dictionary<DayOfWeek, int> actual = AnalyserTools.GetOrdersByDay(newFH.InvoiceRenamingTargets()[0]);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReturnCorrectHoursWorkedByDay()
        {
            FileHandler newFH = new FileHandler(@"C:\Users\luke\Desktop\TestFolder\AnalyserTests\DateTests");

            Dictionary<DayOfWeek, double> expected = new Dictionary<DayOfWeek, double>();
            expected.Add(DayOfWeek.Monday, 0);
            expected.Add(DayOfWeek.Tuesday, 0);
            expected.Add(DayOfWeek.Wednesday, 2.3);
            expected.Add(DayOfWeek.Thursday, 0);
            expected.Add(DayOfWeek.Friday, 6.1);
            expected.Add(DayOfWeek.Saturday, 8.2);
            expected.Add(DayOfWeek.Sunday, 6);

            Dictionary<DayOfWeek, double> actual = AnalyserTools.GetHoursWorkedByDay(newFH.InvoiceRenamingTargets()[0]);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DayDetailsRegexCorrect()
        {
            FileHandler newFH = new FileHandler(@"C:\Users\luke\Desktop\TestFolder\AnalyserTests\DateTests");

            string expected1 = "Friday 06 July 2018 18:16 21:32 3.3h 7: £39.41";
            string expected2 = "Friday 13 July 2018 18:29 21:15 2.8h 5: £24.25";

            string actual1 = AnalyserTools.DayDetails(DayOfWeek.Friday, ExampleData)[0].Value;
            string actual2 = AnalyserTools.DayDetails(DayOfWeek.Friday, ExampleData)[1].Value;

            Assert.Equal(expected1, actual1);
            Assert.Equal(expected2, actual2);
        }

        [Fact]
        public void SuccessullyCreateInvoiceObject()
        {
            Dictionary<DayOfWeek, double> hours = new Dictionary<DayOfWeek, double>();
            hours.Add(DayOfWeek.Monday, 0);
            hours.Add(DayOfWeek.Tuesday, 0);
            hours.Add(DayOfWeek.Wednesday, 0);
            hours.Add(DayOfWeek.Thursday, 0);
            hours.Add(DayOfWeek.Friday, 2.9);
            hours.Add(DayOfWeek.Saturday, 1.3);
            hours.Add(DayOfWeek.Sunday, 0);

            Dictionary<DayOfWeek, int> orders = new Dictionary<DayOfWeek, int>();
            orders.Add(DayOfWeek.Monday, 0);
            orders.Add(DayOfWeek.Tuesday, 0);
            orders.Add(DayOfWeek.Wednesday, 0);
            orders.Add(DayOfWeek.Thursday, 0);
            orders.Add(DayOfWeek.Friday, 4);
            orders.Add(DayOfWeek.Saturday, 1);
            orders.Add(DayOfWeek.Sunday, 0);

            IInvoice expected = new InvoiceModel()
            {
                Total = 25.48m,
                DropFees = 22.48m,
                Tips = 3m,
                Adjustments = 0,
                Date = new DateTime(2019, 05, 28),
                HoursWorked = 4.2,
                OrdersDelivered = 5,
                TransactionFee = 0m,
                OrdersByDay = orders,
                HoursByDay = hours
            };

            IInvoice actual = AnalyserTools.GetInvoiceModel(fh.InvoiceRenamingTargets()[0]);

            Assert.Equal(expected.Date, actual.Date);
            Assert.Equal(expected.Total, actual.Total);
            Assert.Equal(expected.Adjustments, actual.Adjustments);
            Assert.Equal(expected.Tips, actual.Tips);
            Assert.Equal(expected.OrdersDelivered, actual.OrdersDelivered);
            Assert.Equal(expected.HoursWorked, actual.HoursWorked);
            Assert.Equal(expected.DropFees, actual.DropFees);
            Assert.Equal(expected.TransactionFee, actual.TransactionFee);
            Assert.Equal(expected.OrdersByDay, actual.OrdersByDay);
            Assert.Equal(expected.HoursByDay, actual.HoursByDay);
        }

    }
}
