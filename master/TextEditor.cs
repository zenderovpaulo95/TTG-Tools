using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using TTG_Tools.Texts;
using TTG_Tools.ClassesStructs.Text;

namespace TTG_Tools
{
    public partial class TextEditor : Form
    {
        public TextEditor()
        {
            InitializeComponent();
        }

        //Some common variables
        private static List<CommonText> originalTxts = null;
        private static List<CommonText> translatedTxts = null;

        //For main progress bar
        void ProcessorProgress(int progress)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new ProgressHandler(ProcessorProgress), progress);
            }
            else
            {
                progressBar1.Value = progress;
            }
        }

        //For second progress bar (if user works with several files)
        void ProcessorProgress2(int progress)
        {
            if (progressBar2.InvokeRequired)
            {
                progressBar2.Invoke(new ProgressHandler(ProcessorProgress2), progress);
            }
            else
            {
                progressBar2.Value = progress;
            }
        }

        private void firstFileBtn_Click(object sender, EventArgs e)
        {
            if (mergeSingleRB.Checked)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "All supported files|*.txt;*.tsv|Text files (*.txt)|*.txt|TSV files (*.tsv)|*.tsv";
                ofd.FilterIndex = 0;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    firstFilePath.Text = ofd.FileName;
                }
            }
            else
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();

                if(fbd.ShowDialog() == DialogResult.OK)
                {
                    firstFilePath.Text = fbd.SelectedPath;
                }
            }
        }

        private void secondFileBtn_Click(object sender, EventArgs e)
        {
            if (mergeSingleRB.Checked)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "All supported files|*.txt;*.tsv|Text files (*.txt)|*.txt|TSV files (*.tsv)|*.tsv";
                ofd.FilterIndex = 0;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    secondFilePath.Text = ofd.FileName;
                }
            }
            else
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();

                if(fbd.ShowDialog() == DialogResult.OK)
                {
                    secondFilePath.Text = fbd.SelectedPath;
                }
            }
        }

        private void readyFileBtn_Click(object sender, EventArgs e)
        {
            if (mergeSingleRB.Checked)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "All supported files|*.txt;*.tsv|Text files (*.txt)|*.txt|TSV files (*.tsv)|*.tsv";
                sfd.FilterIndex = 0;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    readyFilePath.Text = sfd.FileName;
                }
            }
            else
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();

                if(fbd.ShowDialog() == DialogResult.OK)
                {
                    readyFilePath.Text = fbd.SelectedPath;
                }
            }
        }

        private void TextEditor_Load(object sender, EventArgs e)
        {
            mergeSingleRB.Checked = true;
            replaceSingleRB.Checked = true;
            sortOriginalRB.Checked = true;
        }

        private void mergeSingleRB_CheckedChanged(object sender, EventArgs e)
        {
            progressBar2.Enabled = mergeSeveralRB.Checked;
            progressBar2.Visible = mergeSeveralRB.Checked;
        }
    }
}
