﻿using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Brep
{
    static class Program
    {
        private static Form1 mForm;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Boolean retry = false;

            do
            {
                try
                {
                    retry = false;

                    Inventor.Application oApp = System.Runtime.InteropServices.Marshal.GetActiveObject("Inventor.Application") as Inventor.Application;
                    mForm = new Form1(oApp);
                    mForm.ShowDialog();
                }
                catch (Exception ex)
                {
                    string Message = "Inventor is not running...";
                    string Caption = "Brep Sample Error" + ex.Data.ToString();
                 
                   // string CaptionStack = ex.InnerException.ToString();

                    MessageBoxButtons Buttons = MessageBoxButtons.RetryCancel;

                    DialogResult Result = MessageBox.Show(Caption, Caption, Buttons, MessageBoxIcon.Exclamation);
                    
                    retry = false;

                    switch (Result)
                    {
                        case DialogResult.Retry:
                            retry = true;
                            break;

                        case DialogResult.Cancel:
                            return;
                    }
                }
            }
            while (retry == true);
        }
    }
}
