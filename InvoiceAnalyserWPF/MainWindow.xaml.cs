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

using System.Windows.Forms;
using InvoiceAnalyserLibrary;

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

            examplePath.Content = @"C:\Users\John\Downloads";
            examplePath2.Content = @"C:\Users\Jane\Desktop\Deliveroo Invoices";

            this.MaxHeight = Height;
            this.MaxWidth = Width;
            this.MinHeight = Height;
            this.MinWidth = Width;

            directoryPath.TextChanged += DirectoryPath_TextChanged;
            browseButton.Click += BrowseButton_Click;
            organiseButton.Click += OrganiseButton_Click;
        }

        private void OrganiseButton_Click(object sender, RoutedEventArgs e)
        {
            ConfirmationPromptWindow confirm = new ConfirmationPromptWindow("The program will attempt to rename and reorganise your invoices. No files or folders will be deleted in the process.");
            confirm.ConfirmationGiven += Confirm_ConfirmationGiven;
            confirm.ShowDialog();
         
        }

        private void Confirm_ConfirmationGiven(object sender, EventArgs e)
        {
            FileHandler handler = new FileHandler(directoryPath.Text);
            handler.OrganiseFiles();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var folderBrowser = new FolderBrowserDialog();
            folderBrowser.ShowDialog();
            directoryPath.Text = string.IsNullOrEmpty(folderBrowser.SelectedPath) ? directoryPath.Text : folderBrowser.SelectedPath;
        }

        private void DirectoryPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Directory.Exists(directoryPath.Text))
                errorMessage.Visibility = Visibility.Visible;
            else errorMessage.Visibility = Visibility.Hidden;
        }
    }
}
