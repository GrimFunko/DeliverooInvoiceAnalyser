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
    public partial class AnalysisWindow : Window, INotifyPropertyChanged
    {
        public InvoiceAnalysis Analyser { get; set; }
        public BindingList<IInvoice> SelectedInvoices { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        #region BindableProperties
        private double _hoursWorked;
        public double HoursWorked
        {
            get { return _hoursWorked; }
            set
            {
                _hoursWorked = value;
                OnPropertyChanged("HoursWorked");
            }
        }

        private int _ordersDelivered;
        public int OrdersDelivered
        {
            get { return _ordersDelivered; }
            set
            {
                _ordersDelivered = value;
                OnPropertyChanged("OrdersDelivered");
            }
        }

        private decimal _dropFees;
        public decimal DropFees
        {
            get { return _dropFees; }
            set
            {
                _dropFees = value;
                OnPropertyChanged("DropFees");
            }
        }

        private decimal _tips;
        public decimal Tips
        {
            get { return _tips; }
            set
            {
                _tips = value;
                OnPropertyChanged("Tips");
            }
        }

        private decimal _adjustments;
        public decimal Adjustments
        {
            get { return _adjustments; }
            set
            {
                _adjustments = value;
                OnPropertyChanged("Adjustments");
            }
        }

        private decimal _transactionFees;
        public decimal TransactionFees
        {
            get { return _transactionFees; }
            set
            {
                _transactionFees = value;
                OnPropertyChanged("TransactionFees");
            }
        }

        private decimal? _total;
        public decimal? Total
        {
            get { return _total; }
            set
            {
                _total = value;
                OnPropertyChanged("Total");
            }
        }

        private double _averageOrdersDelivered;
        public double AverageOrdersDelivered
        {
            get { return _averageOrdersDelivered; }
            set
            {
                _averageOrdersDelivered = value;
                OnPropertyChanged("AverageOrdersDelivered");
            }
        }

        private double _averageHoursWorked;
        public double AverageHoursWorked
        {
            get { return _averageHoursWorked; }
            set
            {
                _averageHoursWorked = value;
                OnPropertyChanged("AverageHoursWorked");
            }
        }

        private decimal _averageDropFees;
        public decimal AverageDropFees
        {
            get { return _averageDropFees; }
            set
            {
                _averageDropFees = value;
                OnPropertyChanged("AverageDropFees");
            }
        }

        private decimal _averageTips;
        public decimal AverageTips
        {
            get { return _averageTips; }
            set
            {
                _averageTips = value;
                OnPropertyChanged("AverageTips");
            }
        }

        private decimal? _averageTotal;
        public decimal? AverageTotal
        {
            get { return _averageTotal; }
            set
            {
                _averageTotal = value;
                OnPropertyChanged("AverageTotal");
            }
        }

        private decimal _hourlyEarnings;
        public decimal HourlyEarnings
        {
            get { return _hourlyEarnings; }
            set
            {
                _hourlyEarnings = value;
                OnPropertyChanged("HourlyEarnings");
            }
        }

        private double _ordersPerHour;
        public double OrdersPerHour
        {
            get { return _ordersPerHour; }
            set
            {
                _ordersPerHour = value;
                OnPropertyChanged("OrdersPerHour");
            }
        }

        private decimal _averageOrderFee;
        public decimal AverageOrderFee
        {
            get { return _averageOrderFee; }
            set
            {
                _averageOrderFee = value;
                OnPropertyChanged("AverageOrderFee");
            }
        }

        private decimal _averageTipPerOrder;
        public decimal AverageTipPerOrder
        {
            get { return _averageTipPerOrder; }
            set
            {
                _averageTipPerOrder = value;
                OnPropertyChanged("AverageTipPerOrder");
            }
        }

        private decimal _ordersPerTip;
        public decimal OrdersPerTip
        {
            get { return _ordersPerTip; }
            set
            {
                _ordersPerTip = value;
                OnPropertyChanged("OrdersPerTip");
            }
        }

        private int _daysWorked;
        public int DaysWorked
        {
            get { return _daysWorked; }
            set
            {
                _daysWorked = value;
                OnPropertyChanged("DaysWorked");
            }
        }

        private double _averageShiftDeliveries;
        public double AverageShiftDeliveries
        {
            get { return _averageShiftDeliveries; }
            set
            {
                _averageShiftDeliveries = value;
                OnPropertyChanged("AverageShiftDeliveries");
            }
        }

        private double _averageShiftLength;
        public double AverageShiftLength
        {
            get { return _averageShiftLength; }
            set
            {
                _averageShiftLength = value;
                OnPropertyChanged("AverageShiftLength");
            }    
        }

        #endregion

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

            MinHeight = Height;
            MaxHeight = Height;
            MinWidth = Width;
            MaxWidth = Width;
        }

        private void RefreshProperties()
        {
            var ia = SelectedInvoices.ToArray();
            HoursWorked = Analyser.HoursWorked(ia);
            OrdersDelivered = Analyser.OrdersDelivered(ia);
            DropFees = Analyser.DropFees(ia);
            Tips = Analyser.Tips(ia);
            Adjustments = Analyser.Adjustments(ia);
            TransactionFees = Analyser.TransactionFees(ia);
            Total = Analyser.Total(ia);
            AverageHoursWorked = Analyser.AverageHoursWorked(ia);
            AverageOrdersDelivered = Analyser.AverageOrdersPerInvoice(ia);
            AverageDropFees = Analyser.AverageDropFees(ia);
            AverageTips = Analyser.AverageTips(ia);
            AverageTotal = Analyser.AverageTotal(ia);
            HourlyEarnings = Analyser.HourlyEarnings(ia);
            OrdersPerHour = Analyser.OrdersPerHour(ia);
            AverageOrderFee = Analyser.AverageOrderFee(ia);
            AverageTipPerOrder = Analyser.TipPerOrder(ia);
            OrdersPerTip = Analyser.OrdersPerTip(ia);
            DaysWorked = Analyser.DaysWorked(ia);
            AverageShiftDeliveries = Analyser.AverageOrdersPerShift(ia);
            AverageShiftLength = Analyser.AverageShiftLength(ia);
        }

        private void AnalysisWindow_SelectedInvoicesChanged(object sender, EventArgs e)
        {
            selectedInvoicesGroupBox.Header = $"Selected Invoices ({SelectedInvoices.Count})";
            RefreshProperties();
            return;
        }

        private void AnalyseButton_Click(object sender, RoutedEventArgs e)
        {
            if ((startDate.SelectedDate == null || endDate.SelectedDate == null) || startDate.SelectedDate > endDate.SelectedDate)
                return; //TODO throw in error message

            selectALL.IsChecked = false; 
            var invoices = Array.FindAll(Analyser.Invoices, x => x.Date >= startDate.SelectedDate && x.Date <= endDate.SelectedDate);

            foreach (var inv in invoices)
            {
                // find checkbox with content == inv.date and check it
                CheckBox box = allInvoicesContainer.Children.OfType<CheckBox>().FirstOrDefault(x => (string)x.Content == inv.Date.ToString("ddd, dd MMM yyyy"));
                box.IsChecked = true;
            }
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
            UpdateSelectAllCheckMark();
            var box = (CheckBox)sender;
            var inv = Array.Find(Analyser.Invoices, x => x.Date == DateTime.Parse((string)box.Content));
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

        private void UpdateSelectAllCheckMark()
        {
            var myList = allInvoicesContainer.Children;

            if (myList.Count <= 1)
                return;
            if (myList.Count == 2)
            {
                selectALL.IsChecked = ((CheckBox)allInvoicesContainer.Children[1]).IsChecked;
            }
            
            bool firstChild = (bool)((CheckBox)myList[1]).IsChecked;
            for (int i = 2; i < myList.Count; i++)
            {
                if ((bool)((CheckBox)myList[i]).IsChecked == firstChild)
                    continue;
                else
                {
                    selectALL.IsChecked = null;
                    return;
                }
            }
            selectALL.IsChecked = firstChild;
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

        private void SelectedInvoicesContainer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("Double Click Event.");
            var invoice = (ListViewItem)sender;
            var name = SelectedInvoices.FirstOrDefault(x => x.Date == (((IInvoice)invoice.Content).Date)).FilePath;
            System.Diagnostics.Process.Start(name);
        }
    }
}
