using System;

namespace InvoiceAnalyserLibrary
{
    public interface IInvoice
    {
        decimal Adjustments { get; set; }
        DateTime Date { get; set; }
        decimal DropFees { get; set; }
        double HoursWorked { get; set; }
        int OrdersDelivered { get; set; }
        decimal Tips { get; set; }
        decimal? Total { get; set; }
    }
}