using MeasureMachine.serialPort;
using Microsoft.Office.Interop.Excel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Action = System.Action;
using DataTable = System.Data.DataTable;
using Excel = Microsoft.Office.Interop.Excel;

namespace MeasureMachine
{
    public partial class CompensationForm : Form
    {
        //串口配置
        public static SerialPortBase sp1 = null;  //光栅补偿
        public bool isStop = false;  //串口服务是否停止
        byte[] cmdBuffer = new byte[1];  //数据缓存
        public const byte CMD_ReadCompensation = 0xA1;//分段补偿值输出，将分段补偿原始数据从数显表读取
        

        double m_compensation = 0.0;                    //补偿值 
       
        private static object convert;

        DataTable dt;
        // 创建MyData对象列表  
        List<MyData> myList=new List<MyData>();

        public CompensationForm()
        {
            InitializeComponent();
            //textBox_time_interval.Text = Profile.G_COM_TIME_INTERVAL;
            sp1 = new SerialPortBase();
            // 预置波特率
            switch (Profile.G_BAUDRATE)
            {
                case "300":
                    cbBaudRate.SelectedIndex = 0;
                    break;
                case "600":
                    cbBaudRate.SelectedIndex = 1;
                    break;
                case "1200":
                    cbBaudRate.SelectedIndex = 2;
                    break;
                case "2400":
                    cbBaudRate.SelectedIndex = 3;
                    break;
                case "4800":
                    cbBaudRate.SelectedIndex = 4;
                    break;
                case "9600":
                    cbBaudRate.SelectedIndex = 5;
                    break;
                case "19200":
                    cbBaudRate.SelectedIndex = 6;
                    break;
                case "38400":
                    cbBaudRate.SelectedIndex = 7;
                    break;
                case "115200":
                    cbBaudRate.SelectedIndex = 8;
                    break;
                default:
                    {
                        MessageBox.Show("波特率预置参数错误。");
                        return;
                    }
            }

            cbBaudRate.SelectedIndex = 5;
            txtOverallLength.Text = Profile.G_OVERALL_LENGTH;
            txtGapLength.Text = Profile.G_GAP_LENGTH;

            //检查是否含有串口
            string[] str = SerialPort.GetPortNames();
            if (str.Length == 0)
            {
                MessageBox.Show("本机没有串口！", "Error");
                return;
            }

            //添加串口项目
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {//获取有多少个COM口
                //System.Diagnostics.Debug.WriteLine(s);
                cbSerial.Items.Add(s);
            }
            cbSerial.Text = Profile.G_PORTNAME;
            cbBaudRate.Text = Profile.G_BAUDRATE;
            


            Control.CheckForIllegalCrossThreadCalls = false;    //这个类中我们不检查跨线程的调用是否合法(因为.net 2.0以后加强了安全机制,，不允许在winform中直接跨线程访问控件的属性)
            //sp1.DataReceived += new SerialDataReceivedEventHandler(sp1_DataReceived);
           

 
            //准备就绪              
            sp1.DtrEnable = true;
            sp1.RtsEnable = true;
            sp1.ReadTimeout = 1000;
            sp1.Close();
        }

        private void button_OpenExcel_Click(object sender, EventArgs e)
        {
            int nCount = 1 + Convert.ToInt32(Profile.G_OVERALL_LENGTH) / Convert.ToInt32(Profile.G_GAP_LENGTH);
            int[] nDelta = new int[nCount];
            String strCommand = "A0";
            byte[] sendBytes = new byte[nCount];
            // 创建 Application 对象  
            Excel.Application xlApp = new Excel.Application();

            if (xlApp == null)
            {
                Console.WriteLine("Excel 不能正确启动，请检查你的电脑是否安装了Excel");
                return;
            }

            // 打开 Excel 文件  
            string strCommandLine = Environment.GetCommandLineArgs()[0]; // 获取命令行参数  
            string directoryPath = Path.GetDirectoryName(strCommandLine); // 获取程序运行的目录  
            string filePath = Path.Combine(directoryPath, "data.xlsx"); // 组合文件路径
            XSSFWorkbook workbook = null;
            try
            {
                FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                workbook = new XSSFWorkbook(fs);
                fs.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("excel未正常打开，错误原因：" + ex.Message);
            }

            // 获取第一个工作表
            ISheet sheet = workbook.GetSheetAt(0);

            //获取第一个工作表第一列和第二列数据差值,并转换为16进制
            if (sheet != null)
            {
                for (int rowIndex = 1; rowIndex < nCount; rowIndex++)
                {
                    IRow row = sheet.GetRow(rowIndex);
                    if (row != null)
                    {
                        double columnAValue = GetCellValue(row, 0);
                        double columnBValue = GetCellValue(row, 1);
                        double resultValue = columnAValue - columnBValue;

                        double flDeltaTMP = resultValue / rowIndex;
                        int nDeltaTMP = (int)(flDeltaTMP * 10000);

                        nDelta[rowIndex] = nDeltaTMP;

                        //将差值（nDelta[i]）转化为16进制字符串，
                        //并根据该字符串的长度进行补零操作，
                        //最后将处理后的字符串拼接到strCommand字符串中。
                        if (nDelta[rowIndex] < 0)
                        {
                            string str = nDelta[rowIndex].ToString("X");  //将nDelta[i]转化为16进制字符串并赋值给str。这一步保证了无论nDelta[i]是正数还是负数，都会转化为16进制的字符串。
                            string strAll = str.Substring(str.Length - 4);
                            string str1 = " " + strAll.Substring(0, 2);
                            string str2 = " " + strAll.Substring(2);
                            strCommand += str1 + str2;
                        }
                        else
                        {
                            string str = nDelta[rowIndex].ToString("X");
                            int nLenth = str.Length;
                            for (int i = 0; i < 4 - nLenth; i++)
                            {
                                string str_Add_Zero_Prefix = "0";
                                str = str_Add_Zero_Prefix + str; //补零操作，写入缓存器的16进制值是8位，如果不够8位，在前面补0  
                            }
                            string strAll = str.Substring(str.Length - 4);
                            string str1 = " " + strAll.Substring(0, 2);
                            string str2 = " " + strAll.Substring(2);
                            strCommand += str1 + str2;
                        }
                    }
                }
            }
            
            sendBytes = String2Hex(strCommand, sendBytes);
            openSerial();
            //serialPort1.Write(sendBytes, 0, sendBytes.Length);
            sp1.Write(sendBytes, 0, sendBytes.Length);
        }

        /*
         m_msComm.put_OutBufferCount(0);
         m_msComm.put_Output(COleVariant(senddata));*/



        //for (int rowIndex = 0; rowIndex <= nCount; rowIndex++)
        //{
        //    IRow row = sheet.GetRow(rowIndex);
        //    if (row != null)
        //    {
        //        for (int columnIndex = 0; columnIndex < nCount; columnIndex++)
        //        {
        //            ICell cell = row.GetCell(columnIndex);
        //            if (cell != null)
        //            {

        //                string cellValue = cell.ToString();
        //                Console.WriteLine($"Cell({rowIndex},{columnIndex}) Value: {cellValue}");
        //            }
        //        }
        //    }
        //}


        private static double GetCellValue(IRow row, int columnIndex)
        {
            ICell cell = row.GetCell(columnIndex);
            try
            {
                if (cell != null)
                {
                    return Convert.ToDouble(cell.ToString());
                }
                return 0; // 返回默认值0，如果单元格为空或不存在  
            }
            catch 
            { 
                return 1;
            }
        }

        //private static byte[] ConvertStringToHex(string str)
        //{
        //    // 创建一个字节数组，长度为字符串长度的一半（因为每个字符可以转换为两个字节的十六进制）  
        //    byte[] byteArray = new byte[str.Length / 2];

        //    // 使用StringBuilder将字符串转换为十六进制，然后转换为字节数组  
        //    for (int i = 0; i < str.Length; i += 2)
        //    {
        //        string hex = str.Substring(i, 2);
        //        byteArray[i / 2] = Convert.ToByte(hex, 16);
        //    }
        //    return byteArray;
        //}

        private static char ConvertHexChar(char ch)
        {
            if (ch >= '0' && ch <= '9')
                return (char)(ch - '0');
            else if (ch >= 'A' && ch <= 'F')
                return (char)(ch - 'A' + 10);
            else if (ch >= 'a' && ch <= 'f')
                return (char)(ch - 'a' + 10);
            else
                return '\0'; // or you can return some other value depending on your needs  
        }

        public static byte[] String2Hex(string str, byte[] senddata)
        {
            int hexdata, lowhexdata;
            int hexdatalen = 0;
            int len = str.Length;
            senddata = new byte[len / 2];
            for (int i = 0; i < len;)
            {
                char lstr;
                char hstr = str[i];
                if (hstr == ' ')
                {
                    i++;
                    continue;
                }
                i++;
                if (i >= len)
                    break;
                lstr = str[i];
                hexdata = ConvertHexChar(hstr);
                lowhexdata = ConvertHexChar(lstr);
                if ((hexdata == 16) || (lowhexdata == 16))
                    break;
                else
                    hexdata = hexdata * 16 + lowhexdata;
                i++;
                senddata[hexdatalen] = (byte)hexdata;
                hexdatalen++;
            }
            senddata = senddata.Take(hexdatalen).ToArray();
            return senddata;
        }

        void sp1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            data_received(ref sp1);
        }
        // 创建MyData类
        class MyData
        {
            public int Number { get; set; }
            public string ValueCompensation { get; set; }
            public double ValueInterferometer { get; set; }
            public double ValueGrating { get; set; }
        }


        void data_received(ref SerialPortBase sp)
        {
            if (sp.IsOpen)     //此处可能没有必要判断是否打开串口，但为了严谨性，我还是加上了
            {
                try
                {
                    Thread.Sleep(500);
                    if (sp.BytesToRead < 122)
                    {
                        Console.WriteLine("函数被调用");
                        sp.DiscardInBuffer();
                        return;
                    }
                    //Thread.Sleep(100);
                    Byte[] receivedData = new Byte[sp.BytesToRead];        //创建接收字节数组
                    sp.Read(receivedData, 0, receivedData.Length);         //读取数据
                                                                           //string text = sp1.Read();   //Encoding.ASCII.GetString(receivedData);
                    sp.DiscardInBuffer();                                  //清空SerialPort控件的Buffer

                    string strRcv = null;

                

                    //int decNum = 0;//存储十进制
                    for (int i = 0; i < receivedData.Length; i++) 
                    {
                        strRcv += receivedData[i].ToString("X2");  //16进制显示
                        //sp._bufqueue.Enqueue(receivedData[i]);   //进队列
                    }
                    string datastr = "";
                    try
                    {
                        //strReceive = "0BA207D00A0505F16F6FF16F6DF16FC0F17F70F17F9EF17F7D9F48F0AF33F0AFEEF0CFB2F0CF3DF0CFA2F0EF99F1FFDCF0EFE6F10F24F10F8EF11FB3F12F14F12FAAF13F24F12F9BF13F13F14F0EF14F31F14FC2F14F4AF14FECF14FD7F15F17F15F1BF17F5AF15F42F15F47F14FBAF14FFAF15F83F15F82F1F1"  
                        int nCount = 1 + Convert.ToInt32(Profile.G_OVERALL_LENGTH) / Convert.ToInt32(Profile.G_GAP_LENGTH);

                        myList = new List<MyData>();

                        // 打开 Excel 文件  
                        string strCommandLine = Environment.GetCommandLineArgs()[0]; // 获取命令行参数  
                        string directoryPath = Path.GetDirectoryName(strCommandLine); // 获取程序运行的目录  
                        string filePath = Path.Combine(directoryPath, "data.xlsx"); // 组合文件路径
                        XSSFWorkbook workbook = null;
                        try
                        {
                            FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                            workbook = new XSSFWorkbook(fs);
                            fs.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("excel未正常打开，错误原因：" + ex.Message);
                        }

                        // 获取第一个工作表
                        ISheet sheet = workbook.GetSheetAt(0);

                        //获取第一个工作表第一列和第二列数据
                        if (sheet != null)
                        {
                            for (int i = 1; i < nCount; i++)
                            {
                                string strLeft = strRcv.Substring(0, 4);
                                strRcv = strRcv.Remove(0, 4);
                                char str_HeadBit = char.Parse(strLeft.Substring(0, 1));
                                long l_Temp = 0;
                                if (str_HeadBit == 'F')
                                    l_Temp = Convert.ToInt64(strLeft.Substring(0), 16) - 65536;
                                else
                                    l_Temp = Convert.ToInt64(strLeft.Substring(0), 16);
                                float f_Temp = (float)l_Temp / 10000; //读取缓存器得到脉冲数除以10000  
                                                                      //float f_Temp = (float)l_Temp; //读取缓存器得到脉冲数除以10000  
                                String str_CompensationData;
                                str_CompensationData = f_Temp.ToString(Profile.G_GATINGCOMPENSATION_DECIMAL_NUM);

                                IRow row = sheet.GetRow(i);
                                if (row != null)
                                {
                                    double columnAValue = GetCellValue(row, 0);
                                    double columnBValue = GetCellValue(row, 1);
                                    myList.Add(new MyData { Number = i,  ValueInterferometer = columnAValue,ValueGrating=columnBValue, ValueCompensation = str_CompensationData });
                                }

                                
                            }
                        }


                        
                        //dataGridView1.DataSource = null; // 清除原有数据源  
                        Console.WriteLine("reset the datasource for datagridview");
                        foreach (MyData data in myList)
                        {
                            Console.WriteLine(data.Number+" " + data.ValueCompensation);
                        }
                        //var source = new BindingSource();
                        //source.DataSource = myList;
                        //dataGridView1.DataSource = source;
                        BeginInvoke(new Action(() =>
                        {
                            dataGridView1.DataSource = myList; // 重新绑定数据源
                        }));
                       
                    }
                    catch (Exception er)
                    {

                    }

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



        private void timer1_Tick(object sender, EventArgs e)
        {
            
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

        //byte数组中取int数值，本方法适用于(低位在后，高位在前)的顺序。和intToBytes2（）配套使用
        public int bytesToIntBy2Bytes(byte[] src, int offset)
        {
            int value;
            value = (int)(((src[offset] & 0xFF) << 8)
                    | (src[offset + 1] & 0xFF));
            return value;
        }

        private void CompensationForm_Load(object sender, EventArgs e)
        {
            //openSerial();
            // 创建DataGridView的列并添加到控件中  
            DataGridViewTextBoxColumn numberColumn = new DataGridViewTextBoxColumn();
            numberColumn.HeaderText = "序号";
            numberColumn.DataPropertyName = "Number"; // 与数据源中的属性名一致  
            dataGridView1.Columns.Add(numberColumn);

            DataGridViewTextBoxColumn value2Column = new DataGridViewTextBoxColumn();
            value2Column.HeaderText = "标准值(mm)";
            value2Column.DataPropertyName = "ValueInterferometer"; // 与数据源中的属性名一致  
            dataGridView1.Columns.Add(value2Column);

            DataGridViewTextBoxColumn value3Column = new DataGridViewTextBoxColumn();
            value3Column.HeaderText = "测量值(mm)";
            value3Column.DataPropertyName = "ValueGrating"; // 与数据源中的属性名一致  
            dataGridView1.Columns.Add(value3Column);

            DataGridViewTextBoxColumn value1Column = new DataGridViewTextBoxColumn();
            value1Column.HeaderText = "补偿值(mm)";
            value1Column.DataPropertyName = "ValueCompensation"; // 与数据源中的属性名一致  
            dataGridView1.Columns.Add(value1Column);

            dataGridView1.DataSource = myList; // 绑定数据源
            Console.WriteLine("INIT table header on the load page");
        }

        public void openSerial()
        {
            try
            {
                if (!sp1.IsOpen)
                {
                    //设置串口号
                    if (cbSerial.SelectedItem == null)
                    {
                        MessageBox.Show("未选择串口！", "Error");
                        return;
                    }
                    string serialName = cbSerial.SelectedItem.ToString();
                    sp1.PortName = serialName;

                    //设置各“串口设置”
                    string strBaudRate = cbBaudRate.Text;
                    Int32 iBaudRate = Convert.ToInt32(strBaudRate);

                    sp1.BaudRate = iBaudRate;       //波特率 
                }

                if (sp1.IsOpen == true)//如果打开状态，则先关闭一下
                {
                    sp1.Close();
                }
                //设置必要控件不可用
                cbSerial.Enabled = false;
                cbBaudRate.Enabled = false;
                sp1.Open();     //打开串口
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("打开光栅尺补偿串口失败:" + ex.Message, "Error");
                cbSerial.Enabled = true;
                cbBaudRate.Enabled = true;

                return;
            }
        }

        private void btn_ReadCompensation_Click(object sender, EventArgs e)
        {
            if (isStop) return;
            if (!sp1.IsOpen) //如果没打开
            {
                openSerial();
            }
            if (sp1.IsOpen)//发送温度、湿度、瞄准请求
            {
                try
                {
                    cmdBuffer[0] = CMD_ReadCompensation;
                    sp1.Write(cmdBuffer, 0, cmdBuffer.Length);
                }
                catch (Exception er)
                {

                }
            }

            data_received(ref sp1);
        }

        private void CompensationForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (sp1.IsOpen)
            {
                sp1.Close ();
                cbSerial.Enabled = true;
                cbBaudRate.Enabled = true;
            }
        }
    }
}
