namespace DeliverooInvoiceAnalyserForms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.selectedFolderPath = new System.Windows.Forms.TextBox();
            this.instructionText = new System.Windows.Forms.Label();
            this.FolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.browseButton = new System.Windows.Forms.Button();
            this.noteText = new System.Windows.Forms.Label();
            this.organiseButton = new System.Windows.Forms.Button();
            this.analyseButton = new System.Windows.Forms.Button();
            this.errorMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // selectedFolderPath
            // 
            this.selectedFolderPath.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.selectedFolderPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectedFolderPath.Location = new System.Drawing.Point(279, 240);
            this.selectedFolderPath.Name = "selectedFolderPath";
            this.selectedFolderPath.Size = new System.Drawing.Size(891, 49);
            this.selectedFolderPath.TabIndex = 0;
            this.selectedFolderPath.Text = "e.g. \'C:\\Users\\JohnSmith\\Downloads\'";
            this.selectedFolderPath.TextChanged += new System.EventHandler(this.SelectedFolderPath_TextChanged);
            // 
            // instructionText
            // 
            this.instructionText.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.instructionText.AutoSize = true;
            this.instructionText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.instructionText.Location = new System.Drawing.Point(343, 184);
            this.instructionText.Name = "instructionText";
            this.instructionText.Size = new System.Drawing.Size(764, 36);
            this.instructionText.TabIndex = 1;
            this.instructionText.Text = "Please type in, or select where your invoices are located.";
            // 
            // FolderBrowser
            // 
            this.FolderBrowser.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // browseButton
            // 
            this.browseButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.browseButton.AutoSize = true;
            this.browseButton.Location = new System.Drawing.Point(1180, 240);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(167, 58);
            this.browseButton.TabIndex = 2;
            this.browseButton.Text = "Browse...";
            this.browseButton.UseVisualStyleBackColor = true;
            // 
            // noteText
            // 
            this.noteText.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.noteText.AutoSize = true;
            this.noteText.Location = new System.Drawing.Point(328, 320);
            this.noteText.Name = "noteText";
            this.noteText.Size = new System.Drawing.Size(800, 96);
            this.noteText.TabIndex = 3;
            this.noteText.Text = "*If you have run this program before, this should be the folder\r\nthat contains \'D" +
    "eliveroo Invoices\', NOT the folder itself.\r\nI.e. if \'C:\\Desktop\\Deliveroo Invoic" +
    "es\' exists, select \'C:\\Desktop\'";
            // 
            // organiseButton
            // 
            this.organiseButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.organiseButton.AutoSize = true;
            this.organiseButton.Location = new System.Drawing.Point(464, 523);
            this.organiseButton.Name = "organiseButton";
            this.organiseButton.Size = new System.Drawing.Size(243, 51);
            this.organiseButton.TabIndex = 4;
            this.organiseButton.Text = "Organise";
            this.organiseButton.UseVisualStyleBackColor = true;
            // 
            // analyseButton
            // 
            this.analyseButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.analyseButton.AutoSize = true;
            this.analyseButton.Location = new System.Drawing.Point(743, 523);
            this.analyseButton.Name = "analyseButton";
            this.analyseButton.Size = new System.Drawing.Size(243, 51);
            this.analyseButton.TabIndex = 5;
            this.analyseButton.Text = "Analyse";
            this.analyseButton.UseVisualStyleBackColor = true;
            // 
            // errorMessage
            // 
            this.errorMessage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.errorMessage.AutoSize = true;
            this.errorMessage.ForeColor = System.Drawing.Color.Red;
            this.errorMessage.Location = new System.Drawing.Point(566, 622);
            this.errorMessage.Name = "errorMessage";
            this.errorMessage.Size = new System.Drawing.Size(318, 32);
            this.errorMessage.TabIndex = 6;
            this.errorMessage.Text = "Directory does not exist!";
            this.errorMessage.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1418, 772);
            this.Controls.Add(this.errorMessage);
            this.Controls.Add(this.analyseButton);
            this.Controls.Add(this.organiseButton);
            this.Controls.Add(this.noteText);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.instructionText);
            this.Controls.Add(this.selectedFolderPath);
            this.Name = "MainForm";
            this.Text = "Deliveroo Invoice Analyser";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox selectedFolderPath;
        private System.Windows.Forms.Label instructionText;
        private System.Windows.Forms.FolderBrowserDialog FolderBrowser;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.Label noteText;
        private System.Windows.Forms.Button organiseButton;
        private System.Windows.Forms.Button analyseButton;
        private System.Windows.Forms.Label errorMessage;
    }
}

