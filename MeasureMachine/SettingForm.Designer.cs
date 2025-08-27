namespace MeasureMachine
{
    partial class SettingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.cboRate = new System.Windows.Forms.ComboBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnPath = new System.Windows.Forms.Button();
            this.Label1 = new System.Windows.Forms.Label();
            this.cboPortRaster = new System.Windows.Forms.ComboBox();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.cboRasterFX = new System.Windows.Forms.ComboBox();
            this.txtCoDevice = new System.Windows.Forms.TextBox();
            this.txtTempObj = new System.Windows.Forms.TextBox();
            this.txtTempDevice = new System.Windows.Forms.TextBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.txtCoObj = new System.Windows.Forms.TextBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.lblPortRaster = new System.Windows.Forms.Label();
            this.lblPortProbe = new System.Windows.Forms.Label();
            this.btnSetOK = new System.Windows.Forms.Button();
            this.serialPort2 = new System.IO.Ports.SerialPort(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.GroupBox2.SuspendLayout();
            this.GroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBox2
            // 
            this.GroupBox2.Controls.Add(this.Label3);
            this.GroupBox2.Controls.Add(this.cboRate);
            this.GroupBox2.Controls.Add(this.Label2);
            this.GroupBox2.Controls.Add(this.txtPath);
            this.GroupBox2.Controls.Add(this.btnPath);
            this.GroupBox2.Controls.Add(this.Label1);
            this.GroupBox2.Controls.Add(this.cboPortRaster);
            this.GroupBox2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.GroupBox2.Location = new System.Drawing.Point(48, 66);
            this.GroupBox2.Margin = new System.Windows.Forms.Padding(6);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Padding = new System.Windows.Forms.Padding(6);
            this.GroupBox2.Size = new System.Drawing.Size(417, 523);
            this.GroupBox2.TabIndex = 131;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "装置设置";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.Location = new System.Drawing.Point(54, 220);
            this.Label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(96, 28);
            this.Label3.TabIndex = 134;
            this.Label3.Text = "波特率";
            // 
            // cboRate
            // 
            this.cboRate.Font = new System.Drawing.Font("Calibri", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboRate.FormattingEnabled = true;
            this.cboRate.Items.AddRange(new object[] {
            "300",
            "600",
            "1200",
            "2400",
            "4800",
            "9600",
            "19200",
            "38400",
            "115200"});
            this.cboRate.Location = new System.Drawing.Point(220, 212);
            this.cboRate.Margin = new System.Windows.Forms.Padding(6);
            this.cboRate.Name = "cboRate";
            this.cboRate.Size = new System.Drawing.Size(146, 43);
            this.cboRate.TabIndex = 133;
            this.cboRate.Text = "19200";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(44, 334);
            this.Label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(124, 28);
            this.Label2.TabIndex = 131;
            this.Label2.Text = "保存路径";
            // 
            // txtPath
            // 
            this.txtPath.Font = new System.Drawing.Font("Times New Roman", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPath.Location = new System.Drawing.Point(36, 410);
            this.txtPath.Margin = new System.Windows.Forms.Padding(6);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(330, 40);
            this.txtPath.TabIndex = 130;
            // 
            // btnPath
            // 
            this.btnPath.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPath.Location = new System.Drawing.Point(210, 318);
            this.btnPath.Margin = new System.Windows.Forms.Padding(6);
            this.btnPath.Name = "btnPath";
            this.btnPath.Size = new System.Drawing.Size(160, 60);
            this.btnPath.TabIndex = 129;
            this.btnPath.Text = "选择路径";
            this.btnPath.UseVisualStyleBackColor = true;
            this.btnPath.Click += new System.EventHandler(this.btnPath_Click);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(44, 120);
            this.Label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(124, 28);
            this.Label1.TabIndex = 128;
            this.Label1.Text = "光栅串口";
            // 
            // cboPortRaster
            // 
            this.cboPortRaster.Font = new System.Drawing.Font("Calibri", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboPortRaster.FormattingEnabled = true;
            this.cboPortRaster.Location = new System.Drawing.Point(220, 112);
            this.cboPortRaster.Margin = new System.Windows.Forms.Padding(6);
            this.cboPortRaster.Name = "cboPortRaster";
            this.cboPortRaster.Size = new System.Drawing.Size(146, 43);
            this.cboPortRaster.TabIndex = 127;
            this.cboPortRaster.Text = "串口号";
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.Label6);
            this.GroupBox1.Controls.Add(this.cboRasterFX);
            this.GroupBox1.Controls.Add(this.txtCoDevice);
            this.GroupBox1.Controls.Add(this.txtTempObj);
            this.GroupBox1.Controls.Add(this.txtTempDevice);
            this.GroupBox1.Controls.Add(this.Label5);
            this.GroupBox1.Controls.Add(this.txtCoObj);
            this.GroupBox1.Controls.Add(this.Label4);
            this.GroupBox1.Controls.Add(this.lblPortRaster);
            this.GroupBox1.Controls.Add(this.lblPortProbe);
            this.GroupBox1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.GroupBox1.Location = new System.Drawing.Point(524, 66);
            this.GroupBox1.Margin = new System.Windows.Forms.Padding(6);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Padding = new System.Windows.Forms.Padding(6);
            this.GroupBox1.Size = new System.Drawing.Size(429, 523);
            this.GroupBox1.TabIndex = 130;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "环境设置";
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Font = new System.Drawing.Font("Times New Roman", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label6.Location = new System.Drawing.Point(44, 444);
            this.Label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(127, 33);
            this.Label6.TabIndex = 136;
            this.Label6.Text = "光栅方向";
            // 
            // cboRasterFX
            // 
            this.cboRasterFX.Font = new System.Drawing.Font("Calibri", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboRasterFX.FormattingEnabled = true;
            this.cboRasterFX.Items.AddRange(new object[] {
            "正向",
            "反向"});
            this.cboRasterFX.Location = new System.Drawing.Point(238, 436);
            this.cboRasterFX.Margin = new System.Windows.Forms.Padding(6);
            this.cboRasterFX.Name = "cboRasterFX";
            this.cboRasterFX.Size = new System.Drawing.Size(146, 43);
            this.cboRasterFX.TabIndex = 135;
            this.cboRasterFX.Text = "正向";
            // 
            // txtCoDevice
            // 
            this.txtCoDevice.Font = new System.Drawing.Font("Calibri", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCoDevice.Location = new System.Drawing.Point(238, 239);
            this.txtCoDevice.Margin = new System.Windows.Forms.Padding(6);
            this.txtCoDevice.Name = "txtCoDevice";
            this.txtCoDevice.Size = new System.Drawing.Size(146, 42);
            this.txtCoDevice.TabIndex = 130;
            this.txtCoDevice.Text = "11.5";
            this.txtCoDevice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtTempObj
            // 
            this.txtTempObj.Font = new System.Drawing.Font("Calibri", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTempObj.Location = new System.Drawing.Point(238, 144);
            this.txtTempObj.Margin = new System.Windows.Forms.Padding(6);
            this.txtTempObj.Name = "txtTempObj";
            this.txtTempObj.Size = new System.Drawing.Size(146, 42);
            this.txtTempObj.TabIndex = 129;
            this.txtTempObj.Text = "20.0";
            this.txtTempObj.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtTempDevice
            // 
            this.txtTempDevice.Font = new System.Drawing.Font("Calibri", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTempDevice.Location = new System.Drawing.Point(238, 65);
            this.txtTempDevice.Margin = new System.Windows.Forms.Padding(6);
            this.txtTempDevice.Name = "txtTempDevice";
            this.txtTempDevice.Size = new System.Drawing.Size(146, 42);
            this.txtTempDevice.TabIndex = 128;
            this.txtTempDevice.Text = "20.0";
            this.txtTempDevice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Font = new System.Drawing.Font("Times New Roman", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label5.Location = new System.Drawing.Point(20, 336);
            this.Label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(183, 66);
            this.Label5.TabIndex = 127;
            this.Label5.Text = "零件线胀系数\r\n（μm/℃/m）";
            // 
            // txtCoObj
            // 
            this.txtCoObj.Font = new System.Drawing.Font("Calibri", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCoObj.Location = new System.Drawing.Point(238, 348);
            this.txtCoObj.Margin = new System.Windows.Forms.Padding(6);
            this.txtCoObj.Name = "txtCoObj";
            this.txtCoObj.Size = new System.Drawing.Size(146, 42);
            this.txtCoObj.TabIndex = 126;
            this.txtCoObj.Text = "11.5";
            this.txtCoObj.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Font = new System.Drawing.Font("Times New Roman", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label4.Location = new System.Drawing.Point(20, 231);
            this.Label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(183, 66);
            this.Label4.TabIndex = 125;
            this.Label4.Text = "机器线胀系数\r\n（μm/℃/m）";
            // 
            // lblPortRaster
            // 
            this.lblPortRaster.AutoSize = true;
            this.lblPortRaster.Font = new System.Drawing.Font("Times New Roman", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPortRaster.Location = new System.Drawing.Point(12, 154);
            this.lblPortRaster.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblPortRaster.Name = "lblPortRaster";
            this.lblPortRaster.Size = new System.Drawing.Size(212, 33);
            this.lblPortRaster.TabIndex = 124;
            this.lblPortRaster.Text = "零件温度（℃）";
            // 
            // lblPortProbe
            // 
            this.lblPortProbe.AutoSize = true;
            this.lblPortProbe.Font = new System.Drawing.Font("Times New Roman", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPortProbe.Location = new System.Drawing.Point(12, 73);
            this.lblPortProbe.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblPortProbe.Name = "lblPortProbe";
            this.lblPortProbe.Size = new System.Drawing.Size(212, 33);
            this.lblPortProbe.TabIndex = 123;
            this.lblPortProbe.Text = "机器温度（℃）";
            // 
            // btnSetOK
            // 
            this.btnSetOK.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSetOK.Location = new System.Drawing.Point(422, 626);
            this.btnSetOK.Margin = new System.Windows.Forms.Padding(6);
            this.btnSetOK.Name = "btnSetOK";
            this.btnSetOK.Size = new System.Drawing.Size(150, 60);
            this.btnSetOK.TabIndex = 129;
            this.btnSetOK.Text = "确 定";
            this.btnSetOK.UseVisualStyleBackColor = true;
            this.btnSetOK.Click += new System.EventHandler(this.btnSetOK_Click_1);
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1035, 711);
            this.Controls.Add(this.GroupBox2);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.btnSetOK);
            this.Name = "SettingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设置";
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox2.PerformLayout();
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.GroupBox GroupBox2;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.ComboBox cboRate;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.TextBox txtPath;
        internal System.Windows.Forms.Button btnPath;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.ComboBox cboPortRaster;
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.ComboBox cboRasterFX;
        internal System.Windows.Forms.TextBox txtCoDevice;
        internal System.Windows.Forms.TextBox txtTempObj;
        internal System.Windows.Forms.TextBox txtTempDevice;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.TextBox txtCoObj;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label lblPortRaster;
        internal System.Windows.Forms.Label lblPortProbe;
        internal System.Windows.Forms.Button btnSetOK;
        private System.IO.Ports.SerialPort serialPort2;
        private System.Windows.Forms.Timer timer1;
    }
}