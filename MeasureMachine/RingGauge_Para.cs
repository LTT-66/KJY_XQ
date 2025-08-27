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
    public partial class RingGauge_Para : Form
    {
        public RingGauge_Para()
        {
            InitializeComponent();
        }

        private void button_material_Click(object sender, EventArgs e)
        {
            Profile.G_COMPEN_MATERIAL_HUANGUI_COEF = comboBox_material.Text;
            Profile.SaveHuanguiProfile();
            if (panel1.Visible == true) button_inside_size_b_ok_Click(sender, e);
            this.Close(); 
        }

        private void RingGauge_Para_Load(object sender, EventArgs e)
        {
            comboBox_material.Items.Add(5.0);
            comboBox_material.Items.Add(7.3);
            comboBox_material.Items.Add(11.5);
            comboBox_material.Text = Profile.G_COMPEN_MATERIAL_HUANGUI_COEF;

            textBox_scope_1.Text = Profile.G_COMPEN_CIRCLE_HUANGUI_MAX[0];
            textBox_scope_2.Text = Profile.G_COMPEN_CIRCLE_HUANGUI_MAX[1];
            textBox_scope_3.Text = Profile.G_COMPEN_CIRCLE_HUANGUI_MAX[2];
            textBox_scope_4.Text = Profile.G_COMPEN_CIRCLE_HUANGUI_MAX[3];
            textBox_modify_1.Text = Profile.G_COMPEN_CIRCLE_HUANGUI_COEF[0];
            textBox_modify_2.Text = Profile.G_COMPEN_CIRCLE_HUANGUI_COEF[1];
            textBox_modify_3.Text = Profile.G_COMPEN_CIRCLE_HUANGUI_COEF[2];
            textBox_modify_4.Text = Profile.G_COMPEN_CIRCLE_HUANGUI_COEF[3];
            textBox_modify_5.Text = Profile.G_COMPEN_CIRCLE_HUANGUI_COEF[4];

            textBox_inside_size_k.Text = Profile.G_COMPEN_EXPERENCE_HUANGUI_VALUE_K;
            textBox_inside_size_b.Text = Profile.G_COMPEN_EXPERENCE_HUANGUI_VALUE_B;
        }

        private void button_inside_size_b_ok_Click(object sender, EventArgs e)
        {
            Profile.G_COMPEN_MATERIAL_HUANGUI_COEF = comboBox_material.Text;

            Profile.G_COMPEN_EXPERENCE_HUANGUI_VALUE_B = textBox_inside_size_b.Text;
            Profile.G_COMPEN_EXPERENCE_HUANGUI_VALUE_K = textBox_inside_size_k.Text;

            Profile.G_COMPEN_CIRCLE_HUANGUI_MAX[0] = textBox_scope_1.Text;
            Profile.G_COMPEN_CIRCLE_HUANGUI_MAX[1] = textBox_scope_2.Text;
            Profile.G_COMPEN_CIRCLE_HUANGUI_MAX[2] = textBox_scope_3.Text;
            Profile.G_COMPEN_CIRCLE_HUANGUI_MAX[3] = textBox_scope_4.Text;
            Profile.G_COMPEN_CIRCLE_HUANGUI_COEF[0] = textBox_modify_1.Text;
            Profile.G_COMPEN_CIRCLE_HUANGUI_COEF[1] = textBox_modify_2.Text;
            Profile.G_COMPEN_CIRCLE_HUANGUI_COEF[2] = textBox_modify_3.Text;
            Profile.G_COMPEN_CIRCLE_HUANGUI_COEF[3] = textBox_modify_4.Text;
            Profile.G_COMPEN_CIRCLE_HUANGUI_COEF[4] = textBox_modify_5.Text;
            Profile.SaveHuanguiProfile();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SeniorSetDlg seniorParaSet = new SeniorSetDlg();

            seniorParaSet.ShowDialog();
            if (seniorParaSet.result)
            {
                this.panel1.Visible = true;
            }
        }
    }
}
