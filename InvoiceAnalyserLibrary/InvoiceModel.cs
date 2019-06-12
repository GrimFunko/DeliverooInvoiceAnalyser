using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceAnalyserLibrary
{
    public class InvoiceModel : IInvoice
    {
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public decimal DropFees { get; set; }
        public decimal Adjustments { get; set; }
        public decimal Tips { get; set; }
        public double HoursWorked { get; set; }
        public int OrdersDelivered { get; set; }

    }
}
