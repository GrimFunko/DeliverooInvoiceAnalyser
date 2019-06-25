using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeliverooInvoiceAnalyserForms
{
    public partial class ConfirmationPrompt : Form
    {
        public ConfirmationPrompt(string message)
        {
            InitializeComponent();

            promptMessage.Text = message;
            ConfigureLayout();
        }

        private void ConfigureLayout()
        {
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            int height = (int)Math.Floor((decimal)((screen.Height / 2) * 5 / 7));
            int width = (screen.Width / 4);
            this.Size = new Size(width, height);
            this.MinimumSize = new Size(width, height);
            this.MaximumSize = this.MinimumSize;

            promptMessage.Location = new Point(CenterX(width, promptMessage), LocationY(height, 0.1f));
            continueQuestion.Location = new Point(CenterX(width, continueQuestion), LocationY(height, 0.4f));
            yesButton.Location = new Point(CenterX(width, yesButton) - ((int)Math.Floor((yesButton.Size.Width + (0.1 * width)) / 2)), LocationY(height, 0.6f));
            noButton.Location = new Point(CenterX(width, noButton) + ((int)Math.Floor((yesButton.Size.Width + (0.1 * width)) / 2)), yesButton.Location.Y);

            promptMessage.MaximumSize = new Size((int)Math.Floor(this.Size.Width * 0.9), (int)Math.Floor((decimal)((3 * height) / 10)));
        }

        private int LocationY(int screenHeight, float percentagePosition)
        {
            return (int)Math.Floor((decimal)(screenHeight * percentagePosition));
        }

        private int CenterX(int screenWidth, Control control)
        {
            return (int)Math.Floor((decimal)((screenWidth - control.Size.Width) / 2));
        }
    }
}
