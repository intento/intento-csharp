using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using IntentoSDK;

namespace TestForm
{
    public partial class Form1 : Form
    {
        Intento intento;
        IntentoAiTextTranslate translate;
        IList<dynamic> providers;
        IList<dynamic> languages;
        string asyncId;

        public Form1(string apiKey, Intento intento, IntentoAiTextTranslate translate, IList<dynamic> providers, IList<dynamic> languages)
        {
            InitializeComponent();

            this.intento = intento;
            this.translate = translate;
            this.providers = providers;
            this.languages = languages;

            // Initialize source and target languages
            comboBoxFrom.DisplayMember = "Key";
            comboBoxTo.DisplayMember = "Key";

            comboBoxFrom.Items.Add(new KeyValuePair<string, string>("autodetect", ""));
            comboBoxTo.Items.Add(new KeyValuePair<string, string>("select...", ""));

            comboBoxFrom.SelectedIndex = 0;
            comboBoxTo.SelectedIndex = 0;

            List<KeyValuePair<string, string>> dataLanguages = languages.Select(i => new KeyValuePair<string, string>((string)i.iso_name, (string)i.intento_code)).ToList();
            dataLanguages.Sort((a, b) => string.Compare(a.Key, b.Key));

            foreach (KeyValuePair<string, string> pair in dataLanguages)
            {
                comboBoxFrom.Items.Add(pair);
                comboBoxTo.Items.Add(pair);
            }

            // Initialize list of providers
            comboBoxProvider.DisplayMember = "Key";
            comboBoxProvider.Items.Add(new KeyValuePair<string, string>("smart routing", ""));
            comboBoxProvider.SelectedIndex = 0;

            List<KeyValuePair<string, string>> dataProviders = providers.Select(i => new KeyValuePair<string, string>((string)i.name, (string)i.id)).ToList();
            dataProviders.Sort((a, b) => string.Compare(a.Key, b.Key));

            foreach (KeyValuePair<string, string> pair in dataProviders)
                comboBoxProvider.Items.Add(pair);
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            labelError.Visible = false;
            labelTo.ForeColor = Color.FromName("black");

            string to = ((KeyValuePair<string, string>)comboBoxTo.SelectedItem).Value;
            if (string.IsNullOrEmpty(to))
            {
                labelTo.ForeColor = Color.FromName("red");
                labelError.Visible = true;
                labelError.Text = "Please select Target Language";
                return;
            }

            dynamic result;
            try
            {
                // Call translate intent synchroniously
                result = translate.Fulfill(
                    textBoxText.Text, 
                    to, 
                    from: ((KeyValuePair<string, string>)comboBoxFrom.SelectedItem).Value, 
                    async: false,
                    provider: ((KeyValuePair<string, string>)comboBoxProvider.SelectedItem).Value, 
                    format: checkBoxHtml.Checked ? "html" : null);
            }
            catch(AggregateException ex2)
            {
                Exception ex = ex2.InnerExceptions[0];
                if (ex is IntentoInvalidApiKeyException)
                    textBoxResult.Text = string.Format("Invalid api key");
                else if (ex is IntentoException)
                    textBoxResult.Text = string.Format("Exception {2}: {0}: {1}", ex.Message, ((IntentoException)ex).Content, ex.GetType().Name);
                else
                    textBoxResult.Text = string.Format("Unexpected exception {0}: {1}", ex.GetType().Name, ex.Message);
                return;
            }

            // Show result
            textBoxResult.Text = result.results[0];
            labelTranslateProvider.Text = "via MT provier: " + result.service.provider.name;
        }

        private void buttonSendAsync_Click(object sender, EventArgs e)
        {
            dynamic result;
            try
            {
                // Call translate intent synchroniously
                result = translate.Fulfill(this.textBoxText.Text, (string)comboBoxTo.SelectedValue, from: (string)comboBoxFrom.SelectedValue, async: true, 
                    provider: comboBoxProvider.Text, format: checkBoxHtml.Checked ? "html" : null);
            }
            catch (AggregateException ex2)
            {
                Exception ex = ex2.InnerExceptions[0];
                if (ex is IntentoInvalidApiKeyException)
                    textBoxResult.Text = string.Format("Invalid api key");
                else if (ex is IntentoException)
                    textBoxResult.Text = string.Format("Exception {2}: {0}: {1}", ex.Message, ((IntentoException)ex).Content, ex.GetType().Name);
                else
                    textBoxResult.Text = string.Format("Unexpected exception {0}: {1}", ex.GetType().Name, ex.Message);
                return;
            }

            // Show result
            asyncId = result.id;
            textBoxResult.Text = string.Format("Async ID: {0}", asyncId);
        }

        private void buttonCheckAsync_Click(object sender, EventArgs e)
        {
            if (asyncId == null)
            {
                textBoxResult.Text = "No Async ID";
                return;
            }

            dynamic result;
            try
            {
                // Call translate intent synchroniously
                result = intento.CheckAsyncJob(asyncId);
            }
            catch (AggregateException ex2)
            {
                Exception ex = ex2.InnerExceptions[0];
                if (ex is IntentoInvalidApiKeyException)
                    textBoxResult.Text = string.Format("Invalid api key");
                else if (ex is IntentoException)
                    textBoxResult.Text = string.Format("Exception {2}: {0}: {1}", ex.Message, ((IntentoException)ex).Content, ex.GetType().Name);
                else
                    textBoxResult.Text = string.Format("Unexpected exception {0}: {1}", ex.GetType().Name, ex.Message);
                return;
            }

            // 
            dynamic response = result.response;
            if (result.done == true)
            {
                if (response != null)
                {   // Bug in returned json - need to be in another format
                    textBoxResult.Text = response[0].results;
                }
                else
                {   // Correct answer, but not implemented yet
                    textBoxResult.Text = result.response.results[0];
                }
            }
            else
            {
                textBoxResult.Text = "Not ready yet";
            }

        }

        private void buttonReadFromFile_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "txt files (*.txt)|*.txt|html files (*.html)|*.txt|All files (*.*)|*.*";
            dialog.FilterIndex = 1;
            dialog.RestoreDirectory = false;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var fileName = dialog.FileName;
                FileInfo file = new FileInfo(fileName);
                using (var textStream = file.OpenText())
                    textBoxText.Text = textStream.ReadToEnd();
            }
        }

        private void buttonSaveToFile_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "txt files (*.txt)|*.txt|html files (*.html)|*.txt|All files (*.*)|*.*";
            dialog.FilterIndex = 1;
            dialog.RestoreDirectory = false;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                using (var stream = dialog.OpenFile())
                {
                    var bytes = Encoding.UTF8.GetBytes(textBoxResult.Text);
                    stream.Write(bytes, 0, bytes.Length);
                }
            }
        }
    }
}
