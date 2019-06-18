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

            return output;
        }

        public double AverageHoursWorked(IInvoice[] invoices = null)
        {
            int count;
            if (invoices == null)
                count = Invoices.Length;
            else count = invoices.Length;

            if (count == 0)
                count = 1;

            return HoursWorked(invoices) / count;
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

            return val == null ? 0m : (decimal)val / count;
        }

        public decimal AverageTips(IInvoice[] invoices = null)
        {
            int count;
            if (invoices == null)
                count = Invoices.Length;
            else count = invoices.Length;

            if (count == 0)
                count = 1;

            return Tips(invoices) / count;
        }

        public decimal AverageDropFees(IInvoice[] invoices = null)
        {
            int count;
            if (invoices == null)
                count = Invoices.Length;
            else count = invoices.Length;

            if (count == 0)
                count = 1;

            return DropFees(invoices) / count;
        }

        public double AverageOrdersDelivered(IInvoice[] invoices = null)
        {
            int count;
            if (invoices == null)
                count = Invoices.Length;
            else count = invoices.Length;

            if (count == 0)
                count = 1;

            return (double)OrdersDelivered(invoices) / count;
        }
    }
}
