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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.textBoxText = new System.Windows.Forms.TextBox();
            this.labelTo = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBoxHtml = new System.Windows.Forms.CheckBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxProvider = new System.Windows.Forms.ComboBox();
            this.comboBoxTo = new System.Windows.Forms.ComboBox();
            this.comboBoxFrom = new System.Windows.Forms.ComboBox();
            this.buttonReadFromFile = new System.Windows.Forms.Button();
            this.checkBoxPostProcessing = new System.Windows.Forms.CheckBox();
            this.checkBoxCustomModel = new System.Windows.Forms.CheckBox();
            this.textBoxCustomModel = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelError = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxPreProcessing = new System.Windows.Forms.TextBox();
            this.checkBoxPreProcessing = new System.Windows.Forms.CheckBox();
            this.textBoxOwnCredentials = new System.Windows.Forms.TextBox();
            this.checkBoxOwnCredentials = new System.Windows.Forms.CheckBox();
            this.textBoxPostProcessing = new System.Windows.Forms.TextBox();
            this.groupBoxTranslated = new System.Windows.Forms.GroupBox();
            this.buttonSaveToFile = new System.Windows.Forms.Button();
            this.labelTranslateProvider = new System.Windows.Forms.Label();
            this.checkBoxAsync = new System.Windows.Forms.CheckBox();
            this.labelAsync = new System.Windows.Forms.Label();
            this.progressBarAsync = new System.Windows.Forms.ProgressBar();
            this.buttonLanguagePairs = new System.Windows.Forms.Button();
            this.checkBoxWaitAsync = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBoxTranslated.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxText
            // 
            this.textBoxText.Location = new System.Drawing.Point(6, 41);
            this.textBoxText.Multiline = true;
            this.textBoxText.Name = "textBoxText";
            this.textBoxText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxText.Size = new System.Drawing.Size(423, 99);
            this.textBoxText.TabIndex = 3;
            this.textBoxText.Text = "Hi!";
            this.textBoxText.TextChanged += new System.EventHandler(this.textBoxText_TextChanged);
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
            this.checkBoxHtml.Location = new System.Drawing.Point(6, 17);
            this.checkBoxHtml.Name = "checkBoxHtml";
            this.checkBoxHtml.Size = new System.Drawing.Size(47, 17);
            this.checkBoxHtml.TabIndex = 9;
            this.checkBoxHtml.Text = "Html";
            this.checkBoxHtml.UseVisualStyleBackColor = true;
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(17, 433);
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
            this.textBoxResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxResult.Size = new System.Drawing.Size(424, 89);
            this.textBoxResult.TabIndex = 12;
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
            this.comboBoxProvider.SelectedIndexChanged += new System.EventHandler(this.comboBoxProvider_SelectedIndexChanged);
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
            this.comboBoxFrom.SelectedIndexChanged += new System.EventHandler(this.comboBoxFrom_SelectedIndexChanged);
            // 
            // buttonReadFromFile
            // 
            this.buttonReadFromFile.Location = new System.Drawing.Point(301, 12);
            this.buttonReadFromFile.Name = "buttonReadFromFile";
            this.buttonReadFromFile.Size = new System.Drawing.Size(128, 23);
            this.buttonReadFromFile.TabIndex = 19;
            this.buttonReadFromFile.Text = "Read From File";
            this.buttonReadFromFile.UseVisualStyleBackColor = true;
            this.buttonReadFromFile.Click += new System.EventHandler(this.buttonReadFromFile_Click);
            // 
            // checkBoxPostProcessing
            // 
            this.checkBoxPostProcessing.AutoSize = true;
            this.checkBoxPostProcessing.Location = new System.Drawing.Point(6, 85);
            this.checkBoxPostProcessing.Name = "checkBoxPostProcessing";
            this.checkBoxPostProcessing.Size = new System.Drawing.Size(102, 17);
            this.checkBoxPostProcessing.TabIndex = 24;
            this.checkBoxPostProcessing.Text = "Post Processing";
            this.checkBoxPostProcessing.UseVisualStyleBackColor = true;
            // 
            // checkBoxCustomModel
            // 
            this.checkBoxCustomModel.AutoSize = true;
            this.checkBoxCustomModel.Location = new System.Drawing.Point(6, 41);
            this.checkBoxCustomModel.Name = "checkBoxCustomModel";
            this.checkBoxCustomModel.Size = new System.Drawing.Size(93, 17);
            this.checkBoxCustomModel.TabIndex = 25;
            this.checkBoxCustomModel.Text = "Custom Model";
            this.checkBoxCustomModel.UseVisualStyleBackColor = true;
            this.checkBoxCustomModel.CheckedChanged += new System.EventHandler(this.checkBoxCustomModel_CheckedChanged);
            // 
            // textBoxCustomModel
            // 
            this.textBoxCustomModel.Location = new System.Drawing.Point(104, 39);
            this.textBoxCustomModel.Name = "textBoxCustomModel";
            this.textBoxCustomModel.Size = new System.Drawing.Size(325, 20);
            this.textBoxCustomModel.TabIndex = 26;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.labelTo);
            this.groupBox1.Controls.Add(this.comboBoxFrom);
            this.groupBox1.Controls.Add(this.comboBoxTo);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboBoxProvider);
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
            this.groupBox3.Controls.Add(this.textBoxPreProcessing);
            this.groupBox3.Controls.Add(this.checkBoxPreProcessing);
            this.groupBox3.Controls.Add(this.textBoxOwnCredentials);
            this.groupBox3.Controls.Add(this.checkBoxOwnCredentials);
            this.groupBox3.Controls.Add(this.textBoxPostProcessing);
            this.groupBox3.Controls.Add(this.checkBoxCustomModel);
            this.groupBox3.Controls.Add(this.checkBoxHtml);
            this.groupBox3.Controls.Add(this.checkBoxPostProcessing);
            this.groupBox3.Controls.Add(this.textBoxCustomModel);
            this.groupBox3.Location = new System.Drawing.Point(12, 279);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(435, 137);
            this.groupBox3.TabIndex = 29;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Options";
            // 
            // textBoxPreProcessing
            // 
            this.textBoxPreProcessing.Location = new System.Drawing.Point(104, 61);
            this.textBoxPreProcessing.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxPreProcessing.Name = "textBoxPreProcessing";
            this.textBoxPreProcessing.Size = new System.Drawing.Size(325, 20);
            this.textBoxPreProcessing.TabIndex = 31;
            // 
            // checkBoxPreProcessing
            // 
            this.checkBoxPreProcessing.AutoSize = true;
            this.checkBoxPreProcessing.Location = new System.Drawing.Point(6, 63);
            this.checkBoxPreProcessing.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxPreProcessing.Name = "checkBoxPreProcessing";
            this.checkBoxPreProcessing.Size = new System.Drawing.Size(97, 17);
            this.checkBoxPreProcessing.TabIndex = 30;
            this.checkBoxPreProcessing.Text = "Pre Processing";
            this.checkBoxPreProcessing.UseVisualStyleBackColor = true;
            // 
            // textBoxOwnCredentials
            // 
            this.textBoxOwnCredentials.AccessibleDescription = "";
            this.textBoxOwnCredentials.Location = new System.Drawing.Point(105, 107);
            this.textBoxOwnCredentials.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxOwnCredentials.Name = "textBoxOwnCredentials";
            this.textBoxOwnCredentials.Size = new System.Drawing.Size(324, 20);
            this.textBoxOwnCredentials.TabIndex = 29;
            this.textBoxOwnCredentials.Tag = "";
            // 
            // checkBoxOwnCredentials
            // 
            this.checkBoxOwnCredentials.AutoSize = true;
            this.checkBoxOwnCredentials.Location = new System.Drawing.Point(6, 108);
            this.checkBoxOwnCredentials.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxOwnCredentials.Name = "checkBoxOwnCredentials";
            this.checkBoxOwnCredentials.Size = new System.Drawing.Size(103, 17);
            this.checkBoxOwnCredentials.TabIndex = 28;
            this.checkBoxOwnCredentials.Text = "Own Credentials";
            this.checkBoxOwnCredentials.UseVisualStyleBackColor = true;
            // 
            // textBoxPostProcessing
            // 
            this.textBoxPostProcessing.Location = new System.Drawing.Point(105, 84);
            this.textBoxPostProcessing.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxPostProcessing.Name = "textBoxPostProcessing";
            this.textBoxPostProcessing.Size = new System.Drawing.Size(324, 20);
            this.textBoxPostProcessing.TabIndex = 27;
            // 
            // groupBoxTranslated
            // 
            this.groupBoxTranslated.Controls.Add(this.buttonSaveToFile);
            this.groupBoxTranslated.Controls.Add(this.labelTranslateProvider);
            this.groupBoxTranslated.Controls.Add(this.textBoxResult);
            this.groupBoxTranslated.Location = new System.Drawing.Point(12, 482);
            this.groupBoxTranslated.Name = "groupBoxTranslated";
            this.groupBoxTranslated.Size = new System.Drawing.Size(437, 147);
            this.groupBoxTranslated.TabIndex = 32;
            this.groupBoxTranslated.TabStop = false;
            this.groupBoxTranslated.Text = "Translated";
            // 
            // buttonSaveToFile
            // 
            this.buttonSaveToFile.Location = new System.Drawing.Point(301, 113);
            this.buttonSaveToFile.Name = "buttonSaveToFile";
            this.buttonSaveToFile.Size = new System.Drawing.Size(128, 23);
            this.buttonSaveToFile.TabIndex = 14;
            this.buttonSaveToFile.Text = "Save to File";
            this.buttonSaveToFile.UseVisualStyleBackColor = true;
            this.buttonSaveToFile.Click += new System.EventHandler(this.buttonSaveToFile_Click);
            // 
            // labelTranslateProvider
            // 
            this.labelTranslateProvider.AutoSize = true;
            this.labelTranslateProvider.Location = new System.Drawing.Point(9, 113);
            this.labelTranslateProvider.Name = "labelTranslateProvider";
            this.labelTranslateProvider.Size = new System.Drawing.Size(0, 13);
            this.labelTranslateProvider.TabIndex = 13;
            // 
            // checkBoxAsync
            // 
            this.checkBoxAsync.AutoSize = true;
            this.checkBoxAsync.Location = new System.Drawing.Point(155, 433);
            this.checkBoxAsync.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxAsync.Name = "checkBoxAsync";
            this.checkBoxAsync.Size = new System.Drawing.Size(55, 17);
            this.checkBoxAsync.TabIndex = 33;
            this.checkBoxAsync.Text = "Async";
            this.checkBoxAsync.UseVisualStyleBackColor = true;
            this.checkBoxAsync.CheckedChanged += new System.EventHandler(this.checkBoxAsync_CheckedChanged);
            // 
            // labelAsync
            // 
            this.labelAsync.AutoSize = true;
            this.labelAsync.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.labelAsync.Location = new System.Drawing.Point(171, 465);
            this.labelAsync.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelAsync.Name = "labelAsync";
            this.labelAsync.Size = new System.Drawing.Size(87, 13);
            this.labelAsync.TabIndex = 34;
            this.labelAsync.Text = "Async in process";
            // 
            // progressBarAsync
            // 
            this.progressBarAsync.Location = new System.Drawing.Point(17, 462);
            this.progressBarAsync.Margin = new System.Windows.Forms.Padding(2);
            this.progressBarAsync.Name = "progressBarAsync";
            this.progressBarAsync.Size = new System.Drawing.Size(149, 19);
            this.progressBarAsync.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBarAsync.TabIndex = 35;
            this.progressBarAsync.UseWaitCursor = true;
            // 
            // buttonLanguagePairs
            // 
            this.buttonLanguagePairs.Location = new System.Drawing.Point(328, 433);
            this.buttonLanguagePairs.Name = "buttonLanguagePairs";
            this.buttonLanguagePairs.Size = new System.Drawing.Size(113, 25);
            this.buttonLanguagePairs.TabIndex = 36;
            this.buttonLanguagePairs.Text = "LanguagePairs";
            this.buttonLanguagePairs.UseVisualStyleBackColor = true;
            this.buttonLanguagePairs.Click += new System.EventHandler(this.buttonLanguagePairs_Click);
            // 
            // checkBoxWaitAsync
            // 
            this.checkBoxWaitAsync.AutoSize = true;
            this.checkBoxWaitAsync.Location = new System.Drawing.Point(216, 433);
            this.checkBoxWaitAsync.Name = "checkBoxWaitAsync";
            this.checkBoxWaitAsync.Size = new System.Drawing.Size(77, 17);
            this.checkBoxWaitAsync.TabIndex = 37;
            this.checkBoxWaitAsync.Text = "WaitAsync";
            this.checkBoxWaitAsync.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 639);
            this.Controls.Add(this.checkBoxWaitAsync);
            this.Controls.Add(this.buttonLanguagePairs);
            this.Controls.Add(this.progressBarAsync);
            this.Controls.Add(this.labelAsync);
            this.Controls.Add(this.checkBoxAsync);
            this.Controls.Add(this.groupBoxTranslated);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonSend);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
        private System.Windows.Forms.TextBox textBoxText;
        private System.Windows.Forms.Label labelTo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBoxHtml;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.TextBox textBoxResult;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxProvider;
        private System.Windows.Forms.ComboBox comboBoxTo;
        private System.Windows.Forms.ComboBox comboBoxFrom;
        private System.Windows.Forms.Button buttonReadFromFile;
        private System.Windows.Forms.CheckBox checkBoxPostProcessing;
        private System.Windows.Forms.CheckBox checkBoxCustomModel;
        private System.Windows.Forms.TextBox textBoxCustomModel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBoxTranslated;
        private System.Windows.Forms.Button buttonSaveToFile;
        private System.Windows.Forms.Label labelTranslateProvider;
        private System.Windows.Forms.Label labelError;
        private System.Windows.Forms.TextBox textBoxOwnCredentials;
        private System.Windows.Forms.CheckBox checkBoxOwnCredentials;
        private System.Windows.Forms.TextBox textBoxPostProcessing;
        private System.Windows.Forms.TextBox textBoxPreProcessing;
        private System.Windows.Forms.CheckBox checkBoxPreProcessing;
        private System.Windows.Forms.CheckBox checkBoxAsync;
        private System.Windows.Forms.Label labelAsync;
        private System.Windows.Forms.ProgressBar progressBarAsync;
        private System.Windows.Forms.Button buttonLanguagePairs;
        private System.Windows.Forms.CheckBox checkBoxWaitAsync;
    }
}

