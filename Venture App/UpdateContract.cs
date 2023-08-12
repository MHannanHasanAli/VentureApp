using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Venture_App
{
    public partial class UpdateContract : Form
    {
        public UpdateContract()
        {
            InitializeComponent();
        }
        public UpdateContract(ContractData contract)
        {
            InitializeComponent();
            AssignValues(contract);
        }
        static int extraid = 0; 
        private void gunaGradientButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void AssignValues(ContractData contract)
        {
            extraid = contract.id;
            gunaLineTextBox1.Text = contract.Name;
            gunaLineTextBox2.Text = contract.VName;
            gunaLineTextBox3.Text = contract.Email;
            gunaLineTextBox4.Text = contract.Phone;
            gunaLineTextBox5.Text = contract.Rating.ToString();
            gunaComboBox1.SelectedItem = contract.Status;
            gunaLineTextBox6.Text = contract.Designation;
            gunaPictureBox2.Image = GetImage(contract.Image);
        }

        private Image GetImage(byte[] img)
        {
            MemoryStream ms = new MemoryStream(img);
            return Image.FromStream(ms);
        }
        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        private void gunaGradientButton1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(cs);
            string query = @"UPDATE Contract
                        SET Photo = @Photo,
                            ContractName = @ContractName,
                            VendorName = @VendorName,
                            Email = @Email,
                            Phone = @Phone,
                            Rating = @Rating,
                            Status = @Status
                        WHERE ContractID = @ID";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@ContractName", gunaLineTextBox1.Text);
            cmd.Parameters.AddWithValue("@VendorName", gunaLineTextBox2.Text);
            cmd.Parameters.AddWithValue("@Email", gunaLineTextBox3.Text);
            cmd.Parameters.AddWithValue("@Phone", gunaLineTextBox4.Text);
            cmd.Parameters.AddWithValue("@Rating", gunaLineTextBox5.Text);
            cmd.Parameters.AddWithValue("@Status", gunaComboBox1.SelectedItem);
            cmd.Parameters.AddWithValue("@Designation", gunaLineTextBox6.Text);
            cmd.Parameters.AddWithValue("@Photo", getphoto());
            cmd.Parameters.AddWithValue("@ID", extraid);

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

        private void UpdateContract_Load(object sender, EventArgs e)
        {

        }
    }
}
