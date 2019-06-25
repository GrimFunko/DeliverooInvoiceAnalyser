using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using InvoiceAnalyserLibrary;

namespace DeliverooInvoiceAnalyserForms
{
    public partial class MainForm : Form
    {
        FileHandler handler { get; set; }

        public MainForm()
        {
            InitializeComponent();

            ConfigureLayout();
            browseButton.Click += BrowseButton_Click;

            organiseButton.Click += OrganiseButton_Click;
            //Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            //this.Size = new Size( (screen.Width * 2) / 3, (screen.Height * 5) / 6);
        }

        private void ConfigureLayout()
        {
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            int width = screen.Width / 2;
            int height = screen.Height / 2;
            this.Size = new Size(width, height);
            this.MinimumSize = new Size(width, height);

            instructionText.Location = new Point(CenterPointCalculator(width, instructionText), (int)Math.Floor(0.2*height));
            selectedFolderPath.Location = new Point(CenterPointCalculator(width, selectedFolderPath), (int)Math.Floor(0.3 * height));
            browseButton.Location = new Point(selectedFolderPath.Location.X + 10 + selectedFolderPath.Size.Width, selectedFolderPath.Location.Y);
            noteText.Location = new Point(CenterPointCalculator(width, noteText), (int)Math.Floor(0.4 * height));
            organiseButton.Location = new Point(CenterPointCalculator(width, organiseButton) - ((int)Math.Floor((organiseButton.Size.Width + (0.02 * width)) / 2)), (int)Math.Floor(0.58 * height));
            analyseButton.Location = new Point(CenterPointCalculator(width, analyseButton) + ((int)Math.Floor((organiseButton.Size.Width + (0.02 * width)) / 2)), organiseButton.Location.Y);
            errorMessage.Location = new Point(CenterPointCalculator(width, errorMessage), (int)Math.Floor(0.7 * height));
        }

        private int CenterPointCalculator(int pageWidth, Control control)
        {
            return (int)Math.Floor((decimal)((pageWidth - control.Size.Width) / 2));
        }

        private void OrganiseButton_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(selectedFolderPath.Text))
            {
                handler = new FileHandler(selectedFolderPath.Text);
                bool confirm = ShowConfirmationDialogue("I will now attempt to rename and organise your invoices.");
            }
            else errorMessage.Visible = true;
        }

        private bool ShowConfirmationDialogue(string message)
        {
            Form confirmation = new ConfirmationPrompt(message);
            
            confirmation.ShowDialog();
            return true;
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowser.ShowDialog();
            if (!String.IsNullOrEmpty(FolderBrowser.SelectedPath))
            {
                selectedFolderPath.Text = FolderBrowser.SelectedPath;
                errorMessage.Visible = false;
            }
        }

        private void SelectedFolderPath_TextChanged(object sender, EventArgs e)
        {
            if (!Directory.Exists(selectedFolderPath.Text))
                errorMessage.Visible = true;
            else
                errorMessage.Visible = false;
        }
    }
}
