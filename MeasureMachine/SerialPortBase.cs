using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;

namespace MeasureMachine.serialPort
{
    public  class SerialPortBase : SerialPort
    {
        public Byte rxFE1 = 0x00;
        public Byte rxFE2 = 0x00;
        public Byte ridx = 0x00;
        public Byte rx_len = 0;
        public Byte rxch = 0;
        public Byte[] rx_buf = new Byte[255];
        public bool EnRev = false;
        public Queue<Byte> _bufqueue = new Queue<Byte>();
    }
}
