namespace MHKDotNetTrainingInPersonBatch1.WindowForm
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        

        private void btn_login_Click(object sender, EventArgs e)
        {
            if (textBox_username.Text != "admin" || textBox_password.Text != "admin12345") {
                MessageBox.Show("Wrong Username or Password","Invalid Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.Hide();
            FormMenu formMenu = new FormMenu();
            formMenu.ShowDialog();


        }
    }
}
