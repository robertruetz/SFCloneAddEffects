﻿using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using SoundForge;

public partial class CloneAndAddEffectsForm : Form
{
    public CloneAndAddEffectsForm(IScriptableApp app)
    {
        this.App = app;
        this.DEBUG = true;
        InitializeComponent();
    }
    private bool _DEBUG;
    private string _loopsFolder;
    private List<FXButton> _chosenEffectsList = new List<FXButton>();
    private Button selectLoopsButton;
    private Button addFXButton;
    private Button runScriptButton;
    private Button resetFormButton;
    private FlowLayoutPanel chosenFXPanel;
    private IScriptableApp _app;

    public void clearForm()
    {
        this.ChosenEffectsList.Clear();
        this.LoopsFolder = null;
        this.chosenFXPanel.Controls.Clear();
        this.selectLoopsButton.Text = "Select Loop Files Folder";
        this.runScriptButton.Text = "RUN SCRIPT";
        this.App.OutputText("Form cleared.");
        this.runScriptButton.Enabled = false;
    }

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

    public List<FXButton> ChosenEffectsList
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
            this.runScriptButton = new System.Windows.Forms.Button();
            this.resetFormButton = new System.Windows.Forms.Button();
            this.chosenFXPanel = new System.Windows.Forms.FlowLayoutPanel();
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
            // runScriptButton
            // 
            this.runScriptButton.Enabled = false;
            this.runScriptButton.Location = new System.Drawing.Point(51, 496);
            this.runScriptButton.Name = "runScriptButton";
            this.runScriptButton.Size = new System.Drawing.Size(664, 66);
            this.runScriptButton.TabIndex = 4;
            this.runScriptButton.Text = "RUN SCRIPT";
            this.runScriptButton.UseVisualStyleBackColor = true;
            this.runScriptButton.Click += new System.EventHandler(this.runScriptButton_Click);
            // 
            // resetFormButton
            // 
            this.resetFormButton.Location = new System.Drawing.Point(580, 102);
            this.resetFormButton.Name = "resetFormButton";
            this.resetFormButton.Size = new System.Drawing.Size(135, 66);
            this.resetFormButton.TabIndex = 5;
            this.resetFormButton.Text = "Reset Form";
            this.resetFormButton.UseVisualStyleBackColor = true;
            this.resetFormButton.Click += new System.EventHandler(this.resetFormButton_Click);
            // 
            // chosenFXPanel
            // 
            this.chosenFXPanel.AutoScroll = true;
            this.chosenFXPanel.BackColor = System.Drawing.SystemColors.Window;
            this.chosenFXPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.chosenFXPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.chosenFXPanel.Location = new System.Drawing.Point(51, 176);
            this.chosenFXPanel.Name = "chosenFXPanel";
            this.chosenFXPanel.Size = new System.Drawing.Size(664, 314);
            this.chosenFXPanel.TabIndex = 6;
            this.chosenFXPanel.WrapContents = false;
            // 
            // CloneAndAddEffectsForm
            // 
            this.ClientSize = new System.Drawing.Size(766, 589);
            this.Controls.Add(this.chosenFXPanel);
            this.Controls.Add(this.resetFormButton);
            this.Controls.Add(this.runScriptButton);
            this.Controls.Add(this.addFXButton);
            this.Controls.Add(this.selectLoopsButton);
            this.Name = "CloneAndAddEffectsForm";
            this.Text = "CloneAndAddEffects";
            this.ResumeLayout(false);

    }

    private void selectLoopsButton_Click(object sender, EventArgs e)
    {
        string startDir = @"C:\";
        // For debug purposes, we look for a test_loops folder and start there if it exists.
        if (Directory.Exists(Path.GetFullPath(".\\test_loops")))
            startDir = Path.GetFullPath(".\\test_loops");

        if ((this.LoopsFolder = SfHelpers.ChooseDirectory("Choose the folder with your segments.", startDir)) != null)
        {
            this.selectLoopsButton.Text = "Loops Folder: " + this.LoopsFolder;
            this.runScriptButton.Enabled = true;
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

    public FXButton createFXButton(ISfGenericEffect effect, ISfGenericPreset preset)
    {
        FXButton newButton = new FXButton();
        newButton.Id = Guid.NewGuid();
        newButton.Size = new Size(this.chosenFXPanel.Width - 8, 60);
        newButton.BackColor = Button.DefaultBackColor;
        newButton.ForeColor = Button.DefaultForeColor;
        newButton.FlatStyle = FlatStyle.Flat;
        //newButton.TextAlign = ContentAlignment.MiddleLeft;
        if (preset.Name.Length < 1)
            preset.Name = "No Preset";
        newButton.Text = String.Format("{0}     |     {1}", effect.Name, preset.Name);
        // Want to make sure our button text is not too long.
        if (newButton.Text.Length > 100)
            newButton.Text = String.Format("{0}...", newButton.Text.Substring(0, 100));
        newButton.Effect = effect;
        newButton.Preset = preset;
        newButton.Click += new EventHandler(FXButton_Click);

        Font menuButtonFont = new Font("Microsoft Sans-Serif", 8);
        Size menuButtonSize = new Size(70, newButton.Height / 3);

        Button upButton = new Button();
        upButton.Name = newButton.Id.ToString();
        upButton.FlatStyle = FlatStyle.Flat;
        upButton.Size = menuButtonSize;
        upButton.Font = menuButtonFont;
        upButton.Location = new Point(newButton.Width - upButton.Width, 0);
        upButton.Text = "\u25B4";
        upButton.Text = "move up";
        upButton.Click += new EventHandler(UpButton_Click);

        Button downButton = new Button();
        downButton.Name = newButton.Id.ToString();
        downButton.FlatStyle = FlatStyle.Flat;
        downButton.Size = menuButtonSize;
        downButton.Font = menuButtonFont;
        downButton.Location = new Point(newButton.Width - downButton.Width, newButton.Height - downButton.Height);
        downButton.Text = "\u25BE";
        downButton.Text = "move down";
        downButton.Click += new EventHandler(DownButton_Click);

        Button removeButton = new Button();
        removeButton.Name = newButton.Id.ToString();
        removeButton.FlatStyle = FlatStyle.Flat;
        removeButton.Size = menuButtonSize;
        removeButton.Font = menuButtonFont;
        removeButton.Location = new Point(newButton.Width - removeButton.Width, removeButton.Height);
        removeButton.Text = "\u1F5D1";
        removeButton.Text = "remove";
        removeButton.Click += new EventHandler(RemoveButton_Click);
 
        newButton.Controls.Add(upButton);
        newButton.Controls.Add(downButton);
        newButton.Controls.Add(removeButton);
        return newButton;
    }

    private void RemoveButton_Click(object sender, EventArgs e)
    {
        Button menuButton = (Button)sender;
        int removeIndex = this.ChosenEffectsList.IndexOf(this.ChosenEffectsList.Find(delegate (FXButton x) { return x.Id.ToString() == menuButton.Name; }));
        this.ChosenEffectsList.RemoveAt(removeIndex);
        redrawChosenFXPanel();
    }

    private void DownButton_Click(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void UpButton_Click(object sender, EventArgs e)
    {

    }

    private void redrawChosenFXPanel()
    {
        this.chosenFXPanel.Controls.Clear();
        this.chosenFXPanel.Controls.AddRange(this.ChosenEffectsList.ToArray());
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
        ISfGenericEffect effect = this.App.FindEffect(chosenEffect);
        ISfGenericPreset preset = effect.ChoosePreset(this.Handle, "Default Template");

        if (effect != null && preset != null)
        {
            ChosenEffectsList.Add(createFXButton(effect, preset));
            redrawChosenFXPanel();

            this.App.OutputText(string.Format("Added {0} to chosen effects list. List length: {1}", chosenEffect, this.ChosenEffectsList.Count));
        }
        else
            return;

        //TODO: Add a sense of order to the effects that is sortable with buttons
        //TODO: Reorder with drag and drop?
    }

    private void runScriptButton_Click(object sender, EventArgs e)
    {
        // create output folder
        string outputDir = Path.Combine(this.LoopsFolder, "Output");
        this.App.OutputText(string.Format("outputDir: {0}", outputDir));
        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }
        
        // open each wav file in the given folder of loops
        foreach (string wavFile in Directory.GetFiles(this.LoopsFolder, "*.wav"))
        {
            // open the file
            ISfFileHost loopFile = this.App.OpenFile(wavFile, true, true);
            // copy the audio and paste it twice to make the loop 3x as long
            SfAudioSelection asel = new SfAudioSelection(loopFile);

            long origLength = asel.Length;

            ISfFileHost newFile = loopFile.NewFile(asel);
            SfAudioSelection newAsel = new SfAudioSelection(newFile);
            loopFile.Close(CloseOptions.DiscardChanges);

            for (int y = 0; y < 2; y++)
                newFile.OverwriteAudio(newFile.Length, 0, newFile, newAsel);

            // apply the given effects/presets
            foreach (FXButton fxB in ChosenEffectsList)
                newFile.DoEffect(fxB.Name, fxB.Preset, new SfAudioSelection(newFile), EffectOptions.EffectOnly);

            // trim the original length of the loop from the beginning and end to leave the middle loop
            newFile.CropAudio(origLength, origLength);

            // save to output folder
            string outPath = Path.Combine(outputDir, Path.GetFileName(wavFile));
            this.App.OutputText(string.Format("Saving to {0}.", outPath));
            newFile.SaveAs(outPath, newFile.SaveFormat.Guid, "Default Template", RenderOptions.OverwriteExisting | RenderOptions.WaitForDoneOrCancel);
            newFile.Close(CloseOptions.SaveChanges);

            this.runScriptButton.Text = "DONE.";
        }
    }

    private void resetFormButton_Click(object sender, EventArgs e)
    {
        this.clearForm();
    }

    private void FXButton_Click(object sender, EventArgs e)
    {
        FXButton fxB = (FXButton)sender;
        ISfGenericPreset preset = fxB.Effect.ChoosePreset(this.Handle, "Default Template");
        if (preset != null)
        {
            FXButton newFXButton = createFXButton(fxB.Effect, preset);
            /* 
            Have to use the anonymous method here instead of lambda presumably because SF 10 is using .NET 2?
            .NET 3+ You would use: int oldButton = this.ChosenEffectsList.IndexOf(this.ChosenEffectsList.Find(x => x.Id == fxB.Id)); 
            */
            int oldButton = this.ChosenEffectsList.IndexOf(this.ChosenEffectsList.Find(delegate (FXButton x) { return x.Id == fxB.Id; }));
            this.ChosenEffectsList[oldButton] = newFXButton;
            redrawChosenFXPanel();
        }
        else
            return;
    }
}

partial class CloneAndAddEffectsForm
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

public class FXButton: Button
{
    private ISfGenericEffect _effect;
    private ISfGenericPreset _preset;
    private Guid _id;

    public ISfGenericEffect Effect
    {
        get
        {
            return _effect;
        }

        set
        {
            _effect = value;
        }
    }

    public ISfGenericPreset Preset
    {
        get
        {
            return _preset;
        }

        set
        {
            _preset = value;
        }
    }

    public Guid Id
    {
        get
        {
            return _id;
        }

        set
        {
            _id = value;
        }
    }
}

public class EntryPoint
{
    public void Begin(IScriptableApp app)
    {
        CloneAndAddEffectsForm theForm = new CloneAndAddEffectsForm(app);
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