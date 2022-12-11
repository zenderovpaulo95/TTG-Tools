using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using TTG_Tools.Texts;
using TTG_Tools.ClassesStructs.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Security.Cryptography;

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
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                readyFilePath.Text = fbd.SelectedPath;
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

        private void firstDoubledFileBtn_Click(object sender, EventArgs e)
        {
            if (replaceSingleRB.Checked)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "All supported files|*.txt;*.tsv|Text files (*.txt)|*.txt|TSV files (*.tsv)|*.tsv";

                if(ofd.ShowDialog() == DialogResult.OK)
                {
                    firstDoubledFilePath.Text = ofd.FileName;
                }
            }
            else
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();

                if(fbd.ShowDialog() == DialogResult.OK)
                {
                    firstDoubledFilePath.Text = fbd.SelectedPath;
                }
            }
        }

        private void secondDoubledFileBtn_Click(object sender, EventArgs e)
        {
            if (replaceSingleRB.Checked)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "All supported files|*.txt;*.tsv|Text files (*.txt)|*.txt|TSV files (*.tsv)|*.tsv";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    secondDoubledFilePath.Text = ofd.FileName;
                }
            }
            else
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    secondDoubledFilePath.Text = fbd.SelectedPath;
                }
            }
        }

        private void readyDoubledFileBtn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                readyDoubledFilePath.Text = fbd.SelectedPath;
            }
        }

        private List<CommonText> CheckStrings(string checkPath)
        {
            List<CommonText> checkTxts;
            checkTxts = Texts.ReadText.GetStrings(checkPath);
            List<CommonText> duplicatedStrs = new List<CommonText>();

            if ((checkTxts != null) && (checkTxts.Count > 0))
            {
                string firstString = "", secondString = "";
                string firstActorName = "", secondActorName = "";
                bool hasDuplicate;

                progressBar1.Maximum = checkTxts.Count - 1;

                for (int i = 0; i < checkTxts.Count; i++)
                {
                    hasDuplicate = false;
                    firstString = checkTxts[i].actorSpeechOriginal;
                    firstString = Methods.DeleteCommentary(firstString, "[", "]");
                    firstString = Methods.DeleteCommentary(firstString, "{", "}");
                    firstString = System.Text.RegularExpressions.Regex.Replace(firstString, @"[^\w]", "");

                    firstActorName = checkTxts[i].actorName;

                    for (int j = i + 1; j < checkTxts.Count; j++)
                    {
                        secondString = checkTxts[j].actorSpeechOriginal;
                        secondString = Methods.DeleteCommentary(secondString, "[", "]");
                        secondString = Methods.DeleteCommentary(secondString, "{", "}");
                        secondString = System.Text.RegularExpressions.Regex.Replace(secondString, @"[^\w]", "");

                        secondActorName = checkTxts[j].actorName;

                        if ((firstString.ToLower() == secondString.ToLower()) && (firstString != "")
                            && (firstActorName.ToUpper() == secondActorName.ToUpper()))
                        {
                            hasDuplicate = true;
                            CommonText tmpTxt;
                            tmpTxt.isBothSpeeches = false;
                            tmpTxt.strNumber = checkTxts[j].strNumber;
                            tmpTxt.actorName = checkTxts[j].actorName;
                            tmpTxt.actorSpeechOriginal = checkTxts[j].actorSpeechOriginal;
                            tmpTxt.actorSpeechTranslation = checkTxts[j].actorSpeechTranslation;
                            tmpTxt.flags = checkTxts[j].flags;

                            duplicatedStrs.Add(tmpTxt);
                        }
                    }

                    if (hasDuplicate)
                    {
                        CommonText tmpTxt;
                        tmpTxt.isBothSpeeches = false;
                        tmpTxt.strNumber = checkTxts[i].strNumber;
                        tmpTxt.actorName = checkTxts[i].actorName;
                        tmpTxt.actorSpeechOriginal = checkTxts[i].actorSpeechOriginal;
                        tmpTxt.actorSpeechTranslation = checkTxts[i].actorSpeechTranslation;
                        tmpTxt.flags = checkTxts[i].flags;

                        duplicatedStrs.Add(tmpTxt);
                    }

                    ProcessorProgress(i);
                }

                
                checkTxts.Clear();
                GC.Collect();
            }

            return duplicatedStrs;
        }

        private void checkDuplicatedStrsBtn_Click(object sender, EventArgs e)
        {
            string checkPath = firstFilePath.Text;
            bool tmpTSVFormat = MainMenu.settings.tsvFormat;
            List<CommonText> checkTxts;

            if (mergeSingleRB.Checked && File.Exists(checkPath))
            {
                checkTxts = CheckStrings(checkPath);

                if(checkTxts.Count > 0)
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Title = "Save file with duplicated strings";
                    sfd.Filter = "All supported files|*.txt;*.tsv|Text files (*.txt)|*.txt|TSV files (*.tsv)|*.tsv";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        FileInfo fi = new FileInfo(sfd.FileName);
                        MainMenu.settings.tsvFormat = fi.Extension.ToLower() == ".tsv";

                        Texts.SaveText.OldMethod(checkTxts, false, false, sfd.FileName);
                    }
                }
            }
            else if(mergeSeveralRB.Checked && Directory.Exists(checkPath))
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "Set output directory for founded duplicated strings in file";

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    DirectoryInfo di = new DirectoryInfo(checkPath);
                    FileInfo[] fi = di.GetFiles("*.*", SearchOption.AllDirectories);

                    progressBar2.Maximum = fi.Length - 1;

                    for (int i = 0; i < fi.Length; i++)
                    {
                        if (fi[i].Extension.ToLower() == ".tsv" || fi[i].Extension.ToLower() == ".txt")
                        {
                            FileInfo tmpFI = new FileInfo(fi[i].FullName);
                            checkTxts = CheckStrings(fi[i].FullName);

                            Texts.SaveText.OldMethod(checkTxts, false, false, fbd.SelectedPath + "\\" + tmpFI.Name);
                        }

                        ProcessorProgress2(i);
                    }
                }
            }

            MainMenu.settings.tsvFormat = tmpTSVFormat;
        }

        private void mergeBtn_Click(object sender, EventArgs e)
        {
            string originalPath = firstFilePath.Text;
            string translatePath = secondFilePath.Text;
            string readyPath = readyFilePath.Text;

            if(mergeSingleRB.Checked && File.Exists(originalPath)
                && File.Exists(translatePath) && Directory.Exists(readyPath))
            {
                originalTxts = Texts.ReadText.GetStrings(originalPath);
                translatedTxts = Texts.ReadText.GetStrings(translatePath);

                progressBar1.Maximum = originalTxts.Count - 1;
                bool tmpTSV = MainMenu.settings.tsvFormat;

                for(int i = 0; i < originalTxts.Count; i++)
                {
                    CommonText tmpTxt;
                    tmpTxt.isBothSpeeches = originalTxts[i].isBothSpeeches;
                    tmpTxt.strNumber = originalTxts[i].strNumber;
                    tmpTxt.actorName = originalTxts[i].actorName;
                    tmpTxt.actorSpeechOriginal = originalTxts[i].actorSpeechOriginal;
                    tmpTxt.actorSpeechTranslation = originalTxts[i].actorSpeechTranslation;
                    tmpTxt.flags = originalTxts[i].flags;

                    for (int j = 0; j < translatedTxts.Count; j++)
                    {
                        if ((originalTxts[i].strNumber == translatedTxts[j].strNumber)
                            && (originalTxts[i].actorName == translatedTxts[j].actorName))
                        {
                            tmpTxt.actorSpeechTranslation = translatedTxts[j].actorSpeechTranslation;
                            break;
                        }
                    }

                    originalTxts[i] = tmpTxt;

                    ProcessorProgress(i);
                }

                if (sortStrsCB.Checked)
                {
                    CommonTextClass tmp = new CommonTextClass();
                    tmp.txtList = originalTxts;

                    tmp = Methods.SortString(tmp);

                    originalTxts = new List<CommonText>();
                    
                    for(int i = 0; i < tmp.txtList.Count; i++)
                    {
                        originalTxts.Add(tmp.txtList[i]);
                    }

                    tmp.txtList.Clear();
                }

                translatedTxts.Clear();

                FileInfo fi = new FileInfo(originalPath);
                MainMenu.settings.tsvFormat = fi.Extension.ToLower() == ".tsv";
                string newPath = readyPath + "\\" + fi.Name.Remove(fi.Name.Length - fi.Extension.Length, fi.Extension.Length) + "_merged" + fi.Extension;

                Texts.SaveText.OldMethod(originalTxts, true, false, newPath);

                MainMenu.settings.tsvFormat = tmpTSV;
            }
            else if(mergeSeveralRB.Checked && Directory.Exists(originalPath)
                && Directory.Exists(translatePath))
            {

            }
        }

        private List<CommonText> CheckNonTranslateStrs(string filePath)
        {
            List<CommonText> checkTxt = Texts.ReadText.GetStrings(filePath);
            CommonText tmpTxts;
            List<CommonText> nonTranslate = new List<CommonText>();

            progressBar1.Maximum = checkTxt.Count - 1;

            for (int i = 0; i < checkTxt.Count; i++)
            {
                if ((checkTxt[i].actorSpeechOriginal == checkTxt[i].actorSpeechTranslation) && checkTxt[i].isBothSpeeches)
                {
                    tmpTxts.isBothSpeeches = checkTxt[i].isBothSpeeches;
                    tmpTxts.strNumber = checkTxt[i].strNumber;
                    tmpTxts.actorName = checkTxt[i].actorName;
                    tmpTxts.actorSpeechOriginal = checkTxt[i].actorSpeechOriginal;
                    tmpTxts.actorSpeechTranslation = checkTxt[i].actorSpeechTranslation;
                    tmpTxts.flags = checkTxt[i].flags;

                    nonTranslate.Add(tmpTxts);
                }

                ProcessorProgress(i);
            }

            return nonTranslate;
        }

        //Check non-translated strings button in merge tab
        private void button4_Click(object sender, EventArgs e)
        {
            if (mergeSingleRB.Checked)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "Set original + translated file for check non-translated strings";
                ofd.Filter = "All supported files|*.txt;*.tsv|Text files (*.txt)|*.txt|TSV files (*.tsv)|*.tsv";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    originalTxts = CheckNonTranslateStrs(ofd.FileName);

                    if (originalTxts.Count > 0)
                    {
                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.Title = "Save file with non-translated string";
                        sfd.Filter = "All supported files|*.txt;*.tsv|Text files (*.txt)|*.txt|TSV files (*.tsv)|*.tsv";

                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            FileInfo fi = new FileInfo(sfd.FileName);

                            bool tmpTSV = MainMenu.settings.tsvFormat;
                            MainMenu.settings.tsvFormat = fi.Extension.ToLower() == ".tsv";

                            Texts.SaveText.OldMethod(originalTxts, false, false, fi.FullName);

                            MainMenu.settings.tsvFormat = tmpTSV;
                        }
                    }
                }
            }
            else
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "Select folder path with files to check";

                FolderBrowserDialog fbd2 = new FolderBrowserDialog();
                fbd2.Description = "Set folder for non-translated strings in files";

                if ((fbd.ShowDialog() == DialogResult.OK) && (fbd2.ShowDialog() == DialogResult.OK))
                {
                    DirectoryInfo di = new DirectoryInfo(fbd.SelectedPath);
                    FileInfo[] fi = di.GetFiles("*.*", SearchOption.AllDirectories);
                    progressBar2.Maximum = fi.Length - 1;

                    for (int i = 0; i < fi.Length; i++)
                    {
                        if (fi[i].Extension.ToLower() == ".txt" || fi[i].Extension.ToLower() == ".tsv")
                        {
                            originalTxts = CheckNonTranslateStrs(fi[i].FullName);

                            if (originalTxts.Count > 0)
                            {
                                FileInfo tmpFi = new FileInfo(fi[i].FullName);

                                bool tmpTSV = MainMenu.settings.tsvFormat;
                                MainMenu.settings.tsvFormat = tmpFi.Extension.ToLower() == ".tsv";

                                string tmpFilePath = fbd2.SelectedPath + "\\" + tmpFi.Name;

                                Texts.SaveText.OldMethod(originalTxts, false, false, tmpFilePath);
                                MainMenu.settings.tsvFormat = tmpTSV;
                            }
                        }

                        ProcessorProgress2(i);
                    }
                }
            }
        }
    }
}
