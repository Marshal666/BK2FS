using System.Text;
using System.IO;

namespace BK2_mod_translate_tool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadPath(InputModFolderTextBox);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadPath(GameDataInputFolder);
        }

        #region HELPERS

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

        #endregion

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
    }
}
