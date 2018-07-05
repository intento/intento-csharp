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
    public partial class Form1 : Form
    {
        Intento intento;
        IntentoAiTextTranslate translate;
        string asyncId;

        public Form1(string apiKey)
        {
            InitializeComponent();

            // Create connection to Intento API
            intento = Intento.Create(apiKey);

            // Get translate intent
            translate = intento.Ai.Text.Translate;

        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            dynamic result;
            try
            {
                // Call translate intent synchroniously
                result = translate.Fulfill(this.textBoxText.Text, textBoxTo.Text, from: textBoxFrom.Text, async: false, 
                    provider: comboBoxProvider.Text, format: checkBoxHtml.Checked ? "html" : null);
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
        }

        private void buttonSendAsync_Click(object sender, EventArgs e)
        {
            dynamic result;
            try
            {
                // Call translate intent synchroniously
                result = translate.Fulfill(this.textBoxText.Text, textBoxTo.Text, from: textBoxFrom.Text, async: true, 
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
    }
}
