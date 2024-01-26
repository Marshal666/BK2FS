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
            groupBox1 = new GroupBox();
            button1 = new Button();
            InputModFolderTextBox = new TextBox();
            toolTip1 = new ToolTip(components);
            button2 = new Button();
            groupBox2 = new GroupBox();
            GameDataInputFolder = new TextBox();
            groupBox3 = new GroupBox();
            button3 = new Button();
            LanguageTextBox = new TextBox();
            groupBox4 = new GroupBox();
            button4 = new Button();
            MasterFolderTextBox = new TextBox();
            groupBox5 = new GroupBox();
            TextsCountLabel = new Label();
            groupBox8 = new GroupBox();
            textBox3 = new TextBox();
            groupBox7 = new GroupBox();
            textBox2 = new TextBox();
            groupBox6 = new GroupBox();
            textBox1 = new TextBox();
            button8 = new Button();
            OriginalTextLabel = new Label();
            FilePathLabel = new Label();
            button7 = new Button();
            CreateFoldersButton = new Button();
            ClearLanguagesButton = new Button();
            OpenExistingFolderButton = new Button();
            LanguagesLayout = new FlowLayoutPanel();
            folderBrowserDialog1 = new FolderBrowserDialog();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox5.SuspendLayout();
            groupBox8.SuspendLayout();
            groupBox7.SuspendLayout();
            groupBox6.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button1);
            groupBox1.Controls.Add(InputModFolderTextBox);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(813, 63);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Input mod folder";
            toolTip1.SetToolTip(groupBox1, "Mod to gather .txt files from, it's in the games \"mods\" folder");
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
            // groupBox2
            // 
            groupBox2.Controls.Add(button2);
            groupBox2.Controls.Add(GameDataInputFolder);
            groupBox2.Location = new Point(12, 81);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(813, 63);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Games' data folder";
            toolTip1.SetToolTip(groupBox2, "Data folder of the game, used if mod folder doesn't have all .txt files covered");
            // 
            // GameDataInputFolder
            // 
            GameDataInputFolder.Location = new Point(6, 22);
            GameDataInputFolder.Name = "GameDataInputFolder";
            GameDataInputFolder.Size = new Size(734, 23);
            GameDataInputFolder.TabIndex = 2;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(button3);
            groupBox3.Controls.Add(LanguageTextBox);
            groupBox3.Location = new Point(12, 150);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(140, 63);
            groupBox3.TabIndex = 3;
            groupBox3.TabStop = false;
            groupBox3.Text = "Add languages";
            toolTip1.SetToolTip(groupBox3, "Adds languages to make translations for");
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
            // groupBox4
            // 
            groupBox4.Controls.Add(button4);
            groupBox4.Controls.Add(MasterFolderTextBox);
            groupBox4.Location = new Point(12, 219);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(813, 63);
            groupBox4.TabIndex = 4;
            groupBox4.TabStop = false;
            groupBox4.Text = "Output master folder";
            toolTip1.SetToolTip(groupBox4, "Folder in which language subfolders will be created");
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
            // groupBox5
            // 
            groupBox5.Controls.Add(TextsCountLabel);
            groupBox5.Controls.Add(groupBox8);
            groupBox5.Controls.Add(groupBox7);
            groupBox5.Controls.Add(groupBox6);
            groupBox5.Controls.Add(button8);
            groupBox5.Controls.Add(OriginalTextLabel);
            groupBox5.Controls.Add(FilePathLabel);
            groupBox5.Controls.Add(button7);
            groupBox5.Dock = DockStyle.Bottom;
            groupBox5.Location = new Point(0, 335);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(837, 438);
            groupBox5.TabIndex = 7;
            groupBox5.TabStop = false;
            groupBox5.Text = "Translating";
            toolTip1.SetToolTip(groupBox5, "Translation is done here");
            // 
            // TextsCountLabel
            // 
            TextsCountLabel.AutoSize = true;
            TextsCountLabel.Location = new Point(709, 36);
            TextsCountLabel.Name = "TextsCountLabel";
            TextsCountLabel.Size = new Size(122, 15);
            TextsCountLabel.TabIndex = 7;
            TextsCountLabel.Text = "TEXTS COUNT: 1/6000";
            // 
            // groupBox8
            // 
            groupBox8.Controls.Add(textBox3);
            groupBox8.Location = new Point(427, 96);
            groupBox8.Name = "groupBox8";
            groupBox8.Size = new Size(200, 107);
            groupBox8.TabIndex = 6;
            groupBox8.TabStop = false;
            groupBox8.Text = "English";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(6, 22);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(188, 23);
            textBox3.TabIndex = 0;
            textBox3.Text = "New Unit name";
            // 
            // groupBox7
            // 
            groupBox7.Controls.Add(textBox2);
            groupBox7.Location = new Point(221, 96);
            groupBox7.Name = "groupBox7";
            groupBox7.Size = new Size(200, 107);
            groupBox7.TabIndex = 5;
            groupBox7.TabStop = false;
            groupBox7.Text = "English";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(6, 22);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(188, 23);
            textBox2.TabIndex = 0;
            textBox2.Text = "New Unit name";
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(textBox1);
            groupBox6.Location = new Point(6, 96);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(200, 107);
            groupBox6.TabIndex = 4;
            groupBox6.TabStop = false;
            groupBox6.Text = "German";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(6, 22);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(188, 23);
            textBox1.TabIndex = 0;
            textBox1.Text = "New Unit name";
            // 
            // button8
            // 
            button8.Location = new Point(310, 402);
            button8.Name = "button8";
            button8.Size = new Size(90, 30);
            button8.TabIndex = 3;
            button8.Text = "← Previous";
            button8.UseVisualStyleBackColor = true;
            // 
            // OriginalTextLabel
            // 
            OriginalTextLabel.AutoSize = true;
            OriginalTextLabel.Location = new Point(6, 64);
            OriginalTextLabel.Name = "OriginalTextLabel";
            OriginalTextLabel.Size = new Size(205, 15);
            OriginalTextLabel.TabIndex = 2;
            OriginalTextLabel.Text = "ORIGINAL TEXT: original_text_file_text";
            // 
            // FilePathLabel
            // 
            FilePathLabel.AutoSize = true;
            FilePathLabel.Location = new Point(6, 36);
            FilePathLabel.Name = "FilePathLabel";
            FilePathLabel.Size = new Size(183, 15);
            FilePathLabel.TabIndex = 1;
            FilePathLabel.Text = "FILE PATH: path/to/unit/name.txt";
            // 
            // button7
            // 
            button7.Location = new Point(406, 402);
            button7.Name = "button7";
            button7.Size = new Size(90, 30);
            button7.TabIndex = 0;
            button7.Text = "Next →";
            button7.UseVisualStyleBackColor = true;
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
            ClearLanguagesButton.Location = new Point(187, 288);
            ClearLanguagesButton.Name = "ClearLanguagesButton";
            ClearLanguagesButton.Size = new Size(169, 29);
            ClearLanguagesButton.TabIndex = 6;
            ClearLanguagesButton.Text = "Clear languages";
            toolTip1.SetToolTip(ClearLanguagesButton, "Clears all languages and disabled translation part");
            ClearLanguagesButton.UseVisualStyleBackColor = true;
            ClearLanguagesButton.Click += button6_Click;
            // 
            // OpenExistingFolderButton
            // 
            OpenExistingFolderButton.Location = new Point(362, 288);
            OpenExistingFolderButton.Name = "OpenExistingFolderButton";
            OpenExistingFolderButton.Size = new Size(169, 29);
            OpenExistingFolderButton.TabIndex = 8;
            OpenExistingFolderButton.Text = "Open existing master folder";
            toolTip1.SetToolTip(OpenExistingFolderButton, "Use when you want to continue editing existing translation folder structure");
            OpenExistingFolderButton.UseVisualStyleBackColor = true;
            // 
            // LanguagesLayout
            // 
            LanguagesLayout.Location = new Point(158, 172);
            LanguagesLayout.Name = "LanguagesLayout";
            LanguagesLayout.Size = new Size(667, 41);
            LanguagesLayout.TabIndex = 2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(837, 773);
            Controls.Add(OpenExistingFolderButton);
            Controls.Add(groupBox5);
            Controls.Add(ClearLanguagesButton);
            Controls.Add(CreateFoldersButton);
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(LanguagesLayout);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new Size(853, 812);
            MinimumSize = new Size(853, 812);
            Name = "Form1";
            Text = "BK2 Mod Translator Tool";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            groupBox8.ResumeLayout(false);
            groupBox8.PerformLayout();
            groupBox7.ResumeLayout(false);
            groupBox7.PerformLayout();
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private ToolTip toolTip1;
        private TextBox InputModFolderTextBox;
        private Button button1;
        private FolderBrowserDialog folderBrowserDialog1;
        private GroupBox groupBox2;
        private Button button2;
        private TextBox GameDataInputFolder;
        private FlowLayoutPanel LanguagesLayout;
        private GroupBox groupBox3;
        private TextBox LanguageTextBox;
        private Button button3;
        private GroupBox groupBox4;
        private Button button4;
        private TextBox MasterFolderTextBox;
        private Button CreateFoldersButton;
        private Button ClearLanguagesButton;
        private GroupBox groupBox5;
        private Button button7;
        private Label OriginalTextLabel;
        private Label FilePathLabel;
        private Button button8;
        private GroupBox groupBox7;
        private TextBox textBox2;
        private GroupBox groupBox6;
        private TextBox textBox1;
        private GroupBox groupBox8;
        private TextBox textBox3;
        private Label TextsCountLabel;
        private Button OpenExistingFolderButton;
    }
}
