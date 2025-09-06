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
        public FormProduct()
        {
            InitializeComponent();
        }

        public void button1_Click(object sender, EventArgs e)
        {
            bool check = DataValidation();
            if (check == false)
            {
                return;
            }
            Insert();
            
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
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = ".",
                InitialCatalog = "MHKDotNetTrainingInPersonBatch1POS",
                UserID = "sa",
                Password = "sasa@123",
                TrustServerCertificate = true
            };
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
                ClearControls();
            }
        }

        public void ClearControls()
        {
            textBox_ProductCode.Clear();
            textBox_ProductName.Clear();
            textBox_Price.Clear();
            textBox_Quantity.Clear();
        }
    }
}
