using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeasureMachine
{
    class Profile
    {
        private static IniFile _file;//内置了一个对象
        public static string G_PORTNAME = "COM1";//温度压力
        public static string G_BAUDRATE = "9600";//给ini文件赋新值，并且影响界面下拉框的显示
        public static string G_DATABITS = "8";
        public static string G_STOP = "1";
        public static string G_PARITY = "NONE";
        public static string G_PORTNAME2 = "COM3";//光栅尺
        public static string G_BAUDRATE2 = "9600";
        public static string G_COM_TIME_INTERVAL = "200";//com定时间隔

        
        public static string G_OVERALL_LENGTH = "2000";//全长
        public static string G_GAP_LENGTH = "100";//长度间隔

        public static string G_SAVE_PATH = @"D:\";  //选择路径

        public static string G_GATINGCOMPENSATION_DECIMAL_NUM = "F4";//光栅补偿小数精度
        public static string G_RASTER_DECIMAL_NUM = "F4";//光栅尺小数精度 
        public static string G_PROBE_DECIMAL_NUM = "F4";//长度计小数精度

        //圆弧修正补偿
        public static string[] G_COMPEN_CIRCLE_HUANGUI_COEF = new string[5];//环规圆弧补偿系数
        public static string[] G_COMPEN_CIRCLE_HUANGUI_MAX = new string[4];//圆弧补偿分段边界
        public static string G_COMPEN_MATERIAL_HUANGUI_COEF = "11.5";//材料膨胀系数 
        public static string G_COMPEN_EXPERENCE_HUANGUI_VALUE_B = "0";//经验值delta修正
        public static string G_COMPEN_EXPERENCE_HUANGUI_VALUE_K = "0";//经验值delta修正

        public static void LoadProfile()
        {
            string strPath = AppDomain.CurrentDomain.BaseDirectory;
            _file = new IniFile(strPath + "config.ini");
            G_PORTNAME = _file.ReadString("COM", "portname", "COM1");
            G_BAUDRATE = _file.ReadString("COM", "BaudRate", "9600");    //读数据，下同
            G_DATABITS = _file.ReadString("COM", "DataBits", "8");
            G_STOP = _file.ReadString("COM", "StopBits", "1");
            G_PARITY = _file.ReadString("COM", "Parity", "NONE");
            G_PORTNAME2 = _file.ReadString("COM", "portname2", "COM3");
            G_BAUDRATE2 = _file.ReadString("COM", "BaudRate2", "9600");
            G_COM_TIME_INTERVAL = _file.ReadString("COM", "TimeInterval", "200");

            G_OVERALL_LENGTH = _file.ReadString("LENGTH", "OverallLength", "2000");
            G_GAP_LENGTH = _file.ReadString("LENGTH", "GapLength", "100");

            G_SAVE_PATH = _file.ReadString("PATH", "SavePath", @"D:\");


            //环规圆弧修正补偿
            for (int i = 0; i < 5; i++)
                G_COMPEN_CIRCLE_HUANGUI_COEF[i] = _file.ReadString("COMPENSATE", "G_COMPEN_CIRCLE_HUANGUI_COEF" + (i + 1).ToString(), "5.5");
            G_COMPEN_CIRCLE_HUANGUI_MAX[0] = _file.ReadString("COMPENSATE", "G_COMPEN_CIRCLE_HUANGUI_MAX1", "5.1");
            G_COMPEN_CIRCLE_HUANGUI_MAX[1] = _file.ReadString("COMPENSATE", "G_COMPEN_CIRCLE_HUANGUI_MAX2", "10.1");
            G_COMPEN_CIRCLE_HUANGUI_MAX[2] = _file.ReadString("COMPENSATE", "G_COMPEN_CIRCLE_HUANGUI_MAX3", "100.1");
            G_COMPEN_CIRCLE_HUANGUI_MAX[3] = _file.ReadString("COMPENSATE", "G_COMPEN_CIRCLE_HUANGUI_MAX4", "200.1");
            G_COMPEN_MATERIAL_HUANGUI_COEF = _file.ReadString("COMPENSATE", "G_COMPEN_MATERIAL_HUANGUI_COEF", "11.5");
            G_COMPEN_EXPERENCE_HUANGUI_VALUE_B = _file.ReadString("COMPENSATE", "G_COMPEN_EXPERENCE_HUANGUI_VALUE_B", "0");
            G_COMPEN_EXPERENCE_HUANGUI_VALUE_K = _file.ReadString("COMPENSATE", "G_COMPEN_EXPERENCE_HUANGUI_VALUE_K", "0");
        }


        public static void SaveProfile()
        {
            string strPath = AppDomain.CurrentDomain.BaseDirectory;
            _file = new IniFile(strPath + "config.ini");
            _file.WriteString("COM", "portname", G_PORTNAME);
            _file.WriteString("COM", "BaudRate", G_BAUDRATE);            //写数据，下同
            _file.WriteString("COM", "DataBits", G_DATABITS);
            _file.WriteString("COM", "StopBits", G_STOP);
            _file.WriteString("COM", "G_PARITY", G_PARITY);
            _file.WriteString("LENGTH", "OverallLength", G_OVERALL_LENGTH);
            _file.WriteString("LENGTH", "GapLength", G_GAP_LENGTH);
            _file.WriteString("PATH", "SavePath", G_SAVE_PATH);
        }


        public static void SaveHuanguiProfile()
        {
            string strPath = AppDomain.CurrentDomain.BaseDirectory;
            _file = new IniFile(strPath + "config.ini");
            for (int i = 0; i < 5; i++)
                _file.WriteString("COMPENSATE", "G_COMPEN_CIRCLE_HUANGUI_COEF" + (i + 1).ToString(), G_COMPEN_CIRCLE_HUANGUI_COEF[i]);

            for (int i = 0; i < 4; i++)
                _file.WriteString("COMPENSATE", "G_COMPEN_CIRCLE_HUANGUI_MAX" + (i + 1).ToString(), G_COMPEN_CIRCLE_HUANGUI_MAX[i]);

            _file.WriteString("COMPENSATE", "G_COMPEN_MATERIAL_HUANGUI_COEF", G_COMPEN_MATERIAL_HUANGUI_COEF);
            _file.WriteString("COMPENSATE", "G_COMPEN_EXPERENCE_HUANGUI_VALUE_B", G_COMPEN_EXPERENCE_HUANGUI_VALUE_B);
            _file.WriteString("COMPENSATE", "G_COMPEN_EXPERENCE_HUANGUI_VALUE_K", G_COMPEN_EXPERENCE_HUANGUI_VALUE_K);
        }

    }
}
