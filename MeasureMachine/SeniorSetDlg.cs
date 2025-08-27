using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MeasureMachine
{
    public partial class SeniorSetDlg : Form
    {
        public bool result = false;
        public SeniorSetDlg()
        {
            InitializeComponent();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "123456")
            {
                result = true;
                this.Close();
                // (this.Owner as Duandu_Para).panel1.Visible = true;

            }
            else
            {
                result = false;
                MessageBox.Show("错误，密码错误");
            }
        }
    }
}
