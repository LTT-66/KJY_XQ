using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MeasureMachine
{
    internal class SqliteHelper
    {
        private System.Data.SQLite.SQLiteConnection MyConn;

        public SqliteHelper(string datasource)
        {
           if (!File.Exists(datasource))
            {
                MessageBox.Show("数据异常,没有找到数据源db文件");
                return;
            }
            //连接数据库
            SQLiteConnectionStringBuilder connstr = new SQLiteConnectionStringBuilder();
            connstr.DataSource = datasource;
            //connstr.Version = 3;
            //connstr.Password = password; //设置密码，SQLite ADO.NET实现了数据库密码保护
            MyConn = new SQLiteConnection(connstr.ToString());
        }

        public DataTable GetDataTable(string sql)
        {
            if (MyConn.State != ConnectionState.Open)
                MyConn.Open();
            DataTable dt = new DataTable();
            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(sql, MyConn))
                {
                    using (SQLiteDataAdapter dao = new SQLiteDataAdapter(cmd))
                    {
                        dao.Fill(dt);
                    }
                }
            }
            finally
            {
                MyConn.Close();
            }
            return dt;
        }


        public bool ExecSqlCommand(string sql)
        {
            bool iResult = false;
            if (MyConn.State != ConnectionState.Open) MyConn.Open();
            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(sql, MyConn))
                {
                    iResult = cmd.ExecuteNonQuery() > 0;
                }
            }
            finally
            {
                MyConn.Close();
            }
            return iResult;
        }

    }
}
