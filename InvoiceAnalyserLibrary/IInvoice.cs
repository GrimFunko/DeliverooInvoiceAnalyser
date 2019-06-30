using System;
using System.Collections.Generic;

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
        decimal TransactionFee { get; set; }
        string FilePath { get; set; }
        Dictionary<DayOfWeek, int> OrdersByDay { get; set; }
        Dictionary<DayOfWeek, double> HoursByDay { get; set; }
    }
}