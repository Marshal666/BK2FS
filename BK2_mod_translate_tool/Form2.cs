using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DuoVia.FuzzyStrings;

namespace BK2_mod_translate_tool
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        #region HELPERS

        public void SetFileIndexes((int inx, string file)[] fileIndexes)
        {
            FilesIndexesList.Items.Clear();
            for (int i = 0; i < fileIndexes.Length; i++)
            {
                ListViewItem item = new ListViewItem(fileIndexes[i].inx.ToString());
                item.SubItems.Add(fileIndexes[i].file);
                FilesIndexesList.Items.Add(item);
            }
        }

        public IEnumerable<(int, string)> IndexStringArray(string[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                yield return (i, arr[i]);
            }
        }

        #endregion

        (int, string)[] files;

        public void Init()
        {
            if (App.Instance.workingData == null)
                return;

            var c1 = FilesIndexesList.Columns.Add("Index", 60);
            var c2 = FilesIndexesList.Columns.Add("Path", 700);

            files = IndexStringArray(App.Instance.workingData.Files).ToArray();
            SetFileIndexes(files);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //searching
            var results = SearchFor(textBox1.Text, App.Instance.workingData.Files, 16);
            SetFileIndexes(results);
        }

        public (int, string)[] SearchFor(string search, string[] list, int count)
        {
            if (list == null || list.Length <= 0)
                return new (int, string)[0];
            int retCount = Math.Min(list.Length, count);
            (int, string)[] ret = new (int, string)[retCount];
            (int inx, double similarity)[] Similarities = new (int inx, double similarity)[list.Length];
            for (int i = 0; i < list.Length; i++)
            {
                Similarities[i] = (i, search.FuzzyMatch(list[i]));
            }
            Array.Sort(Similarities, (x, y) => -x.similarity.CompareTo(y.similarity));
            for (int i = 0; i < retCount; i++)
            {
                ret[i] = (Similarities[i].inx, list[Similarities[i].inx]);
            }
            return ret;
        }

        private void ClearResultsButton_Click(object sender, EventArgs e)
        {
            SetFileIndexes(files);
            textBox1.Text = string.Empty;
        }

        private void FilesIndexesList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem? item = FilesIndexesList.GetItemAt(e.X, e.Y);

            if (item == null)
                return;

            if (int.TryParse(item.Text, out int val))
            {
                App.Instance.SwitchEditFileAtIndex(val);
            }
        }

        private void FilesIndexesList_ItemActivate(object sender, EventArgs e)
        {
            
            ListViewItem? item = FilesIndexesList.SelectedItems.Count > 0 ? FilesIndexesList.SelectedItems[0] : null;

            if (item == null)
                return;

            if (int.TryParse(item.Text, out int val))
            {
                App.Instance.SwitchEditFileAtIndex(val);
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
