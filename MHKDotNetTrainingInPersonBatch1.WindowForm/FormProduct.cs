using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MHKDotNetTrainingInPersonBatch1.WindowForm
{
    public partial class FormProduct : Form
    {
        public string? productId;
        SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
        {
            DataSource = ".",
            InitialCatalog = "MHKDotNetTrainingInPersonBatch1POS",
            UserID = "sa",
            Password = "sasa@123",
            TrustServerCertificate = true
        };

        public FormProduct()
        {
            InitializeComponent();
            btnUpdate.Visible = false;
        }

        public void button1_Click(object sender, EventArgs e)
        {
            bool check = DataValidation();
            if (check == false)
            {
                return;
            }
            Insert();
            ClearControls();
        }

        private bool DataValidation()
        {
            if (string.IsNullOrEmpty(textBox_ProductCode.Text.Trim()))
            {
                MessageBox.Show("Product Code is Required", "Empty Text Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox_ProductCode.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(textBox_ProductName.Text.Trim()))
            {
                MessageBox.Show("Product Name is Required", "Empty Text Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox_ProductName.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(textBox_Price.Text.Trim()))
            {
                MessageBox.Show("Price is Required", "Empty Text Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox_Price.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(textBox_Quantity.Text.Trim()))
            {
                MessageBox.Show("Quantity is Required", "Empty Text Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox_Quantity.Focus();
                return false;
            }
            return true;
        }

        private void Insert()
        {
            using (IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                string id = Guid.NewGuid().ToString();
                ProductDTO productDTO = new ProductDTO()
                {
                    ProductId = id,
                    ProductCode = textBox_ProductCode.Text.Trim(),
                    ProductName = textBox_ProductName.Text.Trim(),
                    Price = Convert.ToDecimal(textBox_Price.Text.Trim()),
                    Quantity = Convert.ToInt32(textBox_Quantity.Text.Trim()),
                    DeleteFlag = false
                };
                string query = @"INSERT INTO [dbo].[Table_Product]
                    ([ProductID]
                    ,[ProductCode]
                    ,[ProductName]
                    ,[Price]
                    ,[Quantity]
                    ,[DeleteFlag])
                    VALUES
                    (@ProductId
                    ,@ProductCode
                    ,@ProductName
                    ,@Price
                    ,@Quantity
                    ,@DeleteFlag)";

                db.Open();
                var results = db.Execute(query, productDTO);
                string message = results > 0 ? "Saved Successfully" : "Saved Failed";
                string corOrincor = results > 0 ? "correct save" : "incorrect save";
                MessageBox.Show(message, corOrincor, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvTable.DataSource = db.Query("select * from Table_Product where DeleteFlag = 0").ToList();
            }
        }

        public void ClearControls()
        {
            textBox_ProductCode.Clear();
            textBox_ProductName.Clear();
            textBox_Price.Clear();
            textBox_Quantity.Clear();
            btnUpdate.Visible = false;
        }

        private void FormProduct_Load(object sender, EventArgs e)
        {
            Bind();
        }

        public void Bind()
        {
            using (IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                db.Open();
                dgvTable.DataSource = db.Query("select * from Table_Product where DeleteFlag = 0").ToList();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            Bind();
        }

        public void editDataLoad(DataGridViewCellEventArgs e)
        {
            IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
            btnUpdate.Visible = true;
            productId = dgvTable.Rows[e.RowIndex].Cells["productIdDataGridViewTextBoxColumn"].Value.ToString();
            string query = "select * from Table_Product where ProductID = @ProductId";
            ProductDTO? item = db.QueryFirstOrDefault<ProductDTO>(query, new { ProductId = productId });
            if (item is null)
            {
                MessageBox.Show("Product not found");
                return;
            }
            textBox_ProductCode.Text = item.ProductCode.Trim();
            textBox_ProductName.Text = item.ProductName.Trim();
            textBox_Price.Text = item.Price.ToString().Trim();
            textBox_Quantity.Text = item.Quantity.ToString().Trim();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                editDataLoad(e);
            }
            else if (e.RowIndex >= 0 && e.ColumnIndex == 1)
            {
                DeleteData(e);
                Bind();
            }
        }

        private void DeleteData(DataGridViewCellEventArgs e)
        {
            DialogResult answer = MessageBox.Show("Are you sure to delete?", "Delete Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (answer == DialogResult.Cancel)
            {
                return;
            }
            productId = dgvTable.Rows[e.RowIndex].Cells["productIdDataGridViewTextBoxColumn"].Value.ToString();
            IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
            db.Open();
            ProductDTO product = new ProductDTO()
            {
                ProductId = productId,
                DeleteFlag = true,
            };

            string query = @"UPDATE [dbo].[Table_Product]
                SET
                    DeleteFlag = @DeleteFlag
                WHERE ProductID = @ProductId;";
            int result = db.Execute(query, product);
            MessageBox.Show("Delete Complete", "complete Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateData();                   
            Bind();
        }

        public void UpdateData()
        {
            if (string.IsNullOrEmpty(productId))
            {
                MessageBox.Show("Please Select a product to update");
                return;
            }
            IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
            ProductDTO productDTO = new ProductDTO()
            {
                ProductId = productId,
                ProductCode = textBox_ProductCode.Text,
                ProductName = textBox_ProductName.Text,
                Price = Convert.ToDecimal(textBox_Price.Text),
                Quantity = Convert.ToInt32(textBox_Quantity.Text)
            };
            string query = @"UPDATE [dbo].[Table_Product]
                SET
                    [ProductCode] = @ProductCode
                    ,[ProductName] = @ProductName
                    ,[Price] = @Price
                    ,[Quantity] = @Quantity
                WHERE ProductID = @ProductId;";
            var result = db.Execute(query, productDTO);
            MessageBox.Show("Updating Complete");
            dgvTable.DataSource = db.Query("select * from Table_Product where DeleteFlag = 0").ToList();
            btnUpdate.Visible = false;
        }
    }
}
