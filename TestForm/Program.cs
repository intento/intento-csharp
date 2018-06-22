using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            if (string.IsNullOrEmpty(apiKey) || apiKey == "Your Api Key")
            {
                Form2 form2 = new Form2();
                Application.Run(form2);
                apiKey = form2.apiKey;
            }

            Application.Run(new Form1(apiKey));
        }
    }
}
