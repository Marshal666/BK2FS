namespace BK2_mod_translate_tool
{
    partial class Form3
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            TranslatingGroupBox = new GroupBox();
            button5 = new Button();
            EditCurrentIndexButton = new Button();
            TranslatingFlowUIPanel = new FlowLayoutPanel();
            OriginalTextTextBox = new RichTextBox();
            TextsCountLabel = new Label();
            button8 = new Button();
            FilePathLabel = new Label();
            button7 = new Button();
            button1 = new Button();
            DiffProjectFolderName = new Label();
            folderBrowserDialog1 = new FolderBrowserDialog();
            TranslatingGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // TranslatingGroupBox
            // 
            TranslatingGroupBox.Controls.Add(button5);
            TranslatingGroupBox.Controls.Add(EditCurrentIndexButton);
            TranslatingGroupBox.Controls.Add(TranslatingFlowUIPanel);
            TranslatingGroupBox.Controls.Add(OriginalTextTextBox);
            TranslatingGroupBox.Controls.Add(TextsCountLabel);
            TranslatingGroupBox.Controls.Add(button8);
            TranslatingGroupBox.Controls.Add(FilePathLabel);
            TranslatingGroupBox.Controls.Add(button7);
            TranslatingGroupBox.Dock = DockStyle.Bottom;
            TranslatingGroupBox.Enabled = false;
            TranslatingGroupBox.Location = new Point(0, 52);
            TranslatingGroupBox.Name = "TranslatingGroupBox";
            TranslatingGroupBox.Size = new Size(845, 438);
            TranslatingGroupBox.TabIndex = 8;
            TranslatingGroupBox.TabStop = false;
            TranslatingGroupBox.Text = "Translating";
            // 
            // button5
            // 
            button5.Location = new Point(741, 402);
            button5.Name = "button5";
            button5.Size = new Size(90, 30);
            button5.TabIndex = 13;
            button5.Text = "Save Edits";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // EditCurrentIndexButton
            // 
            EditCurrentIndexButton.Location = new Point(6, 28);
            EditCurrentIndexButton.Name = "EditCurrentIndexButton";
            EditCurrentIndexButton.Size = new Size(47, 22);
            EditCurrentIndexButton.TabIndex = 12;
            EditCurrentIndexButton.Text = "...";
            EditCurrentIndexButton.UseVisualStyleBackColor = true;
            // 
            // TranslatingFlowUIPanel
            // 
            TranslatingFlowUIPanel.Location = new Point(6, 168);
            TranslatingFlowUIPanel.Name = "TranslatingFlowUIPanel";
            TranslatingFlowUIPanel.Size = new Size(825, 228);
            TranslatingFlowUIPanel.TabIndex = 11;
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
            // 
            // TextsCountLabel
            // 
            TextsCountLabel.AutoSize = true;
            TextsCountLabel.Location = new Point(668, 32);
            TextsCountLabel.Name = "TextsCountLabel";
            TextsCountLabel.Size = new Size(163, 15);
            TextsCountLabel.TabIndex = 7;
            TextsCountLabel.Text = "DIFF TEXTS COUNT: ????/?????";
            // 
            // button8
            // 
            button8.Location = new Point(329, 402);
            button8.Name = "button8";
            button8.Size = new Size(90, 30);
            button8.TabIndex = 3;
            button8.Text = "← Previous";
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
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // button1
            // 
            button1.Location = new Point(12, 12);
            button1.Name = "button1";
            button1.Size = new Size(128, 27);
            button1.TabIndex = 9;
            button1.Text = "Load Old Project";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // DiffProjectFolderName
            // 
            DiffProjectFolderName.AutoSize = true;
            DiffProjectFolderName.Location = new Point(146, 18);
            DiffProjectFolderName.Name = "DiffProjectFolderName";
            DiffProjectFolderName.Size = new Size(135, 15);
            DiffProjectFolderName.TabIndex = 10;
            DiffProjectFolderName.Text = "Diff/Project/Folder/Path";
            // 
            // Form3
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(845, 490);
            Controls.Add(DiffProjectFolderName);
            Controls.Add(button1);
            Controls.Add(TranslatingGroupBox);
            Name = "Form3";
            Text = "Diff Mode";
            KeyDown += Form3_KeyDown;
            TranslatingGroupBox.ResumeLayout(false);
            TranslatingGroupBox.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox TranslatingGroupBox;
        private Button button5;
        private Button EditCurrentIndexButton;
        private FlowLayoutPanel TranslatingFlowUIPanel;
        private RichTextBox OriginalTextTextBox;
        private Label TextsCountLabel;
        private Button button8;
        private Label FilePathLabel;
        private Button button7;
        private Button button1;
        private Label DiffProjectFolderName;
        private FolderBrowserDialog folderBrowserDialog1;
    }
}