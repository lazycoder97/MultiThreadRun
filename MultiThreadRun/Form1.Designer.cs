namespace MultiThreadRun
{
    partial class Form1
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
            this.cmdPathTextBox = new System.Windows.Forms.TextBox();
            this.runButton = new System.Windows.Forms.Button();
            this.numThreadBox = new System.Windows.Forms.NumericUpDown();
            this.cmdFileBrowseButton = new System.Windows.Forms.Button();
            this.fileOpenner = new System.Windows.Forms.OpenFileDialog();
            this.stopButton = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.memoryLimiterPathBox = new System.Windows.Forms.TextBox();
            this.useMemoryLimiterCheckBox = new System.Windows.Forms.CheckBox();
            this.memoryLimiterBrowseButton = new System.Windows.Forms.Button();
            this.statusTable = new System.Windows.Forms.DataGridView();
            this.commandColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.numThreadBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusTable)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdPathTextBox
            // 
            this.cmdPathTextBox.Location = new System.Drawing.Point(12, 12);
            this.cmdPathTextBox.Name = "cmdPathTextBox";
            this.cmdPathTextBox.Size = new System.Drawing.Size(400, 22);
            this.cmdPathTextBox.TabIndex = 0;
            // 
            // runButton
            // 
            this.runButton.Location = new System.Drawing.Point(418, 39);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(75, 23);
            this.runButton.TabIndex = 1;
            this.runButton.Text = "Run";
            this.runButton.UseVisualStyleBackColor = true;
            this.runButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // numThreadBox
            // 
            this.numThreadBox.Location = new System.Drawing.Point(350, 40);
            this.numThreadBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numThreadBox.Name = "numThreadBox";
            this.numThreadBox.Size = new System.Drawing.Size(62, 22);
            this.numThreadBox.TabIndex = 3;
            this.numThreadBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cmdFileBrowseButton
            // 
            this.cmdFileBrowseButton.Location = new System.Drawing.Point(418, 10);
            this.cmdFileBrowseButton.Name = "cmdFileBrowseButton";
            this.cmdFileBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.cmdFileBrowseButton.TabIndex = 4;
            this.cmdFileBrowseButton.Text = "Browse";
            this.cmdFileBrowseButton.UseVisualStyleBackColor = true;
            this.cmdFileBrowseButton.Click += new System.EventHandler(this.cmdFileBrowseButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(418, 39);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 6;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // memoryLimiterPathBox
            // 
            this.memoryLimiterPathBox.Enabled = false;
            this.memoryLimiterPathBox.Location = new System.Drawing.Point(12, 97);
            this.memoryLimiterPathBox.Name = "memoryLimiterPathBox";
            this.memoryLimiterPathBox.Size = new System.Drawing.Size(400, 22);
            this.memoryLimiterPathBox.TabIndex = 7;
            // 
            // useMemoryLimiterCheckBox
            // 
            this.useMemoryLimiterCheckBox.AutoSize = true;
            this.useMemoryLimiterCheckBox.Location = new System.Drawing.Point(12, 70);
            this.useMemoryLimiterCheckBox.Name = "useMemoryLimiterCheckBox";
            this.useMemoryLimiterCheckBox.Size = new System.Drawing.Size(163, 21);
            this.useMemoryLimiterCheckBox.TabIndex = 8;
            this.useMemoryLimiterCheckBox.Text = "Use Memory Limiter :";
            this.useMemoryLimiterCheckBox.UseVisualStyleBackColor = true;
            this.useMemoryLimiterCheckBox.CheckedChanged += new System.EventHandler(this.useMemoryLimiterCheckBox_CheckedChanged);
            // 
            // memoryLimiterBrowseButton
            // 
            this.memoryLimiterBrowseButton.Enabled = false;
            this.memoryLimiterBrowseButton.Location = new System.Drawing.Point(418, 96);
            this.memoryLimiterBrowseButton.Name = "memoryLimiterBrowseButton";
            this.memoryLimiterBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.memoryLimiterBrowseButton.TabIndex = 9;
            this.memoryLimiterBrowseButton.Text = "Browse";
            this.memoryLimiterBrowseButton.UseVisualStyleBackColor = true;
            this.memoryLimiterBrowseButton.Click += new System.EventHandler(this.memoryLimiterBrowseButton_Click);
            // 
            // statusTable
            // 
            this.statusTable.AllowUserToAddRows = false;
            this.statusTable.AllowUserToDeleteRows = false;
            this.statusTable.AllowUserToResizeColumns = false;
            this.statusTable.AllowUserToResizeRows = false;
            this.statusTable.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.statusTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.statusTable.ColumnHeadersVisible = false;
            this.statusTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.commandColumn});
            this.statusTable.Location = new System.Drawing.Point(12, 125);
            this.statusTable.Name = "statusTable";
            this.statusTable.ReadOnly = true;
            this.statusTable.RowHeadersVisible = false;
            this.statusTable.RowTemplate.Height = 24;
            this.statusTable.Size = new System.Drawing.Size(481, 310);
            this.statusTable.TabIndex = 10;
            this.statusTable.SelectionChanged += new System.EventHandler(this.statusTable_SelectionChanged);
            // 
            // commandColumn
            // 
            this.commandColumn.HeaderText = "Command";
            this.commandColumn.Name = "commandColumn";
            this.commandColumn.ReadOnly = true;
            this.commandColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.commandColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.commandColumn.Width = 478;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 447);
            this.Controls.Add(this.statusTable);
            this.Controls.Add(this.memoryLimiterBrowseButton);
            this.Controls.Add(this.useMemoryLimiterCheckBox);
            this.Controls.Add(this.memoryLimiterPathBox);
            this.Controls.Add(this.cmdFileBrowseButton);
            this.Controls.Add(this.numThreadBox);
            this.Controls.Add(this.runButton);
            this.Controls.Add(this.cmdPathTextBox);
            this.Controls.Add(this.stopButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "MultiThreadRun";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.numThreadBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox cmdPathTextBox;
        private System.Windows.Forms.Button runButton;
        private System.Windows.Forms.NumericUpDown numThreadBox;
        private System.Windows.Forms.Button cmdFileBrowseButton;
        private System.Windows.Forms.OpenFileDialog fileOpenner;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.TextBox memoryLimiterPathBox;
        private System.Windows.Forms.CheckBox useMemoryLimiterCheckBox;
        private System.Windows.Forms.Button memoryLimiterBrowseButton;
        private System.Windows.Forms.DataGridView statusTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn commandColumn;
    }
}

