namespace TestForm
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
            this.buttonSendAsync = new System.Windows.Forms.Button();
            this.textBoxText = new System.Windows.Forms.TextBox();
            this.textBoxTo = new System.Windows.Forms.TextBox();
            this.textBoxFrom = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBoxHtml = new System.Windows.Forms.CheckBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.buttonCheckAsync = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxProvider = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // buttonSendAsync
            // 
            this.buttonSendAsync.Location = new System.Drawing.Point(89, 471);
            this.buttonSendAsync.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.buttonSendAsync.Name = "buttonSendAsync";
            this.buttonSendAsync.Size = new System.Drawing.Size(234, 42);
            this.buttonSendAsync.TabIndex = 2;
            this.buttonSendAsync.Text = "Send Async Request";
            this.buttonSendAsync.UseVisualStyleBackColor = true;
            this.buttonSendAsync.Click += new System.EventHandler(this.buttonSendAsync_Click);
            // 
            // textBoxText
            // 
            this.textBoxText.Location = new System.Drawing.Point(91, 20);
            this.textBoxText.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxText.Multiline = true;
            this.textBoxText.Name = "textBoxText";
            this.textBoxText.Size = new System.Drawing.Size(357, 133);
            this.textBoxText.TabIndex = 3;
            this.textBoxText.Text = "Hi!";
            // 
            // textBoxTo
            // 
            this.textBoxTo.Location = new System.Drawing.Point(95, 168);
            this.textBoxTo.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxTo.Name = "textBoxTo";
            this.textBoxTo.Size = new System.Drawing.Size(180, 29);
            this.textBoxTo.TabIndex = 4;
            this.textBoxTo.Text = "Es";
            // 
            // textBoxFrom
            // 
            this.textBoxFrom.Location = new System.Drawing.Point(95, 216);
            this.textBoxFrom.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxFrom.Name = "textBoxFrom";
            this.textBoxFrom.Size = new System.Drawing.Size(180, 29);
            this.textBoxFrom.TabIndex = 5;
            this.textBoxFrom.Text = "En";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(47, 168);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 25);
            this.label2.TabIndex = 6;
            this.label2.Text = "To";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 216);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 25);
            this.label4.TabIndex = 7;
            this.label4.Text = "From";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 20);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 25);
            this.label3.TabIndex = 8;
            this.label3.Text = "Text";
            // 
            // checkBoxHtml
            // 
            this.checkBoxHtml.AutoSize = true;
            this.checkBoxHtml.Location = new System.Drawing.Point(95, 266);
            this.checkBoxHtml.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.checkBoxHtml.Name = "checkBoxHtml";
            this.checkBoxHtml.Size = new System.Drawing.Size(77, 29);
            this.checkBoxHtml.TabIndex = 9;
            this.checkBoxHtml.Text = "Html";
            this.checkBoxHtml.UseVisualStyleBackColor = true;
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(94, 399);
            this.buttonSend.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(230, 42);
            this.buttonSend.TabIndex = 11;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // textBoxResult
            // 
            this.textBoxResult.Location = new System.Drawing.Point(89, 608);
            this.textBoxResult.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxResult.Multiline = true;
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.Size = new System.Drawing.Size(357, 160);
            this.textBoxResult.TabIndex = 12;
            // 
            // buttonCheckAsync
            // 
            this.buttonCheckAsync.Location = new System.Drawing.Point(89, 543);
            this.buttonCheckAsync.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonCheckAsync.Name = "buttonCheckAsync";
            this.buttonCheckAsync.Size = new System.Drawing.Size(234, 40);
            this.buttonCheckAsync.TabIndex = 13;
            this.buttonCheckAsync.Text = "Check Async";
            this.buttonCheckAsync.UseVisualStyleBackColor = true;
            this.buttonCheckAsync.Click += new System.EventHandler(this.buttonCheckAsync_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 308);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 25);
            this.label1.TabIndex = 15;
            this.label1.Text = "Provider";
            // 
            // comboBoxProvider
            // 
            this.comboBoxProvider.FormattingEnabled = true;
            this.comboBoxProvider.Items.AddRange(new object[] {
            "ai.text.translate.yandex.translate_api.1-5",
            "ai.text.translate.microsoft.translator_text_api.3-0",
            "ai.text.translate.google.translate_api.2-0",
            "ai.text.translate.baidu.translate_api",
            "ai.text.translate.systran.translation_api.1-0-0"});
            this.comboBoxProvider.Location = new System.Drawing.Point(95, 308);
            this.comboBoxProvider.Name = "comboBoxProvider";
            this.comboBoxProvider.Size = new System.Drawing.Size(353, 32);
            this.comboBoxProvider.TabIndex = 16;
            this.comboBoxProvider.Text = "ai.text.translate.google.translate_api.2-0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 903);
            this.Controls.Add(this.comboBoxProvider);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonCheckAsync);
            this.Controls.Add(this.textBoxResult);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.checkBoxHtml);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxFrom);
            this.Controls.Add(this.textBoxTo);
            this.Controls.Add(this.textBoxText);
            this.Controls.Add(this.buttonSendAsync);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "Form1";
            this.Text = "Form1";
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
        private System.Windows.Forms.Button buttonCheckAsync;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxProvider;
    }
}

