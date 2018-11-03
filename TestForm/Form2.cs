using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IntentoSDK;
using Microsoft.Win32;

namespace TestForm
{
    public partial class Form2 : Form
    {
        public string apiKey = null;
        public Intento intento;
        public IntentoAiTextTranslate translate;
        public IList<dynamic> providers;
        public IList<dynamic> languages;

        public Form2()
        {
            InitializeComponent();
            apiKey = (string)Registry.GetValue("HKEY_CURRENT_USER\\Software\\Intento", "DemoFormApiKey", null);
            if (string.IsNullOrEmpty(apiKey))
            {
                apiKey = System.Environment.GetEnvironmentVariable("APIKEY");
                if (apiKey == "")
                    apiKey = null;
            }
            textBoxApiKey.Text = apiKey;
        }

        private void buttonContinue_Click(object sender, EventArgs e)
        {
            apiKey = textBoxApiKey.Text;
            Prepare();

            // Save ApiKey into registry
            if (checkBoxSaveApiKey.Checked)
                Registry.SetValue("HKEY_CURRENT_USER\\Software\\Intento", "DemoFormApiKey", apiKey);
        }

        public void Prepare()
        {
            buttonContinue.Enabled = false;
            textBoxApiKey.Enabled = false;
            labelWait.Visible = true;
            labelError.Visible = false;

            this.Refresh();

            try
            {
                // Create connection to Intento API
                intento = Intento.Create(apiKey);

                // Get translate intent
                translate = intento.Ai.Text.Translate;

                providers = translate.Providers(filter: new Dictionary<string, string> { { "integrated", "true" } });
                languages = translate.Languages();

                this.Close();
            }
            catch (AggregateException ex2)
            {
                labelWait.Visible = false;
                labelError.Visible = true;
                buttonContinue.Enabled = true;
                textBoxApiKey.Enabled = true;

                Exception ex = ex2.InnerExceptions[0];
                if (ex is IntentoInvalidApiKeyException)
                    labelError.Text = string.Format("Invalid api key");
                else if (ex is IntentoApiException)
                    labelError.Text = string.Format("Api Exception {2}: {0}: {1}", ex.Message, ((IntentoApiException)ex).Content, ex.GetType().Name);
                else if (ex is IntentoSdkException)
                    labelError.Text = string.Format("Sdk Exception {1}: {0}", ex.Message, ex.GetType().Name);
                else
                    labelError.Text = string.Format("Unexpected exception {0}: {1}", ex.GetType().Name, ex.Message);
                return;
            }
        }

        private void textBoxApiKey_TextChanged(object sender, EventArgs e)
        {
            OverallEnableDisable();
        }

        private void OverallEnableDisable()
        {
            buttonContinue.Enabled = !string.IsNullOrEmpty(textBoxApiKey.Text);
        }
    }
}
