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

        public Form2()
        {
            InitializeComponent();
        }

        private void buttonContinue_Click(object sender, EventArgs e)
        {
            apiKey = textBoxApiKey.Text;
            this.Close();
        }
    }
}
