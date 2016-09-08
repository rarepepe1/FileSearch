using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.VisualStyles;

namespace FileSearch
{
    public partial class Form1 : Form
    {
        private List<CFile> _fileList = new List<CFile>();
        private int _folderCount;
        private StringBuilder _errorString = new StringBuilder();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog() {Description = "Select your Path: "})
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    textPath.Text = fbd.SelectedPath;
                }
            }
        }

        private void btnClearPath_Click(object sender, EventArgs e)
        {
            textPath.Text = "";
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            dataGridView.Rows.Clear();
            dataGridView.RowCount = 1;
        }
    
        #region Search
        private void btnSearch_Click(object sender, EventArgs e)
        {
            _folderCount = 1;
            string pathString = textPath.Text;
            if (checkBox.Checked)
            {
                SearchWithSubfolders(pathString);
                WriteOnTable(_fileList);
                if (_errorString.Length > 0)
                {
                    MessageBox.Show(_errorString.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                _errorString.Clear();
            }
            else
            {
                Search(pathString);
            }
            _fileList.Clear();
        }

        private void Search(string dir)
        {
            List<CFile> fileList = new List<CFile>();
            string pattern = "*" + textExtension.Text;
            string search = textSearchString.Text;

            try
            {
                foreach (string files in Directory.GetFiles(dir, pattern))
                {
                    var fileName = Path.GetFileName(files);
                    if (fileName != null && fileName.Contains(search))
                    {
                        FileInfo file = new FileInfo(files);
                        if (file.Exists)
                        {
                            fileList.Add(new CFile(file.Name, file.Length, file.Extension, file.CreationTime,
                                file.LastWriteTime));
                        }
                    }
                }
                WriteOnTable(fileList);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SearchWithSubfolders(string dir)
        {
            string pattern = "*" + textExtension.Text;
            string search = textSearchString.Text;

            try
            {
                foreach (string files in Directory.GetFiles(dir, pattern))
                {
                    var fileName = Path.GetFileName(files);
                    if (fileName != null && fileName.ToLower().Contains(search.ToLower()))
                    {
                        FileInfo file = new FileInfo(files);
                        if (file.Exists)
                        {
                            _fileList.Add(new CFile(file.FullName, file.Length, file.Extension, file.CreationTime,
                                file.LastWriteTime));
                        }
                    }
                }
                foreach (var sdir in Directory.GetDirectories(dir))
                {
                    _folderCount++;
                    SearchWithSubfolders(sdir);
                }
            }
            catch (Exception ex)
            {
                _errorString.Append(ex.Message + "\n");
            }
        }

        private void WriteOnTable(List<CFile> fileList)
        {
            if (fileList.Count == 0)
            {
                dataGridView.Rows.Clear();
                dataGridView.RowCount = 1;
                MessageBox.Show("No files found!", "Nothing found!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                dataGridView.RowCount = fileList.Count;
            }
            toolStripStatusLabel1.Text = BuildString(fileList.Count, _folderCount);

            int i = 0;

            foreach (var cFile in fileList)
            {
                dataGridView["Column1", i].Value = cFile.Name;
                dataGridView["Column2", i].Value = cFile.Size.ToString();
                dataGridView["Column3", i].Value = cFile.Type;
                dataGridView["Column4", i].Value = cFile.CreationDate.ToString(CultureInfo.CurrentCulture);
                dataGridView["Column5", i].Value = cFile.LastChanged.ToString(CultureInfo.CurrentCulture);
                i++;
            }

        }

        private string BuildString(int filecount, int folderCount)
        {
            StringBuilder sb = new StringBuilder("Found " + filecount + " ");
            if (filecount == 1)
            {
                sb.Append("File");
            }
            else
            {
                sb.Append("Files");
            }
            sb.Append(" in " + folderCount + " ");
            if (folderCount == 1)
            {
                sb.Append("Folder");
            }
            else
            {
                sb.Append("Folders");
            }
            return sb.ToString();
        }
        #endregion
    }
}
