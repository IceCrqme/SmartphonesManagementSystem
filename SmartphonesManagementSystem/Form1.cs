using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartphonesApp
{
    public partial class Form1 : Form
    {
        string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=SmartphonesDB;Integrated Security=True";
        int selectedId = -1;

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Smartphones", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewSmartphones.DataSource = dt;
            }
        }

        public Form1()
        {
            InitializeComponent();
            LoadData();
        }

        private void ClearFields()
        {
            textBoxBrand.Clear();
            textBoxModel.Clear();
            textBoxPrice.Clear();
            textBoxRAM.Clear();
            textBoxStorage.Clear();
            comboBoxOS.SelectedIndex = -1;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "INSERT INTO Smartphones (Brand, Model, OS, Price, RAM, Storage) " +
                               "VALUES (@Brand, @Model, @OS, @Price, @RAM, @Storage)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Brand", textBoxBrand.Text);
                cmd.Parameters.AddWithValue("@Model", textBoxModel.Text);
                cmd.Parameters.AddWithValue("@OS", comboBoxOS.Text);
                cmd.Parameters.AddWithValue("@Price", decimal.Parse(textBoxPrice.Text));
                cmd.Parameters.AddWithValue("@RAM", int.Parse(textBoxRAM.Text));
                cmd.Parameters.AddWithValue("@Storage", int.Parse(textBoxStorage.Text));

                cmd.ExecuteNonQuery();
            }

            LoadData();
            ClearFields();
        }

        private void dataGridViewSmartphones_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dataGridViewSmartphones.Rows[e.RowIndex];

            if (row.Cells["Id"].Value == DBNull.Value)
                return;

            selectedId = Convert.ToInt32(row.Cells["Id"].Value);

            textBoxBrand.Text = row.Cells["Brand"].Value.ToString();
            textBoxModel.Text = row.Cells["Model"].Value.ToString();
            comboBoxOS.Text = row.Cells["OS"].Value.ToString();
            textBoxPrice.Text = row.Cells["Price"].Value.ToString();
            textBoxRAM.Text = row.Cells["RAM"].Value.ToString();
            textBoxStorage.Text = row.Cells["Storage"].Value.ToString();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            ClearFields();
            selectedId = -1;
            LoadData();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (selectedId == -1)
            {
                MessageBox.Show("Please select a record to update.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "UPDATE Smartphones SET " +
                               "Brand = @Brand, " +
                               "Model = @Model, " +
                               "OS = @OS, " +
                               "Price = @Price, " +
                               "RAM = @RAM, " +
                               "Storage = @Storage " +
                               "WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Brand", textBoxBrand.Text);
                cmd.Parameters.AddWithValue("@Model", textBoxModel.Text);
                cmd.Parameters.AddWithValue("@OS", comboBoxOS.Text);
                cmd.Parameters.AddWithValue("@Price", decimal.Parse(textBoxPrice.Text));
                cmd.Parameters.AddWithValue("@RAM", int.Parse(textBoxRAM.Text));
                cmd.Parameters.AddWithValue("@Storage", int.Parse(textBoxStorage.Text));
                cmd.Parameters.AddWithValue("@Id", selectedId);

                cmd.ExecuteNonQuery();
            }

            LoadData();
            ClearFields();
            selectedId = -1;
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (selectedId == -1)
            {
                MessageBox.Show("Please select a record to delete.");
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this record?", "Confirm Delete", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "DELETE FROM Smartphones WHERE Id = @Id";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", selectedId);
                    cmd.ExecuteNonQuery();
                }

                LoadData();
                ClearFields();
                selectedId = -1;
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM Smartphones WHERE 1=1";

                if (!string.IsNullOrWhiteSpace(textBoxBrand.Text))
                {
                    query += " AND Brand LIKE @Brand";
                }

                if (!string.IsNullOrWhiteSpace(textBoxModel.Text))
                {
                    query += " AND Model LIKE @Model";
                }

                if (!string.IsNullOrWhiteSpace(comboBoxOS.Text))
                {
                    query += " AND OS = @OS";
                }

                if (!string.IsNullOrWhiteSpace(textBoxPrice.Text))
                {
                    query += " AND Price = @Price";
                }

                if (!string.IsNullOrWhiteSpace(textBoxRAM.Text))
                {
                    query += " AND RAM = @RAM";
                }

                if (!string.IsNullOrWhiteSpace(textBoxStorage.Text))
                {
                    query += " AND Storage = @Storage";
                }

                SqlCommand cmd = new SqlCommand(query, conn);

                if (!string.IsNullOrWhiteSpace(textBoxBrand.Text))
                {
                    cmd.Parameters.AddWithValue("@Brand", "%" + textBoxBrand.Text + "%");
                }

                if (!string.IsNullOrWhiteSpace(textBoxModel.Text))
                {
                    cmd.Parameters.AddWithValue("@Model", "%" + textBoxModel.Text + "%");
                }

                if (!string.IsNullOrWhiteSpace(comboBoxOS.Text))
                {
                    cmd.Parameters.AddWithValue("@OS", comboBoxOS.Text);
                }

                if (!string.IsNullOrWhiteSpace(textBoxPrice.Text))
                {
                    cmd.Parameters.AddWithValue("@Price", decimal.Parse(textBoxPrice.Text));
                }

                if (!string.IsNullOrWhiteSpace(textBoxRAM.Text))
                {
                    cmd.Parameters.AddWithValue("@RAM", int.Parse(textBoxRAM.Text));
                }

                if (!string.IsNullOrWhiteSpace(textBoxStorage.Text))
                {
                    cmd.Parameters.AddWithValue("@Storage", int.Parse(textBoxStorage.Text));
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridViewSmartphones.DataSource = dt;
            }
        }
    }
}
