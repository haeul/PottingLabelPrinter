namespace PottingLabelPrinter.Forms
{
    partial class FormPortSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPortSetting));
            pnlPortsetting = new Panel();
            pnlPortSettingHeader = new Panel();
            lblPortSettingHeader = new Label();
            ImgInfac = new PictureBox();
            tlpPort = new TableLayoutPanel();
            btnPrinter = new Button();
            lblComBoard = new Label();
            lblPrinter = new Label();
            cmbComBoard = new ComboBox();
            cmbPrinter = new ComboBox();
            btnComBoard = new Button();
            pnlPortsetting.SuspendLayout();
            pnlPortSettingHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ImgInfac).BeginInit();
            tlpPort.SuspendLayout();
            SuspendLayout();
            // 
            // pnlPortsetting
            // 
            pnlPortsetting.BackColor = Color.White;
            pnlPortsetting.Controls.Add(pnlPortSettingHeader);
            pnlPortsetting.Controls.Add(tlpPort);
            pnlPortsetting.Location = new Point(4, 4);
            pnlPortsetting.Name = "pnlPortsetting";
            pnlPortsetting.Size = new Size(500, 191);
            pnlPortsetting.TabIndex = 8;
            // 
            // pnlPortSettingHeader
            // 
            pnlPortSettingHeader.BackColor = SystemColors.Control;
            pnlPortSettingHeader.Controls.Add(lblPortSettingHeader);
            pnlPortSettingHeader.Controls.Add(ImgInfac);
            pnlPortSettingHeader.Location = new Point(10, 10);
            pnlPortSettingHeader.Name = "pnlPortSettingHeader";
            pnlPortSettingHeader.Size = new Size(481, 65);
            pnlPortSettingHeader.TabIndex = 0;
            // 
            // lblPortSettingHeader
            // 
            lblPortSettingHeader.AutoSize = true;
            lblPortSettingHeader.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPortSettingHeader.ForeColor = SystemColors.ControlDarkDark;
            lblPortSettingHeader.Location = new Point(190, 10);
            lblPortSettingHeader.Name = "lblPortSettingHeader";
            lblPortSettingHeader.Size = new Size(201, 45);
            lblPortSettingHeader.TabIndex = 1;
            lblPortSettingHeader.Text = "Port Setting";
            // 
            // ImgInfac
            // 
            ImgInfac.Image = (Image)resources.GetObject("ImgInfac.Image");
            ImgInfac.Location = new Point(1, 1);
            ImgInfac.Name = "ImgInfac";
            ImgInfac.Size = new Size(180, 62);
            ImgInfac.SizeMode = PictureBoxSizeMode.Zoom;
            ImgInfac.TabIndex = 0;
            ImgInfac.TabStop = false;
            // 
            // tlpPort
            // 
            tlpPort.BackColor = SystemColors.Control;
            tlpPort.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tlpPort.ColumnCount = 3;
            tlpPort.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 37.9166679F));
            tlpPort.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 37.5F));
            tlpPort.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24.375F));
            tlpPort.Controls.Add(btnPrinter, 2, 1);
            tlpPort.Controls.Add(lblComBoard, 0, 0);
            tlpPort.Controls.Add(lblPrinter, 0, 1);
            tlpPort.Controls.Add(cmbComBoard, 1, 0);
            tlpPort.Controls.Add(cmbPrinter, 1, 1);
            tlpPort.Controls.Add(btnComBoard, 2, 0);
            tlpPort.Location = new Point(10, 85);
            tlpPort.Name = "tlpPort";
            tlpPort.RowCount = 2;
            tlpPort.RowStyles.Add(new RowStyle(SizeType.Percent, 52.68817F));
            tlpPort.RowStyles.Add(new RowStyle(SizeType.Percent, 47.31183F));
            tlpPort.Size = new Size(481, 94);
            tlpPort.TabIndex = 6;
            // 
            // btnPrinter
            // 
            btnPrinter.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPrinter.Location = new Point(366, 52);
            btnPrinter.Name = "btnPrinter";
            btnPrinter.Size = new Size(111, 38);
            btnPrinter.TabIndex = 6;
            btnPrinter.Text = "확인";
            btnPrinter.UseVisualStyleBackColor = true;
            // 
            // lblComBoard
            // 
            lblComBoard.Dock = DockStyle.Fill;
            lblComBoard.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblComBoard.Location = new Point(1, 1);
            lblComBoard.Margin = new Padding(0);
            lblComBoard.Name = "lblComBoard";
            lblComBoard.Size = new Size(181, 47);
            lblComBoard.TabIndex = 0;
            lblComBoard.Text = "COM Board";
            lblComBoard.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblPrinter
            // 
            lblPrinter.Dock = DockStyle.Fill;
            lblPrinter.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPrinter.Location = new Point(1, 49);
            lblPrinter.Margin = new Padding(0);
            lblPrinter.Name = "lblPrinter";
            lblPrinter.Size = new Size(181, 44);
            lblPrinter.TabIndex = 2;
            lblPrinter.Text = "Printer Setting";
            lblPrinter.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cmbComBoard
            // 
            cmbComBoard.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cmbComBoard.FormattingEnabled = true;
            cmbComBoard.Location = new Point(186, 4);
            cmbComBoard.Name = "cmbComBoard";
            cmbComBoard.Size = new Size(173, 38);
            cmbComBoard.TabIndex = 3;
            // 
            // cmbPrinter
            // 
            cmbPrinter.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cmbPrinter.FormattingEnabled = true;
            cmbPrinter.Location = new Point(186, 52);
            cmbPrinter.Name = "cmbPrinter";
            cmbPrinter.Size = new Size(173, 38);
            cmbPrinter.TabIndex = 4;
            // 
            // btnComBoard
            // 
            btnComBoard.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnComBoard.Location = new Point(366, 4);
            btnComBoard.Name = "btnComBoard";
            btnComBoard.Size = new Size(111, 39);
            btnComBoard.TabIndex = 5;
            btnComBoard.Text = "확인";
            btnComBoard.UseVisualStyleBackColor = true;
            // 
            // FormPortSetting
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(508, 198);
            Controls.Add(pnlPortsetting);
            Name = "FormPortSetting";
            Text = "FormPortSetting";
            pnlPortsetting.ResumeLayout(false);
            pnlPortSettingHeader.ResumeLayout(false);
            pnlPortSettingHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ImgInfac).EndInit();
            tlpPort.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlPortsetting;
        private Panel pnlPortSettingHeader;
        private TableLayoutPanel tlpSetting;
        private Button btnPathSetting;
        private Button btnComSetting;
        private Label lblPortSettingHeader;
        private PictureBox ImgInfac;
        private TableLayoutPanel tlpPort;
        private Label lblComBoard;
        private Label lblPrinter;
        private Button btnPrinter;
        private ComboBox cmbComBoard;
        private ComboBox cmbPrinter;
        private Button btnComBoard;
    }
}