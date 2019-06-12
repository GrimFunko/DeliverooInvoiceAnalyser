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
            DateTime output = new DateTime();
            using (PdfReader reader = new PdfReader(fileInfo.FullName))
            {
                using (PdfDocument doc = new PdfDocument(reader))
                {
                    var content = PdfTextExtractor.GetTextFromPage(doc.GetFirstPage());
                    var success = DateTime.TryParse(ExtractDateString(content), out output);
                    if (!success)
                        throw new Exception("DateTime.Parse Error.");
                }
            }
            return output;
        }

        public static string ExtractDateString(string pdfString)
        {
            Regex regex = new Regex(@"Invoice\sDate\:\s\d{2}\s\w+\s\d{4}");
            Match match = regex.Match(pdfString);

            if (match.Success)
                return match.Value.Replace("Invoice Date: ", "");

            return null;
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

        public static decimal GetTotal(FileInfo fileInfo)
        {
            decimal output = 0;
            using (PdfReader reader = new PdfReader(fileInfo.FullName))
            {
                using (PdfDocument doc = new PdfDocument(reader))
                {
                    var content = PdfTextExtractor.GetTextFromPage(doc.GetLastPage());
                    var success = decimal.TryParse(ExtractTotal(content), out output);

                    if (!success)
                        throw new Exception("decimal.Parse Error");
                }
            }
            return output;
        }

        private static string ExtractTotal(string pdfString)
        {
            Regex regex = new Regex(@"Total\s[£$€]\d+\.\d{2}");
            Match match = regex.Match(pdfString);

            if (match.Success)
                return match.Value.Remove(0,7);

            return null;
        }

        public static decimal? GetDropFees(FileInfo fileInfo)
        {
            decimal output;
            using (PdfReader reader = new PdfReader(fileInfo.FullName))
            {
                using (PdfDocument doc = new PdfDocument(reader))
                {
                    var content = PdfTextExtractor.GetTextFromPage(doc.GetLastPage());

                    return decimal.TryParse(ExtractDropFees(content), out output) ?
                        (decimal?)output : null;
                }
            }
            
        }

        public static string ExtractDropFees(string pdfString)
        {
            Regex regex = new Regex(@"Drop\sFees\s[£$€]\d+\.\d{2}");
            Match match = regex.Match(pdfString);

            if (match.Success)
                return match.Value.Remove(0, 11);

            return "0";
        }

        public static decimal? GetAdjustments(FileInfo fileInfo)
        {
            decimal output;
            using (PdfReader reader = new PdfReader(fileInfo.FullName))
            {
                using (PdfDocument doc = new PdfDocument(reader))
                {
                    var content = PdfTextExtractor.GetTextFromPage(doc.GetLastPage());

                    return decimal.TryParse(ExtractAdjustments(content), out output) ?
                        (decimal?)output : null;
                }
            }
        }

        private static string ExtractAdjustments(string pdfString)
        {
            Regex regex = new Regex(@"Adjustments\s[£$€]\d+\.\d{2}");
            Match match = regex.Match(pdfString);

            if (match.Success)
                return match.Value.Remove(0, 13);

            return "0";
        }

        public static decimal? GetTips(FileInfo fileInfo)
        {
            decimal output;
            using (PdfReader reader = new PdfReader(fileInfo.FullName))
            {
                using (PdfDocument doc = new PdfDocument(reader))
                {
                    var content = PdfTextExtractor.GetTextFromPage(doc.GetLastPage());

                    return decimal.TryParse(ExtractTips(content), out output) ?
                        (decimal?)output : null;
                }
            }
        }

        private static string ExtractTips(string pdfString)
        {
            Regex regex = new Regex(@"Tips\s[£$€]\d+\.\d{2}");
            Match match = regex.Match(pdfString);

            if (match.Success)
                return match.Value.Remove(0, 6);

            return "0";
        }

        public static double? GetHoursWorked(FileInfo fileInfo)
        {
            double output;
            using (PdfReader reader = new PdfReader(fileInfo.FullName))
            {
                using (PdfDocument doc = new PdfDocument(reader))
                {
                    var content = PdfTextExtractor.GetTextFromPage(doc.GetFirstPage());

                    return double.TryParse(ExtractHoursWorked(content), out output) ?
                        (double?)output : null;
                }
            }
        }

        private static string ExtractHoursWorked(string pdfString)
        {
            double sum = 0;
            Regex regex = new Regex(@"\d+(\.\d)?h");
            var matches = regex.Matches(pdfString);

            if (matches.Count == 0)
                return "0";

            foreach (Match match in matches)
                sum += double.Parse(match.Value.Replace("h", ""));

            return sum.ToString();
        }

        public static int? GetOrdersDelivered(FileInfo fileInfo)
        {
            int output;
            using (PdfReader reader = new PdfReader(fileInfo.FullName))
            {
                using (PdfDocument doc = new PdfDocument(reader))
                {
                    var content = PdfTextExtractor.GetTextFromPage(doc.GetFirstPage());

                    return int.TryParse(ExtractOrdersDelivered(content), out output) ?
                        (int?)output : null;
                }
            }
        }

        private static string ExtractOrdersDelivered(string pdfString)
        {
            int sum = 0;
            Regex regex = new Regex(@"\d+\:\s?[£$€]");
            var matches = regex.Matches(pdfString);

            if (matches.Count == 0)
                return "0";

            foreach (Match match in matches)
            {
                var val = match.Value.IndexOf(':');
                sum += int.Parse(match.Value.Remove(val));
            }

            return sum.ToString();
        }
    }
}
