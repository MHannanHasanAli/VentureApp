using Guna.UI.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Venture_App
{
    public partial class AddContract : Form
    {
        public AddContract()
        {
            InitializeComponent();
        }

        private void gunaGradientButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gunaGradientButton3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog() == DialogResult.OK ) 
            { 
               gunaPictureBox2.Image = new Bitmap(openFileDialog.FileName);
                    }
        }

        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;

        private void gunaGradientButton1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(cs);
            string query = "insert into Contract values  (@ContractName, @VendorName, @Email, @Phone, @Rating, @Status, @Designation, @Photo)";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@ContractName", gunaLineTextBox1.Text);
            cmd.Parameters.AddWithValue("@VendorName", gunaLineTextBox2.Text);
            cmd.Parameters.AddWithValue("@Email", gunaLineTextBox3.Text);
            cmd.Parameters.AddWithValue("@Phone", gunaLineTextBox4.Text);
            cmd.Parameters.AddWithValue("@Rating", gunaLineTextBox5.Text);
            cmd.Parameters.AddWithValue("@Status", gunaComboBox1.SelectedItem);
            cmd.Parameters.AddWithValue("@Designation", gunaLineTextBox6.Text);
            cmd.Parameters.AddWithValue("@Photo", getphoto());

            con.Open();

            int a = cmd.ExecuteNonQuery();

            if (a > 0)
            {
                MessageBox.Show("Inserted", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Insertion Failed", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            con.Close();
            this.Close();
        }

        private byte[] getphoto()
        {
            MemoryStream stream = new MemoryStream();
            gunaPictureBox2.Image.Save(stream, gunaPictureBox2.Image.RawFormat);
            return stream.GetBuffer();
        }
    }
}
