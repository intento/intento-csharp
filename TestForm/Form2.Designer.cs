﻿namespace TestForm
{
    partial class Form2
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
            this.textBoxApiKey = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonContinue = new System.Windows.Forms.Button();
            this.labelWait = new System.Windows.Forms.Label();
            this.labelError = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxApiKey
            // 
            this.textBoxApiKey.Location = new System.Drawing.Point(117, 12);
            this.textBoxApiKey.Name = "textBoxApiKey";
            this.textBoxApiKey.Size = new System.Drawing.Size(296, 20);
            this.textBoxApiKey.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Api Key";
            // 
            // buttonContinue
            // 
            this.buttonContinue.Location = new System.Drawing.Point(448, 12);
            this.buttonContinue.Name = "buttonContinue";
            this.buttonContinue.Size = new System.Drawing.Size(117, 36);
            this.buttonContinue.TabIndex = 2;
            this.buttonContinue.Text = "Continue";
            this.buttonContinue.UseVisualStyleBackColor = true;
            this.buttonContinue.Click += new System.EventHandler(this.buttonContinue_Click);
            // 
            // labelWait
            // 
            this.labelWait.AutoSize = true;
            this.labelWait.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.labelWait.Location = new System.Drawing.Point(114, 44);
            this.labelWait.Name = "labelWait";
            this.labelWait.Size = new System.Drawing.Size(121, 13);
            this.labelWait.TabIndex = 3;
            this.labelWait.Text = "Initializing. Please wait...";
            this.labelWait.Visible = false;
            // 
            // labelError
            // 
            this.labelError.AutoSize = true;
            this.labelError.ForeColor = System.Drawing.Color.Red;
            this.labelError.Location = new System.Drawing.Point(30, 35);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(29, 13);
            this.labelError.TabIndex = 4;
            this.labelError.Text = "Error";
            this.labelError.Visible = false;
            // 
            // Form2
            // 
            this.ClientSize = new System.Drawing.Size(614, 79);
            this.Controls.Add(this.labelError);
            this.Controls.Add(this.labelWait);
            this.Controls.Add(this.buttonContinue);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxApiKey);
            this.Name = "Form2";
            this.Text = "Intento MT";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonSendAsync;
        private System.Windows.Forms.TextBox textBoxText;
        private System.Windows.Forms.TextBox textBoxTo;
        private System.Windows.Forms.TextBox textBoxFrom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBoxHtml;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.TextBox textBoxResult;
        private System.Windows.Forms.Button buttonWaitAsync;
        private System.Windows.Forms.TextBox textBoxProvider;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxApiKey;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonContinue;
        private System.Windows.Forms.Label labelWait;
        private System.Windows.Forms.Label labelError;
    }
}

