using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Venture_App
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }
        public void loadform(object Form)
        {
            if (this.mainpanel.Controls.Count > 0)
                this.mainpanel.Controls.RemoveAt(0);
            Form f = Form as Form;
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            this.mainpanel.Controls.Add(f);
            this.mainpanel.Tag = f;
            f.Show();
        }

        private void gunaButton1_Click(object sender, EventArgs e)
        {
            loadform(new Dashboard());
        }

        private void gunaButton2_Click(object sender, EventArgs e)
        {
            loadform(new Contracts());
        }

        private void gunaButton3_Click(object sender, EventArgs e)
        {
            loadform(new Transaction());
        }

        private void mainpanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Home_Load(object sender, EventArgs e)
        {

        }

        private void Home_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void gunaGradientButton1_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}
