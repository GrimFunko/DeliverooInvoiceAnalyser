using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InvoiceAnalyserLibrary
{
    public class InvoiceAnalysis
    {
        private IInvoice[] _invoices;
        public IInvoice[] Invoices
        {
            get { return _invoices; }
        }

        public InvoiceAnalysis(FileInfo[] invoices)
        {
            PopulateInvoices(invoices);
        }

        private void PopulateInvoices(FileInfo[] invoices)
        {
            List<IInvoice> output = new List<IInvoice>();
            foreach (FileInfo file in invoices)
            {
                output.Add(AnalyserTools.GetInvoiceModel(file));
            }
            _invoices = output.ToArray();
        }

        public decimal? Total(IInvoice[] invoices = null)
        {
            return invoices == null ? SumTotal(Invoices) : SumTotal(invoices);
        }

        private decimal? SumTotal(IInvoice[] invoices)
        {
            decimal? output = 0m;
            foreach (var inv in invoices)
            {
                if (inv.Total != null)
                    output += inv.Total;
                else
                    throw new Exception("Adding null total error.");
            }
            return output;
        }

        public decimal DropFees(IInvoice[] invoices = null)
        {
            return invoices == null ? SumDropFees(Invoices) : SumDropFees(invoices);
        }

        private decimal SumDropFees(IInvoice[] invoices)
        {
            decimal output = 0m;

            foreach (var inv in invoices)        
                output += inv.DropFees;

            return output;
        }

        public decimal Tips(IInvoice[] invoices = null)
        {
            return invoices == null ? SumTips(Invoices) : SumTips(invoices);
        }

        private decimal SumTips(IInvoice[] invoices)
        {
            decimal output = 0m;

            foreach (var inv in invoices)
                output += inv.Tips;

            return output;
        }

        public decimal Adjustments(IInvoice[] invoices = null)
        {
            return invoices == null ? SumAdjustments(Invoices) : SumAdjustments(invoices);
        }

        private decimal SumAdjustments(IInvoice[] invoices)
        {
            decimal output = 0m;

            foreach (var inv in invoices)
                output += inv.Adjustments;

            return output;
        }

        public decimal TransactionFees(IInvoice[] invoices = null)
        {
            return invoices == null ? SumTransactionFees(Invoices) : SumTransactionFees(invoices);
        }

        private decimal SumTransactionFees(IInvoice[] invoices)
        {
            decimal output = 0m;

            foreach (var inv in invoices)
                output += inv.TransactionFee;

            return output;
        }

        public int OrdersDelivered(IInvoice[] invoices = null)
        {
            return invoices == null ? SumOrdersDelivered(Invoices) : SumOrdersDelivered(invoices);
        }

        private int SumOrdersDelivered(IInvoice[] invoices)
        {
            int output = 0;

            foreach (var inv in invoices)
                output += inv.OrdersDelivered;

            return output;
        }

        public double HoursWorked(IInvoice[] invoices = null)
        {
            return invoices == null ? SumHoursWorked(Invoices) : SumHoursWorked(invoices);
        }

        private double SumHoursWorked(IInvoice[] invoices)
        {
            double output = 0;

            foreach (var inv in invoices)
                output += inv.HoursWorked;

            return Math.Round(output,1,MidpointRounding.AwayFromZero);
        }

        public double AverageHoursWorked(IInvoice[] invoices = null)
        {
            int count;
            if (invoices == null)
                count = Invoices.Length;
            else count = invoices.Length;

            if (count == 0)
                count = 1;

            return Math.Round(HoursWorked(invoices) / count, 2, MidpointRounding.AwayFromZero);
        }

        public decimal AverageTotal(IInvoice[] invoices = null)
        {
            int count;
            if (invoices == null)
                count = Invoices.Length;
            else count = invoices.Length;

            var val = Total(invoices);

            if (count == 0)
                count = 1;

            return val == null ? 0m : Math.Round((decimal)val / count, 2, MidpointRounding.AwayFromZero);
        }

        public decimal AverageTips(IInvoice[] invoices = null)
        {
            int count;
            if (invoices == null)
                count = Invoices.Length;
            else count = invoices.Length;

            if (count == 0)
                count = 1;

            return Math.Round(Tips(invoices) / count, 2, MidpointRounding.AwayFromZero);
        }

        public decimal AverageDropFees(IInvoice[] invoices = null)
        {
            int count;
            if (invoices == null)
                count = Invoices.Length;
            else count = invoices.Length;

            if (count == 0)
                count = 1;

            return Math.Round(DropFees(invoices) / count, 2, MidpointRounding.AwayFromZero);
        }

        public double AverageOrdersPerInvoice(IInvoice[] invoices = null)
        {
            int count;
            if (invoices == null)
                count = Invoices.Length;
            else count = invoices.Length;

            if (count == 0)
                count = 1;

            return Math.Round((double)OrdersDelivered(invoices) / count, 2, MidpointRounding.AwayFromZero);
        }

        public decimal TipPerOrder(IInvoice[] invoices = null)
        {
            var orders = OrdersDelivered(invoices);

            return orders == 0 ? 0 : Math.Round(Tips(invoices) / orders, 2, MidpointRounding.AwayFromZero);
        }

        public decimal OrdersPerTip(IInvoice[] invoices = null)
        {
            var tips = Tips(invoices);
            var orders = (decimal)OrdersDelivered(invoices);

            return tips == 0 ? orders : Math.Round(orders / tips, 2, MidpointRounding.AwayFromZero);
        }

        public double OrdersPerHour(IInvoice[] invoices = null)
        {
            var hours = HoursWorked(invoices);
            return hours == 0 ? 0 : Math.Round(OrdersDelivered(invoices) / hours, 2, MidpointRounding.AwayFromZero);
        }

        public decimal HourlyEarnings(IInvoice[] invoices = null)
        {
            var hours = HoursWorked(invoices);
            var fees = DropFees(invoices);

            return hours == 0 ? fees :Math.Round(fees / (decimal)hours, 2, MidpointRounding.AwayFromZero);
        }

        public decimal AverageOrderFee(IInvoice[] invoices = null)
        {
            var fees = DropFees(invoices);
            var orders = OrdersDelivered(invoices);

            return orders == 0 ? 0 : Math.Round(fees / orders, 2, MidpointRounding.AwayFromZero); 
        }

        public int DaysWorked(IInvoice[] invoices = null)
        {
            return invoices == null ? SumDaysWorked(Invoices) : SumDaysWorked(invoices);
        }

        private int SumDaysWorked(IInvoice[] invoices)
        {
            int output = 0;
            foreach(var inv in invoices)
            {
                foreach(double hours in inv.HoursByDay.Values)
                {
                    if (hours > 0)
                        output += 1;
                }
            }
            return output;
        }

        public double AverageOrdersPerShift(IInvoice[] invoices = null)
        {
            var daysWorked = DaysWorked(invoices);
            if (daysWorked == 0)
                return 0;

            return Math.Round((double)OrdersDelivered(invoices) / daysWorked, 2, MidpointRounding.AwayFromZero);
        }

        public double AverageShiftLength(IInvoice[] invoices = null)
        {
            var daysWorked = DaysWorked(invoices);
            if (daysWorked == 0)
                return 0;

            return Math.Round(HoursWorked(invoices) / daysWorked, 2, MidpointRounding.AwayFromZero);
        }
    }
}
