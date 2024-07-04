using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace BK2_mod_translate_tool
{
    public partial class Form3 : Form
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

        void LoadPath(Label label)
        {
            var res = folderBrowserDialog1.ShowDialog();
            if (res == DialogResult.OK)
                label.Text = folderBrowserDialog1.SelectedPath;
        }

        public void UpdateCountLabel()
        {
            if (App.Instance.editingDataDelta != null)
            {
                TextsCountLabel.Text = $"TEXTS COUNT: {App.Instance.workingDataDelta.CurrentFile + 1}/{App.Instance.workingDataDelta.Files.Length}";
            }
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

        public void ClearWorkingData(bool formEnd = false)
        {
            App.Instance.ClearDeltaData(formEnd);
            TranslatingFlowLayout.Controls.Clear();
            LanguageInputs.Clear();
            LanguageInputsOrder.Clear();
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
            App.Instance.IncrementEditingIndexDelta();
            LanguageInputsOrder.First().Item2.Focus();

        }

        public void DecrementEditingIndex()
        {
            App.Instance.DecrementEditingIndexDelta();
            LanguageInputsOrder.First().Item2.Focus();
        }

        #endregion

        public Label TranslatingFilePathLabel => FilePathLabel;

        public RichTextBox TranslatingFileTextBox => OriginalTextTextBox;

        public FlowLayoutPanel TranslatingFlowLayout => TranslatingFlowUIPanel;

        public Form3()
        {
            InitializeComponent();
            EditingControls = [TranslatingGroupBox];
            LoadDeltaControls = [button1];

            foreach (Control control in EditingControls) control.Enabled = false;

            this.FormClosed += OnClose;
        }

        private void OnClose(object? sender, FormClosedEventArgs e)
        {
            ClearWorkingData(true);
        }

        public void Init()
        {
            App.Instance.DeltaForm = this;
        }

        public Control[] EditingControls;
        public Control[] LoadDeltaControls;


        private void button1_Click(object sender, EventArgs e)
        {
            var res = folderBrowserDialog1.ShowDialog();

            if (res != DialogResult.OK)
                return;

            string path = Path.Combine(folderBrowserDialog1.SelectedPath, App.ConfigFile);

            if (!File.Exists(path))
                return;
            
            App.Instance.LoadDeltaData(path);
            if (App.Instance.editingDataDelta != null)
            {
                DiffProjectFolderName.Text = App.Instance.editingDataDelta.MasterFolder;
            } else
            {
                ClearWorkingData();
            }
            
        }

        public void SwitchToTranslation(int currentFile)
        {
            App.Instance.LoadDeltaFolderFiles();
            if (App.Instance.workingDataDelta.Files.Length < 1)
            {
                MessageBox.Show("No delta files to edit!", "Warning");
                ClearWorkingData();
                return;
            }
            SetControlGroupEnabled(EditingControls, true);
            SetControlGroupEnabled(LoadDeltaControls, false);
            App.Instance.SwitchEditFileAtIndexDelta(currentFile, false);
        }

        private void Form3_KeyDown(object sender, KeyEventArgs e)
        {
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
                App.Instance.SaveEditsDelta();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            App.Instance.SaveEditsDelta();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            App.Instance.IncrementEditingIndexDelta();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            App.Instance.DecrementEditingIndexDelta();
        }

        private void EditCurrentIndexButton_Click(object sender, EventArgs e)
        {
            var f2 = new Form2(App.Instance.workingDataDelta, App.Instance.editingData, true);
            //this.TopMost = false;
            //f2.TopMost = true;
            f2.Init();
            f2.ShowDialog();
        }
    }
}
