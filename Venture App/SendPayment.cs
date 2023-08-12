using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
namespace Venture_App
{
    public partial class SendPayment : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        public SendPayment()
        {
            InitializeComponent();
        }

        private void gunaGradientButton1_Click(object sender, EventArgs e)
        {
            decimal balance = 0;

            string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;

            using (SqlConnection conB = new SqlConnection(cs))
            {
                conB.Open();

                string queryB = "SELECT TOP 1 Balance FROM Transactions ORDER BY TransactionID DESC";

                using (SqlCommand cmdB = new SqlCommand(queryB, conB))
                {
                    object result = cmdB.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        balance = Convert.ToDecimal(result);
                    }
                }
            }

            decimal transactionAmount = decimal.Parse(gunaLineTextBox2.Text);
            var updatedBalance = balance-transactionAmount;

            SqlConnection con = new SqlConnection(cs);
            string query = "insert into Transactions values(@PostingDate,@TransactionDate,@TDescription,@Amount,@Balance)";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@PostingDate", gunaDateTimePicker1.Value);
            cmd.Parameters.AddWithValue("@TransactionDate", gunaDateTimePicker2.Value);
            cmd.Parameters.AddWithValue("@TDescription", gunaLineTextBox1.Text);
            cmd.Parameters.AddWithValue("@Amount", gunaLineTextBox2.Text);
            cmd.Parameters.AddWithValue("@Balance", updatedBalance);

            con.Open();

            int a =cmd.ExecuteNonQuery();

            if(a>0)
            {
                MessageBox.Show("Inserted","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Insertion Failed", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            con.Close();

            this.Close();
        }

        private void gunaGradientButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gunaPictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
