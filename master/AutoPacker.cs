using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using BlowFishCS;

namespace TTG_Tools
{
    public partial class AutoPacker : Form
    {
        public AutoPacker()
        {
            InitializeComponent();
        }

        public static FileInfo[] fi;
        public static FileInfo[] fi_temp;

        public static int numKey;
        public static int selected_index;
        public static int EncVersion;

        Thread threadExport;
        Thread threadImport; 

        public struct langdb
        {
            public byte[] head;
            public byte[] hz_data;
            public byte[] lenght_of_name;
            public string name;
            public byte[] lenght_of_text;
            public string text;
            public byte[] lenght_of_waw;
            public string waw;
            public byte[] lenght_of_animation;
            public string animation;
            public byte[] magic_bytes;
            public byte[] realID;
        }


        public static int number;
        langdb[] database = new langdb[5000];

        void AddNewReport(string report)
        {
            if (listBox1.InvokeRequired)
            {
                listBox1.Invoke(new ReportHandler(AddNewReport), report);
            }
            else
            {
                listBox1.Items.Add(report);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.SelectedIndex = -1;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (MainMenu.settings.clearMessages) listBox1.Items.Clear();

            try
            {
                DirectoryInfo di = new DirectoryInfo(MainMenu.settings.pathForInputFolder);
                fi = di.GetFiles();
            }
            catch
            {
                MessageBox.Show("Open and close program or fix path in config.xml!", "Error!");
                return; 
            }

            /*if (checkUnicode.Checked) MainMenu.settings.unicodeSettings = 0;
            else MainMenu.settings.unicodeSettings = 1;*/

            EncVersion = 2;
            if (comboBox2.SelectedIndex == 1) EncVersion = 7;

            string versionOfGame = " ";
            numKey = comboBox1.SelectedIndex;
            selected_index = comboBox2.SelectedIndex;

            //Create import files thread
            var processImport = new ForThreads();
            processImport.ReportForWork += AddNewReport;
            List<string> parametresImport = new List<string>();
            parametresImport.Add(versionOfGame);
            parametresImport.Add(".dds");
            parametresImport.Add(MainMenu.settings.pathForInputFolder);
            parametresImport.Add(MainMenu.settings.pathForOutputFolder);
            parametresImport.Add(MainMenu.settings.deleteD3DTXafterImport.ToString());
            parametresImport.Add(MainMenu.settings.deleteDDSafterImport.ToString());
            parametresImport.Add(Convert.ToString(EncVersion));
            parametresImport.Add(MainMenu.settings.encLangdb.ToString());
            
            threadImport = new Thread(new ParameterizedThreadStart(processImport.DoImportEncoding));
            threadImport.Start(parametresImport);


            for (int i = 0; i < fi.Length; i++)
            {
                if ((fi[i].Extension == ".lua") || (fi[i].Extension == ".lenc"))
                {
                    byte[] encKey;

                    if (MainMenu.settings.customKey)
                    {
                        encKey = Methods.stringToKey(MainMenu.settings.encCustomKey);

                        if (encKey == null)
                        {
                            MessageBox.Show("You must enter key encryption!", "Error");
                            return;
                        }
                    }
                    else encKey = MainMenu.gamelist[comboBox1.SelectedIndex].key;

                    int version;
                    if (selected_index == 0) version = 2;
                    else version = 7;

                    FileStream fs = new FileStream(fi[i].FullName, FileMode.Open);
                    byte[] luaContent = Methods.ReadFull(fs);
                    fs.Close();

                    luaContent = Methods.encryptLua(luaContent, encKey, CheckNewEngine.Checked, version);

                    if (File.Exists(TTG_Tools.MainMenu.settings.pathForOutputFolder + "\\" + fi[i].Name)) File.Delete(TTG_Tools.MainMenu.settings.pathForOutputFolder + "\\" + fi[i].Name);
                    fs = new FileStream(TTG_Tools.MainMenu.settings.pathForOutputFolder + "\\" + fi[i].Name, FileMode.CreateNew);
                    fs.Write(luaContent, 0, luaContent.Length);
                    fs.Close();

                    listBox1.Items.Add("File " + fi[i].Name + " encrypted.");
                }
                if ((i + 1 < fi.Count()) && (fi[i].Extension == ".font") && (fi[i + 1].Extension.ToLower() == ".ttf") && GetNameOnly(i) == GetNameOnly(i + 1))
                {
                    
                }
                else if (fi[i].Extension == ".font")
                {
                    int version;
                    if (selected_index == 0) version = 2;
                    else version = 7;

                    FileStream fs = new FileStream(fi[i].FullName, FileMode.Open);
                    byte[] fontContent = Methods.ReadFull(fs);
                    fs.Close();

                    byte[] checkHeader = new byte[4];
                    Array.Copy(fontContent, 0, checkHeader, 0, 4);
                    if ((Encoding.ASCII.GetString(checkHeader) != "5VSM") && (Encoding.ASCII.GetString(checkHeader) != "ERTM")
                        && (Encoding.ASCII.GetString(checkHeader) != "6VSM"))
                    {

                        if (Methods.FindStartOfStringSomething(fontContent, 0, "DDS ") < fontContent.Length - 100)
                        {
                            if (version == 2)
                            {
                                //Шифруем заголовок текстуры
                                int poz = Methods.FindStartOfStringSomething(fontContent, 0, "DDS ");
                                byte[] tempHeader = new byte[2048];
                                if (fontContent.Length - poz < tempHeader.Length) tempHeader = new byte[fontContent.Length - poz];

                                Array.Copy(fontContent, poz, tempHeader, 0, tempHeader.Length);
                                BlowFishCS.BlowFish encHeader = new BlowFishCS.BlowFish(MainMenu.gamelist[numKey].key, version);

                                tempHeader = encHeader.Crypt_ECB(tempHeader, version, false);

                                Array.Copy(fontContent, poz, tempHeader, 0, 2048);
                            }
                        }


                        //Шифруем шрифт
                        Methods.meta_crypt(fontContent, MainMenu.gamelist[numKey].key, version, false);

                        fs = new FileStream(MainMenu.settings.pathForOutputFolder + "\\" + fi[i].Name, FileMode.OpenOrCreate);
                        fs.Write(fontContent, 0, fontContent.Length);
                        fs.Close();

                        listBox1.Items.Add("File " + fi[i].Name + " encrypted!");

                    }
                }
            }
        }

        public static string GetNameOnly(int i)
        {
            return fi[i].Name.Substring(0, (fi[i].Name.Length - fi[i].Extension.Length));
        }

        private void buttonDecrypt_Click(object sender, EventArgs e)
        {
            if (MainMenu.settings.clearMessages) listBox1.Items.Clear();

            string versionOfGame = " ";
            numKey = comboBox1.SelectedIndex;
            selected_index = comboBox2.SelectedIndex;

            byte[] encKey;

            string debug = null;

            if (MainMenu.settings.customKey)
            {
                encKey = Methods.stringToKey(MainMenu.settings.encCustomKey);
            }
            else encKey = MainMenu.gamelist[comboBox1.SelectedIndex].key;

            int arc_version = 2;
            if (comboBox2.SelectedIndex == 1) arc_version = 7;

            Methods.DeleteCurrentFile("\\del.me");
            try
            {
                DirectoryInfo di = new DirectoryInfo(MainMenu.settings.pathForInputFolder);
                fi = di.GetFiles();
            }
            catch
            {
                MessageBox.Show("Open and close program or fix path in config.xml!", "Error!");
                return;
            }
            //Создаем нить для экспорта текста из LANGDB
            var processExport = new ForThreads();
            processExport.ReportForWork += AddNewReport;
            List<string> parametresExport = new List<string>();
            parametresExport.Add(MainMenu.settings.pathForInputFolder);
            parametresExport.Add(MainMenu.settings.pathForOutputFolder);
            parametresExport.Add(versionOfGame);
            parametresExport.Add(BitConverter.ToString(encKey).Replace("-", ""));
            parametresExport.Add(Convert.ToString(arc_version));

            threadExport = new Thread(new ParameterizedThreadStart(processExport.DoExportEncoding));
            threadExport.Start(parametresExport);

            for (int i = 0; i < fi.Length; i++)
            {
                if ((fi[i].Extension == ".lenc") || (fi[i].Extension == ".lua"))
                {
                    FileStream fs = new FileStream(fi[i].FullName, FileMode.Open);
                    byte[] luaContent = Methods.ReadFull(fs);
                    fs.Close();

                    int version;
                    if (selected_index == 0) version = 2;
                    else version = 7;
                    luaContent = Methods.decryptLua(luaContent, encKey, version);

                    fs = new FileStream(TTG_Tools.MainMenu.settings.pathForOutputFolder + "\\" + fi[i].Name, FileMode.OpenOrCreate);
                    fs.Write(luaContent, 0, luaContent.Length);
                    fs.Close();
                    listBox1.Items.Add("File " + fi[i].Name + " decrypted.");
                }
            }

            if (debug != null)
            {
                StreamWriter sw = new StreamWriter(MainMenu.settings.pathForOutputFolder + "\\bugs.txt");
                sw.Write(debug);
                sw.Close();
                listBox1.Items.Add("Bugs have been written in file " + MainMenu.settings.pathForOutputFolder + "\\bugs.txt");
            }
        }

        public class Prop
        {
            public byte[] id;
            public byte[] lenght_of_text;
            public string text;

            public Prop() { }
            public Prop(byte[] id, byte[] lenght_of_text, string text)
            {
                this.id = id;
                this.lenght_of_text = lenght_of_text;
                this.text = text;                
            }
        }

        public class Langdb
        {
            public byte[] hz_data;
            public byte[] realID;
            public byte[] lenght_of_textblok;
            public byte[] count_text;
            public byte[] lenght_of_name;
            public string name;
            public byte[] lenght_of_text;
            public string text;
            public byte[] lenght_of_waw;
            public string waw;
            public byte[] lenght_of_animation;
            public string animation;
            public byte[] magic_bytes;

            public Langdb() { }
            public Langdb(byte[] hz_data, byte[] realID, byte[] lenght_of_textblok, byte[] count_text, byte[] lenght_of_name,
            string name, byte[] lenght_of_text, string text, byte[] lenght_of_waw,
            string waw, byte[] lenght_of_animation, string animation, byte[] magic_bytes)
            {
                this.animation = animation;
                this.hz_data = hz_data;
                this.realID = realID;
                this.lenght_of_animation = lenght_of_animation;
                this.lenght_of_name = lenght_of_name;
                this.lenght_of_text = lenght_of_text;
                this.lenght_of_textblok = lenght_of_textblok;
                this.lenght_of_waw = lenght_of_waw;
                this.magic_bytes = magic_bytes;
                this.name = name;
                this.text = text;
                this.waw = waw;
                this.count_text = count_text;
            }
        }

        public static Int32 GetSizeOfByteMassiv(byte[] str)
        {
            try
            {
                return str.Length;
            }
            catch
            {
                return 0;
            }
        }
        public static Int32 GetSizeOfString(string str)
        {
            try
            {
                return str.Length;
            }
            catch
            {
                return 0;
            }
        }
        public static void SaveStringInfo(FileStream MyFileStream, string data, int ASCII_N, int UnicodeMode, bool oldFormat)
        {
            byte[] b1 = BitConverter.GetBytes(data.Length + 8);
            byte[] b2 = BitConverter.GetBytes(data.Length);

            if (((UnicodeMode == 0) || (UnicodeMode == 2)) && (!oldFormat))
            {
                byte[] bin_data = Encoding.UTF8.GetBytes(data);
                b1 = BitConverter.GetBytes(bin_data.Length + 8);
                b2 = BitConverter.GetBytes(bin_data.Length);
            }

            MyFileStream.Write(b1, 0, b1.Length);
            MyFileStream.Write(b2, 0, b2.Length);
            if (data.Length > 0)
            {
                byte[] hex_data = new byte[data.Length];
                //
                if ((MainMenu.settings.unicodeSettings != 1) && (!oldFormat)) hex_data = (byte[])Encoding.UTF8.GetBytes(data);
                else hex_data = (byte[])ASCIIEncoding.GetEncoding(ASCII_N).GetBytes(data);
                MyFileStream.Write(hex_data, 0, hex_data.Length);
            }
        }
        public static void SaveStringInfoForProp(FileStream MyFileStream, string data, int ASCII_N)
        {
            byte[] b = BitConverter.GetBytes(data.Length);
            MyFileStream.Write(b, 0, b.Length);
            if (data.Length > 0)
            {
                byte[] hex_data = new byte[data.Length];
                hex_data = (byte[])ASCIIEncoding.GetEncoding(ASCII_N).GetBytes(data);
                MyFileStream.Write(hex_data, 0, hex_data.Length);
            }
        }


        private void AutoPacker_Load(object sender, EventArgs e)
        {

            #region Load blowfish key list

            comboBox1.Items.Clear();

            for (int i = 0; i < MainMenu.gamelist.Count; i++)
            {
                comboBox1.Items.Add(i + ". " + MainMenu.gamelist[i].gamename);
            }

            #endregion

            comboBox1.SelectedIndex = MainMenu.settings.encKeyIndex;
            comboBox2.SelectedIndex = MainMenu.settings.versionEnc;
            labelUnicode.Text = "Unicode is ";
            labelUnicode.Text += MainMenu.settings.unicodeSettings == 0 ? "set." : "not set.";
            checkEncDDS.Checked = MainMenu.settings.encDDSonly;
            checkIOS.Checked = MainMenu.settings.iOSsupport;
            checkEncLangdb.Checked = MainMenu.settings.encLangdb;
            CheckNewEngine.Checked = MainMenu.settings.encNewLua;

            if (MainMenu.settings.swizzlePS4 || MainMenu.settings.swizzleNintendoSwitch)
            {
                if (MainMenu.settings.swizzleNintendoSwitch) rbSwitchSwizzle.Checked = true;
                else rbPS4Swizzle.Checked = true;
            }
            else rbNoSwizzle.Checked = true;

            if (MainMenu.settings.customKey && Methods.stringToKey(MainMenu.settings.encCustomKey) != null)
            {
                checkCustomKey.Checked = MainMenu.settings.customKey;
                textBox1.Text = MainMenu.settings.encCustomKey;
            }

            if(MainMenu.settings.ASCII_N == 1252)
            {
                //Make unvisible that option for users with windows-1252 encoding
                labelUnicode.Visible = false;
            }
        }

        private void AutoPacker_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((threadExport != null) && threadExport.IsAlive)
            {
                threadExport.Abort();
            }

            if((threadImport != null) && threadImport.IsAlive)
            {
                threadImport.Abort();
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void CheckNewEngine_CheckedChanged(object sender, EventArgs e)
        {
            MainMenu.settings.encNewLua = CheckNewEngine.Checked;
            Settings.SaveConfig(MainMenu.settings);
        }

        private void checkEncDDS_CheckedChanged(object sender, EventArgs e)
        {
            MainMenu.settings.encDDSonly = checkEncDDS.Checked;
            Settings.SaveConfig(MainMenu.settings);
        }

        private void checkEncLangdb_CheckedChanged(object sender, EventArgs e)
        {
            MainMenu.settings.encLangdb = checkEncLangdb.Checked;
            Settings.SaveConfig(MainMenu.settings);
        }

        private void checkIOS_CheckedChanged(object sender, EventArgs e)
        {
            MainMenu.settings.iOSsupport = checkIOS.Checked;
            Settings.SaveConfig(MainMenu.settings);
        }

        private void checkCustomKey_CheckedChanged(object sender, EventArgs e)
        {
            MainMenu.settings.customKey = checkCustomKey.Checked;
            Settings.SaveConfig(MainMenu.settings);

            if((MainMenu.settings.customKey == true) && 
                ((MainMenu.settings.encCustomKey != "") && (MainMenu.settings.encCustomKey != null)))
            {
                textBox1.Text = MainMenu.settings.encCustomKey;
            }
            else
            {
                textBox1.Text = "";
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MainMenu.settings.encKeyIndex = comboBox1.SelectedIndex;
            Settings.SaveConfig(MainMenu.settings);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            MainMenu.settings.versionEnc = comboBox2.SelectedIndex;
            Settings.SaveConfig(MainMenu.settings);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(checkCustomKey.Checked && Methods.stringToKey(textBox1.Text) != null)
            {
                MainMenu.settings.customKey = checkCustomKey.Checked;
                MainMenu.settings.encCustomKey = textBox1.Text;
                Settings.SaveConfig(MainMenu.settings);
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AutoDePackerSettings settingsForm = new AutoDePackerSettings();
            settingsForm.FormClosed += new FormClosedEventHandler(Form_Closed);
            settingsForm.Show(this);
        }

        private void Form_Closed(object sender, FormClosedEventArgs e)
        {
            AutoDePackerSettings settingsForm = (AutoDePackerSettings)sender;

            labelUnicode.Text = "Unicode is ";
            labelUnicode.Text += MainMenu.settings.unicodeSettings == 0 ? "set." : "not set.";
        }

        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void rbNoSwizzle_CheckedChanged(object sender, EventArgs e)
        {
            if(rbNoSwizzle.Checked)
            {
                MainMenu.settings.swizzleNintendoSwitch = false;
                MainMenu.settings.swizzlePS4 = false;
                Settings.SaveConfig(MainMenu.settings);
            }
        }

        private void rbPS4Swizzle_CheckedChanged(object sender, EventArgs e)
        {
            if(rbPS4Swizzle.Checked)
            {
                MainMenu.settings.swizzlePS4 = true;
                MainMenu.settings.swizzleNintendoSwitch = false;
                Settings.SaveConfig(MainMenu.settings);
            }
        }

        private void rbSwitchSwizzle_CheckedChanged(object sender, EventArgs e)
        {
            if(rbSwitchSwizzle.Checked)
            {
                MainMenu.settings.swizzleNintendoSwitch = true;
                MainMenu.settings.swizzlePS4 = false;
                Settings.SaveConfig(MainMenu.settings);
            }
        }
    }
}
