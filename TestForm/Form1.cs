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
using System.Threading;

namespace TestForm
{
    public partial class Form1 : Form
    {
        Intento intento;
        IntentoAiTextTranslate translate;
        IList<dynamic> providers;
        IList<dynamic> languages;
        string asyncId = null;
        bool sendInAction = false;

        public Form1(string apiKey, Intento intento, IntentoAiTextTranslate translate, IList<dynamic> providers, IList<dynamic> languages)
        {
            InitializeComponent();

            this.intento = intento;
            this.translate = translate;
            this.providers = providers;
            this.languages = languages;

            // Initialize source and target languages
            comboBoxFrom.DisplayMember = "Key";
            comboBoxFrom.Items.Add(new KeyValuePair<string, string>("autodetect", ""));
            comboBoxFrom.SelectedIndex = 0;

            comboBoxTo.DisplayMember = "Key";
            comboBoxTo.Items.Add(new KeyValuePair<string, string>("select...", ""));
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

            asyncId = null;
            sendInAction = true;
            textBoxResult.Text = null;
            OverallEnableDisable();

            dynamic result;
            try
            {
                // Call translate intent synchroniously
                result = translate.Fulfill(
                    textBoxText.Text,
                    to,
                    from: ((KeyValuePair<string, string>)comboBoxFrom.SelectedItem).Value,
                    provider: ProviderId,
                    format: checkBoxHtml.Checked ? "html" : null,
                    async: checkBoxAsync.Checked,
                    auth: checkBoxOwnCredentials.Enabled && checkBoxOwnCredentials.Checked ? GetAuth(textBoxOwnCredentials.Text) : null,
                    pre_processing: checkBoxPreProcessing.Enabled && checkBoxPreProcessing.Checked ? textBoxPreProcessing.Text : null,
                    post_processing: checkBoxPostProcessing.Enabled && checkBoxPostProcessing.Checked ? textBoxPostProcessing.Text : null,
                    custom_model: checkBoxCustomModel.Enabled && checkBoxCustomModel.Checked ? textBoxCustomModel.Text : null
                    );
            }
            catch(AggregateException ex2)
            {
                Exception ex = ex2.InnerExceptions[0];
                if (ex is IntentoInvalidApiKeyException)
                    textBoxResult.Text = string.Format("Invalid api key");
                else if (ex is IntentoApiException)
                    textBoxResult.Text = string.Format("Api Exception {2}: {0}: {1}", ex.Message, ((IntentoApiException)ex).Content, ex.GetType().Name);
                else if (ex is IntentoSdkException)
                    textBoxResult.Text = string.Format("Sdk Exception {1}: {0}", ex.Message, ex.GetType().Name);
                else
                    textBoxResult.Text = string.Format("Unexpected exception {0}: {1}", ex.GetType().Name, ex.Message);

                sendInAction = false;
                asyncId = null;
                OverallEnableDisable();
                return;
            }

            if (!checkBoxAsync.Checked)
            {
                // Show result
                labelTranslateProvider.Text = "via MT provier: " + result.service.provider.name;
                textBoxResult.Text = result.results[0];
                sendInAction = false;
                OverallEnableDisable();
            }
            else
            {
                asyncId = result.id;
                OverallEnableDisable();
                labelAsync.Text = string.Format("Async {0} in process, 0 sec", asyncId);

                // Wait for result
                string resultText;
                int n = 0;
                while ((resultText = CheckAsync()) == null)
                {
                    OverallEnableDisable();
                    labelAsync.Text = string.Format("Async {0} in process, {1} sec", asyncId, (int)n/500);
                    Thread.Sleep(500);
                    n += 500;
                }

                asyncId = null;
                textBoxResult.Text = resultText;
                sendInAction = false;
                OverallEnableDisable();
            }
        }

        private string ProviderId
        { get { return ((KeyValuePair<string, string>)comboBoxProvider.SelectedItem).Value; } }

        // Make auth parameter for Fulfill call
        private string GetAuth(string auth)
        {
            if (string.IsNullOrEmpty(auth))
                return null;
            if (string.IsNullOrEmpty(ProviderId))
                throw new Exception("Own Key parameter is incompatible with smart mode");
            return string.Format("{{'{0}':[{1}]}}", ProviderId, auth).Replace('\'', '"');
        }

        private string CheckAsync()
        {
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
                    return string.Format("Error: Invalid api key");
                else if (ex is IntentoApiException)
                    return string.Format("Error: Exception {2}: {0}: {1}", ex.Message, ((IntentoApiException)ex).Content, ex.GetType().Name);
                else
                    return string.Format("Error: Unexpected exception {0}: {1}", ex.GetType().Name, ex.Message);
                return null;
            }

            // 
            dynamic response = result.response;
            if (result.done == true)
            {
                if (response != null)
                {   // Bug in returned json - need to be in another format
                    return response[0].results[0];
                }
                else
                {   // Correct answer, but not implemented yet
                    return result.response.results[0];
                }
            }
            else
                return null;
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

        private void checkBoxCustomModel_CheckedChanged(object sender, EventArgs e)
        {
            textBoxCustomModel.Enabled = checkBoxCustomModel.Checked;
        }

        private void comboBoxProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            OverallEnableDisable();
        }

        private bool IsSmartMode
        { get { return ((KeyValuePair<string, string>)comboBoxProvider.SelectedItem).Value != ""; } }

        private void textBoxText_TextChanged(object sender, EventArgs e)
        {
            OverallEnableDisable();
        }

        private void OverallEnableDisable()
        {
            checkBoxCustomModel.Enabled = textBoxCustomModel.Enabled = checkBoxOwnCredentials.Enabled = textBoxOwnCredentials.Enabled = IsSmartMode;
            labelAsync.Visible = progressBarAsync.Visible = asyncId != null;
            buttonSend.Enabled = !string.IsNullOrEmpty(textBoxText.Text) && !sendInAction;
            // textBoxResult.Enabled = sendInAction;
        }

    }
}
