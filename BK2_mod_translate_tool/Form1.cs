using System.Text;
using System.IO;
using System.Collections.Specialized;

namespace BK2_mod_translate_tool
{
    public partial class Form1 : Form
    {

        #region HELPERS

        static void SetControlGroupEnabled(Control[] controls, bool val)
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
        public List<(string, RichTextBox)> LanguageInputsOrder = new List<(string, RichTextBox)>();
        //public OrderedDictionary LanguageInputs = new OrderedDictionary();

        public void AddLanguageInput(string language, RichTextBox rbt)
        {
            LanguageInputs.Add(language, rbt);
            LanguageInputsOrder.Add((language, rbt));
        }

        public void RemoveLanguageInput(string language)
        {
            LanguageInputs.Remove(language);
            LanguageInputsOrder.RemoveAt(LanguageInputsOrder.FindIndex((v) => v.Item1 == language));
        }

        public void AddLanguageTranslationUI(string language)
        {

            RichTextBox rbt = new RichTextBox();
            rbt.Size = new Size(188 + 10, 79);
            rbt.TabIndex = 0;
            rbt.Text = "";
            rbt.Location = new Point(6, 22);
            rbt.Multiline = true;

            AddLanguageInput(language, rbt);

            GroupBox gb = new GroupBox();
            gb.Size = new Size(200 + 10, 107);
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
                RemoveLanguageInput(language);
            }
        }

        public void IncrementEditingIndex()
        {
            App.Instance.IncrementEditingIndex();
            LanguageInputsOrder.First().Item2.Focus();

        }

        public void DecrementEditingIndex()
        {
            App.Instance.DecrementEditingIndex();
            LanguageInputsOrder.First().Item2.Focus();
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
            IncrementEditingIndex();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DecrementEditingIndex();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //MessageBox.Show("key down");
            if (e.KeyCode == Keys.B && e.Control)
            {
                DecrementEditingIndex();
            }
            if (e.KeyCode == Keys.N && e.Control)
            {
                IncrementEditingIndex();
            }
            if (e.KeyCode == Keys.S && e.Control)
            {
                App.Instance.SaveEdits();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            App.Instance.SaveEditingState();
        }

        private void EditCurrentIndexButton_Click(object sender, EventArgs e)
        {
            var f2 = new Form2(App.Instance.workingData, App.Instance.editingData, false);
            //this.TopMost = false;
            //f2.TopMost = true;
            f2.Init();
            f2.ShowDialog();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            App.Instance.SaveEdits();
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Init();
            form3.ShowDialog();
        }
    }
}
