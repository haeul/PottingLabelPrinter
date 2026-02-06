namespace PottingLabelPrinter
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            pnlHeader = new Panel();
            tlpSetting = new TableLayoutPanel();
            btnPrintSetting = new Button();
            btnPathSetting = new Button();
            btnComSetting = new Button();
            lblHeader = new Label();
            ImgInfac = new PictureBox();
            lblTrayBarcodePrint = new Label();
            txtTrayBarcode = new TextBox();
            panel3 = new Panel();
            tlpCount = new TableLayoutPanel();
            lblErrorRatePer = new Label();
            lblErrorCount = new Label();
            lblOkCount = new Label();
            lblTotalCount = new Label();
            lblErrorRate = new Label();
            lblError = new Label();
            lblOK = new Label();
            lblTotal = new Label();
            tlpProduct = new TableLayoutPanel();
            lblProductNum = new Label();
            txtProductNum = new TextBox();
            lblProductName = new Label();
            txtProductName = new TextBox();
            lblType = new Label();
            txtType = new TextBox();
            lblCarModel = new Label();
            txtCarModel = new TextBox();
            lblALC = new Label();
            txtALC = new TextBox();
            lblBarcode = new Label();
            txtBarcode = new TextBox();
            pnlBacrground = new Panel();
            tlpbutton = new TableLayoutPanel();
            btnReset = new Button();
            btnPrint = new Button();
            dataGridView1 = new DataGridView();
            CheckBodx = new DataGridViewCheckBoxColumn();
            colDateTime = new DataGridViewTextBoxColumn();
            colTrayBarcode = new DataGridViewTextBoxColumn();
            colResult = new DataGridViewTextBoxColumn();
            tlpCommStatus = new TableLayoutPanel();
            lblPortConnectionStatus = new Label();
            lblDoneSignalStatus = new Label();
            pnlHeader.SuspendLayout();
            tlpSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ImgInfac).BeginInit();
            panel3.SuspendLayout();
            tlpCount.SuspendLayout();
            tlpProduct.SuspendLayout();
            pnlBacrground.SuspendLayout();
            tlpbutton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            tlpCommStatus.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = SystemColors.Control;
            pnlHeader.Controls.Add(tlpSetting);
            pnlHeader.Controls.Add(lblHeader);
            pnlHeader.Controls.Add(ImgInfac);
            pnlHeader.Location = new Point(10, 10);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(952, 65);
            pnlHeader.TabIndex = 0;
            pnlHeader.Paint += Panel_LightBorder_Paint;
            // 
            // tlpSetting
            // 
            tlpSetting.BackColor = Color.White;
            tlpSetting.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tlpSetting.ColumnCount = 3;
            tlpSetting.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tlpSetting.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tlpSetting.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tlpSetting.Controls.Add(btnPrintSetting, 2, 0);
            tlpSetting.Controls.Add(btnPathSetting, 1, 0);
            tlpSetting.Controls.Add(btnComSetting, 0, 0);
            tlpSetting.Location = new Point(550, 3);
            tlpSetting.Name = "tlpSetting";
            tlpSetting.RowCount = 1;
            tlpSetting.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpSetting.Size = new Size(399, 59);
            tlpSetting.TabIndex = 4;
            // 
            // btnPrintSetting
            // 
            btnPrintSetting.BackColor = SystemColors.Control;
            btnPrintSetting.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPrintSetting.ForeColor = SystemColors.ControlText;
            btnPrintSetting.Location = new Point(268, 4);
            btnPrintSetting.Name = "btnPrintSetting";
            btnPrintSetting.Size = new Size(127, 51);
            btnPrintSetting.TabIndex = 4;
            btnPrintSetting.Text = "Print Setting";
            btnPrintSetting.UseVisualStyleBackColor = false;
            // 
            // btnPathSetting
            // 
            btnPathSetting.BackColor = SystemColors.Control;
            btnPathSetting.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPathSetting.ForeColor = SystemColors.ControlText;
            btnPathSetting.Location = new Point(136, 4);
            btnPathSetting.Name = "btnPathSetting";
            btnPathSetting.Size = new Size(125, 51);
            btnPathSetting.TabIndex = 2;
            btnPathSetting.Text = "Path Setting";
            btnPathSetting.UseVisualStyleBackColor = false;
            // 
            // btnComSetting
            // 
            btnComSetting.BackColor = SystemColors.Control;
            btnComSetting.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnComSetting.ForeColor = SystemColors.ControlText;
            btnComSetting.Location = new Point(4, 4);
            btnComSetting.Name = "btnComSetting";
            btnComSetting.Size = new Size(125, 51);
            btnComSetting.TabIndex = 3;
            btnComSetting.Text = "Port Setting";
            btnComSetting.UseVisualStyleBackColor = false;
            // 
            // lblHeader
            // 
            lblHeader.AutoSize = true;
            lblHeader.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblHeader.ForeColor = SystemColors.ControlDarkDark;
            lblHeader.Location = new Point(190, 10);
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new Size(311, 45);
            lblHeader.TabIndex = 1;
            lblHeader.Text = "DHS TOUCH 검사기";
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
            // lblTrayBarcodePrint
            // 
            lblTrayBarcodePrint.BackColor = Color.LightGray;
            lblTrayBarcodePrint.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTrayBarcodePrint.Location = new Point(1, 1);
            lblTrayBarcodePrint.Name = "lblTrayBarcodePrint";
            lblTrayBarcodePrint.Size = new Size(305, 55);
            lblTrayBarcodePrint.TabIndex = 0;
            lblTrayBarcodePrint.Text = "Tray Barcode Print";
            lblTrayBarcodePrint.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtTrayBarcode
            // 
            txtTrayBarcode.BorderStyle = BorderStyle.None;
            txtTrayBarcode.Font = new Font("Segoe UI", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtTrayBarcode.Location = new Point(305, 2);
            txtTrayBarcode.Name = "txtTrayBarcode";
            txtTrayBarcode.Size = new Size(646, 50);
            txtTrayBarcode.TabIndex = 3;
            txtTrayBarcode.Text = "TRAY0001 2026-01-23 17:00:00";
            txtTrayBarcode.TextAlign = HorizontalAlignment.Center;
            // 
            // panel3
            // 
            panel3.Controls.Add(lblTrayBarcodePrint);
            panel3.Controls.Add(txtTrayBarcode);
            panel3.Location = new Point(10, 273);
            panel3.Name = "panel3";
            panel3.Size = new Size(952, 57);
            panel3.TabIndex = 4;
            panel3.Paint += Panel_LightBorder_Paint;
            // 
            // tlpCount
            // 
            tlpCount.BackColor = SystemColors.Control;
            tlpCount.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tlpCount.ColumnCount = 4;
            tlpCount.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpCount.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpCount.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpCount.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpCount.Controls.Add(lblErrorRatePer, 3, 1);
            tlpCount.Controls.Add(lblErrorCount, 2, 1);
            tlpCount.Controls.Add(lblOkCount, 1, 1);
            tlpCount.Controls.Add(lblTotalCount, 0, 1);
            tlpCount.Controls.Add(lblErrorRate, 3, 0);
            tlpCount.Controls.Add(lblError, 2, 0);
            tlpCount.Controls.Add(lblOK, 1, 0);
            tlpCount.Controls.Add(lblTotal, 0, 0);
            tlpCount.Location = new Point(492, 85);
            tlpCount.Name = "tlpCount";
            tlpCount.RowCount = 2;
            tlpCount.RowStyles.Add(new RowStyle(SizeType.Percent, 36.8686867F));
            tlpCount.RowStyles.Add(new RowStyle(SizeType.Percent, 63.1313133F));
            tlpCount.Size = new Size(470, 116);
            tlpCount.TabIndex = 5;
            // 
            // lblErrorRatePer
            // 
            lblErrorRatePer.Dock = DockStyle.Fill;
            lblErrorRatePer.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblErrorRatePer.ForeColor = Color.DarkOrange;
            lblErrorRatePer.Location = new Point(352, 43);
            lblErrorRatePer.Margin = new Padding(0);
            lblErrorRatePer.Name = "lblErrorRatePer";
            lblErrorRatePer.Size = new Size(117, 72);
            lblErrorRatePer.TabIndex = 0;
            lblErrorRatePer.Text = " 0.00 %";
            lblErrorRatePer.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblErrorCount
            // 
            lblErrorCount.Dock = DockStyle.Fill;
            lblErrorCount.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblErrorCount.ForeColor = Color.OrangeRed;
            lblErrorCount.Location = new Point(235, 43);
            lblErrorCount.Margin = new Padding(0);
            lblErrorCount.Name = "lblErrorCount";
            lblErrorCount.Size = new Size(116, 72);
            lblErrorCount.TabIndex = 1;
            lblErrorCount.Text = "0";
            lblErrorCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblOkCount
            // 
            lblOkCount.Dock = DockStyle.Fill;
            lblOkCount.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblOkCount.ForeColor = Color.LimeGreen;
            lblOkCount.Location = new Point(118, 43);
            lblOkCount.Margin = new Padding(0);
            lblOkCount.Name = "lblOkCount";
            lblOkCount.Size = new Size(116, 72);
            lblOkCount.TabIndex = 2;
            lblOkCount.Text = "0";
            lblOkCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTotalCount
            // 
            lblTotalCount.Dock = DockStyle.Fill;
            lblTotalCount.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalCount.ForeColor = Color.DodgerBlue;
            lblTotalCount.Location = new Point(1, 43);
            lblTotalCount.Margin = new Padding(0);
            lblTotalCount.Name = "lblTotalCount";
            lblTotalCount.Size = new Size(116, 72);
            lblTotalCount.TabIndex = 3;
            lblTotalCount.Text = "0";
            lblTotalCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblErrorRate
            // 
            lblErrorRate.Dock = DockStyle.Fill;
            lblErrorRate.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblErrorRate.Location = new Point(352, 1);
            lblErrorRate.Margin = new Padding(0);
            lblErrorRate.Name = "lblErrorRate";
            lblErrorRate.Size = new Size(117, 41);
            lblErrorRate.TabIndex = 4;
            lblErrorRate.Text = "불량률";
            lblErrorRate.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblError
            // 
            lblError.Dock = DockStyle.Fill;
            lblError.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblError.Location = new Point(235, 1);
            lblError.Margin = new Padding(0);
            lblError.Name = "lblError";
            lblError.Size = new Size(116, 41);
            lblError.TabIndex = 5;
            lblError.Text = "불량품";
            lblError.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblOK
            // 
            lblOK.Dock = DockStyle.Fill;
            lblOK.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblOK.Location = new Point(118, 1);
            lblOK.Margin = new Padding(0);
            lblOK.Name = "lblOK";
            lblOK.Size = new Size(116, 41);
            lblOK.TabIndex = 6;
            lblOK.Text = "양품";
            lblOK.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTotal
            // 
            lblTotal.Dock = DockStyle.Fill;
            lblTotal.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotal.Location = new Point(1, 1);
            lblTotal.Margin = new Padding(0);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(116, 41);
            lblTotal.TabIndex = 7;
            lblTotal.Text = "합계";
            lblTotal.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tlpProduct
            // 
            tlpProduct.BackColor = SystemColors.Control;
            tlpProduct.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tlpProduct.ColumnCount = 2;
            tlpProduct.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.90929F));
            tlpProduct.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66.09071F));
            tlpProduct.Controls.Add(lblProductNum, 0, 0);
            tlpProduct.Controls.Add(txtProductNum, 1, 0);
            tlpProduct.Controls.Add(lblProductName, 0, 1);
            tlpProduct.Controls.Add(txtProductName, 1, 1);
            tlpProduct.Controls.Add(lblType, 0, 2);
            tlpProduct.Controls.Add(txtType, 1, 2);
            tlpProduct.Controls.Add(lblCarModel, 0, 3);
            tlpProduct.Controls.Add(txtCarModel, 1, 3);
            tlpProduct.Controls.Add(lblALC, 0, 4);
            tlpProduct.Controls.Add(txtALC, 1, 4);
            tlpProduct.Controls.Add(lblBarcode, 0, 5);
            tlpProduct.Controls.Add(txtBarcode, 1, 5);
            tlpProduct.Location = new Point(10, 85);
            tlpProduct.Name = "tlpProduct";
            tlpProduct.RowCount = 6;
            tlpProduct.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tlpProduct.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tlpProduct.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tlpProduct.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tlpProduct.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tlpProduct.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tlpProduct.Size = new Size(470, 175);
            tlpProduct.TabIndex = 6;
            // 
            // lblProductNum
            // 
            lblProductNum.Dock = DockStyle.Fill;
            lblProductNum.Font = new Font("Segoe UI Semibold", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblProductNum.Location = new Point(1, 1);
            lblProductNum.Margin = new Padding(0);
            lblProductNum.Name = "lblProductNum";
            lblProductNum.Size = new Size(158, 28);
            lblProductNum.TabIndex = 0;
            lblProductNum.Text = "품번";
            lblProductNum.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtProductNum
            // 
            txtProductNum.BorderStyle = BorderStyle.None;
            txtProductNum.Dock = DockStyle.Fill;
            txtProductNum.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtProductNum.Location = new Point(160, 1);
            txtProductNum.Margin = new Padding(0);
            txtProductNum.Name = "txtProductNum";
            txtProductNum.Size = new Size(309, 26);
            txtProductNum.TabIndex = 1;
            txtProductNum.Text = "TRAY-PRINT";
            txtProductNum.TextAlign = HorizontalAlignment.Center;
            // 
            // lblProductName
            // 
            lblProductName.Dock = DockStyle.Fill;
            lblProductName.Font = new Font("Segoe UI Semibold", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblProductName.Location = new Point(1, 30);
            lblProductName.Margin = new Padding(0);
            lblProductName.Name = "lblProductName";
            lblProductName.Size = new Size(158, 28);
            lblProductName.TabIndex = 2;
            lblProductName.Text = "품명";
            lblProductName.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtProductName
            // 
            txtProductName.BorderStyle = BorderStyle.None;
            txtProductName.Dock = DockStyle.Fill;
            txtProductName.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtProductName.Location = new Point(160, 30);
            txtProductName.Margin = new Padding(0);
            txtProductName.Name = "txtProductName";
            txtProductName.Size = new Size(309, 26);
            txtProductName.TabIndex = 3;
            txtProductName.Text = "DHS Potting Tray No.";
            txtProductName.TextAlign = HorizontalAlignment.Center;
            // 
            // lblType
            // 
            lblType.Dock = DockStyle.Fill;
            lblType.Font = new Font("Segoe UI Semibold", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblType.Location = new Point(1, 59);
            lblType.Margin = new Padding(0);
            lblType.Name = "lblType";
            lblType.Size = new Size(158, 28);
            lblType.TabIndex = 4;
            lblType.Text = "Type(No)";
            lblType.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtType
            // 
            txtType.BorderStyle = BorderStyle.None;
            txtType.Dock = DockStyle.Fill;
            txtType.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtType.Location = new Point(160, 59);
            txtType.Margin = new Padding(0);
            txtType.Name = "txtType";
            txtType.Size = new Size(309, 26);
            txtType.TabIndex = 5;
            txtType.Text = "10";
            txtType.TextAlign = HorizontalAlignment.Center;
            // 
            // lblCarModel
            // 
            lblCarModel.Dock = DockStyle.Fill;
            lblCarModel.Font = new Font("Segoe UI Semibold", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCarModel.Location = new Point(1, 88);
            lblCarModel.Margin = new Padding(0);
            lblCarModel.Name = "lblCarModel";
            lblCarModel.Size = new Size(158, 28);
            lblCarModel.TabIndex = 6;
            lblCarModel.Text = "차종";
            lblCarModel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtCarModel
            // 
            txtCarModel.BorderStyle = BorderStyle.None;
            txtCarModel.Dock = DockStyle.Fill;
            txtCarModel.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtCarModel.Location = new Point(160, 88);
            txtCarModel.Margin = new Padding(0);
            txtCarModel.Name = "txtCarModel";
            txtCarModel.Size = new Size(309, 26);
            txtCarModel.TabIndex = 7;
            txtCarModel.Text = "COMMON";
            txtCarModel.TextAlign = HorizontalAlignment.Center;
            // 
            // lblALC
            // 
            lblALC.Dock = DockStyle.Fill;
            lblALC.Font = new Font("Segoe UI Semibold", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblALC.Location = new Point(1, 117);
            lblALC.Margin = new Padding(0);
            lblALC.Name = "lblALC";
            lblALC.Size = new Size(158, 28);
            lblALC.TabIndex = 8;
            lblALC.Text = "ALC No.";
            lblALC.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtALC
            // 
            txtALC.BorderStyle = BorderStyle.None;
            txtALC.Dock = DockStyle.Fill;
            txtALC.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtALC.Location = new Point(160, 117);
            txtALC.Margin = new Padding(0);
            txtALC.Name = "txtALC";
            txtALC.Size = new Size(309, 26);
            txtALC.TabIndex = 9;
            txtALC.Text = "NFC TOUCH";
            txtALC.TextAlign = HorizontalAlignment.Center;
            // 
            // lblBarcode
            // 
            lblBarcode.Dock = DockStyle.Fill;
            lblBarcode.Font = new Font("Segoe UI Semibold", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBarcode.Location = new Point(1, 146);
            lblBarcode.Margin = new Padding(0);
            lblBarcode.Name = "lblBarcode";
            lblBarcode.Size = new Size(158, 28);
            lblBarcode.TabIndex = 10;
            lblBarcode.Text = "JIG Barcode";
            lblBarcode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtBarcode
            // 
            txtBarcode.BorderStyle = BorderStyle.None;
            txtBarcode.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtBarcode.Location = new Point(160, 146);
            txtBarcode.Margin = new Padding(0);
            txtBarcode.Name = "txtBarcode";
            txtBarcode.Size = new Size(309, 26);
            txtBarcode.TabIndex = 11;
            txtBarcode.TextAlign = HorizontalAlignment.Center;
            // 
            // pnlBacrground
            // 
            pnlBacrground.BackColor = Color.White;
            pnlBacrground.Controls.Add(tlpbutton);
            pnlBacrground.Controls.Add(pnlHeader);
            pnlBacrground.Controls.Add(tlpProduct);
            pnlBacrground.Controls.Add(tlpCount);
            pnlBacrground.Controls.Add(panel3);
            pnlBacrground.Controls.Add(dataGridView1);
            pnlBacrground.Controls.Add(tlpCommStatus);
            pnlBacrground.Location = new Point(6, 6);
            pnlBacrground.Name = "pnlBacrground";
            pnlBacrground.Size = new Size(972, 952);
            pnlBacrground.TabIndex = 7;
            pnlBacrground.Paint += Panel_LightBorder_Paint;
            // 
            // tlpbutton
            // 
            tlpbutton.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tlpbutton.ColumnCount = 2;
            tlpbutton.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpbutton.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpbutton.Controls.Add(btnReset, 1, 0);
            tlpbutton.Controls.Add(btnPrint, 0, 0);
            tlpbutton.Location = new Point(492, 207);
            tlpbutton.Name = "tlpbutton";
            tlpbutton.RowCount = 1;
            tlpbutton.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpbutton.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpbutton.Size = new Size(470, 53);
            tlpbutton.TabIndex = 7;
            // 
            // btnReset
            // 
            btnReset.BackColor = SystemColors.Control;
            btnReset.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnReset.Location = new Point(238, 4);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(228, 45);
            btnReset.TabIndex = 1;
            btnReset.Text = "Reset";
            btnReset.UseVisualStyleBackColor = false;
            // 
            // btnPrint
            // 
            btnPrint.BackColor = SystemColors.Control;
            btnPrint.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPrint.Location = new Point(4, 4);
            btnPrint.Name = "btnPrint";
            btnPrint.Size = new Size(227, 45);
            btnPrint.TabIndex = 0;
            btnPrint.Text = "Print";
            btnPrint.UseVisualStyleBackColor = false;
            // 
            // dataGridView1
            // 
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.ColumnHeadersHeight = 40;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { CheckBodx, colDateTime, colTrayBarcode, colResult });
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = SystemColors.Window;
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle5.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            dataGridView1.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.Location = new Point(10, 343);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.Size = new Size(952, 560);
            dataGridView1.TabIndex = 0;
            // 
            // CheckBodx
            // 
            CheckBodx.FillWeight = 5F;
            CheckBodx.HeaderText = "";
            CheckBodx.MinimumWidth = 8;
            CheckBodx.Name = "CheckBodx";
            // 
            // colDateTime
            // 
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 129);
            colDateTime.DefaultCellStyle = dataGridViewCellStyle2;
            colDateTime.FillWeight = 40F;
            colDateTime.HeaderText = "Date Time";
            colDateTime.MinimumWidth = 8;
            colDateTime.Name = "colDateTime";
            // 
            // colTrayBarcode
            // 
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 129);
            colTrayBarcode.DefaultCellStyle = dataGridViewCellStyle3;
            colTrayBarcode.FillWeight = 45F;
            colTrayBarcode.HeaderText = "Tray Barcode";
            colTrayBarcode.MinimumWidth = 8;
            colTrayBarcode.Name = "colTrayBarcode";
            // 
            // colResult
            // 
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 129);
            colResult.DefaultCellStyle = dataGridViewCellStyle4;
            colResult.FillWeight = 10F;
            colResult.HeaderText = "Result";
            colResult.MinimumWidth = 8;
            colResult.Name = "colResult";
            // 
            // tlpCommStatus
            // 
            tlpCommStatus.BackColor = SystemColors.Control;
            tlpCommStatus.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tlpCommStatus.ColumnCount = 2;
            tlpCommStatus.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpCommStatus.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpCommStatus.Controls.Add(lblPortConnectionStatus, 0, 0);
            tlpCommStatus.Controls.Add(lblDoneSignalStatus, 1, 0);
            tlpCommStatus.Location = new Point(10, 909);
            tlpCommStatus.Name = "tlpCommStatus";
            tlpCommStatus.RowCount = 1;
            tlpCommStatus.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpCommStatus.Size = new Size(952, 36);
            tlpCommStatus.TabIndex = 8;
            // 
            // lblPortConnectionStatus
            // 
            lblPortConnectionStatus.Dock = DockStyle.Fill;
            lblPortConnectionStatus.Font = new Font("Segoe UI Semibold", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPortConnectionStatus.ForeColor = Color.OrangeRed;
            lblPortConnectionStatus.Location = new Point(1, 1);
            lblPortConnectionStatus.Margin = new Padding(0);
            lblPortConnectionStatus.Name = "lblPortConnectionStatus";
            lblPortConnectionStatus.Size = new Size(474, 34);
            lblPortConnectionStatus.TabIndex = 0;
            lblPortConnectionStatus.Text = "미설정 포트 연결";
            lblPortConnectionStatus.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblDoneSignalStatus
            // 
            lblDoneSignalStatus.Dock = DockStyle.Fill;
            lblDoneSignalStatus.Font = new Font("Segoe UI Semibold", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDoneSignalStatus.ForeColor = Color.Gray;
            lblDoneSignalStatus.Location = new Point(476, 1);
            lblDoneSignalStatus.Margin = new Padding(0);
            lblDoneSignalStatus.Name = "lblDoneSignalStatus";
            lblDoneSignalStatus.Size = new Size(475, 34);
            lblDoneSignalStatus.TabIndex = 1;
            lblDoneSignalStatus.Text = "토출 완료 신호 수신";
            lblDoneSignalStatus.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 964);
            Controls.Add(pnlBacrground);
            Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "FormMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Potting Label Printer";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            tlpSetting.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)ImgInfac).EndInit();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            tlpCount.ResumeLayout(false);
            tlpProduct.ResumeLayout(false);
            tlpProduct.PerformLayout();
            pnlBacrground.ResumeLayout(false);
            tlpbutton.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            tlpCommStatus.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private PictureBox ImgInfac;
        private Label lblHeader;
        private Button btnPathSetting;
        private Label lblTrayBarcodePrint;
        private TextBox txtTrayBarcode;
        private Panel panel3;
        private Button btnComSetting;
        private TableLayoutPanel tlpCount;
        private TableLayoutPanel tlpProduct;
        private Label lblProductNum;
        private TextBox txtBarcode;
        private TextBox txtALC;
        private TextBox txtCarModel;
        private TextBox txtType;
        private TextBox txtProductName;
        private Label lblProductName;
        private Label lblType;
        private Label lblCarModel;
        private Label lblALC;
        private Label lblBarcode;
        private TextBox txtProductNum;
        private Label lblErrorRatePer;
        private Label lblErrorCount;
        private Label lblOkCount;
        private Label lblTotalCount;
        private Label lblErrorRate;
        private Label lblError;
        private Label lblOK;
        private Label lblTotal;
        private Panel pnlBacrground;
        private DataGridView dataGridView1;
        private TableLayoutPanel tlpbutton;
        private Button btnReset;
        private Button btnPrint;
        private TableLayoutPanel tlpSetting;
        private DataGridViewCheckBoxColumn CheckBodx;
        private DataGridViewTextBoxColumn colDateTime;
        private DataGridViewTextBoxColumn colTrayBarcode;
        private DataGridViewTextBoxColumn colResult;
        private Button btnPrintSetting;

        // ✅ 상태바 구성
        private TableLayoutPanel tlpCommStatus;
        private Label lblPortConnectionStatus;
        private Label lblDoneSignalStatus;
    }
}
