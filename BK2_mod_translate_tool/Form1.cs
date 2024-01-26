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

        void UpdateCountLabel()
        {
            if (App.Instance.data != null)
            {
                int count = Directory.EnumerateFiles(
                    Path.Combine(App.Instance.data.MasterFolder, Utils.OriginalFolderName), "*.txt", SearchOption.AllDirectories
                    ).Count();
                TextsCountLabel.Text = $"TEXTS COUNT: {App.Instance.data.CurrentFile}/{count}";
            }
        }

        public void SwitchToTranslation()
        {
            SetControlGroupEnabled(EarlyInputControls, false);
            TranslatingGroupBox.Enabled = true;
        }

        public void SwitchToFolderTransfer()
        {
            SetControlGroupEnabled(EarlyInputControls, true);
            TranslatingGroupBox.Enabled = false;
        }

        public void AddLanguageTranslationUI(string language)
        {
            //// 
            //// groupBox6
            //// 
            //groupBox6.Controls.Add(richTextBox1);
            //groupBox6.Location = new Point(6, 168);
            //groupBox6.Name = "groupBox6";
            //groupBox6.Size = new Size(200, 107);
            //groupBox6.TabIndex = 4;
            //groupBox6.TabStop = false;
            //groupBox6.Text = "German";
            //// 
            //// richTextBox1
            //// 
            //richTextBox1.Location = new Point(6, 22);
            //richTextBox1.Name = "richTextBox1";
            //richTextBox1.Size = new Size(188, 79);
            //richTextBox1.TabIndex = 0;
            //richTextBox1.Text = "New text translation";

        }

        #endregion

        Control[] EarlyInputControls;

        public Button TranslationButton => StartTranslatingButton;

        public FlowLayoutPanel LanguagesLayoutPanel => LanguagesLayout;

        public ToolTip ToolTip => toolTip1;

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
            if(res == DialogResult.OK)
            {
                string path = Path.Combine(folderBrowserDialog1.SelectedPath, App.ConfigFile);
                if(File.Exists(path))
                {
                    App.Instance.LoadData(path);
                }
            }
        }
    }
}
