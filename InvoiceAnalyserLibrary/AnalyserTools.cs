using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;

namespace InvoiceAnalyserLibrary
{
    public class AnalyserTools
    {
        public static DateTime GetDate(FileInfo fileInfo)
        {
            using (PdfReader reader = new PdfReader(fileInfo.FullName))
            {
                using (PdfDocument doc = new PdfDocument(reader))
                {
                    var content = PdfTextExtractor.GetTextFromPage(doc.GetFirstPage());
                    return ExtractDate(content);
                }
            }
        }

        public static DateTime ExtractDate(string pdfString)
        {
            Regex regex = new Regex(@"Invoice\sDate\:\s\d{2}\s\w+\s\d{4}");
            Match match = regex.Match(pdfString);

            return match.Success ?
                (DateTime.TryParse(match.Value.Replace("Invoice Date: ", ""), out DateTime tmp) ?
                tmp : throw new Exception("Date match parse error.")) : new DateTime();
        }

        public static string IdentifyTaxYear(DateTime invoiceDate)
        {
            int startYear;
            int endYear;
            string output;

            if (invoiceDate.Month > 4 || (invoiceDate.Month == 4 && invoiceDate.Day >= 6))
            {
                startYear = invoiceDate.Year;
                endYear = invoiceDate.Year + 1;
            }
            else
            {
                startYear = invoiceDate.Year - 1;
                endYear = invoiceDate.Year;
            }

            output = $"{startYear}-{endYear.ToString().Remove(0,2)} Tax Year";

            return output;
        }

        public static decimal? GetTotal(FileInfo fileInfo)
        {
            using (PdfReader reader = new PdfReader(fileInfo.FullName))
            {
                using (PdfDocument doc = new PdfDocument(reader))
                {
                    var content = PdfTextExtractor.GetTextFromPage(doc.GetLastPage());

                    return ExtractTotal(content);
                }
            }
        }

        private static decimal? ExtractTotal(string pdfString)
        {
            Regex regex = new Regex(@"Total\s[£$€]\d+\.\d{2}");
            Match match = regex.Match(pdfString);

            return match.Success ?
                (decimal.TryParse(match.Value.Remove(0,7),out decimal tmp) ?
                (decimal?)tmp : throw new Exception("Total match parse error")) : null;
        }

        public static decimal GetDropFees(FileInfo fileInfo)
        {
            using (PdfReader reader = new PdfReader(fileInfo.FullName))
            {
                using (PdfDocument doc = new PdfDocument(reader))
                {
                    var content = PdfTextExtractor.GetTextFromPage(doc.GetLastPage());

                    return ExtractDropFees(content);
                }
            }     
        }

        public static decimal ExtractDropFees(string pdfString)
        {
            Regex regex = new Regex(@"Drop\s[Ff]ees\s[£$€]\d+\.\d{2}");
            Match match = regex.Match(pdfString);

            return match.Success ?
                (decimal.TryParse(match.Value.Remove(0, 11), out decimal tmp) ?
                    tmp : throw new Exception("DropFees match parse error.")) : 0m;
        }

        public static decimal GetAdjustments(FileInfo fileInfo)
        {
            using (PdfReader reader = new PdfReader(fileInfo.FullName))
            {
                using (PdfDocument doc = new PdfDocument(reader))
                {
                    var content = PdfTextExtractor.GetTextFromPage(doc.GetLastPage());

                    return ExtractAdjustments(content);
                }
            }
        }

        private static decimal ExtractAdjustments(string pdfString)
        {
            Regex regex = new Regex(@"Adjustments\s[£$€]\d+\.\d{2}");
            Match match = regex.Match(pdfString);

            return match.Success ?
                (decimal.TryParse(match.Value.Remove(0, 13), out decimal tmp) ?
                    tmp : throw new Exception("Adjustments match parse error.")) : 0m;      
        }

        public static decimal GetTips(FileInfo fileInfo)
        {
            using (PdfReader reader = new PdfReader(fileInfo.FullName))
            {
                using (PdfDocument doc = new PdfDocument(reader))
                {
                    var content = PdfTextExtractor.GetTextFromPage(doc.GetLastPage());

                    return ExtractTips(content);
                }
            }
        }

        private static decimal ExtractTips(string pdfString)
        {
            Regex regex = new Regex(@"Tips\s[£$€]\d+\.\d{2}");
            Match match = regex.Match(pdfString);

            return match.Success ?
                (decimal.TryParse(match.Value.Remove(0, 6), out decimal output) ? 
                    output : throw new Exception("Tips match parse error.")) : 0m;
        }

        public static decimal GetTransactionFees(FileInfo fileInfo)
        {
            using (PdfReader reader = new PdfReader(fileInfo.FullName))
            {
                using (PdfDocument doc = new PdfDocument(reader))
                {
                    var content = PdfTextExtractor.GetTextFromPage(doc.GetLastPage());

                    return ExtractTransactionFees(content);
                }
            }
        }

        private static decimal ExtractTransactionFees(string pdfString)
        {
            Regex regex = new Regex(@"Transaction\s[Ff]ee\s[£$€]-?\d+\.\d{2}");
            Match match = regex.Match(pdfString);

            return match.Success ?
                (decimal.TryParse(match.Value.Remove(0, 17), out decimal tmp) ?
                    tmp : throw new Exception("Transaction Fee match parse error.")) : 0m;
        }

        public static double GetHoursWorked(FileInfo fileInfo)
        {
            using (PdfReader reader = new PdfReader(fileInfo.FullName))
            {
                using (PdfDocument doc = new PdfDocument(reader))
                {
                    var content = PdfTextExtractor.GetTextFromPage(doc.GetFirstPage());

                    return ExtractHoursWorked(content);
                }
            }
        }

        private static double ExtractHoursWorked(string pdfString)
        {
            double sum = 0;
            Regex regex = new Regex(@"\d+(\.\d)?h");
            var matches = regex.Matches(pdfString);

            if (matches.Count == 0)
                return 0;

            foreach (Match match in matches)
                sum += double.TryParse(match.Value.Replace("h", ""), out double tmp) ? 
                    tmp : throw new Exception("Hours match parsing error.");

            return sum;
        }

        public static int GetOrdersDelivered(FileInfo fileInfo)
        {
            using (PdfReader reader = new PdfReader(fileInfo.FullName))
            {
                using (PdfDocument doc = new PdfDocument(reader))
                {
                    var content = PdfTextExtractor.GetTextFromPage(doc.GetFirstPage());

                    return ExtractOrdersDelivered(content);
                }
            }
        }

        private static int ExtractOrdersDelivered(string pdfString)
        {
            int sum = 0;
            Regex regex = new Regex(@"\d+\:\s?[£$€]");
            var matches = regex.Matches(pdfString);

            if (matches.Count == 0)
                return 0;

            foreach (Match match in matches)
            {
                var val = match.Value.IndexOf(':');
                sum += int.TryParse(match.Value.Remove(val), out int tmp) ?
                    tmp : throw new Exception("Order match parsing error.");
            }

            return sum;
        }

        public static Dictionary<DayOfWeek, int> GetOrdersByDay(FileInfo fileInfo)
        {
            using (PdfReader reader = new PdfReader(fileInfo.FullName))
            {
                using (PdfDocument doc = new PdfDocument(reader))
                {
                    var content = PdfTextExtractor.GetTextFromPage(doc.GetFirstPage());

                    return ExtractOrdersByDay(content);
                }
            }
        }

        private static Dictionary<DayOfWeek, int> ExtractOrdersByDay(string content)
        {
            Dictionary<DayOfWeek, int> output = new Dictionary<DayOfWeek, int>();
            foreach(var day in Enum.GetValues(typeof(DayOfWeek)))
            {
                var matches = DayDetails((DayOfWeek)day, content);
                if (matches.Count == 0)
                {
                    output.Add((DayOfWeek)day, 0);
                    continue;
                }

                int orderSum = 0;
                foreach (Match match in matches)
                {
                    orderSum += ExtractOrdersDelivered(match.Value);
                }
                output.Add((DayOfWeek)day, orderSum);
            }
            return output;
        }

        public static Dictionary<DayOfWeek, double> GetHoursWorkedByDay(FileInfo fileInfo)
        {
            using (PdfReader reader = new PdfReader(fileInfo.FullName))
            {
                using (PdfDocument doc = new PdfDocument(reader))
                {
                    var content = PdfTextExtractor.GetTextFromPage(doc.GetFirstPage());

                    return ExtractHoursWorkedByDay(content);
                }
            }
        }

        private static Dictionary<DayOfWeek, double> ExtractHoursWorkedByDay(string content)
        {
            Dictionary<DayOfWeek, double> output = new Dictionary<DayOfWeek, double>();
            foreach (var day in Enum.GetValues(typeof(DayOfWeek)))
            {
                var matches = DayDetails((DayOfWeek)day, content);
                if (matches.Count == 0)
                {
                    output.Add((DayOfWeek)day, 0);
                    continue;
                }

                double orderSum = 0;
                foreach (Match match in matches)
                {
                    orderSum += ExtractHoursWorked(match.Value);
                }
                output.Add((DayOfWeek)day, orderSum);
            }
            return output;
        }

        public static MatchCollection DayDetails(DayOfWeek day, string invoice)
        {
            Regex reg = new Regex(day + @"\s\d{2}\s\w{3,9}\s\d{4}\s\d{2}:\d{2}\s\d{2}:\d{2}\s\d{1,2}\.\dh\s\d+:\s£\d+\.\d+");
            MatchCollection matches = reg.Matches(invoice);

            return matches;
        }

        public static IInvoice GetInvoiceModel(FileInfo fileInfo)
        {
            using (PdfReader reader = new PdfReader(fileInfo.FullName))
            {
                using (PdfDocument doc = new PdfDocument(reader))
                {
                    var firstPage = PdfTextExtractor.GetTextFromPage(doc.GetFirstPage());
                    var lastPage = PdfTextExtractor.GetTextFromPage(doc.GetLastPage());

                    return new InvoiceModel() {
                        Date = ExtractDate(firstPage),
                        Total = ExtractTotal(lastPage),
                        Adjustments = ExtractAdjustments(lastPage),
                        Tips = ExtractTips(lastPage),
                        DropFees = ExtractDropFees(firstPage),
                        TransactionFee = ExtractTransactionFees(lastPage),
                        OrdersDelivered = ExtractOrdersDelivered(firstPage),
                        HoursWorked = ExtractHoursWorked(firstPage),
                        FilePath = fileInfo.FullName,
                        OrdersByDay = ExtractOrdersByDay(firstPage),
                        HoursByDay = ExtractHoursWorkedByDay(firstPage)
                    };

                }
            }
        }
    }
}
