namespace MtxReport
{
    partial class MainForm
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
            this.runReport = new System.Windows.Forms.Button();
            this.tasksBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // runReport
            // 
            this.runReport.Location = new System.Drawing.Point(12, 12);
            this.runReport.Name = "runReport";
            this.runReport.Size = new System.Drawing.Size(97, 38);
            this.runReport.TabIndex = 1;
            this.runReport.Text = "Report";
            this.runReport.UseVisualStyleBackColor = true;
            this.runReport.Click += new System.EventHandler(this.button1_Click);
            // 
            // tasksBox
            // 
            this.tasksBox.Location = new System.Drawing.Point(12, 56);
            this.tasksBox.Multiline = true;
            this.tasksBox.Name = "tasksBox";
            this.tasksBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tasksBox.Size = new System.Drawing.Size(859, 473);
            this.tasksBox.TabIndex = 0;
            this.tasksBox.WordWrap = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 541);
            this.Controls.Add(this.tasksBox);
            this.Controls.Add(this.runReport);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mtx Report Builder";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button runReport;
        private System.Windows.Forms.TextBox tasksBox;
    }
}

