using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BK2_mod_translate_tool
{
    public class App
    {

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

        HashSet<string> languages = new HashSet<string>();

        public IReadOnlySet<string> Languages => languages;

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
                nb.Click += (s, e) => { RemoveLanguage(language); layout.Controls.Remove(nb); };
                layout.Controls.Add(nb);
            }
        }

        public void RemoveLanguage(string language)
        {
            languages.Remove(language);
        }

        public void ClearLanguages(FlowLayoutPanel layout)
        {
            layout.Controls.Clear();
            languages.Clear();
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
            string[] textFiles = default;
            FolderStructure.Instance.Create(modFolder, dataFolder, masterFolder, ref textFiles);

            MessageBox.Show($"Folder structure created in: \"{masterFolder}\"", "Success");
        }

    }
}
