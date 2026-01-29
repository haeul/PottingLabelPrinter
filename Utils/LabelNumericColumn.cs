using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace PottingLabelPrinter.Forms
{
    public class LabelNumericColumn : DataGridViewColumn
    {
        public int DecimalPlaces { get; set; } = 1;
        public decimal Increment { get; set; } = 0.1M;
        public decimal Minimum { get; set; } = 0M;
        public decimal Maximum { get; set; } = 100M;

        public LabelNumericColumn()
            : base(new DataGridViewNumericUpDownCell()) { }

        public override object Clone()
        {
            var clonedColumn = (LabelNumericColumn)base.Clone();
            clonedColumn.DecimalPlaces = DecimalPlaces;
            clonedColumn.Increment = Increment;
            clonedColumn.Minimum = Minimum;
            clonedColumn.Maximum = Maximum;
            return clonedColumn;
        }
    }

    public class DataGridViewNumericUpDownCell : DataGridViewTextBoxCell
    {
        public DataGridViewNumericUpDownCell()
        {
            Style.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        public override Type EditType => typeof(DataGridViewNumericUpDownEditingControl);
        public override Type ValueType => typeof(decimal);
        public override object DefaultNewRowValue => 0M;

        protected override object GetFormattedValue(
            object value,
            int rowIndex,
            ref DataGridViewCellStyle cellStyle,
            TypeConverter valueTypeConverter,
            TypeConverter formattedValueTypeConverter,
            DataGridViewDataErrorContexts context)
        {
            string formatString = (cellStyle?.Format) ?? "0.###";
            if (value is decimal decimalValue) return decimalValue.ToString(formatString);
            if (value is double doubleValue) return ((decimal)doubleValue).ToString(formatString);
            return base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
        }

        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle cellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, cellStyle);

            if (DataGridView.EditingControl is DataGridViewNumericUpDownEditingControl numericEditor &&
                OwningColumn is LabelNumericColumn col)
            {
                numericEditor.DecimalPlaces = col.DecimalPlaces;
                numericEditor.Increment = col.Increment;
                numericEditor.Minimum = col.Minimum;
                numericEditor.Maximum = col.Maximum;
            }

            decimal currentValue = 0M;
            var rawValue = Value ?? initialFormattedValue;
            if (rawValue is decimal decimalValue) currentValue = decimalValue;
            else if (rawValue is double doubleValue) currentValue = (decimal)doubleValue;
            else if (rawValue is string s && decimal.TryParse(s, out var parsed)) currentValue = parsed;

            if (DataGridView.EditingControl is DataGridViewNumericUpDownEditingControl editor)
            {
                if (currentValue < editor.Minimum) currentValue = editor.Minimum;
                if (currentValue > editor.Maximum) currentValue = editor.Maximum;
                editor.Value = currentValue;
            }
        }
    }

    public class DataGridViewNumericUpDownEditingControl : NumericUpDown, IDataGridViewEditingControl
    {
        public DataGridViewNumericUpDownEditingControl()
        {
            BorderStyle = BorderStyle.FixedSingle;
            ThousandsSeparator = false;
            ValueChanged += (_, __) =>
            {
                EditingControlValueChanged = true;
                EditingControlDataGridView?.NotifyCurrentCellDirty(true);
            };
        }

        public DataGridView EditingControlDataGridView { get; set; } = null!;
        public object EditingControlFormattedValue
        {
            get => Value.ToString();
            set
            {
                if (value is string s && decimal.TryParse(s, out var d))
                    Value = Clamp(d);
            }
        }
        public int EditingControlRowIndex { get; set; }
        public bool EditingControlValueChanged { get; set; }
        public Cursor EditingPanelCursor => Cursors.IBeam;
        public bool RepositionEditingControlOnValueChange => false;

        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle style)
        {
            Font = style.Font;
            ForeColor = style.ForeColor;
            BackColor = style.BackColor;
        }

        public void PrepareEditingControlForEdit(bool selectAll) { }
        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context) => Value.ToString();

        public bool EditingControlWantsInputKey(Keys keyData, bool gridWants)
        {
            switch (keyData & Keys.KeyCode)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Home:
                case Keys.End:
                case Keys.PageUp:
                case Keys.PageDown:
                    return true;
                default:
                    return !gridWants;
            }
        }

        private decimal Clamp(decimal value) => Math.Min(Math.Max(value, Minimum), Maximum);
    }
}