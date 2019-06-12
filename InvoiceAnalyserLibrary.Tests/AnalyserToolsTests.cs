using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using InvoiceAnalyserLibrary;

namespace InvoiceAnalyserLibrary.Tests
{
    public class AnalyserToolsTests
    {
        FileHandler fh = new FileHandler(@"C:\Users\luke\Desktop\TestFolder\GetFileNameTest");
        
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

            string expected = "28 May 2019";

            string actual = AnalyserTools.ExtractDateString(exampleData);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReturnCorrectInvoiceDateTime()
        {
            DateTime expected = new DateTime(2019, 5, 28);

            DateTime actual = AnalyserTools.GetDate(fh.RenamingTargetInvoices[0]);

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

    }
}
