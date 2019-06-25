using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InvoiceAnalyserWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            noteText.Text = @"*If you have run this program before, this should be the folder that contains 'Deliveroo Invoices', NOT the folder itself." +
                    "\n" +@"I.e. if 'C:\Desktop\Deliveroo Invoices' exists, select 'C:\Desktop'";

            this.MaxHeight = Height;
            this.MaxWidth = Width;
            this.MinHeight = Height;
            this.MinWidth = Width;

            directoryPath.TextChanged += DirectoryPath_TextChanged;
        }

        private void DirectoryPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Directory.Exists(directoryPath.Text))
                errorMessage.Visibility = Visibility.Visible;
            else errorMessage.Visibility = Visibility.Hidden;
        }
    }
}
