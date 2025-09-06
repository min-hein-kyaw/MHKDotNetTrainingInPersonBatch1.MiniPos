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
    public partial class FormMenu : Form
    {
        public FormMenu()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void addProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
           FormProduct product = new FormProduct();
            product.ShowDialog();
        }
    }
}
