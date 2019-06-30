using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceAnalyserLibrary
{
    public class InvoiceModel : IInvoice
    {
        public DateTime Date { get; set; }
        public decimal? Total { get; set; }
        public decimal DropFees { get; set; }
        public decimal Adjustments { get; set; }
        public decimal Tips { get; set; }
        public decimal TransactionFee { get; set; }
        public double HoursWorked { get; set; }
        public int OrdersDelivered { get; set; }
        public string FilePath { get; set; }
        public Dictionary<DayOfWeek, int> OrdersByDay { get; set; }
        public Dictionary<DayOfWeek, double> HoursByDay { get; set; }
    }
}
