namespace ThreadApp2_ThreadPool
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
            this.btnThreadPool1 = new System.Windows.Forms.Button();
            this.rtbResults = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnThreadPool2 = new System.Windows.Forms.Button();
            this.btnThreadPool3 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // btnThreadPool1
            // 
            this.btnThreadPool1.Location = new System.Drawing.Point(12, 12);
            this.btnThreadPool1.Name = "btnThreadPool1";
            this.btnThreadPool1.Size = new System.Drawing.Size(306, 29);
            this.btnThreadPool1.TabIndex = 0;
            this.btnThreadPool1.Text = "ThreadPool 1 Start - Simple Parameter";
            this.btnThreadPool1.UseVisualStyleBackColor = true;
            this.btnThreadPool1.Click += new System.EventHandler(this.btnThreadPool1_Click);
            // 
            // rtbResults
            // 
            this.rtbResults.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbResults.Location = new System.Drawing.Point(324, 47);
            this.rtbResults.Name = "rtbResults";
            this.rtbResults.Size = new System.Drawing.Size(273, 203);
            this.rtbResults.TabIndex = 1;
            this.rtbResults.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(325, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "label1";
            // 
            // btnThreadPool2
            // 
            this.btnThreadPool2.Location = new System.Drawing.Point(12, 47);
            this.btnThreadPool2.Name = "btnThreadPool2";
            this.btnThreadPool2.Size = new System.Drawing.Size(306, 29);
            this.btnThreadPool2.TabIndex = 3;
            this.btnThreadPool2.Text = "ThreadPool 2 Start - Class as parameter";
            this.btnThreadPool2.UseVisualStyleBackColor = true;
            this.btnThreadPool2.Click += new System.EventHandler(this.btnThreadPool2_Click);
            // 
            // btnThreadPool3
            // 
            this.btnThreadPool3.Location = new System.Drawing.Point(12, 82);
            this.btnThreadPool3.Name = "btnThreadPool3";
            this.btnThreadPool3.Size = new System.Drawing.Size(306, 29);
            this.btnThreadPool3.TabIndex = 4;
            this.btnThreadPool3.Text = "ThreadPool 3 Start - Form Progressbar Display";
            this.btnThreadPool3.UseVisualStyleBackColor = true;
            this.btnThreadPool3.Click += new System.EventHandler(this.btnThreadPool3_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 256);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(585, 23);
            this.progressBar1.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 288);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnThreadPool3);
            this.Controls.Add(this.btnThreadPool2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rtbResults);
            this.Controls.Add(this.btnThreadPool1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "ThreadApp2-ThreadPool";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnThreadPool1;
        private System.Windows.Forms.RichTextBox rtbResults;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnThreadPool2;
        private System.Windows.Forms.Button btnThreadPool3;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}

