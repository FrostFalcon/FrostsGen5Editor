﻿namespace NewEditor.Forms
{
    partial class ExpCurveEditor
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.curveIDDropdown = new System.Windows.Forms.ComboBox();
            this.xpRateChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.xpRateDataGrid = new System.Windows.Forms.DataGridView();
            this.applyButton = new System.Windows.Forms.Button();
            this.statusText = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.xpRateChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xpRateDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // curveIDDropdown
            // 
            this.curveIDDropdown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.curveIDDropdown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.curveIDDropdown.FormattingEnabled = true;
            this.curveIDDropdown.Location = new System.Drawing.Point(14, 12);
            this.curveIDDropdown.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.curveIDDropdown.Name = "curveIDDropdown";
            this.curveIDDropdown.Size = new System.Drawing.Size(192, 24);
            this.curveIDDropdown.TabIndex = 71;
            this.curveIDDropdown.SelectedIndexChanged += new System.EventHandler(this.curveIDDropdown_SelectedIndexChanged);
            // 
            // xpRateChart
            // 
            chartArea1.AxisX.Maximum = 100D;
            chartArea1.AxisX.Minimum = 0D;
            chartArea1.AxisY.Maximum = 2000000D;
            chartArea1.AxisY.Minimum = 0D;
            chartArea1.Name = "ChartArea1";
            this.xpRateChart.ChartAreas.Add(chartArea1);
            this.xpRateChart.Location = new System.Drawing.Point(14, 50);
            this.xpRateChart.Name = "xpRateChart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.xpRateChart.Series.Add(series1);
            this.xpRateChart.Size = new System.Drawing.Size(598, 240);
            this.xpRateChart.TabIndex = 72;
            this.xpRateChart.Text = "chart1";
            // 
            // xpRateDataGrid
            // 
            this.xpRateDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.xpRateDataGrid.Location = new System.Drawing.Point(14, 300);
            this.xpRateDataGrid.Name = "xpRateDataGrid";
            this.xpRateDataGrid.Size = new System.Drawing.Size(598, 88);
            this.xpRateDataGrid.TabIndex = 73;
            // 
            // applyButton
            // 
            this.applyButton.Location = new System.Drawing.Point(213, 9);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(80, 28);
            this.applyButton.TabIndex = 74;
            this.applyButton.Text = "Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // statusText
            // 
            this.statusText.AutoSize = true;
            this.statusText.Location = new System.Drawing.Point(11, 396);
            this.statusText.Name = "statusText";
            this.statusText.Size = new System.Drawing.Size(0, 16);
            this.statusText.TabIndex = 75;
            // 
            // ExpCurveEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 421);
            this.Controls.Add(this.statusText);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.xpRateDataGrid);
            this.Controls.Add(this.xpRateChart);
            this.Controls.Add(this.curveIDDropdown);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ExpCurveEditor";
            this.Text = "Exp Curve Editor";
            ((System.ComponentModel.ISupportInitialize)(this.xpRateChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xpRateDataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox curveIDDropdown;
        private System.Windows.Forms.DataVisualization.Charting.Chart xpRateChart;
        private System.Windows.Forms.DataGridView xpRateDataGrid;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.Label statusText;
    }
}