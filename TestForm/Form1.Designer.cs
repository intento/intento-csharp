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
            this.labelTo = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBoxHtml = new System.Windows.Forms.CheckBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.buttonCheckAsync = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxProvider = new System.Windows.Forms.ComboBox();
            this.comboBoxTo = new System.Windows.Forms.ComboBox();
            this.comboBoxFrom = new System.Windows.Forms.ComboBox();
            this.buttonReadFromFile = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.radioButtonViaIntento = new System.Windows.Forms.RadioButton();
            this.radioButtonOwnCredentials = new System.Windows.Forms.RadioButton();
            this.checkBoxPostProcessing = new System.Windows.Forms.CheckBox();
            this.checkBoxCustomModel = new System.Windows.Forms.CheckBox();
            this.textBoxCusomModel = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelError = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioButtonSync = new System.Windows.Forms.RadioButton();
            this.radioButtonAsync = new System.Windows.Forms.RadioButton();
            this.groupBoxTranslated = new System.Windows.Forms.GroupBox();
            this.buttonSaveToFile = new System.Windows.Forms.Button();
            this.labelTranslateProvider = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBoxTranslated.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonSendAsync
            // 
            this.buttonSendAsync.Location = new System.Drawing.Point(307, 351);
            this.buttonSendAsync.Name = "buttonSendAsync";
            this.buttonSendAsync.Size = new System.Drawing.Size(128, 23);
            this.buttonSendAsync.TabIndex = 2;
            this.buttonSendAsync.Text = "Send Async Request";
            this.buttonSendAsync.UseVisualStyleBackColor = true;
            this.buttonSendAsync.Visible = false;
            this.buttonSendAsync.Click += new System.EventHandler(this.buttonSendAsync_Click);
            // 
            // textBoxText
            // 
            this.textBoxText.Location = new System.Drawing.Point(6, 41);
            this.textBoxText.Multiline = true;
            this.textBoxText.Name = "textBoxText";
            this.textBoxText.Size = new System.Drawing.Size(423, 99);
            this.textBoxText.TabIndex = 3;
            this.textBoxText.Text = "Hi!";
            // 
            // labelTo
            // 
            this.labelTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTo.AutoSize = true;
            this.labelTo.Location = new System.Drawing.Point(201, 16);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(89, 13);
            this.labelTo.TabIndex = 6;
            this.labelTo.Text = "Target Language";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Source Language";
            // 
            // checkBoxHtml
            // 
            this.checkBoxHtml.AutoSize = true;
            this.checkBoxHtml.Location = new System.Drawing.Point(6, 19);
            this.checkBoxHtml.Name = "checkBoxHtml";
            this.checkBoxHtml.Size = new System.Drawing.Size(47, 17);
            this.checkBoxHtml.TabIndex = 9;
            this.checkBoxHtml.Text = "Html";
            this.checkBoxHtml.UseVisualStyleBackColor = true;
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(21, 360);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(125, 23);
            this.buttonSend.TabIndex = 11;
            this.buttonSend.Text = "Translate";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // textBoxResult
            // 
            this.textBoxResult.Location = new System.Drawing.Point(5, 18);
            this.textBoxResult.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxResult.Multiline = true;
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.Size = new System.Drawing.Size(424, 89);
            this.textBoxResult.TabIndex = 12;
            // 
            // buttonCheckAsync
            // 
            this.buttonCheckAsync.Location = new System.Drawing.Point(307, 362);
            this.buttonCheckAsync.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCheckAsync.Name = "buttonCheckAsync";
            this.buttonCheckAsync.Size = new System.Drawing.Size(128, 22);
            this.buttonCheckAsync.TabIndex = 13;
            this.buttonCheckAsync.Text = "Check Async";
            this.buttonCheckAsync.UseVisualStyleBackColor = true;
            this.buttonCheckAsync.Visible = false;
            this.buttonCheckAsync.Click += new System.EventHandler(this.buttonCheckAsync_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 57);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "MT Provider";
            // 
            // comboBoxProvider
            // 
            this.comboBoxProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxProvider.FormattingEnabled = true;
            this.comboBoxProvider.Location = new System.Drawing.Point(9, 72);
            this.comboBoxProvider.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxProvider.Name = "comboBoxProvider";
            this.comboBoxProvider.Size = new System.Drawing.Size(153, 21);
            this.comboBoxProvider.TabIndex = 16;
            // 
            // comboBoxTo
            // 
            this.comboBoxTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTo.FormattingEnabled = true;
            this.comboBoxTo.Location = new System.Drawing.Point(204, 33);
            this.comboBoxTo.Name = "comboBoxTo";
            this.comboBoxTo.Size = new System.Drawing.Size(153, 21);
            this.comboBoxTo.TabIndex = 17;
            // 
            // comboBoxFrom
            // 
            this.comboBoxFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFrom.FormattingEnabled = true;
            this.comboBoxFrom.Location = new System.Drawing.Point(9, 33);
            this.comboBoxFrom.Name = "comboBoxFrom";
            this.comboBoxFrom.Size = new System.Drawing.Size(153, 21);
            this.comboBoxFrom.TabIndex = 18;
            // 
            // buttonReadFromFile
            // 
            this.buttonReadFromFile.Enabled = false;
            this.buttonReadFromFile.Location = new System.Drawing.Point(301, 12);
            this.buttonReadFromFile.Name = "buttonReadFromFile";
            this.buttonReadFromFile.Size = new System.Drawing.Size(128, 23);
            this.buttonReadFromFile.TabIndex = 19;
            this.buttonReadFromFile.Text = "Read From File";
            this.buttonReadFromFile.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(201, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Auth";
            // 
            // radioButtonViaIntento
            // 
            this.radioButtonViaIntento.AutoSize = true;
            this.radioButtonViaIntento.Checked = true;
            this.radioButtonViaIntento.Enabled = false;
            this.radioButtonViaIntento.Location = new System.Drawing.Point(204, 72);
            this.radioButtonViaIntento.Name = "radioButtonViaIntento";
            this.radioButtonViaIntento.Size = new System.Drawing.Size(75, 17);
            this.radioButtonViaIntento.TabIndex = 21;
            this.radioButtonViaIntento.TabStop = true;
            this.radioButtonViaIntento.Text = "via Intento";
            this.radioButtonViaIntento.UseVisualStyleBackColor = true;
            // 
            // radioButtonOwnCredentials
            // 
            this.radioButtonOwnCredentials.AutoSize = true;
            this.radioButtonOwnCredentials.Enabled = false;
            this.radioButtonOwnCredentials.Location = new System.Drawing.Point(204, 89);
            this.radioButtonOwnCredentials.Name = "radioButtonOwnCredentials";
            this.radioButtonOwnCredentials.Size = new System.Drawing.Size(99, 17);
            this.radioButtonOwnCredentials.TabIndex = 22;
            this.radioButtonOwnCredentials.Text = "own credentials";
            this.radioButtonOwnCredentials.UseVisualStyleBackColor = true;
            // 
            // checkBoxPostProcessing
            // 
            this.checkBoxPostProcessing.AutoSize = true;
            this.checkBoxPostProcessing.Enabled = false;
            this.checkBoxPostProcessing.Location = new System.Drawing.Point(85, 19);
            this.checkBoxPostProcessing.Name = "checkBoxPostProcessing";
            this.checkBoxPostProcessing.Size = new System.Drawing.Size(102, 17);
            this.checkBoxPostProcessing.TabIndex = 24;
            this.checkBoxPostProcessing.Text = "Post Processing";
            this.checkBoxPostProcessing.UseVisualStyleBackColor = true;
            // 
            // checkBoxCustomModel
            // 
            this.checkBoxCustomModel.AutoSize = true;
            this.checkBoxCustomModel.Enabled = false;
            this.checkBoxCustomModel.Location = new System.Drawing.Point(195, 19);
            this.checkBoxCustomModel.Name = "checkBoxCustomModel";
            this.checkBoxCustomModel.Size = new System.Drawing.Size(93, 17);
            this.checkBoxCustomModel.TabIndex = 25;
            this.checkBoxCustomModel.Text = "Custom Model";
            this.checkBoxCustomModel.UseVisualStyleBackColor = true;
            // 
            // textBoxCusomModel
            // 
            this.textBoxCusomModel.Enabled = false;
            this.textBoxCusomModel.Location = new System.Drawing.Point(295, 19);
            this.textBoxCusomModel.Name = "textBoxCusomModel";
            this.textBoxCusomModel.Size = new System.Drawing.Size(134, 20);
            this.textBoxCusomModel.TabIndex = 26;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.labelTo);
            this.groupBox1.Controls.Add(this.comboBoxFrom);
            this.groupBox1.Controls.Add(this.comboBoxTo);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.radioButtonOwnCredentials);
            this.groupBox1.Controls.Add(this.comboBoxProvider);
            this.groupBox1.Controls.Add(this.radioButtonViaIntento);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(12, 164);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(435, 109);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelError);
            this.groupBox2.Controls.Add(this.textBoxText);
            this.groupBox2.Controls.Add(this.buttonReadFromFile);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(437, 146);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Text to Translate";
            // 
            // labelError
            // 
            this.labelError.AutoSize = true;
            this.labelError.ForeColor = System.Drawing.Color.Red;
            this.labelError.Location = new System.Drawing.Point(9, 17);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(0, 13);
            this.labelError.TabIndex = 20;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBoxCustomModel);
            this.groupBox3.Controls.Add(this.checkBoxHtml);
            this.groupBox3.Controls.Add(this.checkBoxPostProcessing);
            this.groupBox3.Controls.Add(this.textBoxCusomModel);
            this.groupBox3.Location = new System.Drawing.Point(12, 279);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(435, 66);
            this.groupBox3.TabIndex = 29;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Options";
            // 
            // radioButtonSync
            // 
            this.radioButtonSync.AutoSize = true;
            this.radioButtonSync.Checked = true;
            this.radioButtonSync.Enabled = false;
            this.radioButtonSync.Location = new System.Drawing.Point(180, 360);
            this.radioButtonSync.Name = "radioButtonSync";
            this.radioButtonSync.Size = new System.Drawing.Size(49, 17);
            this.radioButtonSync.TabIndex = 30;
            this.radioButtonSync.TabStop = true;
            this.radioButtonSync.Text = "Sync";
            this.radioButtonSync.UseVisualStyleBackColor = true;
            // 
            // radioButtonAsync
            // 
            this.radioButtonAsync.AutoSize = true;
            this.radioButtonAsync.Enabled = false;
            this.radioButtonAsync.Location = new System.Drawing.Point(248, 360);
            this.radioButtonAsync.Name = "radioButtonAsync";
            this.radioButtonAsync.Size = new System.Drawing.Size(54, 17);
            this.radioButtonAsync.TabIndex = 31;
            this.radioButtonAsync.Text = "Async";
            this.radioButtonAsync.UseVisualStyleBackColor = true;
            // 
            // groupBoxTranslated
            // 
            this.groupBoxTranslated.Controls.Add(this.buttonSaveToFile);
            this.groupBoxTranslated.Controls.Add(this.labelTranslateProvider);
            this.groupBoxTranslated.Controls.Add(this.textBoxResult);
            this.groupBoxTranslated.Location = new System.Drawing.Point(12, 389);
            this.groupBoxTranslated.Name = "groupBoxTranslated";
            this.groupBoxTranslated.Size = new System.Drawing.Size(437, 147);
            this.groupBoxTranslated.TabIndex = 32;
            this.groupBoxTranslated.TabStop = false;
            this.groupBoxTranslated.Text = "Translated";
            // 
            // buttonSaveToFile
            // 
            this.buttonSaveToFile.Enabled = false;
            this.buttonSaveToFile.Location = new System.Drawing.Point(301, 113);
            this.buttonSaveToFile.Name = "buttonSaveToFile";
            this.buttonSaveToFile.Size = new System.Drawing.Size(128, 23);
            this.buttonSaveToFile.TabIndex = 14;
            this.buttonSaveToFile.Text = "Save to File";
            this.buttonSaveToFile.UseVisualStyleBackColor = true;
            // 
            // labelTranslateProvider
            // 
            this.labelTranslateProvider.AutoSize = true;
            this.labelTranslateProvider.Location = new System.Drawing.Point(9, 113);
            this.labelTranslateProvider.Name = "labelTranslateProvider";
            this.labelTranslateProvider.Size = new System.Drawing.Size(0, 13);
            this.labelTranslateProvider.TabIndex = 13;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 548);
            this.Controls.Add(this.groupBoxTranslated);
            this.Controls.Add(this.radioButtonAsync);
            this.Controls.Add(this.radioButtonSync);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCheckAsync);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.buttonSendAsync);
            this.Name = "Form1";
            this.Text = "Intento MT";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBoxTranslated.ResumeLayout(false);
            this.groupBoxTranslated.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonSendAsync;
        private System.Windows.Forms.TextBox textBoxText;
        private System.Windows.Forms.Label labelTo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBoxHtml;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.TextBox textBoxResult;
        private System.Windows.Forms.Button buttonCheckAsync;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxProvider;
        private System.Windows.Forms.ComboBox comboBoxTo;
        private System.Windows.Forms.ComboBox comboBoxFrom;
        private System.Windows.Forms.Button buttonReadFromFile;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton radioButtonViaIntento;
        private System.Windows.Forms.RadioButton radioButtonOwnCredentials;
        private System.Windows.Forms.CheckBox checkBoxPostProcessing;
        private System.Windows.Forms.CheckBox checkBoxCustomModel;
        private System.Windows.Forms.TextBox textBoxCusomModel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioButtonSync;
        private System.Windows.Forms.RadioButton radioButtonAsync;
        private System.Windows.Forms.GroupBox groupBoxTranslated;
        private System.Windows.Forms.Button buttonSaveToFile;
        private System.Windows.Forms.Label labelTranslateProvider;
        private System.Windows.Forms.Label labelError;
    }
}

