using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BK2_mod_translate_tool
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public void Init()
        {
            if (App.Instance.workingData == null)
                return;
            var inxs = FilesIndexesList.Columns.Add("Index", 60);
            var paths = FilesIndexesList.Columns.Add("Path", 700);
            for (int i = 0; i < App.Instance.workingData.Files.Length; i++)
            {
                string file = App.Instance.workingData.Files[i];
                ListViewItem item = new ListViewItem(i.ToString());
                item.SubItems.Add(file);
                FilesIndexesList.Items.Add(item);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //searching

        }
    }
}
