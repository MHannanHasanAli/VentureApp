using LiveCharts.WinForms;
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
using System.Windows.Forms.DataVisualization.Charting;

namespace Venture_App
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            var sumAmounts = GetSumAmountsForCurrentYear();
                gunaLabel2.Text = "$ " + sumAmounts.ToString();

            var monthamount = GetSumAmountsForCurrentMonth();
            gunaLabel3.Text = "$ " + monthamount.ToString();

            var totaltransactions = GetTotalTransactionCount();
            gunaLabel7.Text = totaltransactions.ToString();
            gunaLabel7.TextAlign = ContentAlignment.MiddleCenter;

            var balance = GetBalanceFromLastRecord();
            gunaLabel10.Text = "$ " + balance.ToString();

            var contracts = GetOpenContractCount();
            gunaLabel11.Text = contracts.ToString();

            string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                int currentYear = DateTime.Today.Year;

                string query = "SELECT TransactionDate, Amount FROM Transactions WHERE YEAR(TransactionDate) = @CurrentYear";

                con.Open();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CurrentYear", currentYear);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime date = reader.GetDateTime(0);
                            decimal amount = reader.GetDecimal(1);
                            chart1.Series["Amount"].Points.AddY(amount);
                        }
                    }
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                // Get the start and end dates of the current month
                DateTime today = DateTime.Today;
                DateTime startOfMonth = new DateTime(today.Year, today.Month, 1);
                DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                string query = "SELECT TransactionDate, Amount FROM Transactions WHERE TransactionDate >= @StartOfMonth AND TransactionDate <= @EndOfMonth";

                con.Open();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@StartOfMonth", startOfMonth);
                    cmd.Parameters.AddWithValue("@EndOfMonth", endOfMonth);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime date = reader.GetDateTime(0);
                            decimal amount = reader.GetDecimal(1);
                            chart2.Series["Amount"].Points.AddY(amount);
                        }
                    }
                }
            }

        }
        public decimal GetSumAmountsForCurrentYear()
        {
            decimal sumAmounts = 0;

            string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                int currentYear = DateTime.Today.Year;

                string query = "SELECT Amount FROM Transactions WHERE YEAR(TransactionDate) = @CurrentYear";

                con.Open();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CurrentYear", currentYear);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            decimal amount = reader.GetDecimal(0);
                            sumAmounts += amount;
                        }
                    }
                }
            }

            return sumAmounts;
        }
        public decimal GetSumAmountsForCurrentMonth()
        {
            decimal sumAmounts = 0;

            string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                int currentYear = DateTime.Today.Year;
                int currentMonth = DateTime.Today.Month;

                string query = "SELECT Amount FROM Transactions WHERE YEAR(TransactionDate) = @CurrentYear AND MONTH(TransactionDate) = @CurrentMonth";

                con.Open();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CurrentYear", currentYear);
                    cmd.Parameters.AddWithValue("@CurrentMonth", currentMonth);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            decimal amount = reader.GetDecimal(0);
                            sumAmounts += amount;
                        }
                    }
                }
            }

            return sumAmounts;
        }
        public int GetTotalTransactionCount()
        {
            int totalTransactionCount = 0;

            string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT COUNT(*) FROM Transactions";

                con.Open();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    totalTransactionCount = (int)cmd.ExecuteScalar();
                }
            }

            return totalTransactionCount;
        }
        public decimal GetBalanceFromLastRecord()
        {
            decimal balance = 0;

            string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                string query = "SELECT TOP 1 Balance FROM Transactions ORDER BY TransactionID DESC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        balance = Convert.ToDecimal(result);
                    }
                }
            }

            return balance;
        }
        public int GetOpenContractCount()
        {
            int openContractCount = 0;

            string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                string query = "SELECT COUNT(*) FROM Contract WHERE Status = 'Open'";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    openContractCount = (int)cmd.ExecuteScalar();
                }
            }

            return openContractCount;
        }
        private void gunaGradientButton1_Click(object sender, EventArgs e)
        {
            ModifyBalance obj = new ModifyBalance();
            obj.Show();
        }
    }
}
   
