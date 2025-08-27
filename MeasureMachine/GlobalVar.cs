using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasureMachine
{
    internal class GlobalVar
    {
        public static bool partdel = false;//部分删除，不删除条件信息
        public static string username = "";
        public static int userright = -1;

        public static string connectstr = "";
        public static int userid = 0;//特殊用户

        public static string auto_check_str = "";
        public static int auto_check = 0;
        public static int close_win = 1;
    }
}
