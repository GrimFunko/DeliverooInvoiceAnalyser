﻿using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using InvoiceAnalyserLibrary;

namespace InvoiceAnalyserLibrary.Tests
{
    public class InvoiceAnalysisTests
    {
        FileHandler fh = new FileHandler(@"C:\Users\luke\Desktop\TestFolder\TestAnalysis");

        [Fact]
        public void InvoicesPropertyCorrectlyPopulated()
        {
            InvoiceAnalysis ia = new InvoiceAnalysis(fh.InvoiceFiles());

            int expected = 6;

            int actual = ia.Invoices.Length;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CorrectTotalReturned_AllInvoices()
        {
            InvoiceAnalysis ia = new InvoiceAnalysis(fh.InvoiceFiles());
            decimal expected = 321.90m;
            decimal? actual = ia.Total();

            Assert.Equal(expected, actual); 
        }

        [Fact]
        public void CorrectTotalReturned_1819TaxYear()
        {
            InvoiceAnalysis ia = new InvoiceAnalysis(fh.InvoiceFiles());
            decimal expected = 116.06m;
            decimal? actual = ia.Total(Array.FindAll(ia.Invoices, x => (x.Date <= new DateTime(2019, 4, 5) && x.Date >= new DateTime(2018, 4, 6))));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CorrectTotalReturned_1920TaxYear()
        {
            InvoiceAnalysis ia = new InvoiceAnalysis(fh.InvoiceFiles());
            decimal expected = 199.84m;
            decimal? actual = ia.Total(Array.FindAll(ia.Invoices, x => (x.Date <= new DateTime(2020, 4, 5) && x.Date >= new DateTime(2019, 4, 6))));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CorrectDropFeesReturned()
        {
            InvoiceAnalysis ia = new InvoiceAnalysis(fh.InvoiceFiles());
            // All
            decimal expected = 317.40m;
            decimal actual = ia.DropFees();

            Assert.Equal(expected, actual);

            // 18-19 Tax Year
            decimal expected1 = 116.06m;
            decimal actual1 = ia.DropFees(Array.FindAll(ia.Invoices, x => (x.Date <= new DateTime(2019, 4, 5) && x.Date >= new DateTime(2018, 4, 6))));

            Assert.Equal(expected1, actual1);
        }

        [Fact]
        public void CorrectTipsReturned()
        {
            InvoiceAnalysis ia = new InvoiceAnalysis(fh.InvoiceFiles());
            // All
            decimal expected = 3m;
            decimal actual = ia.Tips();

            Assert.Equal(expected, actual);

            // 18-19 Tax Year
            decimal expected1 = 0m;
            decimal actual1 = ia.Tips(Array.FindAll(ia.Invoices, x => (x.Date <= new DateTime(2019, 4, 5) && x.Date >= new DateTime(2018, 4, 6))));

            Assert.Equal(expected1, actual1);
        }

        [Fact]
        public void CorrectAdjustmentsReturned()
        {
            InvoiceAnalysis ia = new InvoiceAnalysis(fh.InvoiceFiles());
            // All
            decimal expected = 2m;
            decimal actual = ia.Adjustments();

            Assert.Equal(expected, actual);

            // 18-19 Tax Year
            decimal expected1 = 0m;
            decimal actual1 = ia.Adjustments(Array.FindAll(ia.Invoices, x => (x.Date <= new DateTime(2019, 4, 5) && x.Date >= new DateTime(2018, 4, 6))));

            Assert.Equal(expected1, actual1);
        }

        [Fact]
        public void CorrectTransactionFeesReturned()
        {
            InvoiceAnalysis ia = new InvoiceAnalysis(fh.InvoiceFiles());
            // All
            decimal expected = -0.5m;
            decimal actual = ia.TransactionFees();

            Assert.Equal(expected, actual);

            // 18-19 Tax Year
            decimal expected1 = 0m;
            decimal actual1 = ia.TransactionFees(Array.FindAll(ia.Invoices, x => (x.Date <= new DateTime(2019, 4, 5) && x.Date >= new DateTime(2018, 4, 6))));

            Assert.Equal(expected1, actual1);
        }

        [Fact]
        public void CorrectOrdersDeliveredReturned()
        {
            InvoiceAnalysis ia = new InvoiceAnalysis(fh.InvoiceFiles());
            // All
            int expected = 74;
            int actual = ia.OrdersDelivered();

            Assert.Equal(expected, actual);

            // 18-19 Tax Year
            int expected1 = 28;
            int actual1 = ia.OrdersDelivered(Array.FindAll(ia.Invoices, x => (x.Date <= new DateTime(2019, 4, 5) && x.Date >= new DateTime(2018, 4, 6))));

            Assert.Equal(expected1, actual1);
        }

        [Fact]
        public void CorrectHoursWorkedReturned()
        {
            InvoiceAnalysis ia = new InvoiceAnalysis(fh.InvoiceFiles());
            // All
            double expected = 37.6;
            double actual = ia.HoursWorked();

            Assert.Equal(expected, actual);

            // 18-19 Tax Year
            double expected1 = 14.4;
            double actual1 = ia.HoursWorked(Array.FindAll(ia.Invoices, x => (x.Date <= new DateTime(2019, 4, 5) && x.Date >= new DateTime(2018, 4, 6))));

            Assert.Equal(expected1, actual1, 1);
        }

        [Fact]
        public void AverageHoursWorkedReturned()
        {
            InvoiceAnalysis ia = new InvoiceAnalysis(fh.InvoiceFiles());
            // All
            double expected = 6.27;
            double actual = ia.AverageHoursWorked();

            Assert.Equal(expected, actual, 2);

            // 18-19 Tax Year
            double expected1 = 7.2;
            double actual1 = ia.AverageHoursWorked(Array.FindAll(ia.Invoices, x => (x.Date <= new DateTime(2019, 4, 5) && x.Date >= new DateTime(2018, 4, 6))));

            Assert.Equal(expected1, actual1, 2);
        }

        [Fact]
        public void AverageTotalReturned()
        {
            InvoiceAnalysis ia = new InvoiceAnalysis(fh.InvoiceFiles());
            // All
            decimal expected = 53.65m;
            decimal actual = ia.AverageTotal();

            Assert.Equal(expected, actual, 2);

            // 18-19 Tax Year
            decimal expected1 = 58.03m;
            decimal actual1 = ia.AverageTotal(Array.FindAll(ia.Invoices, x => (x.Date <= new DateTime(2019, 4, 5) && x.Date >= new DateTime(2018, 4, 6))));

            Assert.Equal(expected1, actual1, 2);
        }

        [Fact]
        public void AverageTipsReturned()
        {
            InvoiceAnalysis ia = new InvoiceAnalysis(fh.InvoiceFiles());
            // All
            decimal expected = 0.50m;
            decimal actual = ia.AverageTips();

            Assert.Equal(expected, actual, 2);

            // 18-19 Tax Year
            decimal expected1 = 0m;
            decimal actual1 = ia.AverageTips(Array.FindAll(ia.Invoices, x => (x.Date <= new DateTime(2019, 4, 5) && x.Date >= new DateTime(2018, 4, 6))));

            Assert.Equal(expected1, actual1, 2);
        }

        [Fact]
        public void AverageDropFeesReturned()
        {
            InvoiceAnalysis ia = new InvoiceAnalysis(fh.InvoiceFiles());
            // All
            decimal expected = 52.90m;
            decimal actual = ia.AverageDropFees();

            Assert.Equal(expected, actual, 2);

            // 18-19 Tax Year
            decimal expected1 = 58.03m;
            decimal actual1 = ia.AverageDropFees(Array.FindAll(ia.Invoices, x => (x.Date <= new DateTime(2019, 4, 5) && x.Date >= new DateTime(2018, 4, 6))));

            Assert.Equal(expected1, actual1, 2);
        }

        [Fact]
        public void AverageOrdersDeliveredReturned()
        {
            InvoiceAnalysis ia = new InvoiceAnalysis(fh.InvoiceFiles());
            // All
            double expected = 12.3;
            double actual = ia.AverageOrdersDelivered();

            Assert.Equal(expected, actual, 1);

            // 18-19 Tax Year
            double expected1 = 14;
            double actual1 = ia.AverageOrdersDelivered(Array.FindAll(ia.Invoices, x => (x.Date <= new DateTime(2019, 4, 5) && x.Date >= new DateTime(2018, 4, 6))));

            Assert.Equal(expected1, actual1, 1);
        }
    }
}
