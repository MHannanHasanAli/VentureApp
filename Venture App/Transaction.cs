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
    public partial class Transaction : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        public Transaction()
        {
            InitializeComponent();
        }

        private void gunaButton4_Click(object sender, EventArgs e)
        {
            SendPayment obj = new SendPayment();
            obj.Show();
        }

        private void BindDataWithGrid()
        {
            SqlConnection con = new SqlConnection(cs);
            string query = "select * from Transactions ORDER BY TransactionID DESC";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataTable data = new DataTable();
            sda.Fill(data);

            gunaDataGridView1.DataSource = data;

            
            gunaDataGridView1.Columns["PostingDate"].HeaderText = "Posting Date";
            gunaDataGridView1.Columns["TransactionDate"].HeaderText = "Transaction Date";
            gunaDataGridView1.Columns["Description"].HeaderText = "Description";
            gunaDataGridView1.Columns["Amount"].HeaderText = "Amount";
            gunaDataGridView1.Columns["Balance"].HeaderText = "Balance";
         
            gunaDataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Set ColumnHeadersHeightSizeMode to enable resizing of column headers
            gunaDataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // Set selection mode to only select entire rows
            gunaDataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Hide the TransactionID column
            gunaDataGridView1.Columns["TransactionID"].Visible = false;

            gunaDataGridView1.RowTemplate.Height = 40;
            foreach (DataGridViewColumn column in gunaDataGridView1.Columns)
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            // Align column headers (names) to center
            gunaDataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            // Customize column header appearance
            gunaDataGridView1.EnableHeadersVisualStyles = false; // Prevent default styling
            gunaDataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.White; // Set background color
            gunaDataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black; // Set text color

            // Hook up the CellFormatting event to handle formatting of the "Amount" column
            gunaDataGridView1.CellFormatting += GunaDataGridView1_CellFormatting;
            
        }

        private void GunaDataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Check if the current column is the "Amount" column and the cell value is a valid number
            if (gunaDataGridView1.Columns[e.ColumnIndex].Name == "Amount" && e.Value != null && e.Value != DBNull.Value)
            {
                if (decimal.TryParse(e.Value.ToString(), out decimal amount))
                {
                    // Format the value with a dollar sign and two decimal places
                    e.Value = string.Format("${0:N2}", amount);
                    e.FormattingApplied = true;
                }
            }
        }



        private void Transaction_Load(object sender, EventArgs e)
        {
            BindDataWithGrid();
        }

        private void gunaDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void gunaComboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            //if (gunaComboBox1.SelectedItem != null && gunaComboBox1.SelectedItem is DateTime)
            //{
            //    DateTime selectedDate = (DateTime)gunaComboBox1.SelectedItem;

            //    DataView dv = ((DataTable)gunaDataGridView1.DataSource).DefaultView;
            //    dv.RowFilter = string.Format("PostingDate = '{0:yyyy-MM-dd}'", selectedDate);

            //    gunaDataGridView1.DataSource = dv.ToTable();
            //}
        }
       
        private void gunaDateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            //BindDataWithGrid(gunaDateTimePicker1.Value);
        }
        private void BindDataWithGridByDate()
        {
            SqlConnection con = new SqlConnection(cs);
            string query = "SELECT * FROM Transactions ORDER BY TransactionDate ASC"; 
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataTable data = new DataTable();
            sda.Fill(data);

            
            gunaDataGridView1.DataSource = data;

            gunaDataGridView1.Columns["TransactionID"].HeaderText = "Transaction ID";
            gunaDataGridView1.Columns["PostingDate"].HeaderText = "Posting Date";
            gunaDataGridView1.Columns["TransactionDate"].HeaderText = "Transaction Date";
            gunaDataGridView1.Columns["Description"].HeaderText = "Description";
            gunaDataGridView1.Columns["Amount"].HeaderText = "Amount";
            gunaDataGridView1.Columns["Balance"].HeaderText = "Balance";

            gunaDataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Set ColumnHeadersHeightSizeMode to enable resizing of column headers
            gunaDataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // Set selection mode to only select entire rows
            gunaDataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Hide the TransactionID column
            gunaDataGridView1.Columns["TransactionID"].Visible = false;

            gunaDataGridView1.RowTemplate.Height = 40;
            foreach (DataGridViewColumn column in gunaDataGridView1.Columns)
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            // Align column headers (names) to center
            gunaDataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            // Customize column header appearance
            gunaDataGridView1.EnableHeadersVisualStyles = false; // Prevent default styling
            gunaDataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.White; // Set background color
            gunaDataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black; // Set text color

            // Hook up the CellFormatting event to handle formatting of the "Amount" column
            gunaDataGridView1.CellFormatting += GunaDataGridView1_CellFormatting;

        }

        private void gunaButton1_Click(object sender, EventArgs e)
        {
            var input = gunaComboBox1.SelectedItem;
            if(input == "Default")
            {
                BindDataWithGrid();
            }
            else
            {
                BindDataWithGridByDate();
            }
        }
    }
}
