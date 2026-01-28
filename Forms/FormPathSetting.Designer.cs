namespace PottingLabelPrinter.Forms
{
    partial class FormPathSetting
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
                components.Dispose();

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPathSetting));
            pnlPathSettingBacrground = new Panel();
            tlpPath = new TableLayoutPanel();
            btnPathSave = new Button();
            lblPath = new Label();
            txtPath = new TextBox();
            pnlPathSettingHeader = new Panel();
            lblPathsettingHeader = new Label();
            ImgInfac = new PictureBox();
            pnlPathSettingBacrground.SuspendLayout();
            tlpPath.SuspendLayout();
            pnlPathSettingHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ImgInfac).BeginInit();
            SuspendLayout();
            // 
            // pnlPathSettingBacrground
            // 
            pnlPathSettingBacrground.BackColor = Color.White;
            pnlPathSettingBacrground.Controls.Add(tlpPath);
            pnlPathSettingBacrground.Controls.Add(pnlPathSettingHeader);
            pnlPathSettingBacrground.Dock = DockStyle.Fill;
            pnlPathSettingBacrground.Location = new Point(0, 0);
            pnlPathSettingBacrground.Name = "pnlPathSettingBacrground";
            pnlPathSettingBacrground.Padding = new Padding(10);
            pnlPathSettingBacrground.Size = new Size(825, 161);
            pnlPathSettingBacrground.TabIndex = 0;
            // 
            // tlpPath
            // 
            tlpPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tlpPath.BackColor = Color.White;
            tlpPath.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tlpPath.ColumnCount = 3;
            tlpPath.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
            tlpPath.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpPath.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 124F));
            tlpPath.Controls.Add(btnPathSave, 2, 0);
            tlpPath.Controls.Add(lblPath, 0, 0);
            tlpPath.Controls.Add(txtPath, 1, 0);
            tlpPath.Location = new Point(10, 85);
            tlpPath.Name = "tlpPath";
            tlpPath.RowCount = 1;
            tlpPath.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPath.Size = new Size(805, 49);
            tlpPath.TabIndex = 1;
            // 
            // btnPathSave
            // 
            btnPathSave.BackColor = SystemColors.Control;
            btnPathSave.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPathSave.Location = new Point(683, 4);
            btnPathSave.Name = "btnPathSave";
            btnPathSave.Size = new Size(118, 41);
            btnPathSave.TabIndex = 2;
            btnPathSave.Text = "Save";
            btnPathSave.UseVisualStyleBackColor = false;
            // 
            // lblPath
            // 
            lblPath.BackColor = SystemColors.Control;
            lblPath.Dock = DockStyle.Fill;
            lblPath.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPath.Location = new Point(1, 1);
            lblPath.Margin = new Padding(0);
            lblPath.Name = "lblPath";
            lblPath.Size = new Size(160, 47);
            lblPath.TabIndex = 0;
            lblPath.Text = "Path";
            lblPath.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtPath
            // 
            txtPath.Anchor = AnchorStyles.None;
            txtPath.BackColor = Color.White;
            txtPath.BorderStyle = BorderStyle.None;
            txtPath.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPath.Location = new Point(162, 10);
            txtPath.Margin = new Padding(0);
            txtPath.Name = "txtPath";
            txtPath.ReadOnly = false;
            txtPath.Size = new Size(517, 28);
            txtPath.TabIndex = 1;
            // 
            // pnlPathSettingHeader
            // 
            pnlPathSettingHeader.BackColor = SystemColors.Control;
            pnlPathSettingHeader.Controls.Add(lblPathsettingHeader);
            pnlPathSettingHeader.Controls.Add(ImgInfac);
            pnlPathSettingHeader.Dock = DockStyle.Top;
            pnlPathSettingHeader.Location = new Point(10, 10);
            pnlPathSettingHeader.Name = "pnlPathSettingHeader";
            pnlPathSettingHeader.Size = new Size(805, 65);
            pnlPathSettingHeader.TabIndex = 0;
            // 
            // lblPathsettingHeader
            // 
            lblPathsettingHeader.AutoSize = true;
            lblPathsettingHeader.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPathsettingHeader.ForeColor = SystemColors.ControlDarkDark;
            lblPathsettingHeader.Location = new Point(190, 10);
            lblPathsettingHeader.Name = "lblPathsettingHeader";
            lblPathsettingHeader.Size = new Size(281, 45);
            lblPathsettingHeader.TabIndex = 1;
            lblPathsettingHeader.Text = "Save Path Setting";
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
            // FormPathSetting
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(825, 161);
            Controls.Add(pnlPathSettingBacrground);
            MinimumSize = new Size(700, 200);
            Name = "FormPathSetting";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Path Setting";
            pnlPathSettingBacrground.ResumeLayout(false);
            tlpPath.ResumeLayout(false);
            tlpPath.PerformLayout();
            pnlPathSettingHeader.ResumeLayout(false);
            pnlPathSettingHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ImgInfac).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlPathSettingBacrground;
        private Panel pnlPathSettingHeader;
        private Label lblPathsettingHeader;
        private PictureBox ImgInfac;
        private TableLayoutPanel tlpPath;
        private Label lblPath;
        private TextBox txtPath;
        private Button btnPathSave;
    }
}
