using Guna.UI.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Jenga.Theme;
using static Venture_App.Program;

namespace Venture_App
{
    public partial class Contracts : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        public Contracts()
        {
            InitializeComponent();
        }

        private void Contracts_Load(object sender, EventArgs e)
        {
            BindDataWithGrid();
        }

        private void gunaButton4_Click(object sender, EventArgs e)
        {
            AddContract obj = new AddContract();
            obj.Show();
        }
        private void BindDataWithGrid()
        {
            SqlConnection con = new SqlConnection(cs);
            string query = "SELECT Photo, ContractName, VendorName, Email, Phone, Rating, Status, Designation, ContractID  FROM Contract ORDER BY ContractID DESC";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataTable data = new DataTable();
            sda.Fill(data);

            gunaDataGridView1.DataSource = data;

            gunaDataGridView1.Columns["Photo"].HeaderText = " ";
            gunaDataGridView1.Columns["ContractName"].HeaderText = "Contract Name";
            gunaDataGridView1.Columns["VendorName"].HeaderText = "Vendor Name";
            gunaDataGridView1.Columns["Email"].HeaderText = "Email";
            gunaDataGridView1.Columns["Phone"].HeaderText = "Phone";
            gunaDataGridView1.Columns["Rating"].HeaderText = "Rating";
            gunaDataGridView1.Columns["Status"].HeaderText = "Status";
            gunaDataGridView1.Columns["Designation"].HeaderText = "Designation";
            gunaDataGridView1.Columns["ContractID"].Visible = false;

            // Set left alignment for the Contract Name column
            gunaDataGridView1.Columns["ContractName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            gunaDataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gunaDataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gunaDataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            gunaDataGridView1.RowTemplate.Height = 50;
            foreach (DataGridViewColumn column in gunaDataGridView1.Columns)
            {
                if (column.HeaderText == " ")
                {
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                else if (column.HeaderText == "Contract Name")
                {
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else
                {
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }

            gunaDataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
            gunaDataGridView1.EnableHeadersVisualStyles = false;
            gunaDataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            gunaDataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

            // Hook up the CellFormatting event to handle formatting of the "Rating" column and image display
            gunaDataGridView1.CellFormatting += GunaDataGridView1_CellFormatting;
            gunaDataGridView1.CellPainting += gunaDataGridView1_CellPainting;

            DataGridViewButtonColumn actionsColumn = new DataGridViewButtonColumn
            {
                Name = "Actions",
                HeaderText = "Actions",
                UseColumnTextForButtonValue = true,
                Text = "...",
                FlatStyle = FlatStyle.Flat,
                CellTemplate = new DataGridViewButtonCell { UseColumnTextForButtonValue = true }
            };
            gunaDataGridView1.Columns.Add(actionsColumn);
            
            // Attach the CellClick event to the DataGridView to handle the button click
            gunaDataGridView1.CellClick += gunaDataGridView1_CellClick;
            int lastRowIndex = gunaDataGridView1.Rows.Count - 1;
            if (gunaDataGridView1.Rows[lastRowIndex].IsNewRow)
            {
                // Cancel the new row, effectively removing it from the DataGridView
                gunaDataGridView1.CancelEdit();
            }
            else
            {
                // Remove the last row
                gunaDataGridView1.Rows.RemoveAt(lastRowIndex);
            }
        }

        private void gunaDataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 7) // Assuming "Status" column is at index 6
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                // Calculate the position for the tag
                int tagX = e.CellBounds.Left + 5;
                int tagY = e.CellBounds.Top + (e.CellBounds.Height - 15) / 2; // Adjust the height value as needed

                // Retrieve the value from the "Status" cell
                string status = gunaDataGridView1.Rows[e.RowIndex].Cells["Status"].Value?.ToString();

                if (!string.IsNullOrEmpty(status))
                {
                    // Determine the tag color based on the status value
                    Color tagColor = status.Equals("Open", StringComparison.OrdinalIgnoreCase) ? Color.Green : Color.Red;

                    // Draw the colored tag
                    using (Brush brush = new SolidBrush(tagColor))
                    {
                        e.Graphics.FillEllipse(brush, tagX, tagY, 15, 15);
                    }
                }

                e.Handled = true;
            }
        }



        private void GunaDataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && gunaDataGridView1.Columns[e.ColumnIndex].Name == "Photo")
            {
                // Get the photo data from the cell value
                if (e.Value is byte[] imageData)
                {
                    // Convert the byte[] data to an Image
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        Image photoImage = Image.FromStream(ms);

                        // Create a circular image using Graphics
                        Bitmap circularImage = new Bitmap(40, 40);
                        using (Graphics graphics = Graphics.FromImage(circularImage))
                        {
                            graphics.SmoothingMode = SmoothingMode.AntiAlias;
                            graphics.Clear(Color.Transparent);

                            // Draw the image within a circular path
                            using (GraphicsPath path = new GraphicsPath())
                            {
                                path.AddEllipse(0, 0, 39, 39);
                                graphics.SetClip(path);
                                graphics.DrawImage(photoImage, 0, 0, 40, 40);
                            }
                        }

                        // Set the cell value as the circular image
                        e.Value = circularImage;
                        e.FormattingApplied = true;
                    }
                }
            }
            else if (gunaDataGridView1.Columns[e.ColumnIndex].Name == "Rating" && e.Value != null && e.Value != DBNull.Value)
            {
                if (float.TryParse(e.Value.ToString(), out float rating))
                {
                    // Add a star symbol before the rating value
                    e.Value = "★ " + string.Format("{0:N2}", rating);
                    e.FormattingApplied = true;
                }
            }

            if (e.RowIndex >= 0 && e.ColumnIndex == gunaDataGridView1.Columns["Actions"].Index)
            {
                // Change the cell color of the last column to white
                e.CellStyle.BackColor = Color.White;
                e.CellStyle.ForeColor = Color.Black;
            }
        }

        private void gunaDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == gunaDataGridView1.Columns["Actions"].Index)
            {
                // Display a dropdown menu with options for edit and delete
                ContextMenuStrip contextMenu = new ContextMenuStrip();

                // Edit option
                ToolStripMenuItem editMenuItem = new ToolStripMenuItem("Edit");
                editMenuItem.Click += (s, args) =>
                {
                    // Get the value of the "ID" column for the clicked row
                    string id = gunaDataGridView1.Rows[e.RowIndex].Cells["ContractID"].Value.ToString();

                    // Implement your edit logic here
             int id2 = Convert.ToInt32(id);
                    GetContractById(id2);
                };
                contextMenu.Items.Add(editMenuItem);

                // Delete option
                ToolStripMenuItem deleteMenuItem = new ToolStripMenuItem("Delete");
                deleteMenuItem.Click += (s, args) =>
                {
                    string id = gunaDataGridView1.Rows[e.RowIndex].Cells["ContractID"].Value.ToString();
                    
                    int id2 = Convert.ToInt32(id);
                    DeleteRecord(id2);
                };
                contextMenu.Items.Add(deleteMenuItem);

                // Show the dropdown menu
                DataGridViewCell cell = gunaDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                Rectangle cellRect = gunaDataGridView1.GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, false);
                contextMenu.Show(gunaDataGridView1, cellRect.Left + cellRect.Width, cellRect.Top);
            }
        }

        private void DeleteRecord(int id2)
        {
            SqlConnection con = new SqlConnection(cs);
            string query = "delete from Contract where ContractID = @id ";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", id2);
           

            con.Open();

            int a = cmd.ExecuteNonQuery();

            if (a > 0)
            {
                MessageBox.Show("Deleted", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Deletion Failed", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            con.Close();
        }

        public void GetContractById(int contractId)
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    string query = "SELECT * FROM Contract WHERE ContractID = @ContractID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ContractID", contractId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ContractData contract = new ContractData()
                                {
                                    id = (int)reader["ContractID"],
                                    Image = (byte[])reader["Photo"],
                                    Name = reader["ContractName"].ToString(),
                                    VName = reader["VendorName"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Phone = reader["Phone"].ToString(),
                                    Rating = Convert.ToSingle(reader["Rating"]),
                                    Status = reader["Status"].ToString(),
                                    Designation = reader["Designation"].ToString()
                                };
                            UpdateContract obj = new UpdateContract(contract);
                            obj.Show();
                            }
                        }
                    }
                }

                // Contract with the given ID was not found
            }

        private void gunaDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }




        //private void gunaDataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        //{
        //    if (e.ColumnIndex == 0 && e.RowIndex >= 0)
        //    {
        //        e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
        //    }
        //}
    }
}
