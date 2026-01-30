namespace PottingLabelPrinter.Forms
{
    partial class FormPrintSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPrintSetting));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            pnlBacrground = new Panel();
            panel2 = new Panel();
            tableLayoutPanel2 = new TableLayoutPanel();
            btnPrint = new Button();
            btnReset = new Button();
            btnSave = new Button();
            pnlPreview = new Panel();
            lblPreview = new Label();
            panel1 = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            nudYoffset = new NumericUpDown();
            nudXoffset = new NumericUpDown();
            lblYoffset = new Label();
            lblXoffset = new Label();
            lblOffsetSetting = new Label();
            lblPrintSetting = new Label();
            tlpProduct = new TableLayoutPanel();
            nudLabelStartNum = new NumericUpDown();
            lblLabelStartNum = new Label();
            nudLabelSpace = new NumericUpDown();
            lblLabelSpace = new Label();
            nudPrintQuantity = new NumericUpDown();
            nudLabelHeight = new NumericUpDown();
            nudPrintSpeed = new NumericUpDown();
            nudPrintDarkness = new NumericUpDown();
            lblPrintQuantity = new Label();
            lblPrintSpeed = new Label();
            lblPrintDarkness = new Label();
            lblPrintDirection = new Label();
            lblLabelWidth = new Label();
            lblLabelHeight = new Label();
            cmbPrintDirection = new ComboBox();
            nudLabelWidth = new NumericUpDown();
            pnlHeader = new Panel();
            lblHeader = new Label();
            ImgInfac = new PictureBox();
            dataGridView2 = new DataGridView();
            순번 = new DataGridViewTextBoxColumn();
            X좌표 = new DataGridViewTextBoxColumn();
            Y좌표 = new DataGridViewTextBoxColumn();
            회전 = new DataGridViewTextBoxColumn();
            크기 = new DataGridViewTextBoxColumn();
            X비율 = new DataGridViewTextBoxColumn();
            Y비율 = new DataGridViewTextBoxColumn();
            데이터 = new DataGridViewTextBoxColumn();
            pnlBacrground.SuspendLayout();
            panel2.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudYoffset).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudXoffset).BeginInit();
            tlpProduct.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudLabelStartNum).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudLabelSpace).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudPrintQuantity).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudLabelHeight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudPrintSpeed).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudPrintDarkness).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudLabelWidth).BeginInit();
            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ImgInfac).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            SuspendLayout();
            // 
            // pnlBacrground
            // 
            pnlBacrground.BackColor = Color.White;
            pnlBacrground.Controls.Add(panel2);
            pnlBacrground.Controls.Add(panel1);
            pnlBacrground.Controls.Add(pnlHeader);
            pnlBacrground.Controls.Add(dataGridView2);
            pnlBacrground.Location = new Point(1, 1);
            pnlBacrground.Name = "pnlBacrground";
            pnlBacrground.Size = new Size(972, 522);
            pnlBacrground.TabIndex = 8;
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.Control;
            panel2.Controls.Add(tableLayoutPanel2);
            panel2.Controls.Add(pnlPreview);
            panel2.Controls.Add(lblPreview);
            panel2.Location = new Point(563, 86);
            panel2.Name = "panel2";
            panel2.Size = new Size(399, 232);
            panel2.TabIndex = 8;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.Controls.Add(btnPrint, 2, 0);
            tableLayoutPanel2.Controls.Add(btnReset, 1, 0);
            tableLayoutPanel2.Controls.Add(btnSave, 0, 0);
            tableLayoutPanel2.Location = new Point(0, 184);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(399, 43);
            tableLayoutPanel2.TabIndex = 2;
            // 
            // btnPrint
            // 
            btnPrint.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnPrint.Location = new Point(268, 4);
            btnPrint.Name = "btnPrint";
            btnPrint.Size = new Size(126, 35);
            btnPrint.TabIndex = 2;
            btnPrint.Text = "Print";
            btnPrint.UseVisualStyleBackColor = true;
            // 
            // btnReset
            // 
            btnReset.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnReset.Location = new Point(136, 4);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(125, 35);
            btnReset.TabIndex = 1;
            btnReset.Text = "Reset";
            btnReset.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            btnSave.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSave.Location = new Point(4, 4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(125, 35);
            btnSave.TabIndex = 0;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            // 
            // pnlPreview
            // 
            pnlPreview.BackColor = SystemColors.Control;
            pnlPreview.Location = new Point(3, 37);
            pnlPreview.Name = "pnlPreview";
            pnlPreview.Size = new Size(393, 143);
            pnlPreview.TabIndex = 1;
            // 
            // lblPreview
            // 
            lblPreview.BackColor = Color.LightGray;
            lblPreview.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPreview.Location = new Point(0, 0);
            lblPreview.Name = "lblPreview";
            lblPreview.Size = new Size(399, 35);
            lblPreview.TabIndex = 0;
            lblPreview.Text = "미리 보기";
            lblPreview.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            panel1.Controls.Add(tableLayoutPanel1);
            panel1.Controls.Add(lblOffsetSetting);
            panel1.Controls.Add(lblPrintSetting);
            panel1.Controls.Add(tlpProduct);
            panel1.Location = new Point(10, 86);
            panel1.Name = "panel1";
            panel1.Size = new Size(543, 237);
            panel1.TabIndex = 7;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = SystemColors.Control;
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel1.Controls.Add(nudYoffset, 3, 0);
            tableLayoutPanel1.Controls.Add(nudXoffset, 1, 0);
            tableLayoutPanel1.Controls.Add(lblYoffset, 2, 0);
            tableLayoutPanel1.Controls.Add(lblXoffset, 0, 0);
            tableLayoutPanel1.Location = new Point(4, 200);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(536, 32);
            tableLayoutPanel1.TabIndex = 9;
            // 
            // nudYoffset
            // 
            nudYoffset.Dock = DockStyle.Fill;
            nudYoffset.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            nudYoffset.Location = new Point(378, 4);
            nudYoffset.Name = "nudYoffset";
            nudYoffset.Size = new Size(154, 25);
            nudYoffset.TabIndex = 18;
            // 
            // nudXoffset
            // 
            nudXoffset.Dock = DockStyle.Fill;
            nudXoffset.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            nudXoffset.Location = new Point(111, 4);
            nudXoffset.Name = "nudXoffset";
            nudXoffset.Size = new Size(153, 25);
            nudXoffset.TabIndex = 16;
            // 
            // lblYoffset
            // 
            lblYoffset.Anchor = AnchorStyles.None;
            lblYoffset.BackColor = SystemColors.Control;
            lblYoffset.Font = new Font("Segoe UI Semibold", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblYoffset.Location = new Point(268, 1);
            lblYoffset.Margin = new Padding(0);
            lblYoffset.Name = "lblYoffset";
            lblYoffset.Size = new Size(106, 29);
            lblYoffset.TabIndex = 17;
            lblYoffset.Text = "Y 오프셋";
            lblYoffset.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblXoffset
            // 
            lblXoffset.Anchor = AnchorStyles.None;
            lblXoffset.BackColor = SystemColors.Control;
            lblXoffset.Font = new Font("Segoe UI Semibold", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblXoffset.Location = new Point(1, 1);
            lblXoffset.Margin = new Padding(0);
            lblXoffset.Name = "lblXoffset";
            lblXoffset.Size = new Size(106, 29);
            lblXoffset.TabIndex = 1;
            lblXoffset.Text = "X 오프셋";
            lblXoffset.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblOffsetSetting
            // 
            lblOffsetSetting.BackColor = Color.LightGray;
            lblOffsetSetting.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblOffsetSetting.Location = new Point(3, 161);
            lblOffsetSetting.Name = "lblOffsetSetting";
            lblOffsetSetting.Size = new Size(537, 35);
            lblOffsetSetting.TabIndex = 8;
            lblOffsetSetting.Text = "오프셋 설정";
            lblOffsetSetting.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblPrintSetting
            // 
            lblPrintSetting.BackColor = Color.LightGray;
            lblPrintSetting.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPrintSetting.Location = new Point(0, 0);
            lblPrintSetting.Name = "lblPrintSetting";
            lblPrintSetting.Size = new Size(540, 35);
            lblPrintSetting.TabIndex = 7;
            lblPrintSetting.Text = "인쇄 설정";
            lblPrintSetting.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tlpProduct
            // 
            tlpProduct.BackColor = SystemColors.Control;
            tlpProduct.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tlpProduct.ColumnCount = 4;
            tlpProduct.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpProduct.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tlpProduct.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpProduct.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tlpProduct.Controls.Add(nudLabelStartNum, 3, 3);
            tlpProduct.Controls.Add(lblLabelStartNum, 2, 3);
            tlpProduct.Controls.Add(nudLabelSpace, 1, 3);
            tlpProduct.Controls.Add(lblLabelSpace, 0, 3);
            tlpProduct.Controls.Add(nudPrintQuantity, 3, 2);
            tlpProduct.Controls.Add(nudLabelHeight, 1, 2);
            tlpProduct.Controls.Add(nudPrintSpeed, 3, 1);
            tlpProduct.Controls.Add(nudPrintDarkness, 3, 0);
            tlpProduct.Controls.Add(lblPrintQuantity, 2, 2);
            tlpProduct.Controls.Add(lblPrintSpeed, 2, 1);
            tlpProduct.Controls.Add(lblPrintDarkness, 2, 0);
            tlpProduct.Controls.Add(lblPrintDirection, 0, 0);
            tlpProduct.Controls.Add(lblLabelWidth, 0, 1);
            tlpProduct.Controls.Add(lblLabelHeight, 0, 2);
            tlpProduct.Controls.Add(cmbPrintDirection, 1, 0);
            tlpProduct.Controls.Add(nudLabelWidth, 1, 1);
            tlpProduct.Location = new Point(3, 37);
            tlpProduct.Name = "tlpProduct";
            tlpProduct.RowCount = 4;
            tlpProduct.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tlpProduct.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tlpProduct.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tlpProduct.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tlpProduct.Size = new Size(537, 121);
            tlpProduct.TabIndex = 6;
            // 
            // nudLabelStartNum
            // 
            nudLabelStartNum.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            nudLabelStartNum.Location = new Point(378, 94);
            nudLabelStartNum.Name = "nudLabelStartNum";
            nudLabelStartNum.Size = new Size(155, 25);
            nudLabelStartNum.TabIndex = 20;
            // 
            // lblLabelStartNum
            // 
            lblLabelStartNum.Dock = DockStyle.Fill;
            lblLabelStartNum.Font = new Font("Segoe UI Semibold", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblLabelStartNum.Location = new Point(268, 91);
            lblLabelStartNum.Margin = new Padding(0);
            lblLabelStartNum.Name = "lblLabelStartNum";
            lblLabelStartNum.Size = new Size(106, 29);
            lblLabelStartNum.TabIndex = 19;
            lblLabelStartNum.Text = "시작 번호";
            lblLabelStartNum.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // nudLabelSpace
            // 
            nudLabelSpace.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            nudLabelSpace.Location = new Point(111, 94);
            nudLabelSpace.Name = "nudLabelSpace";
            nudLabelSpace.Size = new Size(153, 25);
            nudLabelSpace.TabIndex = 18;
            // 
            // lblLabelSpace
            // 
            lblLabelSpace.Dock = DockStyle.Fill;
            lblLabelSpace.Font = new Font("Segoe UI Semibold", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblLabelSpace.Location = new Point(1, 91);
            lblLabelSpace.Margin = new Padding(0);
            lblLabelSpace.Name = "lblLabelSpace";
            lblLabelSpace.Size = new Size(106, 29);
            lblLabelSpace.TabIndex = 17;
            lblLabelSpace.Text = "라벨 간격";
            lblLabelSpace.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // nudPrintQuantity
            // 
            nudPrintQuantity.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            nudPrintQuantity.Location = new Point(378, 64);
            nudPrintQuantity.Name = "nudPrintQuantity";
            nudPrintQuantity.Size = new Size(155, 25);
            nudPrintQuantity.TabIndex = 16;
            // 
            // nudLabelHeight
            // 
            nudLabelHeight.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            nudLabelHeight.Location = new Point(111, 64);
            nudLabelHeight.Name = "nudLabelHeight";
            nudLabelHeight.Size = new Size(153, 25);
            nudLabelHeight.TabIndex = 15;
            // 
            // nudPrintSpeed
            // 
            nudPrintSpeed.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            nudPrintSpeed.Location = new Point(378, 34);
            nudPrintSpeed.Name = "nudPrintSpeed";
            nudPrintSpeed.Size = new Size(155, 25);
            nudPrintSpeed.TabIndex = 14;
            // 
            // nudPrintDarkness
            // 
            nudPrintDarkness.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            nudPrintDarkness.Location = new Point(378, 4);
            nudPrintDarkness.Name = "nudPrintDarkness";
            nudPrintDarkness.Size = new Size(155, 25);
            nudPrintDarkness.TabIndex = 13;
            // 
            // lblPrintQuantity
            // 
            lblPrintQuantity.Dock = DockStyle.Fill;
            lblPrintQuantity.Font = new Font("Segoe UI Semibold", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPrintQuantity.Location = new Point(268, 61);
            lblPrintQuantity.Margin = new Padding(0);
            lblPrintQuantity.Name = "lblPrintQuantity";
            lblPrintQuantity.Size = new Size(106, 29);
            lblPrintQuantity.TabIndex = 10;
            lblPrintQuantity.Text = "인쇄 수량";
            lblPrintQuantity.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblPrintSpeed
            // 
            lblPrintSpeed.Dock = DockStyle.Fill;
            lblPrintSpeed.Font = new Font("Segoe UI Semibold", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPrintSpeed.Location = new Point(268, 31);
            lblPrintSpeed.Margin = new Padding(0);
            lblPrintSpeed.Name = "lblPrintSpeed";
            lblPrintSpeed.Size = new Size(106, 29);
            lblPrintSpeed.TabIndex = 8;
            lblPrintSpeed.Text = "인쇄 속도";
            lblPrintSpeed.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblPrintDarkness
            // 
            lblPrintDarkness.Dock = DockStyle.Fill;
            lblPrintDarkness.Font = new Font("Segoe UI Semibold", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPrintDarkness.Location = new Point(268, 1);
            lblPrintDarkness.Margin = new Padding(0);
            lblPrintDarkness.Name = "lblPrintDarkness";
            lblPrintDarkness.Size = new Size(106, 29);
            lblPrintDarkness.TabIndex = 6;
            lblPrintDarkness.Text = "인쇄 농도";
            lblPrintDarkness.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblPrintDirection
            // 
            lblPrintDirection.Dock = DockStyle.Fill;
            lblPrintDirection.Font = new Font("Segoe UI Semibold", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPrintDirection.Location = new Point(1, 1);
            lblPrintDirection.Margin = new Padding(0);
            lblPrintDirection.Name = "lblPrintDirection";
            lblPrintDirection.Size = new Size(106, 29);
            lblPrintDirection.TabIndex = 0;
            lblPrintDirection.Text = "인쇄 방향";
            lblPrintDirection.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblLabelWidth
            // 
            lblLabelWidth.Dock = DockStyle.Fill;
            lblLabelWidth.Font = new Font("Segoe UI Semibold", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblLabelWidth.Location = new Point(1, 31);
            lblLabelWidth.Margin = new Padding(0);
            lblLabelWidth.Name = "lblLabelWidth";
            lblLabelWidth.Size = new Size(106, 29);
            lblLabelWidth.TabIndex = 2;
            lblLabelWidth.Text = "라벨 너비";
            lblLabelWidth.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblLabelHeight
            // 
            lblLabelHeight.Dock = DockStyle.Fill;
            lblLabelHeight.Font = new Font("Segoe UI Semibold", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblLabelHeight.Location = new Point(1, 61);
            lblLabelHeight.Margin = new Padding(0);
            lblLabelHeight.Name = "lblLabelHeight";
            lblLabelHeight.Size = new Size(106, 29);
            lblLabelHeight.TabIndex = 4;
            lblLabelHeight.Text = "라벨 높이";
            lblLabelHeight.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cmbPrintDirection
            // 
            cmbPrintDirection.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbPrintDirection.FormattingEnabled = true;
            cmbPrintDirection.Location = new Point(111, 4);
            cmbPrintDirection.Name = "cmbPrintDirection";
            cmbPrintDirection.Size = new Size(153, 25);
            cmbPrintDirection.TabIndex = 11;
            // 
            // nudLabelWidth
            // 
            nudLabelWidth.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            nudLabelWidth.Location = new Point(111, 34);
            nudLabelWidth.Name = "nudLabelWidth";
            nudLabelWidth.Size = new Size(153, 25);
            nudLabelWidth.TabIndex = 12;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = SystemColors.Control;
            pnlHeader.Controls.Add(lblHeader);
            pnlHeader.Controls.Add(ImgInfac);
            pnlHeader.Location = new Point(10, 10);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(952, 65);
            pnlHeader.TabIndex = 0;
            // 
            // lblHeader
            // 
            lblHeader.AutoSize = true;
            lblHeader.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblHeader.ForeColor = SystemColors.ControlDarkDark;
            lblHeader.Location = new Point(190, 10);
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new Size(209, 45);
            lblHeader.TabIndex = 1;
            lblHeader.Text = "Print Setting";
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
            // dataGridView2
            // 
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView2.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView2.ColumnHeadersHeight = 40;
            dataGridView2.Columns.AddRange(new DataGridViewColumn[] { 순번, X좌표, Y좌표, 회전, 크기, X비율, Y비율, 데이터 });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("맑은 고딕", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dataGridView2.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridView2.EnableHeadersVisualStyles = false;
            dataGridView2.Location = new Point(10, 336);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowHeadersWidth = 62;
            dataGridView2.Size = new Size(952, 176);
            dataGridView2.TabIndex = 0;
            // 
            // 순번
            // 
            순번.HeaderText = "순번";
            순번.Name = "순번";
            // 
            // X좌표
            // 
            X좌표.HeaderText = "X좌표";
            X좌표.Name = "X좌표";
            // 
            // Y좌표
            // 
            Y좌표.HeaderText = "Y좌표";
            Y좌표.Name = "Y좌표";
            // 
            // 회전
            // 
            회전.HeaderText = "회전";
            회전.Name = "회전";
            // 
            // 크기
            // 
            크기.HeaderText = "크기";
            크기.Name = "크기";
            // 
            // X비율
            // 
            X비율.HeaderText = "X비율";
            X비율.Name = "X비율";
            // 
            // Y비율
            // 
            Y비율.HeaderText = "Y비율";
            Y비율.Name = "Y비율";
            // 
            // 데이터
            // 
            데이터.HeaderText = "데이터";
            데이터.Name = "데이터";
            // 
            // FormPrintSetting
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(977, 525);
            Controls.Add(pnlBacrground);
            Margin = new Padding(2);
            Name = "FormPrintSetting";
            Text = "FormPrintSetting";
            pnlBacrground.ResumeLayout(false);
            panel2.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nudYoffset).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudXoffset).EndInit();
            tlpProduct.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nudLabelStartNum).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudLabelSpace).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudPrintQuantity).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudLabelHeight).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudPrintSpeed).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudPrintDarkness).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudLabelWidth).EndInit();
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ImgInfac).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlBacrground;
        private Panel pnlHeader;
        private Label lblHeader;
        private PictureBox ImgInfac;
        private TableLayoutPanel tlpProduct;
        private Label lblPrintDirection;
        private Label lblLabelWidth;
        private Label lblLabelHeight;
        private DataGridView dataGridView2;
        private DataGridViewTextBoxColumn 순번;
        private DataGridViewTextBoxColumn X좌표;
        private DataGridViewTextBoxColumn Y좌표;
        private DataGridViewTextBoxColumn 회전;
        private DataGridViewTextBoxColumn 크기;
        private DataGridViewTextBoxColumn X비율;
        private DataGridViewTextBoxColumn Y비율;
        private DataGridViewTextBoxColumn 데이터;
        private Label lblPrintQuantity;
        private Label lblPrintSpeed;
        private Label lblPrintDarkness;
        private Panel panel1;
        private Label lblPrintSetting;
        private NumericUpDown nudPrintQuantity;
        private NumericUpDown nudLabelHeight;
        private NumericUpDown nudPrintSpeed;
        private NumericUpDown nudPrintDarkness;
        private ComboBox cmbPrintDirection;
        private NumericUpDown nudLabelWidth;
        private TableLayoutPanel tableLayoutPanel1;
        private Label lblOffsetSetting;
        private NumericUpDown nudYoffset;
        private Label lblYoffset;
        private NumericUpDown nudXoffset;
        private Label lblXoffset;
        private Label lblLabelSpace;
        private Panel panel2;
        private Label lblPreview;
        private NumericUpDown nudLabelStartNum;
        private Label lblLabelStartNum;
        private NumericUpDown nudLabelSpace;
        private TableLayoutPanel tableLayoutPanel2;
        private Button btnPrint;
        private Button btnReset;
        private Button btnSave;
        private Panel pnlPreview;
    }
}