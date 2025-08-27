using MeasureMachine.serialPort;
using NPOI.SS.Formula.PTG;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MeasureMachine
{
    public partial class SettingForm : Form
    {
        public delegate void DecodeDelegate(string valuestr1,string valuestr2);//1
        public event DecodeDelegate DecodeDelegateFun; //2

        //定义委托方法
        public delegate void TextChangedEventHandler(string text1,string text2,string text3,string text4);
        // 定义一个事件，当textBox1的值改变时触发  
        public event TextChangedEventHandler TextChanged;


        //串口配置
        public static SerialPortBase sp2 = null;  //光栅补偿
        double m_gating = 0.0;                    //光栅尺 
        double m_gating2 = 0.0;                    //补偿后光栅尺
        double m_probe = 0.0;                     //长度计
        double m_measure = 0.0;                   //测量值
        public bool isStop = false;  //串口服务是否停止
        byte[] cmdBuffer = new byte[1];  //数据缓存
        public const byte CMD_GRATING_LOCATION = 0xA4;//光栅尺当前位置
        public const byte CMD_GRATING_ZERO = 0xA0;//光栅尺清零
        public const byte CMD_HEADER1_S2 = 0x5A;//
        public const byte CMD_HEADER2_S2 = 0xA5;//

        //补偿设置
        Compensation m_compen = new Compensation();
        double[] compen_circle_max = new double[4];
        double[] compen_circle_coef = new double[5];
        double compen_material_coef = 11.5;//11.5
        double compen_experience_k = 0;
        double compen_experience_b = 0;

        // 声明一个全局变量  
        public static string RasterValue;
        public static int intRasterFX;
        public static int intProbeFX;

        public SettingForm()
        {
            InitializeComponent();

            sp2 = new SerialPortBase();

            //DecodeDelegateFun += new DecodeDelegate(DecodeDelegateEvent);//2


            // 预置波特率
            switch (Profile.G_BAUDRATE)
            {
                case "300":
                    cboRate.SelectedIndex = 0;
                    break;
                case "600":
                    cboRate.SelectedIndex = 1;
                    break;
                case "1200":
                    cboRate.SelectedIndex = 2;
                    break;
                case "2400":
                    cboRate.SelectedIndex = 3;
                    break;
                case "4800":
                    cboRate.SelectedIndex = 4;
                    break;
                case "9600":
                    cboRate.SelectedIndex = 5;
                    break;
                case "19200":
                    cboRate.SelectedIndex = 6;
                    break;
                case "38400":
                    cboRate.SelectedIndex = 7;
                    break;
                case "115200":
                    cboRate.SelectedIndex = 8;
                    break;
                default:
                    {
                        MessageBox.Show("波特率预置参数错误。");
                        return;
                    }
            }

            cboRate.SelectedIndex = 5;
         

            //检查是否含有串口
            string[] str = SerialPort.GetPortNames();
            if (str.Length == 0)
            {
                MessageBox.Show("本机光栅尺没有串口！", "Error");
                return;
            }

            //添加串口项目
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {//获取有多少个COM口
                //System.Diagnostics.Debug.WriteLine(s);
                cboPortRaster.Items.Add(s);
            }
            cboPortRaster.Text = Profile.G_PORTNAME;
            cboRate.Text = Profile.G_BAUDRATE;

            txtPath.Text = Profile.G_SAVE_PATH;

            Control.CheckForIllegalCrossThreadCalls = false;    //这个类中我们不检查跨线程的调用是否合法(因为.net 2.0以后加强了安全机制,，不允许在winform中直接跨线程访问控件的属性)
            sp2.DataReceived += new SerialDataReceivedEventHandler(sp2_DataReceived);

            //准备就绪              
            sp2.DtrEnable = true;
            sp2.RtsEnable = true;
            sp2.ReadTimeout = 1000;
            sp2.Close();

        }

 

        private void btnPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbdPath = new FolderBrowserDialog();
            fbdPath.Description = "选择保存路径";
            fbdPath.RootFolder = Environment.SpecialFolder.MyComputer;
            fbdPath.ShowNewFolderButton = true;

            if (fbdPath.ShowDialog() == DialogResult.OK)
            {
                string strFolder = fbdPath.SelectedPath + "\\";
                txtPath.Text = strFolder;
            }
        }

        private void btnSetOK_Click_1(object sender, EventArgs e)
        {
            if (cboRasterFX.SelectedItem.ToString() == "正向")
            {
                intRasterFX = 1;
            }
            else
            {
                intRasterFX = -1;
            }
            

            TextChanged(txtTempDevice.Text, txtTempObj.Text,txtCoDevice.Text,txtCoObj.Text);
            //TextChanged?.Invoke(txtTempDevice.Text, txtTempObj.Text);

            openSerial();
            timer1.Start(); // 启动定时器 
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (sp2.IsOpen)//发送光栅尺读取当前计数值请求
            {
                try
                {
                    cmdBuffer[0] = CMD_GRATING_LOCATION;
                    sp2.Write(cmdBuffer, 0, cmdBuffer.Length);
                }
                catch (Exception er)
                {

                }
            }
        }

        
        void data_received(ref SerialPortBase sp, byte HEADER1, byte HEADER2)
        {
            if (sp.IsOpen)     //此处可能没有必要判断是否打开串口，但为了严谨性，我还是加上了
            {
                try
                {
                    if (sp.BytesToRead < 10)
                    {
                        Console.WriteLine("函数被调用");
                        return;
                    }
                    Byte[] receivedData = new Byte[sp.BytesToRead];        //创建接收字节数组
                    sp.Read(receivedData, 0, receivedData.Length);         //读取数据
                                                                           //string text = sp1.Read();   //Encoding.ASCII.GetString(receivedData);
                    sp.DiscardInBuffer();                                  //清空SerialPort控件的Buffer

                    string strRcv = null;
                    //int decNum = 0;//存储十进制
                    for (int i = 0; i < receivedData.Length; i++) //窗体显示
                    {
                        strRcv += receivedData[i].ToString("X2") + " ";  //16进制显示
                        sp._bufqueue.Enqueue(receivedData[i]);//进队列
                    }
                    //队列解码
                    read_from_queue(ref sp, HEADER1, HEADER2);//环境、瞄准电压、光栅尺
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message, "出错提示");
                }
            }
            else
            {
                MessageBox.Show("请打开某个串口", "错误提示");
            }
        }

        void sp2_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            data_received(ref sp2, CMD_HEADER1_S2, CMD_HEADER2_S2);
        }


        /// <summary>
        /// 接收的数据加入队列，依据帧的指定长度拼出完整一帧数据  环境、瞄准电压、光栅尺
        /// </summary>
        /// <param name="sp">串口</param>
        /// <param name="HEADER1">帧头1</param>
        /// <param name="HEADER2">帧头2</param>
        /// <param name="process_method">最终解码的方法</param>
        private void read_from_queue(ref SerialPortBase sp, byte HEADER1, byte HEADER2)
        {
            byte len_endn = 10;
            while (sp._bufqueue.Count() > 0)
            {
                sp.rxFE1 = sp.rxFE2;
                sp.rxFE2 = sp.rxch;

                sp.rxch = sp._bufqueue.Dequeue();

                if ((sp.rxFE1 == HEADER1) && (sp.rxFE2 == HEADER2) && !sp.EnRev)
                {
                    sp.EnRev = true;//找到开头
                    sp.rx_buf[0] = HEADER1;
                    sp.rx_buf[1] = HEADER2;
                    sp.ridx = 2;//帧头长度
                }
                if (sp.EnRev)
                {
                    sp.rx_buf[sp.ridx++] = sp.rxch;
                    if (sp.ridx == len_endn) // a frame data is received completed
                    {
                        sp.ridx = 0;
                        sp.EnRev = false;//可以重新找开头0xFE
                        process_data(ref sp);  //光栅尺
                    }
                }
            }
        }

        private void process_data(ref SerialPortBase sp)
        {
            string datastr_gating = "";
            string datastr_gating2 = "";
            try
            {
                m_gating = bytesToInt2(sp.rx_buf, 6) / 10000.0;
                m_gating2 = CalculateCompensatedResult(m_gating);

                datastr_gating = m_gating.ToString(Profile.G_RASTER_DECIMAL_NUM);
                datastr_gating2 = m_gating2.ToString(Profile.G_RASTER_DECIMAL_NUM);

                DecodeDelegateFun(datastr_gating, datastr_gating2);
            }
            catch (Exception er)
            {

            }

            sp.rxFE1 = 0x00;
            sp.rxFE2 = 0x00;
            sp.ridx = 0x00;
            sp.rx_len = 0;
            sp.rxch = 0;
            sp.EnRev = false;
        }


        public double CalculateCompensatedResult(double result)
        {
            for (int i = 0; i < 5; i++)
                compen_circle_coef[i] = Convert.ToDouble(Profile.G_COMPEN_CIRCLE_HUANGUI_COEF[i]);//5.5
            for (int i = 0; i < 4; i++)
                compen_circle_max[i] = Convert.ToDouble(Profile.G_COMPEN_CIRCLE_HUANGUI_MAX[i]);
            compen_material_coef = Convert.ToDouble(Profile.G_COMPEN_MATERIAL_HUANGUI_COEF);//11.5
            compen_experience_k = Convert.ToDouble(Profile.G_COMPEN_EXPERENCE_HUANGUI_VALUE_K);
            compen_experience_b = Convert.ToDouble(Profile.G_COMPEN_EXPERENCE_HUANGUI_VALUE_B);

            //圆弧补偿 
            result = m_compen.Compensate_Circle_Method(result, compen_circle_max, compen_circle_coef);

            //材料温度膨胀 
            double t = 0;
            t = Convert.ToDouble(this.txtTempDevice.Text.Trim());  //机器温度、环境温度
            if (t > 0 && t < 50)
            {
                result = m_compen.Compensate_Material_Method(result, compen_material_coef, t);
            }

            //线性修正 
            result = result + compen_experience_k * result + compen_experience_b;

            return result;
        }


        //byte数组中取int数值，本方法适用于(低位在后，高位在前)的顺序。和intToBytes2（）配套使用

        public int bytesToInt2(byte[] src, int offset)
        {
            int value;
            value = (int)(((src[offset] & 0xFF) << 24)
                    | ((src[offset + 1] & 0xFF) << 16)
                    | ((src[offset + 2] & 0xFF) << 8)
                    | (src[offset + 3] & 0xFF));
            return value;
        }

        ///// <summary>
        ///// 更新界面数据
        ///// </summary>
        ///// <param name="type"></param>
        ///// <param name="valuestr"></param>
        //private void DecodeDelegateEvent(string valuestr)//3
        //{
        //    //RasterValue = valuestr; 
        //    lblRasterValue = valuestr;
        //}

        public void openSerial()
        {
            try
            {
                if (!sp2.IsOpen)
                {
                    //设置串口号
                    if (cboPortRaster.SelectedItem == null)
                    {
                        MessageBox.Show("未选择光栅尺串口！", "Error");
                        return;
                    }
                    string serialName = cboPortRaster.SelectedItem.ToString();
                    sp2.PortName = serialName;

                    //设置各“串口设置”
                    string strBaudRate = cboRate.Text;
                    Int32 iBaudRate = Convert.ToInt32(strBaudRate);

                    sp2.BaudRate = iBaudRate;       //波特率 
                }

                if (sp2.IsOpen == true)//如果打开状态，则先关闭一下
                {
                    sp2.Close();
                }
                //设置必要控件不可用
                cboPortRaster.Enabled = false;
                cboRate.Enabled = false;
                sp2.Open();     //打开串口
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("打开光栅尺串口失败:" + ex.Message, "Error");
                cboPortRaster.Enabled = true;
                cboRate.Enabled = true;

                return;
            }
        }

    }
}
