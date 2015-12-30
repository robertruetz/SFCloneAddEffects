using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using SoundForge;

public partial class Form1 : Form
{
    public Form1(IScriptableApp app)
    {
        this.App = app;
        this.DEBUG = true;
        InitializeComponent();
    }

    private bool _DEBUG;
    private string _loopsFolder;
    private Button selectLoopsButton;
    private IScriptableApp _app;

    public bool DEBUG
    {
        get
        {
            return _DEBUG;
        }

        set
        {
            _DEBUG = value;
        }
    }

    public IScriptableApp App
    {
        get
        {
            return _app;
        }

        set
        {
            _app = value;
        }
    }

    public string LoopsFolder
    {
        get
        {
            return _loopsFolder;
        }

        set
        {
            _loopsFolder = value;
        }
    }

    private void InitializeComponent()
    {
        this.selectLoopsButton = new System.Windows.Forms.Button();
        this.SuspendLayout();
        // 
        // selectLoopsButton
        // 
        this.selectLoopsButton.Location = new System.Drawing.Point(51, 32);
        this.selectLoopsButton.Name = "selectLoopsButton";
        this.selectLoopsButton.Size = new System.Drawing.Size(664, 64);
        this.selectLoopsButton.TabIndex = 0;
        this.selectLoopsButton.Text = "Select Loop Files Folder";
        this.selectLoopsButton.UseVisualStyleBackColor = true;
        this.selectLoopsButton.Click += new System.EventHandler(this.selectLoopsButton_Click);
        // 
        // Form1
        // 
        this.ClientSize = new System.Drawing.Size(792, 511);
        this.Controls.Add(this.selectLoopsButton);
        this.Name = "Form1";
        this.Text = "CloneAndAddEffects";
        this.ResumeLayout(false);

    }

    private void selectLoopsButton_Click(object sender, EventArgs e)
    {
        string startDir = @"C:\";
        if (this.DEBUG)
            // shortcut to local files for debugging purposes
            startDir = @"F:\Robert\CodingDump\moh-maker\sample_audio\VO";

        if ((this.LoopsFolder = SfHelpers.ChooseDirectory("Choose the folder with your segments.", startDir)) != null)
        {
            this.selectLoopsButton.Text = "Loops Folder: " + this.LoopsFolder;
        }
        else
        {
            return;
        }

    }
}

partial class Form1
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }
}


public class EntryPoint
{
    public void Begin(IScriptableApp app)
    {
        Form1 theForm = new Form1(app);
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