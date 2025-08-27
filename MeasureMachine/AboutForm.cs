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
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {

        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            this.LabelProductName.Text = Application.ProductName;
            this.LabelVersion.Text = Application.ProductVersion;
            this.LabelCompanyName.Text = Application.CompanyName;
            this.LabelCopyright.Text = "Copyright ©  2023";
            this.TextBoxDescription.Text = "测长机应用软件\r\n中国航空工业集团公司\r\n北京长城计量测试技术研究所\r\n长度研究部";

        }
    }
}
