using MeasureMachine.serialPort;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MeasureMachine.SettingForm;
using System.Data;
using System.IO;
using System.Diagnostics;

namespace MeasureMachine
{
    public delegate void ToPrintDelegate(string filename);//1
    public partial class Form1 : Form
    {
        DataTable dt;
        int row_count = 0;
        SqliteHelper sqlLiteHelper = null;
        public event ToPrintDelegate ToPrintDelegateFun; //2

        private SettingForm settingDlg = new SettingForm();
        private AboutForm aboutDlg = new AboutForm();
        public Form1()
        {
            InitializeComponent();
            //SettingForm settingForm = new SettingForm();
            settingDlg.DecodeDelegateFun += new DecodeDelegate(DecodeDelegateEvent);//2
            settingDlg.TextChanged += new TextChangedEventHandler(settingForm_TextChanged);
        }

        
        private void 装置设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settingDlg.ShowDialog();
        }

        private void btnZero_Click(object sender, EventArgs e)
        {
            //todo  make sure which portal we need to send commend
            try
            {
                if (!sp2.IsOpen)
                {

                }
                byte[] cmdBuffer = new byte[1];
                cmdBuffer[0] = CMD_GRATING_ZERO;
                sp2.Write(cmdBuffer, 0, cmdBuffer.Length);
            }
            catch (Exception ex)
            {
                //TODO show error message in the view
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 更新界面数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="valuestr"></param>
        private void DecodeDelegateEvent(string valuestr_gating, string valuestr_gating2)//3
        {
            //lblRasterValue为原始数据，lblRasterValue2为补偿后的数据
            lblRasterValue.Text = valuestr_gating;
            lblRasterValue2.Text = valuestr_gating2;
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            DataRow dr;
            dr = dt.NewRow();
            //dr["测量值(mm)"] = Convert.ToDouble(lblMeaValue.Text.Trim());
            //dr["光栅(mm)"] = Convert.ToDouble(lblRasterValue.Text.Trim());
            //dr["长度计(mm)"] = Convert.ToDouble(lblProbeValue.Text.Trim());
            dr["光栅(mm)"] = Math.Round(Convert.ToDouble(lblRasterValue2.Text.Trim()), 4);
           

            //dt.Rows.InsertAt(dr, 0);
            dt.Rows.InsertAt(dr, dt.Rows.Count);  // 将索引值设置为DataTable的行数减一，新行将被添加到DataTable的底部。
            //重新写序号
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["序号"] = i + 1;
                this.dataGridView1.Rows[i].Cells["序号"].Value = i + 1;
            }
            row_count++;
            insertRow2DB();
        }

        private void insertRow2DB()
        {
            if (!System.IO.File.Exists(GlobalVar.connectstr))
            {
                //MessageBox.Show("MeasureMachine.db数据库文件不存在或存在问题！");
                return;
            }
            sqlLiteHelper = new SqliteHelper(GlobalVar.connectstr);
            string cmdstr = "insert into measure(grating_value) values ('" +
                lblRasterValue2.Text.ToString()  +  ")";

            sqlLiteHelper.ExecSqlCommand(cmdstr);

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.RestoreDirectory = true;
            sfd.Filter = "Excel文件(*.xls;*.xlsx)|*.xls;*.xlsx";
            //设置默认保存文件名称 
            sfd.FileName = "光栅_导出时间_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                DataTable exdt = this.dt.Copy();
                // exdt.Columns.Remove("序号");

                string localFilePath = sfd.FileName.ToString(); //获得文件路径 
                NPOIHelper npoih = new NPOIHelper();
                try
                {
                    npoih.DtToExcelWorkbook(localFilePath, exdt, "sheet1", 1);
                    npoih.DtToExcelFsWrite(); MessageBox.Show("导出成功！");
                }//datatable导出到EXCEl
                catch (Exception err) { MessageBox.Show("导出失败！\r\n" + err.ToString()); }

            }

            System.GC.Collect();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Profile.LoadProfile();//加载所有参数

            dt = new DataTable();//建立个数据表

            dt.Columns.Add(new DataColumn("序号", typeof(int)));
            dt.Columns.Add(new DataColumn("光栅(mm)", typeof(double)));

            dt.Columns["序号"].SetOrdinal(0);
            dt.Columns["光栅(mm)"].SetOrdinal(1);


            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.DataSource = dt;
            // this.dataGridView1.Columns[1].DefaultCellStyle.Format = Profile.G_ITERFER_DECIMAL_NUM;
            // this.dataGridView1.Columns[2].DefaultCellStyle.Format = Profile.G_ITERFER_DECIMAL_NUM;
            // this.dataGridView1.Columns[3].DefaultCellStyle.Format = Profile.G_ITERFER_DECIMAL_NUM;
            DataGridViewImageColumn columnDel = new DataGridViewImageColumn();//增加删除按钮           
            columnDel.Name = "del";
            columnDel.HeaderText = "删除";
            columnDel.DefaultCellStyle.NullValue = null;
            columnDel.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            columnDel.Image = global::MeasureMachine.Properties.Resources.del16x16;
            this.dataGridView1.Columns.Add(columnDel);
        }

        
        private void settingForm_TextChanged(string newText1, string newText2, string newText3, string newText4)
        {
            // 更新textBox2的Text属性  
            txtTempDevice.Text = newText1;
            txtTempObj.Text = newText2;
            txtCoDevice.Text = newText3;
            txtCoObj.Text = newText4;
        }

        private void 开始ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settingDlg.ShowDialog();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aboutDlg.ShowDialog();
        }

        private void dataGridView1_Click(object sender, DataGridViewCellEventArgs e)
        {
            int CIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;

            if (CIndex == 2)
            {
                if (MessageBox.Show(this, "是否删除此行,请确认！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    //DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                    // this.dataGridView1.Rows.Remove(row);

                    dt.Rows.RemoveAt(e.RowIndex);
                    row_count--;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["序号"] = i + 1;
                        this.dataGridView1.Rows[i].Cells["序号"].Value = i + 1;
                    }

                }

            }
        }

        private void 帮助ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string strCommandLine = Environment.GetCommandLineArgs()[0]; // 获取命令行参数  
            string directoryPath = Path.GetDirectoryName(strCommandLine); // 获取程序运行的目录  
            string filePath = Path.Combine(directoryPath, "help.pdf"); // 组合文件路径
            //Process.Start(filePath);
        }

        private void ToolStripMenuItem_param_huangui_Click(object sender, EventArgs e)
        {
            RingGauge_Para s_RingGauge_parameter = new RingGauge_Para();
            s_RingGauge_parameter.ShowDialog();
        }
    }
}
