namespace WinFormsApp2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txtResults = new System.Windows.Forms.TextBox();
            this.btnDelegate1 = new System.Windows.Forms.Button();
            this.btnDelegate2 = new System.Windows.Forms.Button();
            this.btnDelegate3 = new System.Windows.Forms.Button();
            this.btnDelegate4 = new System.Windows.Forms.Button();
            this.btnDelegate5 = new System.Windows.Forms.Button();
            this.btnTaskEventArg = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDelegate6 = new System.Windows.Forms.Button();
            this.txtActionResults = new System.Windows.Forms.TextBox();
            this.txtFuncResults = new System.Windows.Forms.TextBox();
            this.btnDelegate7 = new System.Windows.Forms.Button();
            this.rtbResults = new System.Windows.Forms.RichTextBox();
            this.btnDelegate8 = new System.Windows.Forms.Button();
            this.txtTaskResults = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoUse = new System.Windows.Forms.RadioButton();
            this.rdoNoUse = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtResults
            // 
            this.txtResults.Location = new System.Drawing.Point(311, 145);
            this.txtResults.Name = "txtResults";
            this.txtResults.Size = new System.Drawing.Size(279, 20);
            this.txtResults.TabIndex = 1;
            // 
            // btnDelegate1
            // 
            this.btnDelegate1.Location = new System.Drawing.Point(21, 26);
            this.btnDelegate1.Name = "btnDelegate1";
            this.btnDelegate1.Size = new System.Drawing.Size(284, 23);
            this.btnDelegate1.TabIndex = 3;
            this.btnDelegate1.Text = "btnDelegate1 - Basic Delegate";
            this.btnDelegate1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelegate1.UseVisualStyleBackColor = true;
            this.btnDelegate1.Click += new System.EventHandler(this.btnDelegate1_Click);
            // 
            // btnDelegate2
            // 
            this.btnDelegate2.Location = new System.Drawing.Point(21, 55);
            this.btnDelegate2.Name = "btnDelegate2";
            this.btnDelegate2.Size = new System.Drawing.Size(284, 23);
            this.btnDelegate2.TabIndex = 4;
            this.btnDelegate2.Text = "btnDelegate2 - Delegate Multicasting";
            this.btnDelegate2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelegate2.UseVisualStyleBackColor = true;
            this.btnDelegate2.Click += new System.EventHandler(this.btnDelegate2_Click);
            // 
            // btnDelegate3
            // 
            this.btnDelegate3.Location = new System.Drawing.Point(22, 84);
            this.btnDelegate3.Name = "btnDelegate3";
            this.btnDelegate3.Size = new System.Drawing.Size(284, 23);
            this.btnDelegate3.TabIndex = 0;
            this.btnDelegate3.Text = "btnDelegate3 - MessageBox";
            this.btnDelegate3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelegate3.UseVisualStyleBackColor = true;
            this.btnDelegate3.Click += new System.EventHandler(this.btnDelegate3_Click);
            // 
            // btnDelegate4
            // 
            this.btnDelegate4.Location = new System.Drawing.Point(21, 113);
            this.btnDelegate4.Name = "btnDelegate4";
            this.btnDelegate4.Size = new System.Drawing.Size(284, 23);
            this.btnDelegate4.TabIndex = 7;
            this.btnDelegate4.Text = "btnDelegate4 - Form Control Update - Loop";
            this.btnDelegate4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelegate4.UseVisualStyleBackColor = true;
            this.btnDelegate4.Click += new System.EventHandler(this.btnDelegate4_Click);
            // 
            // btnDelegate5
            // 
            this.btnDelegate5.Location = new System.Drawing.Point(21, 142);
            this.btnDelegate5.Name = "btnDelegate5";
            this.btnDelegate5.Size = new System.Drawing.Size(284, 23);
            this.btnDelegate5.TabIndex = 2;
            this.btnDelegate5.Text = "btnDelegate5 - Form Control Update - Class 5 Thread";
            this.btnDelegate5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelegate5.UseVisualStyleBackColor = true;
            this.btnDelegate5.Click += new System.EventHandler(this.btnDelegate5_Click);
            // 
            // btnTaskEventArg
            // 
            this.btnTaskEventArg.Location = new System.Drawing.Point(21, 268);
            this.btnTaskEventArg.Name = "btnTaskEventArg";
            this.btnTaskEventArg.Size = new System.Drawing.Size(284, 23);
            this.btnTaskEventArg.TabIndex = 5;
            this.btnTaskEventArg.Text = "Task EventArgs Return - Form Control Update";
            this.btnTaskEventArg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTaskEventArg.UseVisualStyleBackColor = true;
            this.btnTaskEventArg.Click += new System.EventHandler(this.btnTaskEventArg_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(311, 303);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 18);
            this.label1.TabIndex = 6;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(311, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(247, 96);
            this.label2.TabIndex = 8;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // btnDelegate6
            // 
            this.btnDelegate6.Location = new System.Drawing.Point(22, 172);
            this.btnDelegate6.Name = "btnDelegate6";
            this.btnDelegate6.Size = new System.Drawing.Size(284, 23);
            this.btnDelegate6.TabIndex = 9;
            this.btnDelegate6.Text = "btnDelegate6 - Form Control Update - Loop (Action)";
            this.btnDelegate6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelegate6.UseVisualStyleBackColor = true;
            this.btnDelegate6.Click += new System.EventHandler(this.btnDelegate6_Click);
            // 
            // txtActionResults
            // 
            this.txtActionResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtActionResults.Location = new System.Drawing.Point(311, 173);
            this.txtActionResults.Name = "txtActionResults";
            this.txtActionResults.Size = new System.Drawing.Size(279, 22);
            this.txtActionResults.TabIndex = 10;
            // 
            // txtFuncResults
            // 
            this.txtFuncResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFuncResults.Location = new System.Drawing.Point(311, 203);
            this.txtFuncResults.Name = "txtFuncResults";
            this.txtFuncResults.Size = new System.Drawing.Size(279, 22);
            this.txtFuncResults.TabIndex = 12;
            // 
            // btnDelegate7
            // 
            this.btnDelegate7.Location = new System.Drawing.Point(22, 203);
            this.btnDelegate7.Name = "btnDelegate7";
            this.btnDelegate7.Size = new System.Drawing.Size(284, 23);
            this.btnDelegate7.TabIndex = 11;
            this.btnDelegate7.Text = "btnDelegate7 - Form Control Update - Loop (Func)";
            this.btnDelegate7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelegate7.UseVisualStyleBackColor = true;
            this.btnDelegate7.Click += new System.EventHandler(this.btnDelegate7_Click);
            // 
            // rtbResults
            // 
            this.rtbResults.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbResults.Location = new System.Drawing.Point(22, 303);
            this.rtbResults.Name = "rtbResults";
            this.rtbResults.Size = new System.Drawing.Size(283, 131);
            this.rtbResults.TabIndex = 13;
            this.rtbResults.Text = "";
            // 
            // btnDelegate8
            // 
            this.btnDelegate8.Location = new System.Drawing.Point(21, 232);
            this.btnDelegate8.Name = "btnDelegate8";
            this.btnDelegate8.Size = new System.Drawing.Size(284, 23);
            this.btnDelegate8.TabIndex = 14;
            this.btnDelegate8.Text = "btnDelegate8 - Form Control Update - Delegate/Task";
            this.btnDelegate8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelegate8.UseVisualStyleBackColor = true;
            this.btnDelegate8.Click += new System.EventHandler(this.btnDelegate8_Click);
            // 
            // txtTaskResults
            // 
            this.txtTaskResults.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTaskResults.Location = new System.Drawing.Point(311, 232);
            this.txtTaskResults.Name = "txtTaskResults";
            this.txtTaskResults.Size = new System.Drawing.Size(279, 23);
            this.txtTaskResults.TabIndex = 15;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoNoUse);
            this.groupBox1.Controls.Add(this.rdoUse);
            this.groupBox1.Location = new System.Drawing.Point(314, 259);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(244, 38);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "TaskEventArg - Task.WaitAll()";
            // 
            // rdoUse
            // 
            this.rdoUse.AutoSize = true;
            this.rdoUse.Location = new System.Drawing.Point(17, 15);
            this.rdoUse.Name = "rdoUse";
            this.rdoUse.Size = new System.Drawing.Size(86, 17);
            this.rdoUse.TabIndex = 0;
            this.rdoUse.Text = "Use WaitAll()";
            this.rdoUse.UseVisualStyleBackColor = true;
            // 
            // rdoNoUse
            // 
            this.rdoNoUse.AutoSize = true;
            this.rdoNoUse.Checked = true;
            this.rdoNoUse.Location = new System.Drawing.Point(128, 15);
            this.rdoNoUse.Name = "rdoNoUse";
            this.rdoNoUse.Size = new System.Drawing.Size(104, 17);
            this.rdoNoUse.TabIndex = 1;
            this.rdoNoUse.TabStop = true;
            this.rdoNoUse.Text = "Not use WaitAll()";
            this.rdoNoUse.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(603, 446);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtTaskResults);
            this.Controls.Add(this.btnDelegate8);
            this.Controls.Add(this.rtbResults);
            this.Controls.Add(this.txtFuncResults);
            this.Controls.Add(this.btnDelegate7);
            this.Controls.Add(this.txtActionResults);
            this.Controls.Add(this.btnDelegate6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnDelegate4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDelegate1);
            this.Controls.Add(this.btnDelegate2);
            this.Controls.Add(this.btnDelegate3);
            this.Controls.Add(this.btnDelegate5);
            this.Controls.Add(this.btnTaskEventArg);
            this.Controls.Add(this.txtResults);
            this.Name = "Form1";
            this.Text = "WinFormsApp2-Delegate";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtResults;
        private System.Windows.Forms.Button btnDelegate1;
        private System.Windows.Forms.Button btnDelegate2;
        private System.Windows.Forms.Button btnDelegate3;
        private System.Windows.Forms.Button btnDelegate4;
        private System.Windows.Forms.Button btnDelegate5;
        private System.Windows.Forms.Button btnDelegate6;
        private System.Windows.Forms.Button btnDelegate7;
        private System.Windows.Forms.Button btnDelegate8;
        private System.Windows.Forms.Button btnTaskEventArg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtActionResults;
        private System.Windows.Forms.TextBox txtFuncResults;
        private System.Windows.Forms.RichTextBox rtbResults;
        private System.Windows.Forms.TextBox txtTaskResults;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoNoUse;
        private System.Windows.Forms.RadioButton rdoUse;
    }
}

