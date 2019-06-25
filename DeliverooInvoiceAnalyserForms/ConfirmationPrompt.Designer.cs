namespace DeliverooInvoiceAnalyserForms
{
    partial class ConfirmationPrompt
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
            this.promptMessage = new System.Windows.Forms.Label();
            this.continueQuestion = new System.Windows.Forms.Label();
            this.yesButton = new System.Windows.Forms.Button();
            this.noButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // promptMessage
            // 
            this.promptMessage.AutoSize = true;
            this.promptMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.900001F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.promptMessage.Location = new System.Drawing.Point(205, 100);
            this.promptMessage.Name = "promptMessage";
            this.promptMessage.Size = new System.Drawing.Size(257, 39);
            this.promptMessage.TabIndex = 0;
            this.promptMessage.Text = "placeholder text";
            this.promptMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // continueQuestion
            // 
            this.continueQuestion.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.continueQuestion.AutoSize = true;
            this.continueQuestion.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.continueQuestion.Location = new System.Drawing.Point(90, 188);
            this.continueQuestion.Margin = new System.Windows.Forms.Padding(0);
            this.continueQuestion.Name = "continueQuestion";
            this.continueQuestion.Size = new System.Drawing.Size(511, 44);
            this.continueQuestion.TabIndex = 1;
            this.continueQuestion.Text = "Would you like to continue?";
            // 
            // yesButton
            // 
            this.yesButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.yesButton.AutoSize = true;
            this.yesButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.yesButton.Location = new System.Drawing.Point(156, 321);
            this.yesButton.Name = "yesButton";
            this.yesButton.Size = new System.Drawing.Size(120, 54);
            this.yesButton.TabIndex = 2;
            this.yesButton.Text = "Yes";
            this.yesButton.UseVisualStyleBackColor = true;
            // 
            // noButton
            // 
            this.noButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.noButton.AutoSize = true;
            this.noButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.noButton.Location = new System.Drawing.Point(411, 321);
            this.noButton.Name = "noButton";
            this.noButton.Size = new System.Drawing.Size(120, 54);
            this.noButton.TabIndex = 3;
            this.noButton.Text = "No";
            this.noButton.UseVisualStyleBackColor = true;
            // 
            // ConfirmationPrompt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(801, 590);
            this.Controls.Add(this.noButton);
            this.Controls.Add(this.yesButton);
            this.Controls.Add(this.continueQuestion);
            this.Controls.Add(this.promptMessage);
            this.Name = "ConfirmationPrompt";
            this.Text = "Confirmation";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label promptMessage;
        private System.Windows.Forms.Label continueQuestion;
        private System.Windows.Forms.Button yesButton;
        private System.Windows.Forms.Button noButton;
    }
}