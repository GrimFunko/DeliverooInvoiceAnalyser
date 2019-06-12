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

            int invoiceYear = invoiceDate.Year;
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
    }
}
