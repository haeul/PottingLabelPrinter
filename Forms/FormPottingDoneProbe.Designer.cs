namespace PottingLabelPrinter.Forms
{
    partial class FormDoneProbe
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblNow;
        private System.Windows.Forms.Label lblInterval;
        private System.Windows.Forms.Label lblDone;
        private System.Windows.Forms.Label lblRaw;
        private System.Windows.Forms.Label lblRiseAt;
        private System.Windows.Forms.Label lblFallAt;
        private System.Windows.Forms.Label lblDuration;

        private System.Windows.Forms.ListBox listLog;

        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnClose;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblNow = new Label();
            lblInterval = new Label();
            lblDone = new Label();
            lblRaw = new Label();
            lblRiseAt = new Label();
            lblFallAt = new Label();
            lblDuration = new Label();
            listLog = new ListBox();
            btnCopy = new Button();
            btnClear = new Button();
            btnClose = new Button();
            SuspendLayout();
            // 
            // lblNow
            // 
            lblNow.AutoSize = true;
            lblNow.Location = new Point(12, 12);
            lblNow.Name = "lblNow";
            lblNow.Size = new Size(67, 25);
            lblNow.TabIndex = 0;
            lblNow.Text = "Now: -";
            // 
            // lblInterval
            // 
            lblInterval.AutoSize = true;
            lblInterval.Location = new Point(12, 34);
            lblInterval.Name = "lblInterval";
            lblInterval.Size = new Size(118, 25);
            lblInterval.TabIndex = 1;
            lblInterval.Text = "Interval: - ms";
            // 
            // lblDone
            // 
            lblDone.AutoSize = true;
            lblDone.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDone.Location = new Point(12, 58);
            lblDone.Name = "lblDone";
            lblDone.Size = new Size(121, 28);
            lblDone.TabIndex = 2;
            lblDone.Text = "IN2 Done: -";
            // 
            // lblRaw
            // 
            lblRaw.AutoSize = true;
            lblRaw.Font = new Font("Consolas", 10F);
            lblRaw.Location = new Point(12, 82);
            lblRaw.Name = "lblRaw";
            lblRaw.Size = new Size(142, 23);
            lblRaw.TabIndex = 3;
            lblRaw.Text = "Raw(byte): -";
            // 
            // lblRiseAt
            // 
            lblRiseAt.AutoSize = true;
            lblRiseAt.Location = new Point(12, 108);
            lblRiseAt.Name = "lblRiseAt";
            lblRiseAt.Size = new Size(102, 25);
            lblRiseAt.TabIndex = 4;
            lblRiseAt.Text = "Last RISE: -";
            // 
            // lblFallAt
            // 
            lblFallAt.AutoSize = true;
            lblFallAt.Location = new Point(12, 130);
            lblFallAt.Name = "lblFallAt";
            lblFallAt.Size = new Size(106, 25);
            lblFallAt.TabIndex = 5;
            lblFallAt.Text = "Last FALL: -";
            // 
            // lblDuration
            // 
            lblDuration.AutoSize = true;
            lblDuration.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDuration.Location = new Point(12, 154);
            lblDuration.Name = "lblDuration";
            lblDuration.Size = new Size(200, 28);
            lblDuration.TabIndex = 6;
            lblDuration.Text = "Last Duration(ms): -";
            // 
            // listLog
            // 
            listLog.FormattingEnabled = true;
            listLog.HorizontalScrollbar = true;
            listLog.IntegralHeight = false;
            listLog.ItemHeight = 25;
            listLog.Location = new Point(12, 184);
            listLog.Name = "listLog";
            listLog.Size = new Size(736, 280);
            listLog.TabIndex = 7;
            // 
            // btnCopy
            // 
            btnCopy.Location = new Point(468, 476);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new Size(90, 30);
            btnCopy.TabIndex = 8;
            btnCopy.Text = "Copy";
            btnCopy.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(564, 476);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(90, 30);
            btnClear.TabIndex = 9;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(658, 476);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(90, 30);
            btnClose.TabIndex = 10;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // FormDoneProbe
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(760, 520);
            Controls.Add(lblNow);
            Controls.Add(lblInterval);
            Controls.Add(lblDone);
            Controls.Add(lblRaw);
            Controls.Add(lblRiseAt);
            Controls.Add(lblFallAt);
            Controls.Add(lblDuration);
            Controls.Add(listLog);
            Controls.Add(btnCopy);
            Controls.Add(btnClear);
            Controls.Add(btnClose);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "FormDoneProbe";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Done Probe";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
