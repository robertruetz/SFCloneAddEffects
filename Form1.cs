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
    private List<string> _chosenEffectsList = new List<string>();
    private Button selectLoopsButton;
    private Button addFXButton;
    private ListBox effectsListBox;
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

    public List<string> ChosenEffectsList
    {
        get
        {
            return _chosenEffectsList;
        }

        set
        {
            _chosenEffectsList = value;
        }
    }

    private void InitializeComponent()
    {
            this.selectLoopsButton = new System.Windows.Forms.Button();
            this.addFXButton = new System.Windows.Forms.Button();
            this.effectsListBox = new System.Windows.Forms.ListBox();
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
            // addFXButton
            // 
            this.addFXButton.Location = new System.Drawing.Point(51, 103);
            this.addFXButton.Name = "addFXButton";
            this.addFXButton.Size = new System.Drawing.Size(135, 66);
            this.addFXButton.TabIndex = 2;
            this.addFXButton.Text = "Add Effect";
            this.addFXButton.UseVisualStyleBackColor = true;
            this.addFXButton.Click += new System.EventHandler(this.addFXButton_Click);
            // 
            // effectsListBox
            // 
            this.effectsListBox.FormattingEnabled = true;
            this.effectsListBox.ItemHeight = 20;
            this.effectsListBox.Location = new System.Drawing.Point(51, 176);
            this.effectsListBox.Name = "effectsListBox";
            this.effectsListBox.DataSource = this.ChosenEffectsList;
            this.effectsListBox.Size = new System.Drawing.Size(664, 304);
            this.effectsListBox.TabIndex = 3;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(792, 511);
            this.Controls.Add(this.effectsListBox);
            this.Controls.Add(this.addFXButton);
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

    private void populateEffectsListBox()
    {
        ISfEffectList fxList = this.App.Effects;
    }

    private void addFXButton_Click(object sender, EventArgs e)
    {
        //TODO: Consider doing this once during object init
        ISfEffectList fxList = this.App.Effects;
        string[] fxNames = new string[fxList.Count];
        for(int x = 0; x < fxList.Count; x++)
        {
            fxNames[x] = fxList[x].Name;
        }
        string chosenEffect = SfHelpers.ChooseItemFromList("Select destination file type:", fxNames).ToString();
        ChosenEffectsList.Add(chosenEffect);
        this.effectsListBox.DataSource = null;
        this.effectsListBox.DataSource = ChosenEffectsList;
        this.App.OutputText(string.Format("Added {0} to chosen effects list.", chosenEffect));
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