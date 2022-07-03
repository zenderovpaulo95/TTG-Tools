using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TTG_Tools
{
    public partial class TextCollector : Form
    {
        public class TXT_collection
        {
            public int number;
            public uint realId;
            public string name;
            public string text;
            public bool exported;
            public TXT_collection() { }
            public TXT_collection(int _number, uint _realId, string _name, string _text, bool _exported)
            {
                this.realId = _realId;
                this.name = _name;
                this.number = _number;
                this.text = _text;
                this.exported = _exported;
            }
        }

        public TextCollector()
        {
            InitializeComponent();
        }
        public static OpenFileDialog ofdOrig = new OpenFileDialog();

        private void buttonOpenOrig_Click(object sender, EventArgs e)
        {
            ofdOrig = new OpenFileDialog();
            ofdOrig.Filter = "txt files (*.txt)|*.txt";
            ofdOrig.Title = "Set Original file!";
            if (ofdOrig.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = ofdOrig.FileName.ToString();
                if (textBox2.Text == "")
                {
                    buttonOpenTranslated_Click(sender, e);
                }
            }
        }

        private void buttonOpenTranslated_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdTransl = new OpenFileDialog();
            ofdTransl.Filter = "text files (*.txt)|*.txt";
            ofdTransl.Title = "Set Translated file!";
            if (ofdTransl.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = ofdTransl.FileName.ToString();
                if (textBox3.Text == "")
                {
                    buttonOpenOutput_Click(sender, e);
                }
            }
        }

        private void buttonOpenOutput_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfdOutput = new SaveFileDialog();
            sfdOutput.Filter = "text files (*.txt)|*.txt";
            sfdOutput.Title = "Set Output file!";
            sfdOutput.FileName = Methods.GetNameOfFileOnly(ofdOrig.SafeFileName.ToString(), ".txt") + "(all).txt";
            if (sfdOutput.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = sfdOutput.FileName.ToString();
                if (System.IO.File.Exists(textBox1.Text) && System.IO.File.Exists(textBox2.Text))
                {
                    buttonDoItWithOne_Click(sender, e);
                }
            }
        }

        public List<TXT_collection> sortList(List<TXT_collection> array)
        {
            bool bSort = false; ;

            do
            {
                bSort = false;
                for (int i = 1; i < array.Count; i++)
                {
                    if (array[i].number < array[i - 1].number)
                    {
                        int c = array[i].number;
                        string name = array[i].name;
                        string text = array[i].text;

                        array[i].number = array[i - 1].number;
                        array[i].text = array[i - 1].text;
                        array[i].name = array[i - 1].name;

                        array[i - 1].text = text;
                        array[i - 1].name = name;
                        array[i - 1].number = c;
                        bSort = true;
                    }
                }
            } while (bSort);
            return array;
        }

        private void buttonDoItWithOne_Click(object sender, EventArgs e)
        {
            List<TXT_collection> originalTxt = new List<TXT_collection>();
            List<TXT_collection> translatedTxt = new List<TXT_collection>();
            List<TXT_collection> exportingText = new List<TXT_collection>();

            if (System.IO.File.Exists(textBox1.Text) && System.IO.File.Exists(textBox2.Text))
            {
                string error = string.Empty;
                originalTxt = AutoPacker.ImportTXT(textBox1.Text, ref originalTxt, false, MainMenu.settings.ASCII_N, "\r\n", ref error);
                translatedTxt = AutoPacker.ImportTXT(textBox2.Text, ref translatedTxt, false, MainMenu.settings.ASCII_N, "\r\n", ref error);

                Methods.DeleteCurrentFile(textBox3.Text);
                FileStream ExportStream = new FileStream(textBox3.Text, FileMode.Create);

                originalTxt = sortList(originalTxt);
                translatedTxt = sortList(translatedTxt);


                exportingText = CreateExportingTXTfromTwoFiles(originalTxt, translatedTxt, exportingText);

                for (int i = 0; i < exportingText.Count; i++)
                {
                    SaveString(ExportStream, (exportingText[i].number + ") " + exportingText[i].name + "\r\n"), MainMenu.settings.ASCII_N);
                    SaveString(ExportStream, (exportingText[i].text + "\r\n"), MainMenu.settings.ASCII_N);
                }
                ExportStream.Close();
                MessageBox.Show(originalTxt.Count.ToString() + " " + translatedTxt.Count.ToString() + " " + exportingText.Count.ToString());
            }
            else if (System.IO.File.Exists(textBox1.Text) == false)
            { MessageBox.Show("Cann't find original text! Check path."); }
            else if (System.IO.File.Exists(textBox2.Text) == false)
            { MessageBox.Show("Cann't find translated text! Check path."); }
        }
        //----------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------
        public static char[] abc_eng = new char[255];//массив английских букв
        public static char[] abc_rus = new char[255];//массив русских букв
        public static int abc_count = 0;

        public static string Translator(char[] abc_old, char[] abc_new, string str)
        {
            char[] chars = str.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] == Convert.ToChar("["))
                {
                    while (chars[i] != Convert.ToChar("]"))
                    {
                        i++;
                    }
                    i++;
                }
                if (chars[i] == Convert.ToChar("{"))
                {
                    while (chars[i] != Convert.ToChar("}"))
                    {
                        i++;
                    }
                    i++;
                }
                for (int z = 0; z < abc_count; z++)
                {
                    if (chars[i] == abc_old[z])
                    {
                        chars[i] = abc_new[z];
                        break;

                    }

                }
            }
            //for (int z = 0; z < abc_number; z++)
            //{
            //    str = str.Replace(abc_old[z], abc_new[z]);
            //}
            return new String(chars);
        }

        public static bool IsStringsSame(string first, string second, bool isComplytelySame)
        {
            //MessageBox.Show(DeleteCommentsAndOther(first) + "\r\n\r\n" + DeleteCommentsAndOther(second));
            if (isComplytelySame == true)
            {
                if (first == second)
                { return true; }
                else
                { return false; }
            }
            else
            {
                if (DeleteCommentsAndOther(first) == DeleteCommentsAndOther(second))
                { return true; }
                else { return false; }
            }
        }

        public static int textLenght(string s)
        {
            int i = 0;
            try
            {
                i = s.Length;
            }
            catch { i = 0; }
            return i;
        }

        public static void SaveString(FileStream MyFileStream, string data, int UnicodeMode)
        {
            if (textLenght(data) != 0)
            {
                byte[] hexData = new byte[data.Length];
                switch(UnicodeMode)
                {
                    case 0:
                        hexData = (byte[])UnicodeEncoding.UTF8.GetBytes(data);
                        break;
                    case 2:
                        hexData = (byte[])Encoding.UTF8.GetBytes(data);
                        hexData = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(1252), hexData);
                        hexData = Encoding.Convert(Encoding.GetEncoding(MainMenu.settings.ASCII_N), Encoding.UTF8, hexData);
                        break;
                    default:
                        hexData = (byte[])UnicodeEncoding.UTF8.GetBytes(data);
                        break;
                }
                    
                
                
                MyFileStream.Write(hexData, 0, hexData.Length);
            }
        }

        

        public static string DeleteCommentsAndOther(string str)
        {
            if (str != null && str != string.Empty)
            {
                str = str.ToUpper();
                str = str.Replace(".", string.Empty);
                str = str.Replace(" ", string.Empty);
                str = str.Replace("!", string.Empty);
                str = str.Replace("?", string.Empty);
                str = str.Replace("-", string.Empty);
                str = str.Replace(",", string.Empty);
                str = str.Replace(":", string.Empty);
                str = str.Replace("'", string.Empty);
                str = str.Replace("\"", string.Empty);

                str = Methods.DeleteCommentary(str, "[", "]");
                str = Methods.DeleteCommentary(str, "{", "}");
            }
            //MessageBox.Show(str + "\n\r \n\r" + new_str);
            return str;
        }



        public static List<TXT_collection> CreateExportingTXTfromTwoFiles(List<TXT_collection> txt_orig, List<TXT_collection> txt_trans, List<TXT_collection> txt_export)
        {
            txt_export.Clear();
            for (int i = 0; i < txt_orig.Count; i++)
            {
                if (txt_orig[i].exported == false)
                {
                    txt_export.Add(txt_orig[i]);
                    txt_export.Add(txt_trans[i]);
                    txt_orig[i].exported = true;
                    txt_trans[i].exported = true;

                    for (int j = i + 1; j < txt_trans.Count; j++)
                    {
                        if (IsStringsSame(txt_orig[i].text, txt_orig[j].text, false) == true)
                        {
                            txt_export.Add(txt_orig[j]);
                            txt_export.Add(txt_trans[j]);
                            txt_orig[j].exported = true;
                            txt_trans[j].exported = true;
                        }
                    }
                }
            }
            return txt_export;
        }

        public static List<TXT_collection> CreateExportingTXTfromOneFile(List<TXT_collection> txt, ref List<TXT_collection> txt_export)
        {
            if (MainMenu.settings.sortSameString)
            {
                txt_export.Clear();
                for (int i = 0; i < txt.Count; i++)
                {
                    if (txt[i].text == null)
                    {
                        txt[i].exported = true;
                    }
                }
                for (int i = 0; i < txt.Count; i++)
                {
                    if (txt[i].exported == false)
                    {
                        txt_export.Add(txt[i]);
                        txt[i].exported = true;
                        for (int j = i + 1; j < txt.Count; j++)
                        {
                            if (txt[j].exported == false)
                            {
                                if (IsStringsSame(txt[i].text, txt[j].text, false) == true)
                                {
                                    txt_export.Add(txt[j]);
                                    txt[j].exported = true;
                                }
                            }
                        }
                    }
                }
                return txt_export;
            }
            else
            {
                txt_export = txt;
                return txt_export;
            }
        }

        private void buttonOpenOrigFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbdOrig = new FolderBrowserDialog();
            fbdOrig.ShowNewFolderButton = false;
            if (fbdOrig.ShowDialog() == DialogResult.OK)
            {
                textBox6.Text = fbdOrig.SelectedPath.ToString();
                if (textBox5.Text == "")
                {
                    buttonOpenTranslFolder_Click(sender, e);
                }
            }
        }
        private void buttonOpenTranslFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbdTransl = new FolderBrowserDialog();
            fbdTransl.ShowNewFolderButton = false;
            if (fbdTransl.ShowDialog() == DialogResult.OK)
            {
                textBox5.Text = fbdTransl.SelectedPath.ToString();
                if (textBox4.Text == "")
                {
                    buttonOpenOutputFolder_Click(sender, e);
                }
            }
        }

        private void buttonOpenOutputFolder_Click(object sender, EventArgs e)
        {

        }

        private void buttonDoItWithAll_Click(object sender, EventArgs e)
        {

        }

        private void button_Clear_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void TextCollector_Load(object sender, EventArgs e)
        {
            groupBox2.Enabled = false;
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }




    }
}
