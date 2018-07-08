using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using IntentoSDK;

namespace TestForm
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string apiKey = "Your Api Key";
            Intento intento;
            IntentoAiTextTranslate translate;
            Dictionary<string, string> providers;
            Dictionary<string, string> languages;
            Form2 form2;

            form2 = new Form2();
            Application.Run(form2);

            Application.Run(new Form1(form2.apiKey, form2.intento, form2.translate, form2.providers, form2.languages));
        }
    }
}
