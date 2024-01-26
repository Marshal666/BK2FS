using System.Text;
using System.IO;

namespace BK2_mod_translate_tool
{
    public partial class Form1 : Form
    {

        #region HELPERS

        void SetControlGroupEnabled(Control[] controls, bool val)
        {
            if (controls == null || controls.Length == 0)
                return;
            foreach (Control control in controls)
            {
                control.Enabled = val;
            }
        }

        void LoadPath(TextBox box)
        {
            var res = folderBrowserDialog1.ShowDialog();
            if (res == DialogResult.OK)
                box.Text = folderBrowserDialog1.SelectedPath;
        }

        void AddLanguage(string language)
        {
            if (!Utils.IsValidDirectoryName(language))
            {
                MessageBox.Show($"\"{language}\" is invalid path name!", "Error");
                return;
            }
            //MessageBox.Show($"Added {LanguageTextBox.Text}");
            App.Instance.AddLanguage(language, LanguagesLayout, toolTip1);
        }

        public void UpdateCountLabel()
        {
            if (App.Instance.editingData != null)
            {
                TextsCountLabel.Text = $"TEXTS COUNT: {App.Instance.workingData.CurrentFile + 1}/{App.Instance.workingData.Files.Length}";
            }
        }

        public void SwitchToTranslation(int currentFile = 0)
        {
            SetControlGroupEnabled(EarlyInputControls, false);
            TranslatingGroupBox.Enabled = true;
            CreateFoldersButton.Enabled = false;
            App.Instance.LoadFolderFiles();
            App.Instance.SwitchEditFileAtIndex(currentFile, false);
        }

        public void SwitchToFolderTransfer()
        {
            SetControlGroupEnabled(EarlyInputControls, true);
            TranslatingGroupBox.Enabled = false;
            TranslationButton.Enabled = false;
            CreateFoldersButton.Enabled = true;
        }

        public Dictionary<string, RichTextBox> LanguageInputs = new Dictionary<string, RichTextBox>();

        public void AddLanguageTranslationUI(string language)
        {

            RichTextBox rbt = new RichTextBox();
            rbt.Size = new Size(188, 79);
            rbt.TabIndex = 0;
            rbt.Text = "";
            rbt.Location = new Point(6, 22);

            LanguageInputs.Add(language, rbt);

            GroupBox gb = new GroupBox();
            gb.Size = new Size(200, 107);
            gb.TabIndex = 4;
            gb.TabStop = false;
            gb.Text = language;
            gb.Controls.Add(rbt);

            TranslatingFlowUIPanel.Controls.Add(gb);

        }

        public void RemoveLanguageTranslationUI(string language)
        {
            GroupBox gb = null;
            foreach (var control in TranslatingFlowUIPanel.Controls)
            {
                if (control is GroupBox box && box.Text == language)
                {
                    gb = box;
                    break;
                }
            }
            if (gb != null)
            {
                TranslatingFlowUIPanel.Controls.Remove(gb);
                LanguageInputs.Remove(language);
            }
        }

        #endregion

        Control[] EarlyInputControls;

        public Button TranslationButton => StartTranslatingButton;

        public FlowLayoutPanel LanguagesLayoutPanel => LanguagesLayout;

        public FlowLayoutPanel TranslationLayoutPanel => TranslatingFlowUIPanel;

        public ToolTip ToolTip => toolTip1;

        public Label TranslatingFilePathLabel => FilePathLabel;

        public RichTextBox TranslatingFileTextBox => OriginalTextTextBox;

        public Form1()
        {
            InitializeComponent();

            EarlyInputControls =
                [
                    InputModGroupBox,
                    GameDataGroupBox,
                    AddLanguagesGroupBox,
                    LanguagesLayout,
                    OutputMasterGroupBox
                ];

            App.Instance.Form = this;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadPath(InputModFolderTextBox);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadPath(GameDataInputFolder);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddLanguage(LanguageTextBox.Text);
        }

        private void LanguageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddLanguage(LanguageTextBox.Text);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadPath(MasterFolderTextBox);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            App.Instance.ClearLanguages(LanguagesLayout);
        }

        private void CreateFoldersButton_Click(object sender, EventArgs e)
        {
            App.Instance.CreateFolderStructure(InputModFolderTextBox.Text, GameDataInputFolder.Text, MasterFolderTextBox.Text);
        }

        private void StartTranslatingButton_Click(object sender, EventArgs e)
        {
            SwitchToTranslation();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            UpdateCountLabel();
        }

        private void OpenExistingFolderButton_Click(object sender, EventArgs e)
        {
            var res = folderBrowserDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                string path = Path.Combine(folderBrowserDialog1.SelectedPath, App.ConfigFile);
                if (File.Exists(path))
                {
                    App.Instance.LoadData(path);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            App.Instance.IncrementEditingIndex();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            App.Instance.DecrementEditingIndex();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //MessageBox.Show("key down");
            if (e.KeyCode == Keys.Y && e.Control)
            {
                App.Instance.DecrementEditingIndex();
            }
            if (e.KeyCode == Keys.X && e.Control)
            {
                App.Instance.IncrementEditingIndex();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            App.Instance.SaveEditingState();
        }

        private void EditCurrentIndexButton_Click(object sender, EventArgs e)
        {
            var f2 = new Form2();
            //this.TopMost = false;
            //f2.TopMost = true;
            f2.Init();
            f2.ShowDialog();
        }
    }
}
