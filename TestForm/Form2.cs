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
        }

        private void buttonContinue_Click(object sender, EventArgs e)
        {
            buttonContinue.Enabled = false;
            textBoxApiKey.Enabled = false;
            labelWait.Visible = true;
            labelError.Visible = false;
            apiKey = textBoxApiKey.Text;

            this.Refresh();

            try
            {
                // Create connection to Intento API
                intento = Intento.Create(apiKey);

                // Get translate intent
                translate = intento.Ai.Text.Translate;

                providers = translate.Providers();
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
                else if (ex is IntentoException)
                    labelError.Text = string.Format("Exception {2}: {0}: {1}", ex.Message, ((IntentoException)ex).Content, ex.GetType().Name);
                else
                    labelError.Text = string.Format("Unexpected exception {0}: {1}", ex.GetType().Name, ex.Message);
                return;
            }
        }
    }
}
