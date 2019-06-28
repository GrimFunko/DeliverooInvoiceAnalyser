using InvoiceAnalyserLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InvoiceAnalyserWPF
{
    /// <summary>
    /// Interaction logic for AnalysisWindow.xaml
    /// </summary>
    public partial class AnalysisWindow : Window
    {
        public InvoiceAnalysis Analyser { get; set; }
        public BindingList<IInvoice> SelectedInvoices { get; set; }

        public AnalysisWindow(InvoiceAnalysis invoiceAnalysis)
        {
            InitializeComponent();
            DataContext = this;
            SelectedInvoices = new BindingList<IInvoice>();
            Analyser = invoiceAnalysis;
            allInvoicesGroupBox.Header = $"All Invoices ({Analyser.Invoices.Count()})";
            analyseButton.Click += AnalyseButton_Click;
            SelectedInvoices.ListChanged += AnalysisWindow_SelectedInvoicesChanged;
            PopulateAllInvoicesContainer();
        }


        private void AnalysisWindow_SelectedInvoicesChanged(object sender, EventArgs e)
        {
            selectedInvoicesGroupBox.Header = SelectedInvoices.Count == 0 ? "Selected Invoices" : $"Selected Invoices ({SelectedInvoices.Count})";
            return;
        }

        private void AnalyseButton_Click(object sender, RoutedEventArgs e)
        {
            if ((startDate.SelectedDate == null || endDate.SelectedDate == null) || startDate.SelectedDate > endDate.SelectedDate)
                return; //throw in error message

            SelectedInvoices.Clear();
            var invoices = Array.FindAll(Analyser.Invoices, x => x.Date >= startDate.SelectedDate && x.Date <= endDate.SelectedDate);

            foreach (var inv in invoices)
                SelectedInvoices.Add(inv);
        }

        private void PopulateAllInvoicesContainer()
        {
            var orderedInvoices = Analyser.Invoices.OrderBy(x => x.Date);
            foreach (var inv in orderedInvoices)
            {
                CheckBox allCB = new CheckBox() { IsChecked = false, Content = inv.Date.ToString("ddd, dd MMM yyyy") };
                allCB.Checked += AllContainerCB_Check;
                allCB.Unchecked += AllContainerCB_Check;
                allInvoicesContainer.Children.Add(allCB);
            }
        }

        private void AllContainerCB_Check(object sender, RoutedEventArgs e)
        {
            var box = (CheckBox)sender;
            var inv = Array.Find(Analyser.Invoices, x => x.Date == DateTime.Parse((string)(box).Content));
            if (box.IsChecked == true)
            {
                if(SelectedInvoices.Count == 0)
                {
                    SelectedInvoices.Add(inv);
                    return;
                }
                else if (inv.Date >= SelectedInvoices[SelectedInvoices.Count - 1].Date)
                {
                    SelectedInvoices.Add(inv);
                    return;
                }

                var index = SelectedInvoices.IndexOf(SelectedInvoices.FirstOrDefault(x => x.Date > inv.Date));
 
                SelectedInvoices.Insert(index, inv);  
            }
            else
            {
                SelectedInvoices.Remove(inv);
            }
        }



        private void SelectALL_CheckChanged(object sender, RoutedEventArgs e)
        {
            bool check = (selectALL.IsChecked == true);
            foreach(CheckBox cb in allInvoicesContainer.Children)
            {
                if (cb == selectALL)
                    continue;
                cb.IsChecked = check;
                              
            }
        }
    }
}
