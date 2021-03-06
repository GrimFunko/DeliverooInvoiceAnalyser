﻿using System;
using System.Collections.Generic;
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
    /// Interaction logic for ConfirmationPromptWindow.xaml
    /// </summary>
    public partial class ConfirmationPromptWindow : Window
    {
        public event EventHandler<EventArgs> ConfirmationGiven;
        public ConfirmationPromptWindow(string _promptMessage)
        {
            InitializeComponent();
            promptMessage.Text = _promptMessage;
            cancelButton.Click += CancelButton_Click;
            okButton.Click += OkButton_Click;


            MinHeight = Height;
            MinWidth = Width;

            MaxHeight = Height;
            MaxWidth = Width;
            
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            ConfirmationGiven?.Invoke(this, EventArgs.Empty);
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
