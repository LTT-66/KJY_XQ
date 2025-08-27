namespace MeasureMachine
{
    partial class CompensationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompensationForm));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbSerial = new System.Windows.Forms.ComboBox();
            this.txtOverallLength = new System.Windows.Forms.TextBox();
            this.txtGapLength = new System.Windows.Forms.TextBox();
            this.button_OpenExcel = new System.Windows.Forms.Button();
            this.cbBaudRate = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btn_ReadCompensation = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(23, 50);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 82;
            this.dataGridView1.RowTemplate.Height = 37;
            this.dataGridView1.Size = new System.Drawing.Size(963, 752);
            this.dataGridView1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(1035, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(165, 37);
            this.label1.TabIndex = 1;
            this.label1.Text = "端口号：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(1034, 268);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(214, 42);
            this.label2.TabIndex = 2;
            this.label2.Text = "全长(mm)：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(1035, 363);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(214, 42);
            this.label3.TabIndex = 3;
            this.label3.Text = "间隔(mm)：";
            // 
            // cbSerial
            // 
            this.cbSerial.Font = new System.Drawing.Font("Times New Roman", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbSerial.FormattingEnabled = true;
            this.cbSerial.Location = new System.Drawing.Point(1282, 100);
            this.cbSerial.Name = "cbSerial";
            this.cbSerial.Size = new System.Drawing.Size(121, 50);
            this.cbSerial.TabIndex = 4;
            // 
            // txtOverallLength
            // 
            this.txtOverallLength.Font = new System.Drawing.Font("Times New Roman", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOverallLength.Location = new System.Drawing.Point(1282, 274);
            this.txtOverallLength.Name = "txtOverallLength";
            this.txtOverallLength.Size = new System.Drawing.Size(121, 50);
            this.txtOverallLength.TabIndex = 5;
            // 
            // txtGapLength
            // 
            this.txtGapLength.Font = new System.Drawing.Font("Times New Roman", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGapLength.Location = new System.Drawing.Point(1282, 363);
            this.txtGapLength.Name = "txtGapLength";
            this.txtGapLength.Size = new System.Drawing.Size(121, 50);
            this.txtGapLength.TabIndex = 6;
            // 
            // button_OpenExcel
            // 
            this.button_OpenExcel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.button_OpenExcel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_OpenExcel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_OpenExcel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_OpenExcel.Location = new System.Drawing.Point(1041, 463);
            this.button_OpenExcel.Name = "button_OpenExcel";
            this.button_OpenExcel.Size = new System.Drawing.Size(247, 71);
            this.button_OpenExcel.TabIndex = 7;
            this.button_OpenExcel.Text = "写入补偿文件";
            this.button_OpenExcel.UseVisualStyleBackColor = false;
            this.button_OpenExcel.Click += new System.EventHandler(this.button_OpenExcel_Click);
            // 
            // cbBaudRate
            // 
            this.cbBaudRate.Font = new System.Drawing.Font("Times New Roman", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbBaudRate.FormattingEnabled = true;
            this.cbBaudRate.Items.AddRange(new object[] {
            "300",
            "600",
            "1200",
            "2400",
            "4800",
            "9600",
            "19200",
            "38400",
            "115200"});
            this.cbBaudRate.Location = new System.Drawing.Point(1282, 181);
            this.cbBaudRate.Name = "cbBaudRate";
            this.cbBaudRate.Size = new System.Drawing.Size(121, 50);
            this.cbBaudRate.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(1035, 181);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(165, 37);
            this.label4.TabIndex = 8;
            this.label4.Text = "波特率：";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btn_ReadCompensation
            // 
            this.btn_ReadCompensation.BackColor = System.Drawing.Color.YellowGreen;
            this.btn_ReadCompensation.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_ReadCompensation.Font = new System.Drawing.Font("宋体", 12F);
            this.btn_ReadCompensation.Location = new System.Drawing.Point(1041, 565);
            this.btn_ReadCompensation.Name = "btn_ReadCompensation";
            this.btn_ReadCompensation.Size = new System.Drawing.Size(246, 67);
            this.btn_ReadCompensation.TabIndex = 10;
            this.btn_ReadCompensation.Text = "读取补偿文件";
            this.btn_ReadCompensation.UseVisualStyleBackColor = false;
            this.btn_ReadCompensation.Click += new System.EventHandler(this.btn_ReadCompensation_Click);
            // 
            // CompensationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1605, 832);
            this.Controls.Add(this.btn_ReadCompensation);
            this.Controls.Add(this.cbBaudRate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button_OpenExcel);
            this.Controls.Add(this.txtGapLength);
            this.Controls.Add(this.txtOverallLength);
            this.Controls.Add(this.cbSerial);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CompensationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Compensation";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CompensationForm_FormClosed);
            this.Load += new System.EventHandler(this.CompensationForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbSerial;
        private System.Windows.Forms.TextBox txtOverallLength;
        private System.Windows.Forms.TextBox txtGapLength;
        private System.Windows.Forms.Button button_OpenExcel;
        private System.Windows.Forms.ComboBox cbBaudRate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btn_ReadCompensation;
    }
}