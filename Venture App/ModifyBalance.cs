using Guna.UI.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Venture_App
{
    public partial class ModifyBalance : Form
    {
        public ModifyBalance()
        {
            InitializeComponent();
        }

        private void gunaGradientButton1_Click(object sender, EventArgs e)
        {
            var balance = gunaLineTextBox1.Text;
            UpdateBalanceForLastRecord(balance);
        }
        private void gunaGradientButton2_Click(object sender, EventArgs e)
        {
            var balance = gunaLineTextBox1.Text;
            WithdrawBalanceForLastRecord(balance);
        }
        public void UpdateBalanceForLastRecord(string newBalanceString)
        {
            if (decimal.TryParse(newBalanceString, out decimal newBalance))
            {
                string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;

                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    // Get the current balance from the last record
                    string selectQuery = "SELECT TOP 1 Balance FROM Transactions ORDER BY TransactionID DESC";

                    using (SqlCommand selectCmd = new SqlCommand(selectQuery, con))
                    {
                        decimal currentBalance = (decimal)selectCmd.ExecuteScalar();

                        // Calculate the updated balance
                        decimal updatedBalance = currentBalance + newBalance;

                        // Update the balance in the last record
                        string updateQuery = "UPDATE Transactions SET Balance = @UpdatedBalance WHERE TransactionID = (SELECT MAX(TransactionID) FROM Transactions)";

                        using (SqlCommand updateCmd = new SqlCommand(updateQuery, con))
                        {
                            updateCmd.Parameters.AddWithValue("@UpdatedBalance", updatedBalance);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            else
            {
                // Handle parsing error
                // Display an error message or take appropriate action
            }
        }
        public void WithdrawBalanceForLastRecord(string newBalanceString)
        {
            if (decimal.TryParse(newBalanceString, out decimal newBalance))
            {
                string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;

                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    // Get the current balance from the last record
                    string selectQuery = "SELECT TOP 1 Balance FROM Transactions ORDER BY TransactionID DESC";

                    using (SqlCommand selectCmd = new SqlCommand(selectQuery, con))
                    {
                        decimal currentBalance = (decimal)selectCmd.ExecuteScalar();

                        // Calculate the updated balance
                        decimal updatedBalance = currentBalance - newBalance;

                        // Update the balance in the last record
                        string updateQuery = "UPDATE Transactions SET Balance = @UpdatedBalance WHERE TransactionID = (SELECT MAX(TransactionID) FROM Transactions)";

                        using (SqlCommand updateCmd = new SqlCommand(updateQuery, con))
                        {
                            updateCmd.Parameters.AddWithValue("@UpdatedBalance", updatedBalance);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            else
            {
                // Handle parsing error
                // Display an error message or take appropriate action
            }
        }
    }
}
