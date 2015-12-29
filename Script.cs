using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using SoundForge;

namespace SFCloneAddEffects
{
    class Script
    {
        static void Main(string[] args)
        {
        }
    }

    public class EntryPoint
    {
        public void Begin(IScriptableApp app)
        {
            Form1 theForm = new Form1();
            theForm.InitializeComponent();
            theForm.Appl = app;
            theForm.ShowDialog();
        }


        public void FromSoundForge(IScriptableApp app)
        {
            ForgeApp = app; //execution begins here
            app.SetStatusText(string.Format("Script '{0}' is running.", SoundForge.Script.Name));
            Begin(app);
            app.SetStatusText(string.Format("Script '{0}' is done.", SoundForge.Script.Name));
        }
        public static IScriptableApp ForgeApp = null;
        public static void DPF(string sz) { ForgeApp.OutputText(sz); }
        public static void DPF(string fmt, params object[] args) { ForgeApp.OutputText(String.Format(fmt, args)); }
    } //EntryPoint
}
