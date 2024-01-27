namespace BK2_mod_translate_tool
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            InputModGroupBox = new GroupBox();
            button1 = new Button();
            InputModFolderTextBox = new TextBox();
            toolTip1 = new ToolTip(components);
            button2 = new Button();
            GameDataGroupBox = new GroupBox();
            GameDataInputFolder = new TextBox();
            AddLanguagesGroupBox = new GroupBox();
            button3 = new Button();
            LanguageTextBox = new TextBox();
            OutputMasterGroupBox = new GroupBox();
            button4 = new Button();
            MasterFolderTextBox = new TextBox();
            TranslatingGroupBox = new GroupBox();
            EditCurrentIndexButton = new Button();
            TranslatingFlowUIPanel = new FlowLayoutPanel();
            OriginalTextTextBox = new RichTextBox();
            TextsCountLabel = new Label();
            button8 = new Button();
            FilePathLabel = new Label();
            button7 = new Button();
            CreateFoldersButton = new Button();
            ClearLanguagesButton = new Button();
            OpenExistingFolderButton = new Button();
            StartTranslatingButton = new Button();
            LanguagesLayout = new FlowLayoutPanel();
            folderBrowserDialog1 = new FolderBrowserDialog();
            InputModGroupBox.SuspendLayout();
            GameDataGroupBox.SuspendLayout();
            AddLanguagesGroupBox.SuspendLayout();
            OutputMasterGroupBox.SuspendLayout();
            TranslatingGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // InputModGroupBox
            // 
            InputModGroupBox.Controls.Add(button1);
            InputModGroupBox.Controls.Add(InputModFolderTextBox);
            InputModGroupBox.Location = new Point(12, 12);
            InputModGroupBox.Name = "InputModGroupBox";
            InputModGroupBox.Size = new Size(813, 63);
            InputModGroupBox.TabIndex = 0;
            InputModGroupBox.TabStop = false;
            InputModGroupBox.Text = "Input mod folder";
            toolTip1.SetToolTip(InputModGroupBox, "Mod to gather .txt files from, it's in the games \"mods\" folder");
            // 
            // button1
            // 
            button1.Location = new Point(746, 22);
            button1.Name = "button1";
            button1.Size = new Size(61, 23);
            button1.TabIndex = 1;
            button1.Text = "...";
            toolTip1.SetToolTip(button1, "Mod to gather .txt files from, it's in the games \"mods\" folder");
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // InputModFolderTextBox
            // 
            InputModFolderTextBox.Location = new Point(6, 22);
            InputModFolderTextBox.Name = "InputModFolderTextBox";
            InputModFolderTextBox.Size = new Size(734, 23);
            InputModFolderTextBox.TabIndex = 0;
            // 
            // button2
            // 
            button2.Location = new Point(746, 22);
            button2.Name = "button2";
            button2.Size = new Size(61, 23);
            button2.TabIndex = 2;
            button2.Text = "...";
            toolTip1.SetToolTip(button2, "Data folder of the game, used if mod folder doesn't have all .txt files covered");
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // GameDataGroupBox
            // 
            GameDataGroupBox.Controls.Add(button2);
            GameDataGroupBox.Controls.Add(GameDataInputFolder);
            GameDataGroupBox.Location = new Point(12, 81);
            GameDataGroupBox.Name = "GameDataGroupBox";
            GameDataGroupBox.Size = new Size(813, 63);
            GameDataGroupBox.TabIndex = 1;
            GameDataGroupBox.TabStop = false;
            GameDataGroupBox.Text = "Game's data folder";
            toolTip1.SetToolTip(GameDataGroupBox, "Data folder of the game, used if mod folder doesn't have all .txt files covered");
            // 
            // GameDataInputFolder
            // 
            GameDataInputFolder.Location = new Point(6, 22);
            GameDataInputFolder.Name = "GameDataInputFolder";
            GameDataInputFolder.Size = new Size(734, 23);
            GameDataInputFolder.TabIndex = 2;
            // 
            // AddLanguagesGroupBox
            // 
            AddLanguagesGroupBox.Controls.Add(button3);
            AddLanguagesGroupBox.Controls.Add(LanguageTextBox);
            AddLanguagesGroupBox.Location = new Point(12, 150);
            AddLanguagesGroupBox.Name = "AddLanguagesGroupBox";
            AddLanguagesGroupBox.Size = new Size(140, 63);
            AddLanguagesGroupBox.TabIndex = 3;
            AddLanguagesGroupBox.TabStop = false;
            AddLanguagesGroupBox.Text = "Add languages";
            toolTip1.SetToolTip(AddLanguagesGroupBox, "Adds languages to make translations for");
            // 
            // button3
            // 
            button3.Location = new Point(111, 22);
            button3.Name = "button3";
            button3.Size = new Size(23, 23);
            button3.TabIndex = 4;
            button3.Text = "+";
            toolTip1.SetToolTip(button3, "Adds languages to make translations for");
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // LanguageTextBox
            // 
            LanguageTextBox.Location = new Point(6, 22);
            LanguageTextBox.Name = "LanguageTextBox";
            LanguageTextBox.Size = new Size(99, 23);
            LanguageTextBox.TabIndex = 0;
            LanguageTextBox.KeyDown += LanguageTextBox_KeyDown;
            // 
            // OutputMasterGroupBox
            // 
            OutputMasterGroupBox.Controls.Add(button4);
            OutputMasterGroupBox.Controls.Add(MasterFolderTextBox);
            OutputMasterGroupBox.Location = new Point(12, 219);
            OutputMasterGroupBox.Name = "OutputMasterGroupBox";
            OutputMasterGroupBox.Size = new Size(813, 63);
            OutputMasterGroupBox.TabIndex = 4;
            OutputMasterGroupBox.TabStop = false;
            OutputMasterGroupBox.Text = "Output master folder";
            toolTip1.SetToolTip(OutputMasterGroupBox, "Folder in which language subfolders will be created");
            // 
            // button4
            // 
            button4.Location = new Point(746, 22);
            button4.Name = "button4";
            button4.Size = new Size(61, 23);
            button4.TabIndex = 1;
            button4.Text = "...";
            toolTip1.SetToolTip(button4, "Folder in which language subfolders will be created");
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // MasterFolderTextBox
            // 
            MasterFolderTextBox.Location = new Point(6, 22);
            MasterFolderTextBox.Name = "MasterFolderTextBox";
            MasterFolderTextBox.Size = new Size(734, 23);
            MasterFolderTextBox.TabIndex = 0;
            // 
            // TranslatingGroupBox
            // 
            TranslatingGroupBox.Controls.Add(EditCurrentIndexButton);
            TranslatingGroupBox.Controls.Add(TranslatingFlowUIPanel);
            TranslatingGroupBox.Controls.Add(OriginalTextTextBox);
            TranslatingGroupBox.Controls.Add(TextsCountLabel);
            TranslatingGroupBox.Controls.Add(button8);
            TranslatingGroupBox.Controls.Add(FilePathLabel);
            TranslatingGroupBox.Controls.Add(button7);
            TranslatingGroupBox.Dock = DockStyle.Bottom;
            TranslatingGroupBox.Enabled = false;
            TranslatingGroupBox.Location = new Point(0, 335);
            TranslatingGroupBox.Name = "TranslatingGroupBox";
            TranslatingGroupBox.Size = new Size(837, 438);
            TranslatingGroupBox.TabIndex = 7;
            TranslatingGroupBox.TabStop = false;
            TranslatingGroupBox.Text = "Translating";
            toolTip1.SetToolTip(TranslatingGroupBox, "Translation is done here");
            // 
            // EditCurrentIndexButton
            // 
            EditCurrentIndexButton.Location = new Point(6, 28);
            EditCurrentIndexButton.Name = "EditCurrentIndexButton";
            EditCurrentIndexButton.Size = new Size(47, 22);
            EditCurrentIndexButton.TabIndex = 12;
            EditCurrentIndexButton.Text = "...";
            EditCurrentIndexButton.UseVisualStyleBackColor = true;
            EditCurrentIndexButton.Click += EditCurrentIndexButton_Click;
            // 
            // TranslatingFlowUIPanel
            // 
            TranslatingFlowUIPanel.Location = new Point(6, 168);
            TranslatingFlowUIPanel.Name = "TranslatingFlowUIPanel";
            TranslatingFlowUIPanel.Size = new Size(825, 228);
            TranslatingFlowUIPanel.TabIndex = 11;
            toolTip1.SetToolTip(TranslatingFlowUIPanel, "Here you can edit language specific texts");
            // 
            // OriginalTextTextBox
            // 
            OriginalTextTextBox.Location = new Point(6, 54);
            OriginalTextTextBox.Name = "OriginalTextTextBox";
            OriginalTextTextBox.ReadOnly = true;
            OriginalTextTextBox.Size = new Size(825, 108);
            OriginalTextTextBox.TabIndex = 8;
            OriginalTextTextBox.TabStop = false;
            OriginalTextTextBox.Text = "ORIGINAL TEXT: og file text";
            toolTip1.SetToolTip(OriginalTextTextBox, "Original files' text");
            // 
            // TextsCountLabel
            // 
            TextsCountLabel.AutoSize = true;
            TextsCountLabel.Location = new Point(719, 32);
            TextsCountLabel.Name = "TextsCountLabel";
            TextsCountLabel.Size = new Size(112, 15);
            TextsCountLabel.TabIndex = 7;
            TextsCountLabel.Text = "TEXTS COUNT: ??/??";
            // 
            // button8
            // 
            button8.Location = new Point(329, 402);
            button8.Name = "button8";
            button8.Size = new Size(90, 30);
            button8.TabIndex = 3;
            button8.Text = "← Previous";
            toolTip1.SetToolTip(button8, "Previous file to translate (Ctrl + Y)");
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // FilePathLabel
            // 
            FilePathLabel.AutoSize = true;
            FilePathLabel.Location = new Point(59, 32);
            FilePathLabel.Name = "FilePathLabel";
            FilePathLabel.Size = new Size(183, 15);
            FilePathLabel.TabIndex = 1;
            FilePathLabel.Text = "FILE PATH: path/to/unit/name.txt";
            // 
            // button7
            // 
            button7.Location = new Point(425, 402);
            button7.Name = "button7";
            button7.Size = new Size(90, 30);
            button7.TabIndex = 0;
            button7.Text = "Next →";
            toolTip1.SetToolTip(button7, "Next file to translate (Ctrl + X)");
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // CreateFoldersButton
            // 
            CreateFoldersButton.Location = new Point(12, 288);
            CreateFoldersButton.Name = "CreateFoldersButton";
            CreateFoldersButton.Size = new Size(169, 29);
            CreateFoldersButton.TabIndex = 5;
            CreateFoldersButton.Text = "Create folders with files";
            toolTip1.SetToolTip(CreateFoldersButton, "Creates appropriate folder structures for translation");
            CreateFoldersButton.UseVisualStyleBackColor = true;
            CreateFoldersButton.Click += CreateFoldersButton_Click;
            // 
            // ClearLanguagesButton
            // 
            ClearLanguagesButton.Location = new Point(362, 288);
            ClearLanguagesButton.Name = "ClearLanguagesButton";
            ClearLanguagesButton.Size = new Size(169, 29);
            ClearLanguagesButton.TabIndex = 6;
            ClearLanguagesButton.Text = "Clear languages";
            toolTip1.SetToolTip(ClearLanguagesButton, "Clears all languages and disabled translation part (it does not delete existing folders)");
            ClearLanguagesButton.UseVisualStyleBackColor = true;
            ClearLanguagesButton.Click += button6_Click;
            // 
            // OpenExistingFolderButton
            // 
            OpenExistingFolderButton.Location = new Point(537, 288);
            OpenExistingFolderButton.Name = "OpenExistingFolderButton";
            OpenExistingFolderButton.Size = new Size(169, 29);
            OpenExistingFolderButton.TabIndex = 8;
            OpenExistingFolderButton.Text = "Open existing master folder";
            toolTip1.SetToolTip(OpenExistingFolderButton, "Use when you want to continue editing existing translation folder structure");
            OpenExistingFolderButton.UseVisualStyleBackColor = true;
            OpenExistingFolderButton.Click += OpenExistingFolderButton_Click;
            // 
            // StartTranslatingButton
            // 
            StartTranslatingButton.Enabled = false;
            StartTranslatingButton.Location = new Point(187, 288);
            StartTranslatingButton.Name = "StartTranslatingButton";
            StartTranslatingButton.Size = new Size(169, 29);
            StartTranslatingButton.TabIndex = 9;
            StartTranslatingButton.Text = "Start Translating";
            toolTip1.SetToolTip(StartTranslatingButton, "Start the translation process");
            StartTranslatingButton.UseVisualStyleBackColor = true;
            StartTranslatingButton.Click += StartTranslatingButton_Click;
            // 
            // LanguagesLayout
            // 
            LanguagesLayout.Location = new Point(158, 163);
            LanguagesLayout.Name = "LanguagesLayout";
            LanguagesLayout.Size = new Size(667, 50);
            LanguagesLayout.TabIndex = 2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(837, 773);
            Controls.Add(StartTranslatingButton);
            Controls.Add(OpenExistingFolderButton);
            Controls.Add(TranslatingGroupBox);
            Controls.Add(ClearLanguagesButton);
            Controls.Add(CreateFoldersButton);
            Controls.Add(OutputMasterGroupBox);
            Controls.Add(AddLanguagesGroupBox);
            Controls.Add(LanguagesLayout);
            Controls.Add(GameDataGroupBox);
            Controls.Add(InputModGroupBox);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            MaximizeBox = false;
            MaximumSize = new Size(853, 812);
            MinimumSize = new Size(853, 812);
            Name = "Form1";
            Text = "BK2 Mod Translator Tool";
            FormClosing += Form1_FormClosing;
            KeyDown += Form1_KeyDown;
            InputModGroupBox.ResumeLayout(false);
            InputModGroupBox.PerformLayout();
            GameDataGroupBox.ResumeLayout(false);
            GameDataGroupBox.PerformLayout();
            AddLanguagesGroupBox.ResumeLayout(false);
            AddLanguagesGroupBox.PerformLayout();
            OutputMasterGroupBox.ResumeLayout(false);
            OutputMasterGroupBox.PerformLayout();
            TranslatingGroupBox.ResumeLayout(false);
            TranslatingGroupBox.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox InputModGroupBox;
        private ToolTip toolTip1;
        private TextBox InputModFolderTextBox;
        private Button button1;
        private FolderBrowserDialog folderBrowserDialog1;
        private GroupBox GameDataGroupBox;
        private Button button2;
        private TextBox GameDataInputFolder;
        private FlowLayoutPanel LanguagesLayout;
        private GroupBox AddLanguagesGroupBox;
        private TextBox LanguageTextBox;
        private Button button3;
        private GroupBox OutputMasterGroupBox;
        private Button button4;
        private TextBox MasterFolderTextBox;
        private Button CreateFoldersButton;
        private Button ClearLanguagesButton;
        private GroupBox TranslatingGroupBox;
        private Button button7;
        private Label FilePathLabel;
        private Button button8;
        private Label TextsCountLabel;
        private Button OpenExistingFolderButton;
        private Button StartTranslatingButton;
        private RichTextBox OriginalTextTextBox;
        private FlowLayoutPanel TranslatingFlowUIPanel;
        private Button EditCurrentIndexButton;
    }
}
