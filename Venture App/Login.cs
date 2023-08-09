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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gunaLineTextBox1.Text = "admin";
            gunaLineTextBox2.Text = "123456";
        }      
        private void gunaLineTextBox1_Leave(object sender, EventArgs e)
        {
            Guna.UI.WinForms.GunaLineTextBox textBox = (Guna.UI.WinForms.GunaLineTextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Username";
            }
        }
        private void gunaLineTextBox1_Enter(object sender, EventArgs e)
        {
            Guna.UI.WinForms.GunaLineTextBox textBox = (Guna.UI.WinForms.GunaLineTextBox)sender;
            textBox.Text = "";
        }
        private void gunaLineTextBox2_Leave(object sender, EventArgs e)
        {
            Guna.UI.WinForms.GunaLineTextBox textBox = (Guna.UI.WinForms.GunaLineTextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Password";
            }
        }
        private void gunaLineTextBox2_Enter(object sender, EventArgs e)
        {
            Guna.UI.WinForms.GunaLineTextBox textBox = (Guna.UI.WinForms.GunaLineTextBox)sender;
            textBox.Text = "";
        }

    
        private void gunaGradientButton1_Click(object sender, EventArgs e)
        {
            var username = gunaLineTextBox1.Text;
            var password = gunaLineTextBox2.Text;

            string cn_string = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;

            SqlConnection cn_connection = new SqlConnection(cn_string);
            
                cn_connection.Open();

                // Create a parameterized query to avoid SQL injection
                string sql_Text = "SELECT * FROM Users WHERE Username = @Username AND UPassword = @Password";

            SqlCommand command = new SqlCommand(sql_Text, cn_connection);
                
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

            SqlDataReader reader = command.ExecuteReader();
                    
                        if (reader.HasRows == true)
                        {
                            Home obj = new Home();
                            this.Hide();
                            obj.Show();
                        }
                        else
                        {
                            MessageBox.Show("Invalid Username or Password");
                        }
                    
                
            

        }















        //private void load_list()
        //{
        //    string cn_string = Properties.Settings.Default.DatabaseVentureConnectionString;

        //    SqlConnection cn_connection = new SqlConnection(cn_string);

        //    if (cn_connection.State != ConnectionState.Open) cn_connection.Open();

        //    string sql_Text = "SELECT * FROM tbl_Car";

        //    DataTable tbl = new DataTable();
        //    SqlDataAdapter adapter = new SqlDataAdapter(sql_Text, cn_connection);
        //    adapter.Fill(tbl);

        //    listCars.DisplayMember = "Car";
        //    listCars.ValueMember = "IDCar";
        //    listCars.DataSource = tbl;
        //}
        //private void add_car()
        //{
        //    string cn_string = Properties.Settings.Default.DatabaseVentureConnectionString;

        //    SqlConnection cn_connection = new SqlConnection(cn_string);

        //    if (cn_connection.State != ConnectionState.Open) cn_connection.Open();

        //    string New_Car = inputentry.Text;
        //    string sql_Text = "INSERT INTO tbl_Car (Car) VALUES('" + New_Car + "')";

        //    SqlCommand SQL_CMD = new SqlCommand(sql_Text, cn_connection);
        //    SQL_CMD.ExecuteNonQuery();



        //    load_list();
        //}

        //private void btndel_Click(object sender, EventArgs e)
        //{

        //    string cn_string = Properties.Settings.Default.DatabaseVentureConnectionString;

        //    SqlConnection cn_connection = new SqlConnection(cn_string);

        //    if (cn_connection.State != ConnectionState.Open) cn_connection.Open();

        //    DataRowView row = listCars.SelectedItem as DataRowView;
        //    string IdAuto = row["IDCar"].ToString();
        //    string sql_Text = "DELETE FROM tbl_Car WHERE(IDCar = " + IdAuto + ")";


        //    SqlCommand SQL_CMD = new SqlCommand(sql_Text, cn_connection);
        //    SQL_CMD.ExecuteNonQuery();



        //    load_list();
        //}

        //private void btnupdt_Click(object sender, EventArgs e)
        //{
        //    string cn_string = Properties.Settings.Default.DatabaseVentureConnectionString;

        //    SqlConnection cn_connection = new SqlConnection(cn_string);

        //    if (cn_connection.State != ConnectionState.Open) cn_connection.Open();

        //    DataRowView row = listCars.SelectedItem as DataRowView;
        //    string IdAuto = row["IDCar"].ToString();
        //    string sql_Text = "UPDATE tbl_Car SET Car = '" + inputentry.Text + "' WHERE IDCar = " + IdAuto;


        //    SqlCommand SQL_CMD = new SqlCommand(sql_Text, cn_connection);
        //    SQL_CMD.ExecuteNonQuery();



        //    load_list();
        //}

        //private void listCars_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DataRowView row = listCars.SelectedItem as DataRowView;
        //    inputentry.Text = row["Car"].ToString();
        //}
    }
}
