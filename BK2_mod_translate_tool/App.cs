using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZipFileSystem;
using System.Text.Json;
using System.Linq;

namespace BK2_mod_translate_tool
{
    public class App
    {

        public static readonly string ConfigFile = "config.json";

        public class EditingData
        {
            public string[] Languages { get; set; }
            public int CurrentFile { get; set; } = 1;
            public string ModFolder { get; set; }
            public string DataFolder { get; set; }
            public string MasterFolder { get; set; }

            public EditingData() { }

            public void SaveToJSON(string path)
            {
                var data = JsonSerializer.Serialize(this);
                File.WriteAllText(path, data);
            }

            public static EditingData LoadFromJSON(string path)
            {
                string data = File.ReadAllText(path);
                var ret = JsonSerializer.Deserialize<EditingData>(data);

                string currentPath = Path.GetDirectoryName(path);
                if (!Path.Exists(ret.MasterFolder) || ret.MasterFolder.FormattedPath().TrimEnd('\\') != currentPath.FormattedPath().TrimEnd('\\'))
                {
                    
                    bool currentPathValid = true;

                    foreach(var lang in ret.Languages)
                    {
                        if (!Path.Exists(Path.Combine(currentPath, lang))) return ret;
                    }

                    if (currentPathValid)
                        ret.MasterFolder = currentPath;
                }

                return ret;
            }

            public EditingData(string[] languages, int currentFile, string modFolder, string dataFolder, string masterFolder)
            {
                Languages = languages;
                CurrentFile = currentFile;
                ModFolder = modFolder;
                DataFolder = dataFolder;
                MasterFolder = masterFolder;
            }
        }

        public EditingData editingData;
        public EditingData editingDataDelta;

        public class WorkingData
        {
            public string[] Files { get; set; }
            public int CurrentFile { get; set; } = 0;

            public string CurrentFilePath => Files[CurrentFile];
        }

        public WorkingData workingData;
        public WorkingData workingDataDelta;

        static App instance;

        static object locker = new object();

        public static App Instance { 
            get {
                if (instance != null)
                    return instance;
                lock (locker)
                {
                    if (instance == null)
                        instance = new App();
                }
                return instance;
            }
        }

        private App() { }

        Form1 form;
        Form3 deltaForm;

        public Form1 Form
        {
            get => form;
            set => form = value;
        }

        public Form3 DeltaForm
        {
            get => deltaForm;
            set => deltaForm = value;
        }

        HashSet<string> languages = new HashSet<string>();
        HashSet<string> languagesDelta = new HashSet<string>();

        public IReadOnlySet<string> Languages => languages;

        public IReadOnlySet<string> LanguagesDelta => languagesDelta;

        public void AddLanguage(string language, FlowLayoutPanel layout, ToolTip tooltip = null)
        {
            if(languages.Add(language))
            {
                Button nb = new Button();
                nb.AutoSize = true;
                if(tooltip != null)
                {
                    tooltip.SetToolTip(nb, $"Remove {language} from the list");
                }
                nb.Text = $"- {language}";
                nb.Click += (s, e) => { RemoveLanguage(language); layout.Controls.Remove(nb); form.RemoveLanguageTranslationUI(language); };
                layout.Controls.Add(nb);
                form.AddLanguageTranslationUI(language);
            }
        }

        public void AddLanguageDelta(string language, FlowLayoutPanel layout, ToolTip tooltip = null)
        {
            if (languagesDelta.Add(language))
            {
                deltaForm.AddLanguageTranslationUI(language);
            }
        }

        public void RemoveLanguage(string language)
        {
            languages.Remove(language);
        }

        public void ClearLanguages(FlowLayoutPanel layout)
        {
            SaveEdits();
            SaveEditingState();
            layout.Controls.Clear();
            form.TranslationLayoutPanel.Controls.Clear();
            form.LanguageInputs.Clear();
            languages.Clear();
            editingData = null;
            form.SwitchToFolderTransfer();
        }

        public void LoadData(string file)
        {
            editingData = EditingData.LoadFromJSON(file);

            foreach (string language in editingData.Languages)
            {
                AddLanguage(language, form.LanguagesLayoutPanel, form.ToolTip);
            }

            form.SwitchToTranslation(editingData.CurrentFile);
        }

        public void LoadDeltaData(string file)
        {
            editingDataDelta = EditingData.LoadFromJSON(file);
            editingDataDelta.CurrentFile = 0;

            foreach(var language in editingDataDelta.Languages)
            {
                AddLanguageDelta(language, deltaForm.TranslatingFlowLayout);
            }

            deltaForm.SwitchToTranslation(editingDataDelta.CurrentFile);
        }

        public void CreateFolderStructure(string modFolder, string dataFolder, string masterFolder)
        {
            if(languages.Count <= 0)
            {
                MessageBox.Show("You need to add at least one language in order to create a folder structure!", "Error");
                return;
            }
            if(!Path.Exists(modFolder) && !Path.Exists(dataFolder)) 
            {
                MessageBox.Show("Input folder(s): mod or data are invalid!", "Error");
                return;
            }
            if(!Utils.IsValidPath(masterFolder))
            {
                MessageBox.Show("Invalid master folder path", "Error");
                return;
            }

            // create folder structure
            // TODO
            string[] textFiles = null;
            VirtualFileSystem fileSystem = null;
            FolderStructure.Instance.Create(modFolder, dataFolder, masterFolder, ref textFiles, ref fileSystem);

            if(textFiles == null || textFiles.Length <= 0) 
            {
                MessageBox.Show("No text files to translate", "Error");
                return;
            }

            CreateFolders(Utils.OriginalFolderName, true);

            foreach(var language in Languages)
            {
                CreateFolders(language);
            }

            form.TranslationButton.Enabled = true;

            editingData = new EditingData(Languages.ToArray(), 0, modFolder, dataFolder, masterFolder);

            editingData.SaveToJSON(Path.Combine(masterFolder, ConfigFile));

            MessageBox.Show($"Folder structure created in: \"{masterFolder}\"", "Success");

            void CreateFolders(string folder, bool keepOriginal=false)
            {
                folder = Path.Combine(masterFolder, folder);
                Directory.CreateDirectory(folder);
                foreach (var file in textFiles)
                {
                    string output = Path.Combine(folder, file);
                    string content = "";
                    if(keepOriginal)
                    {
                        content = fileSystem.ReadTextFile(file);
                    }
                    new FileInfo(output).Directory.Create();
                    File.WriteAllText(output, content, Encoding.Unicode);
                }
            }

        }

        public void LoadFolderFiles()
        {
            if (editingData == null)
                return;
            string path = Path.Combine(editingData.MasterFolder, Utils.OriginalFolderName);
            var files = Directory.EnumerateFiles(path, "*.txt", SearchOption.AllDirectories).ToArray();

            workingData = new WorkingData();

            workingData.CurrentFile = 0;
            workingData.Files = files;
        }

        public void LoadDeltaFolderFiles()
        {
            if (editingDataDelta == null)
                return;
            string path = Path.Combine(editingDataDelta.MasterFolder, Utils.OriginalFolderName);

            var CurrentFiles = new HashSet<string>();
            foreach (var file in workingData.Files)
            {
                CurrentFiles.Add(Utils.RelativePath(editingData.MasterFolder, file).FormattedPath());
            }

            var OldFiles = new HashSet<string>();
            foreach (var file in Directory.EnumerateFiles(path, "*.txt", SearchOption.AllDirectories))
            {
                OldFiles.Add(Utils.RelativePath(editingDataDelta.MasterFolder, file).FormattedPath());
            }

            var DeltaFiles = CurrentFiles.Except(OldFiles).ToArray();
            for (int i = 0; i < DeltaFiles.Length; i++)
                DeltaFiles[i] = Path.Combine(editingData.MasterFolder, DeltaFiles[i]);

            workingDataDelta = new WorkingData();
            workingDataDelta.CurrentFile = 0;
            workingDataDelta.Files = DeltaFiles;
        }

        public void SaveEditingState()
        {
            if(editingData == null) return;
            if(workingData == null) return;
            editingData.CurrentFile = workingData.CurrentFile;
            editingData.SaveToJSON(Path.Combine(editingData.MasterFolder, ConfigFile));
        }

        public void SwitchEditFileAtIndex(int index, bool save = true)
        {
            if(workingData == null) 
                return;
            if (save)
                SaveEdits();
            index = Math.Clamp(index, 0, workingData.Files.Length - 1);
            workingData.CurrentFile = index;
            form.TranslatingFilePathLabel.Text = Path.GetRelativePath(Path.Combine(editingData.MasterFolder, Utils.OriginalFolderName), workingData.Files[index]);
            form.TranslatingFileTextBox.Text = File.ReadAllText(workingData.Files[index]);
            form.UpdateCountLabel();
            LoadFileStates();
        }

        public void SwitchEditFileAtIndexDelta(int index, bool save = true)
        {
            if (workingDataDelta == null)
                return;
            if (save)
                SaveEditsDelta();
            index = Math.Clamp(index, 0, workingDataDelta.Files.Length - 1);
            workingDataDelta.CurrentFile = index;
            deltaForm.TranslatingFilePathLabel.Text = workingDataDelta.Files[index];
            deltaForm.TranslatingFileTextBox.Text = File.ReadAllText(workingDataDelta.Files[index]);
            deltaForm.UpdateCountLabel();
            LoadFileStatesDelta();
        }

        public void IncrementEditingIndex()
        {
            if (workingData == null)
                return;
            SwitchEditFileAtIndex(workingData.CurrentFile + 1);
        }

        public void DecrementEditingIndex()
        {
            if (workingData == null)
                return;
            SwitchEditFileAtIndex(workingData.CurrentFile - 1);
        }

        public void IncrementEditingIndexDelta()
        {
            if (workingDataDelta == null)
                return;
            SwitchEditFileAtIndexDelta(workingDataDelta.CurrentFile + 1);
        }

        public void DecrementEditingIndexDelta()
        {
            if (workingDataDelta == null)
                return;
            SwitchEditFileAtIndexDelta(workingDataDelta.CurrentFile - 1);
        }

        public void SaveEdits()
        {
            var inputs = form.LanguageInputs;
            if(inputs.Count != editingData.Languages.Length)
            {
                MessageBox.Show("Shit happened.. bruh", "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            string fp = Path.GetRelativePath(Path.Combine(editingData.MasterFolder, Utils.OriginalFolderName), workingData.CurrentFilePath);
            foreach (var lang in inputs.Keys)
            {
                string path = Path.Combine(editingData.MasterFolder, lang, fp);
                string data = File.ReadAllText(path);
                if (inputs[lang].Text != data)
                    File.WriteAllText(path, inputs[lang].Text, Encoding.Unicode);
            }
        }

        public void SaveEditsDelta()
        {
            var inputs = deltaForm.LanguageInputs;
            if (inputs.Count != editingDataDelta.Languages.Length)
            {
                MessageBox.Show("Shit happened.. bruh", "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            string fp = Path.GetRelativePath(Path.Combine(editingData.MasterFolder, Utils.OriginalFolderName), workingDataDelta.CurrentFilePath);
            foreach (var lang in inputs.Keys)
            {
                string path = Path.Combine(editingData.MasterFolder, lang, fp);
                string data = File.ReadAllText(path);
                if (inputs[lang].Text != data)
                    File.WriteAllText(path, inputs[lang].Text, Encoding.Unicode);
            }
        }

        public void LoadFileStates()
        {
            var inputs = form.LanguageInputs;
            if (inputs.Count != editingData.Languages.Length)
            {
                MessageBox.Show("Something went wrong.. bruh", "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            string fp = Path.GetRelativePath(Path.Combine(editingData.MasterFolder, Utils.OriginalFolderName), workingData.CurrentFilePath);
            foreach (var lang in inputs.Keys)
            {
                string path = Path.Combine(editingData.MasterFolder, lang, fp);
                string data = File.ReadAllText(path);
                inputs[lang].Text = data;
            }
        }

        public void LoadFileStatesDelta()
        {
            var inputs = deltaForm.LanguageInputs;
            if (inputs.Count != editingDataDelta.Languages.Length)
            {
                MessageBox.Show("Something went wrong.. bruh", "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            string fp = Path.GetRelativePath(Path.Combine(editingData.MasterFolder, Utils.OriginalFolderName), workingDataDelta.CurrentFilePath);
            foreach (var lang in inputs.Keys)
            {
                string path = Path.Combine(editingData.MasterFolder, lang, fp);
                string data = File.ReadAllText(path);
                inputs[lang].Text = data;
            }
        }

        public void ClearDeltaData(bool cleanForm)
        {
            editingDataDelta = null;
            workingDataDelta = null;
            languagesDelta.Clear();
            if (cleanForm)
                deltaForm = null;
        }

    }
}
