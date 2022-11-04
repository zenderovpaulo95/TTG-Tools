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

        public static string ConvertHexToString(byte[] binContent, int poz, int len_string)
        {
            byte[] temp_hex_string = new byte[len_string];
            Array.Copy(binContent, poz, temp_hex_string, 0, len_string);
            return ASCIIEncoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(temp_hex_string);
        }

        public static FileInfo[] fi;
        public static FileInfo[] fi_temp;
        //public static List<TextureWorker.Texture_format> tex_format = new List<TextureWorker.Texture_format>(); //Список с форматами текстур

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

            if (checkUnicode.Checked) MainMenu.settings.unicodeSettings = 0;
            else MainMenu.settings.unicodeSettings = 2;
            
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
                    byte[] vector_font = File.ReadAllBytes(fi[i + 1].FullName);
                    byte[] font = File.ReadAllBytes(fi[i].FullName);

                    byte[] header = new byte[4];
                    byte[] version = new byte[4];

                    Array.Copy(font, 0, header, 0, header.Length);
                    Array.Copy(font, 16, version, 0, version.Length);

                    if (Encoding.ASCII.GetString(header) != "6VSM" && BitConverter.ToInt32(version, 0) != 1)
                    {
                        vector_font = null;
                        font = null;
                        GC.Collect();

                        listBox1.Items.Add("This file doesn't have vector fonts: " + fi[i].Name);
                        return;
                    }

                    byte[] beg_chunk; //Copy begin of font file
                    byte[] block_sz = new byte[4];
                    byte[] hz_size = new byte[4]; //I don't know why calculate addition 8 bytes to font size
                    byte[] font_size = new byte[4]; //Font size
                    byte[] last_chunk; //Copy ending font files after ttf file

                    int pos = 32;
                    int beg_pos = pos;
                    byte[] tmp = new byte[4];

                    Array.Copy(font, pos, tmp, 0, tmp.Length);

                    pos += BitConverter.ToInt32(tmp, 0) + 28;

                    Array.Copy(font, pos, hz_size, 0, hz_size.Length);
                    pos += 4;
                    Array.Copy(font, pos, font_size, 0, font_size.Length);

                    beg_chunk = new byte[pos + 4];
                    Array.Copy(font, 0, beg_chunk, 0, beg_chunk.Length);

                    beg_pos = pos + 4;

                    pos += 4 + BitConverter.ToInt32(font_size, 0);
                    last_chunk = new byte[font.Length - pos];
                    Array.Copy(font, pos, last_chunk, 0, last_chunk.Length);

                    int new_block_sz = (beg_chunk.Length - 32) + vector_font.Length + last_chunk.Length;
                    hz_size = BitConverter.GetBytes((int)(vector_font.Length + 8));
                    font_size = BitConverter.GetBytes((int)vector_font.Length);
                    block_sz = BitConverter.GetBytes(new_block_sz);

                    Array.Copy(block_sz, 0, beg_chunk, 4, block_sz.Length);
                    Array.Copy(hz_size, 0, beg_chunk, beg_pos - 8, hz_size.Length);
                    Array.Copy(font_size, 0, beg_chunk, beg_pos - 4, font_size.Length);

                    if (File.Exists(MainMenu.settings.pathForOutputFolder + "\\" + fi[i].Name)) File.Delete(MainMenu.settings.pathForOutputFolder + "\\" + fi[i].Name);

                    FileStream fs = new FileStream(MainMenu.settings.pathForOutputFolder + "\\" + fi[i].Name, FileMode.CreateNew);
                    fs.Write(beg_chunk, 0, beg_chunk.Length);
                    fs.Write(vector_font, 0, vector_font.Length);
                    fs.Write(last_chunk, 0, last_chunk.Length);
                    fs.Close();

                    listBox1.Items.Add("File " + fi[i].Name + " successfully imported!");
                }
                else if (fi[i].Extension == ".vox") //Experiments with vox files in old Telltale games.
                {
                    //Test on Sam & Max Season One
                    FileStream fs = new FileStream(fi[i].FullName, FileMode.Open);
                    BinaryReader br = new BinaryReader(fs);
                    int version_enc = 2; //version of decryption
                    if (comboBox2.SelectedIndex == 1) version_enc = 7;

                    byte[] keyEncryption = MainMenu.gamelist[comboBox1.SelectedIndex].key; //Get key of encryption                    

                    if (MainMenu.settings.customKey)
                    {
                        keyEncryption = Methods.stringToKey(MainMenu.settings.encCustomKey);
                    }

                    int fileLength;
                    float time; //Playing time sound
                    int[] chunkOffs;
                    int[] chunksSizes; //Chunk sizes
                    int chunkCount;
                    int blockLength; //length block with count of chunks and it sizes
                    int tmp; //for skipping some parameters
                    byte[] header = br.ReadBytes(4);
                    tmp = br.ReadInt32(); //First read count datas
                    int c = tmp;
                    int smpRate; //Sample rate
                    int channels; //No channels
                    int bitRate; //Bit rate (need divide by 10?)

                    byte[] b_tmp = null;

                    for (int l = 0; l < c; l++)
                    {
                        tmp = br.ReadInt32();
                        b_tmp = br.ReadBytes(tmp);
                        tmp = br.ReadInt32();
                    }

                    byte one = br.ReadByte();

                    time = br.ReadSingle();
                    fileLength = br.ReadInt32();
                    bitRate = br.ReadInt32();
                    smpRate = br.ReadInt32();

                    int WavSmpRate = 16000;

                    switch (smpRate)
                    {
                        case 32000:
                            WavSmpRate = 16000;
                            break;

                        case 44100:
                            WavSmpRate = 22050;
                            break;

                        case 22050:
                            WavSmpRate = 11025;
                            break;
                    }

                    channels = br.ReadInt32();
                    blockLength = br.ReadInt32();
                    chunkCount = br.ReadInt32();
                    chunkOffs = new int[chunkCount];
                    chunksSizes = new int[chunkCount];

                    for (int l = 0; l < chunkCount; l++)
                    {
                        chunkOffs[l] = br.ReadInt32();
                    }

                    for (int l = 0; l < chunkCount - 1; l++)
                    {
                        chunksSizes[l] = chunkOffs[l + 1] - chunkOffs[l];
                    }

                    chunksSizes[chunkCount - 1] = fileLength - chunkOffs[chunkCount - 1];

                    int pose = (int)br.BaseStream.Position;

                    byte[] block = br.ReadBytes(fileLength);

                    int offset = 0;

                    for (int l = 0; l < chunkCount; l++)
                    {
                        if ((l == 0) || (l % 64 == 0))
                        {
                            b_tmp = new byte[chunksSizes[l]];
                            Array.Copy(block, offset, b_tmp, 0, b_tmp.Length);
                            BlowFish blow = new BlowFish(keyEncryption, version_enc);
                            b_tmp = blow.Crypt_ECB(b_tmp, version_enc, false);
                            Array.Copy(b_tmp, 0, block, offset, b_tmp.Length);
                        }

                        offset += chunksSizes[l];
                    }

                    br.BaseStream.Seek(0, SeekOrigin.Begin);
                    byte[] head_block = br.ReadBytes(pose);

                    br.Close();
                    fs.Close();

                    if (File.Exists(MainMenu.settings.pathForOutputFolder + "\\" + fi[i].Name)) File.Delete(MainMenu.settings.pathForOutputFolder + "\\" + fi[i].Name);

                    fs = new FileStream(MainMenu.settings.pathForOutputFolder + "\\" + fi[i].Name, FileMode.OpenOrCreate);
                    fs.Write(head_block, 0, head_block.Length);
                    fs.Write(block, 0, block.Length);
                    fs.Close();

                    listBox1.Items.Add("VOX has successfully encrypted from file " + fi[i].Name);
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
                if (i + 1 < fi.Count())
                {
                    bool work = false;
                    int offset1 = 0;
                    int offset2 = 1;

                    if (fi[i].Extension == ".dlog" && fi[i + 1].Extension == ".txt" && GetNameOnly(i) == GetNameOnly(i + 1))
                    {
                        offset1 = 0;
                        offset2 = 1;
                        work = true;
                    }
                    if (fi[i + 1].Extension == ".dlog" && fi[i].Extension == ".txt" && Methods.DeleteCommentary(GetNameOnly(i), "(", ")") == GetNameOnly(i + 1))
                    {
                        offset1 = 1;
                        offset2 = 0;
                        work = true;
                    }

                    if (work)
                    {
                        List<Langdb> database = new List<Langdb>();
                        FileStream fs = new FileStream(fi[i + offset1].FullName, FileMode.Open);
                        byte[] binContent = Methods.ReadFull(fs);
                        byte version = 0;
                        ReadDlog(binContent, first_database, database, version);
                        int size_first = BitConverter.ToInt32(first_database[0].lenght_of_langdb1, 0);
                        fs.Close();
                        if (database.Count != 0)
                        {
                            List<TextCollector.TXT_collection> all_text = new List<TextCollector.TXT_collection>();
                            string error = string.Empty;
                            ImportTXT(fi[i + offset2].FullName, ref all_text, false, MainMenu.settings.ASCII_N, "\r\n", ref error);
                            for (int q = 0; q < all_text.Count; q++)
                            {
                                if(MainMenu.settings.importingOfName)
                                {
                                    database[all_text[q].number - 1].name = all_text[q].name;
                                    database[all_text[q].number - 1].lenght_of_name = BitConverter.GetBytes(database[all_text[q].number - 1].name.Length);
                                }
                                if (BitConverter.ToInt32(database[all_text[q].number - 1].lenght_of_textblok, 0) != 8)
                                {
                                    database[all_text[q].number - 1].text = all_text[q].text.Replace("\r\n", "\n");
                                    database[all_text[q].number - 1].lenght_of_text = BitConverter.GetBytes(database[all_text[q].number - 1].text.Length);
                                }
                            }
                            Methods.DeleteCurrentFile(MainMenu.settings.pathForOutputFolder + "\\" + fi[i + offset1].Name.ToString());
                            CreateDlog(first_database, database, 0, (MainMenu.settings.pathForOutputFolder + "\\" + fi[i + offset1].Name.ToString()));
                            listBox1.Items.Add("File " + fi[i + offset2].Name + " imported in " + fi[i + offset1].Name);
                        }
                        else
                        {
                            listBox1.Items.Add("File " + fi[i + offset1].Name + " is EMPTY!");
                        }
                        i++;
                        work = false;
                    }
                }
            }
        }

        public static List<TextCollector.TXT_collection> ImportTSV(string path, ref List<TextCollector.TXT_collection> txt_collection, string enter, ref string error)
        {
            string[] strings = File.ReadAllLines(path);
            for (int k = 0; k < strings.Length; k++)
            {
                string[] temp_strings = strings[k].Split('\t');
                if (MainMenu.settings.exportRealID)
                {
                    txt_collection.Add(new TextCollector.TXT_collection(k + 1, Convert.ToUInt32(temp_strings[0]), temp_strings[1], temp_strings[2], false));
                }
                else txt_collection.Add(new TextCollector.TXT_collection(Convert.ToInt32(temp_strings[0]), 0, temp_strings[1], temp_strings[2], false));
            }

            if (txt_collection.Count > 0)
            {
                for (int l = 0; l < txt_collection.Count; l++)
                {
                    txt_collection[l].text = txt_collection[l].text.Replace("\\n", "\n");
                }
            }

            return txt_collection;
        }

        public static List<TextCollector.TXT_collection> ImportTXT(string path, ref List<TextCollector.TXT_collection> txt_collection, bool to_translite, int ASCII, string enter, ref string error)
        {
            //StreamReader sr = new StreamReader(path, System.Text.ASCIIEncoding.GetEncoding(ASCII));
            //StreamReader sr = new StreamReader(path, System.Text.UnicodeEncoding.GetEncoding(ASCII));
            string[] texts = File.ReadAllLines(path);

            //string curLine;
            Int64 pos_str = 0;
            string name = "";
            bool number_find = false;
            int n_str = -1;
            int counter = 1;
            bool need_count = false;
            //int number_of_next_list = 0;

            for (int i = 0; i < texts.Length; i++)
            {
                if (texts[i].IndexOf(")") > -1 && number_find == false)
                {
                    if ((Methods.IsNumeric(texts[i].Substring(0, texts[i].IndexOf(")")))))
                    {
                        pos_str = Convert.ToInt64(texts[i].Substring(0, texts[i].IndexOf(")")));
                        try
                        {
                            string[] s;
                            s = texts[i].Split(')');
                            if (s.Count() > 1)
                            {
                                //Remove useless spaces (тут отрезаем тупые пробелы между скобкой)
                                name = "";
                                string[] temp = s[1].Split(' ');
                                foreach (string s_temp in temp)
                                {
                                    if (s_temp != string.Empty)
                                    {
                                        if (name == string.Empty)
                                        { name += s_temp; }
                                        else
                                        { name += " " + s_temp; }
                                    }
                                }
                                //name = s[1]; 
                            }
                            else
                            { name = ""; }
                            //name = curLine.Substring(curLine.IndexOf(")"), (curLine.Length - curLine.IndexOf(")") - 1));
                            if (MainMenu.settings.exportRealID)
                            {
                                txt_collection.Add(new TextCollector.TXT_collection(counter, (UInt32)pos_str, name, "", false));
                                if(need_count)
                                {
                                    counter++;
                                    need_count = false;
                                }
                                else
                                {
                                    need_count = true;
                                }
                            }
                            else
                            {
                                txt_collection.Add(new TextCollector.TXT_collection((Int32)pos_str, 0, name, "", false));
                            }
                            //number_of_next_list++;
                            number_find = true;
                            n_str++;
                            //counter++;
                        }
                        catch
                        {
                            MessageBox.Show("Error in string: " + texts[i] + "\r\n", "Error!");
                            error = "Error in string: " + texts[i];
                            break;
                        }
                    }
                    else
                        {
                            txt_collection[n_str].text += enter + texts[i];
                            number_find = false;
                        }
                    }
                    else
                    {
                        txt_collection[n_str].text += enter + texts[i];
                        number_find = false;
                    }
            }

                for (int i = 0; i < txt_collection.Count; i++)
                {
                    if (txt_collection[i].text.Length > 0)
                    {
                        txt_collection[i].text = txt_collection[i].text.Substring(enter.Length, txt_collection[i].text.Length - enter.Length);
                    }
                }
            return txt_collection;
        }

        public static void ReadLangdb(byte[] binContent, langdb[] database, byte version)
        {
            List<langdb> db = new List<langdb>();
            {
                number = 0;
                int poz = 0;
                if (version == 0)
                {
                    database[0].head = new byte[95];
                    Array.Copy(binContent, poz, database[0].head, 0, 95);
                    poz = 95;
                }
                if (version == 1)
                {
                    database[0].head = new byte[52];
                    Array.Copy(binContent, poz, database[0].head, 0, 52);
                    poz = 52;
                }
                if (version == 2)
                {
                    database[0].head = new byte[48];
                    Array.Copy(binContent, poz, database[0].head, 0, 48);
                    poz = 48;
                }
                if (version == 3)//test
                {
                    database[0].head = new byte[1];
                    Array.Copy(binContent, poz, database[0].head, 0, 0);
                    poz = 0;
                }
                if(version == 4) //Temporary fix for some old langdb files
                {
                    database[0].head = new byte[76];
                    Array.Copy(binContent, poz, database[0].head, 0, 76);
                    poz = 76;
                }
                while (poz < binContent.Length)
                {
                    //8 байт неизвестного происхождения
                    database[number].hz_data = new byte[8];
                    Array.Copy(binContent, poz, database[number].hz_data, 0, 8);
                    database[number].realID = new byte[4];
                    Array.Copy(database[number].hz_data, 0, database[number].realID, 0, 4);
                    poz += 8;
                    //4 байта длинны имени
                    //в первых двух (0 и 1) версиях пропускаем дублирование длинны! 
                    if (version <= 1 || version == 4)
                    {
                        poz += 4;
                    }
                    database[number].lenght_of_name = new byte[4];
                    Array.Copy(binContent, poz, database[number].lenght_of_name, 0, 4);
                    poz += 4;
                    //получаем имя
                    int len_name = BitConverter.ToInt32(database[number].lenght_of_name, 0);
                        database[number].name = ConvertHexToString(binContent, poz, len_name);
                        poz += len_name;
                        //получаем 4 байта длины текста не забывая о 0 и 1 версии
                        if (version <= 1 || version == 4)
                        {
                            poz += 4;
                        }
                        database[number].lenght_of_text = new byte[4];
                        Array.Copy(binContent, poz, database[number].lenght_of_text, 0, 4);
                        poz += 4;
                        //получаем текст
                        int len_text = BitConverter.ToInt32(database[number].lenght_of_text, 0);
                        database[number].text = ConvertHexToString(binContent, poz, len_text);
                        poz += len_text;
                        //получаем 4 байта длины анимации не забывая о 0 и 1 версии
                        if (version <= 1 || version == 4)
                        {
                            poz += 4;
                        }
                        database[number].lenght_of_animation = new byte[4];
                        Array.Copy(binContent, poz, database[number].lenght_of_animation, 0, 4);
                        poz += 4;
                        //получаем анимацию
                        int len_animation = BitConverter.ToInt32(database[number].lenght_of_animation, 0);
                        database[number].animation = ConvertHexToString(binContent, poz, len_animation);
                        poz += len_animation;
                        //получаем 4 байта длины озвучки не забывая о 0 и 1 версии
                        if (version <= 1 || version == 4)
                        {
                            poz += 4;
                        }
                        database[number].lenght_of_waw = new byte[4];
                        Array.Copy(binContent, poz, database[number].lenght_of_waw, 0, 4);
                        poz += 4;
                        //получаем озвучки
                        int len_waw = BitConverter.ToInt32(database[number].lenght_of_waw, 0);
                        database[number].waw = ConvertHexToString(binContent, poz, len_waw);
                        poz += len_waw;
                        //получаем магические байты
                        database[number].magic_bytes = new byte[7];
                        Array.Copy(binContent, poz, database[number].magic_bytes, 0, 7);
                        poz += 7;
                        number++;
                    }
                number--;
                //langdb db = new langdb[number];
                
            }
        }


        public static void CreateLangdb(langdb[] database, byte version, string path)
        {
            //проверяем наличие файла, удаляем его и создаем пустой
            FileStream MyFileStream;

            if (System.IO.File.Exists(path) == true)
            {
                System.IO.File.Delete(path);
            }
            MyFileStream = new FileStream(path, FileMode.OpenOrCreate);
            //записываем заголовок
            int numb = 0;
            MyFileStream.Write(database[0].head, 0, database[0].head.Length);
            //записываем всё остальное
            while (numb <= number)
            {
                //сохраняем хз байты =)
                MyFileStream.Write(database[numb].hz_data, 0, database[numb].hz_data.Length);
                //имя
                SaveStringInfo(MyFileStream, database[numb].name, version);
                //текст
                SaveStringInfo(MyFileStream, database[numb].text, version);
                //анимация
                SaveStringInfo(MyFileStream, database[numb].animation, version);
                //озвучка
                SaveStringInfo(MyFileStream, database[numb].waw, version);
                //магические байты
                MyFileStream.Write(database[numb].magic_bytes, 0, database[numb].magic_bytes.Length);
                //счетчик++
                numb++;
            }
            //закрываем поток
            MyFileStream.Close();
        }

        public static void SaveStringInfo(FileStream MyFileStream, string data, byte version)
        {
            byte[] b = BitConverter.GetBytes(data.Length);
            if (version <= 1 || version == 4) //FIX THAT LATER
            {
                MyFileStream.Write(BitConverter.GetBytes(data.Length + 8), 0, 4);
            }
            MyFileStream.Write(b, 0, 4);
            if (data.Length > 0)
            {
                byte[] hex_data = (byte[])ASCIIEncoding.GetEncoding(MainMenu.settings.ASCII_N).GetBytes(data);
                MyFileStream.Write(hex_data, 0, hex_data.Length);
            }
        }

        public static string GetNameOnly(int i)
        {
            return fi[i].Name.Substring(0, (fi[i].Name.Length - fi[i].Extension.Length));
        }

        public static void ExportDDSfromD3DTX(FileInfo[] inputFiles, int i, string pathOutput, string fileName)
        {

            //try
            {
                FileStream fs = new FileStream(inputFiles[i].FullName, FileMode.Open);
                byte[] binContent = Methods.ReadFull(fs);
                fs.Close();

                Methods.DeleteCurrentFile(pathOutput + "\\" + fileName);


                //listBox1.Items.Add("File " + inputFiles[i].Name + " exported in " + fileName);//ReportForWork("File " + inputFiles[i].Name + " exported in " + fileName);

            }
            //catch
            //{
            //    ReportForWork("Expoort from file: " + inputFiles[i].Name + " is incorrect!");
            //}
        }

        private void buttonDecrypt_Click(object sender, EventArgs e)
        {
            if (MainMenu.settings.clearMessages) listBox1.Items.Clear();

            string versionOfGame = " ";//CheckVersionOfGameFromCombobox(comboBox1.SelectedIndex);
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
                if (fi[i].Extension == ".vox") //Experiments with vox files in old Telltale games.
                {
                    //Test on Sam & Max Season One
                    FileStream fs = new FileStream(fi[i].FullName, FileMode.Open);
                    BinaryReader br = new BinaryReader(fs);
                    int version_enc = 2; //version of decryption
                    if (comboBox2.SelectedIndex == 1) version_enc = 7;

                    byte[] keyEncryption = MainMenu.gamelist[comboBox1.SelectedIndex].key; //Get key of encryption                    

                    if (MainMenu.settings.customKey)
                    {
                        keyEncryption = Methods.stringToKey(MainMenu.settings.encCustomKey);
                    }

                    int fileLength;
                    float time; //Playing time sound
                    int[] chunkOffs;
                    int[] chunksSizes; //Chunk sizes
                    int chunkCount;
                    int blockLength; //length block with count of chunks and it sizes
                    int tmp; //for skipping some parameters
                    byte[] header = br.ReadBytes(4);
                    tmp = br.ReadInt32(); //First read count datas
                    int c = tmp;
                    int smpRate; //Sample rate
                    int channels; //No channels
                    int bitRate; //Bit rate (need divide by 10?)

                    byte[] b_tmp = null;

                    for (int l = 0; l < c; l++)
                    {
                        tmp = br.ReadInt32();
                        b_tmp = br.ReadBytes(tmp);
                        tmp = br.ReadInt32();
                    }

                    byte one = br.ReadByte();

                    time = br.ReadSingle();
                    fileLength = br.ReadInt32();
                    bitRate = br.ReadInt32();
                    smpRate = br.ReadInt32();

                    int WavSmpRate = 16000;

                    switch(smpRate)
                    {
                        case 32000:
                            WavSmpRate = 16000;
                            break;

                        case 44100:
                            WavSmpRate = 22050;
                            break;

                        case 22050:
                            WavSmpRate = 11025;
                            break;
                    }

                    channels = br.ReadInt32();
                    blockLength = br.ReadInt32();
                    chunkCount = br.ReadInt32();
                    chunkOffs = new int[chunkCount];
                    chunksSizes = new int[chunkCount];

                    for(int l = 0; l < chunkCount; l++)
                    {
                        chunkOffs[l] = br.ReadInt32();
                    }

                    for(int l = 0; l < chunkCount - 1; l++)
                    {
                        chunksSizes[l] = chunkOffs[l + 1] - chunkOffs[l];
                    }

                    chunksSizes[chunkCount - 1] = fileLength - chunkOffs[chunkCount - 1];

                    int pose = (int)br.BaseStream.Position;

                    byte[] block = br.ReadBytes(fileLength);

                    int offset = 0;

                    for(int l = 0; l < chunkCount; l++)
                    {
                        if((l == 0) || (l % 64 == 0))
                        {
                            b_tmp = new byte[chunksSizes[l]];
                            Array.Copy(block, offset, b_tmp, 0, b_tmp.Length);
                            BlowFish blow = new BlowFish(keyEncryption, version_enc);
                            b_tmp = blow.Crypt_ECB(b_tmp, version_enc, true);
                            Array.Copy(b_tmp, 0, block, offset, b_tmp.Length);
                        }

                        offset += chunksSizes[l];
                    }

                    offset = chunksSizes[0];
                    int sampleRate = 0;
                    int FrameSize = 0;

                    listBox1.Items.Add("VOX has successfully decrypted from file " + fi[i].Name + ". Sample rate: " + sampleRate + ". Frame size: " + FrameSize);
                }
                else if(fi[i].Extension == ".font") //Может, доработать чуть позже авторасшифровку в Font Editor и удалить отсюда этот код?
                {
                    FileStream fs = new FileStream(fi[i].FullName, FileMode.Open);
                    byte[] fontContent = Methods.ReadFull(fs);
                    fs.Close();

                    byte[] checkHeader = new byte[4];
                    Array.Copy(fontContent, 0, checkHeader, 0, 4);

                    bool vectorFont = false;

                    if ((Encoding.ASCII.GetString(checkHeader) != "ERTM") && (Encoding.ASCII.GetString(checkHeader) != "5VSM"))
                    {
                        byte[] checkVer = new byte[4];
                        Array.Copy(fontContent, 4, checkVer, 0, 4);
                        if (Encoding.ASCII.GetString(checkHeader) == "6VSM")
                        {
                            Array.Copy(fontContent, 16, checkVer, 0, 4);
                            vectorFont = BitConverter.ToInt32(checkVer, 0) == 1;

                            if (!vectorFont) listBox1.Items.Add("This is not a vector font: " + fi[i].Name);
                            else
                            {
                                byte[] getoffset = new byte[4];
                                Array.Copy(fontContent, 32, getoffset, 0, 4);

                                int font_off = 64 + BitConverter.ToInt32(getoffset, 0);

                                byte[] binLength = new byte[4];
                                Array.Copy(fontContent, font_off, binLength, 0, 4);

                                int len = BitConverter.ToInt32(binLength, 0);

                                font_off += 4;

                                byte[] vector_font = new byte[len];
                                Array.Copy(fontContent, font_off, vector_font, 0, vector_font.Length);

                                if (File.Exists(MainMenu.settings.pathForOutputFolder + "\\" + Methods.GetNameOfFileOnly(fi[i].Name, ".font") + ".ttf")) File.Delete(MainMenu.settings.pathForOutputFolder + "\\" + Methods.GetNameOfFileOnly(fi[i].Name, ".font") + ".ttf");

                                fs = new FileStream(MainMenu.settings.pathForOutputFolder + "\\" + Methods.GetNameOfFileOnly(fi[i].Name, ".font") + ".ttf", FileMode.CreateNew);
                                fs.Write(vector_font, 0, vector_font.Length);
                                fs.Close();

                                fontContent = null;
                                vector_font = null;
                                GC.Collect();

                                listBox1.Items.Add("Vector font has successfully extracted from file " + fi[i].Name);
                            }
                        }
                        else
                        {
                            if ((BitConverter.ToInt32(checkVer, 0) < 0) || (BitConverter.ToInt32(checkVer, 0) > 6))
                            {
                                Methods.FindingDecrytKey(fontContent, "font", ref encKey, ref EncVersion);
                                //Methods.meta_crypt(fontContent, MainMenu.gamelist[selected_index].key, 2, true);
                                checkVer = new byte[4];
                                Array.Copy(fontContent, 4, checkVer, 0, 4);

                                if ((BitConverter.ToInt32(checkVer, 0) > 0) && (BitConverter.ToInt32(checkVer, 0) < 6))
                                {
                                    if (File.Exists(MainMenu.settings.pathForOutputFolder + "\\" + fi[i].Name)) File.Delete(MainMenu.settings.pathForOutputFolder + "\\" + fi[i].Name);

                                    fs = new FileStream(MainMenu.settings.pathForOutputFolder + "\\" + fi[i].Name, FileMode.CreateNew);
                                    fs.Write(fontContent, 0, fontContent.Length);
                                    fs.Close();

                                    listBox1.Items.Add("File " + fi[i].Name + " decrypted!");
                                }
                                else listBox1.Items.Add("Font couldn't decrypt. Try another key.");
                            }
                            else
                            {
                                if (Methods.FindStartOfStringSomething(fontContent, 0, "DDS") > fontContent.Length - 100)
                                {
                                    Methods.FindingDecrytKey(fontContent, "font", ref encKey, ref EncVersion);

                                    if (File.Exists(MainMenu.settings.pathForOutputFolder + "\\" + fi[i].Name)) File.Delete(MainMenu.settings.pathForOutputFolder + "\\" + fi[i].Name);
                                    fs = new FileStream(MainMenu.settings.pathForOutputFolder + "\\" + fi[i].Name, FileMode.CreateNew);
                                    fs.Write(fontContent, 0, fontContent.Length);
                                    fs.Close();

                                    listBox1.Items.Add("File " + fi[i].Name + " decrypted!");
                                }
                                else listBox1.Items.Add("Font couldn't decrypt. Try another key.");
                            }
                        }
                    }
                }
                else if ((fi[i].Extension == ".lenc") || (fi[i].Extension == ".lua"))
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
                StreamWriter sw = new StreamWriter(MainMenu.settings.pathForOutputFolder + "\\Баги.txt");
                sw.Write(debug);
                sw.Close();
                listBox1.Items.Add("Bugs have been written in file " + MainMenu.settings.pathForOutputFolder + "\\Баги.txt");
            }
        }

        public static void ReadDlog(byte[] binContent, dlog[] first_database, List<Langdb> database, byte version)
        {
            {
                int number = 0;
                int poz = 0;
                int poz_add = 0;
                int last_pozition = 0;
                int poz_start = 0;

                //вычисляем начало файла
                byte[] temp1 = new byte[4];
                byte[] temp2 = new byte[4];
                int tmp1 = 0;
                int tmp2 = 0;
                for (int i = poz; i < binContent.Length; i++)
                {
                    Array.Copy(binContent, i, temp1, 0, 4);
                    tmp1 = BitConverter.ToInt32(temp1, 0);
                    Array.Copy(binContent, i + 4, temp2, 0, 4);
                    tmp2 = BitConverter.ToInt32(temp2, 0);
                    if ((tmp1 - 8) == tmp2 && tmp2 < 255 && tmp2 != 0)
                    {
                        poz_add = i;
                        poz_start = i;
                        break;
                    }
                }
                poz_start = poz_add;
                first_database[0].head = new byte[poz_add];
                Array.Copy(binContent, poz, first_database[0].head, 0, poz_add);
                poz += poz_add;

                poz_add = 4;
                first_database[0].lenght_of_name_langdb = new byte[poz_add];
                Array.Copy(binContent, poz, first_database[0].lenght_of_name_langdb, 0, poz_add);
                poz += poz_add;

                poz_add = 4;
                first_database[0].lenght_of_name_langdb_minus_4 = new byte[poz_add];
                Array.Copy(binContent, poz, first_database[0].lenght_of_name_langdb_minus_4, 0, poz_add);
                poz += poz_add;

                int len_text = BitConverter.ToInt32(first_database[0].lenght_of_name_langdb_minus_4, 0);
                byte[] hex_text = new byte[len_text];
                //получаем имя базы
                first_database[number].name_of_langdb = Methods.ConvertHexToString(binContent, poz, len_text, MainMenu.settings.ASCII_N, 1);
                poz += len_text;

                poz_add = 20;//0x14
                first_database[0].hlam = new byte[poz_add];
                Array.Copy(binContent, poz, first_database[0].hlam, 0, poz_add);
                poz += poz_add;
                //MessageBox.Show(poz.ToString());

                poz_add = 4;
                first_database[0].lenght_of_langdb1 = new byte[poz_add];
                Array.Copy(binContent, poz, first_database[0].lenght_of_langdb1, 0, poz_add);
                poz += poz_add;
                poz_start = poz;
                //poz -= 4;
                int lenght_of_langdb = BitConverter.ToInt32(first_database[0].lenght_of_langdb1, 0);
                poz_add = lenght_of_langdb;
                first_database[0].langdb = new byte[poz_add - 4];
                Array.Copy(binContent, poz, first_database[0].langdb, 0, poz_add - 4);
                poz += poz_add - 4;

                poz_add = binContent.Length - poz;
                first_database[0].other = new byte[poz_add];
                Array.Copy(binContent, poz, first_database[0].other, 0, poz_add);
                poz += poz_add;


                //начинаем langdb разбирать
                poz_add = 16;
                poz = 0;
                first_database[0].start_langdb = new byte[poz_add];
                Array.Copy(first_database[0].langdb, poz, first_database[0].start_langdb, 0, poz_add);
                poz_add = 0;
                poz = 16;
                #region
                try
                {
                    while (poz < first_database[0].langdb.Length)
                    {
                        //надо пропустить н-байт, которые хз_дата, а потом развинтить всё остальное
                        byte[] hz_data = new byte[20];
                        Array.Copy(first_database[0].langdb, poz, hz_data, 0, 20);
                        poz += 20;

                        poz += 4;
                        byte[] lenght_of_animation = new byte[4];
                        Array.Copy(first_database[0].langdb, poz, lenght_of_animation, 0, 4);
                        poz += 4;
                        //получаем анимацию
                        int len_animation = BitConverter.ToInt32(lenght_of_animation, 0);
                        string animation = Methods.ConvertHexToString(first_database[0].langdb, poz, len_animation, MainMenu.settings.ASCII_N, 1);
                        poz += len_animation;

                        poz += 4;
                        byte[] lenght_of_waw = new byte[4];
                        Array.Copy(first_database[0].langdb, poz, lenght_of_waw, 0, 4);
                        poz += 4;
                        //получаем озвучки
                        int len_waw = BitConverter.ToInt32(lenght_of_waw, 0);
                        string waw = Methods.ConvertHexToString(first_database[0].langdb, poz, len_waw, MainMenu.settings.ASCII_N, 1);
                        poz += len_waw;


                        //далее идут 8 байт.
                        //4 байта = длина текстового блока
                        byte[] lenght_of_textblok = new byte[4];
                        Array.Copy(first_database[0].langdb, poz, lenght_of_textblok, 0, 4);
                        poz += 4;
                        int len_of_text_blok = BitConverter.ToInt32(lenght_of_textblok, 0);
                        if (len_of_text_blok != 8)
                        {
                            //4 байта = номер строки в записи
                            byte[] count_text = new byte[4];
                            Array.Copy(first_database[0].langdb, poz, count_text, 0, 4);
                            poz += 4;

                            poz += 4;
                            byte[] lenght_of_name = new byte[4];
                            Array.Copy(first_database[0].langdb, poz, lenght_of_name, 0, 4);
                            poz += 4;
                            //получаем имя
                            int len_name = BitConverter.ToInt32(lenght_of_name, 0);

                            byte[] temp_hex_name = new byte[len_name];
                            Array.Copy(first_database[0].langdb, poz, temp_hex_name, 0, len_name);
                            string name = ASCIIEncoding.GetEncoding(1251).GetString(temp_hex_name);
                            poz += len_name;

                            poz += 4;
                            byte[] lenght_of_text = new byte[4];
                            Array.Copy(first_database[0].langdb, poz, lenght_of_text, 0, 4);
                            poz += 4;
                            //получаем текст
                            len_text = BitConverter.ToInt32(lenght_of_text, 0);
                            string text = Methods.ConvertHexToString(first_database[0].langdb, poz, len_text, MainMenu.settings.ASCII_N, 1);
                            //MessageBox.Show(database[number].text);
                            poz += len_text;

                            //получаем магические байты
                            int len_magic = len_of_text_blok - 8 - 8 - len_name - len_text - 8;

                            if (BitConverter.ToInt32(count_text, 0) == 1)
                            {
                                byte[] magic_bytes = new byte[len_magic];
                                Array.Copy(first_database[0].langdb, poz, magic_bytes, 0, len_magic);
                                database.Add(new Langdb(hz_data, null, lenght_of_textblok, count_text, lenght_of_name, name, lenght_of_text, text, lenght_of_waw, waw, lenght_of_animation, animation, magic_bytes));
                                poz += len_magic;
                            }
                            else if ((BitConverter.ToInt32(count_text, 0) == 2))
                            {
                                byte[] magic_bytes = new byte[8];
                                Array.Copy(first_database[0].langdb, poz, magic_bytes, 0, 8);
                                database.Add(new Langdb(hz_data, null, lenght_of_textblok, count_text, lenght_of_name, name, lenght_of_text, text, lenght_of_waw, waw, lenght_of_animation, animation, magic_bytes));
                                poz += 8;

                                poz += 4;
                                lenght_of_name = new byte[4];
                                Array.Copy(first_database[0].langdb, poz, lenght_of_name, 0, 4);
                                poz += 4;
                                //получаем имя
                                len_name = BitConverter.ToInt32(lenght_of_name, 0);

                                temp_hex_name = new byte[len_name];
                                Array.Copy(first_database[0].langdb, poz, temp_hex_name, 0, len_name);
                                name = ASCIIEncoding.GetEncoding(1251).GetString(temp_hex_name);
                                poz += len_name;

                                poz += 4;
                                lenght_of_text = new byte[4];
                                Array.Copy(first_database[0].langdb, poz, lenght_of_text, 0, 4);
                                poz += 4;
                                //получаем текст
                                len_text = BitConverter.ToInt32(lenght_of_text, 0);
                                text = Methods.ConvertHexToString(first_database[0].langdb, poz, len_text, MainMenu.settings.ASCII_N, 1);
                                //MessageBox.Show(database[number].text);
                                poz += len_text;

                                magic_bytes = new byte[8];
                                Array.Copy(first_database[0].langdb, poz, magic_bytes, 0, 8);
                                poz += 8;

                                database.Add(new Langdb(null, null, lenght_of_textblok, null, lenght_of_name, name, lenght_of_text, text, null, null, null, null, magic_bytes));
                            }
                            else
                            {
                                MessageBox.Show("Напишите разработчику! В одной из записей dlog находится три строки!");
                            }
                        }
                        else
                        {
                            //MessageBox.Show("Нулевая строчка!!!");
                            int len_magic = 4;
                            byte[] magic_bytes = new byte[len_magic];
                            Array.Copy(first_database[0].langdb, poz, magic_bytes, 0, len_magic);
                            poz += len_magic;
                            database.Add(new Langdb(hz_data, null, lenght_of_textblok, null, null, null, null, null, lenght_of_waw, waw, lenght_of_animation, animation, magic_bytes));
                        }
                        number++;
                        last_pozition = poz;
                    }
                }
                catch
                {
                    //MessageBox.Show('"' + "Конец" + '"' + ':' + (last_pozition - 8).ToString("x"));
                }
                #endregion
                first_database[0].end_langdb = new byte[(lenght_of_langdb - last_pozition - 4)];
                Array.Copy(first_database[0].langdb, last_pozition, first_database[0].end_langdb, 0, (lenght_of_langdb - last_pozition - 4));
                number--;
            }
        }
        public struct dlog
        {
            public byte[] head;
            public byte[] lenght_of_name_langdb_minus_4;
            public byte[] lenght_of_name_langdb;
            public string name_of_langdb;
            public byte[] hlam;
            public byte[] lenght_of_langdb1;
            public byte[] lenght_of_langdb2;
            public byte[] start_langdb;
            public byte[] langdb;
            public byte[] end_langdb;
            public byte[] other;
        }
        
        public static dlog[] first_database = new dlog[1];

        public static void ReadLandb(byte[] binContent, List<Langdb> landb, ref List<byte[]> header, ref byte[] lenght_of_all_text, ref List<byte[]> end_of_file)
        {

            int poz = 0;

            int UnicodeSupport = 1;
            byte[] ERTM = new byte[4];//первые 4 байта - версия
            Array.Copy(binContent, 0, ERTM, 0, 4);
            header.Add(ERTM);
            poz += 4;
            
            //string etalon = "5VSM";
            if (ConvertHexToString(ERTM, 0, 4) == "5VSM" || ConvertHexToString(ERTM, 0, 4) == "6VSM")
            {
                poz = 4;
                byte[] textBlock = new byte[4];//вторые - длина блока с текстом
                Array.Copy(binContent, poz, textBlock, 0, 4);
                int lenghtOfTextBlock = BitConverter.ToInt32(textBlock, 0);
                header.Add(textBlock);
                poz += 4;
                if (lenghtOfTextBlock > 60)
                {
                    byte[] otherBlock = new byte[4];//третьи - длина блока с остальным
                    Array.Copy(binContent, poz, otherBlock, 0, 4);
                    header.Add(otherBlock);
                    poz += 4;
                    byte[] somthing4Byte = new byte[4];
                    Array.Copy(binContent, poz, somthing4Byte, 0, 4);
                    header.Add(somthing4Byte);
                    poz += 4;
                    //проверяем версию
                    byte[] vers_bytes = new byte[4];
                    Array.Copy(binContent, poz, vers_bytes, 0, 4);
                    header.Add(vers_bytes);
                    poz += 4;
                    int vers = BitConverter.ToInt32(vers_bytes, 0);
                    int lenghtForHeaderForVersion = 0;//в зависимости от версии указанной в фаиле задаем в Case необходимые параметры
                    int startPoz = 0;
                    int len_magic = 0; //len_of_text_blok - 4 - 8 - len_name - len_text - 8; - В Волке вроде так

                    if (vers == 9 || vers == 10)
                    {
                        switch (vers)
                        {
                            case 9:
                                {
                                    lenghtForHeaderForVersion = 108;
                                    len_magic = 12;
                                    break;
                                }
                            case 10:
                                {
                                    lenghtForHeaderForVersion = 120;
                                    len_magic = 20;
                                    if (MainMenu.settings.unicodeSettings != 1) UnicodeSupport = 0;
                                    break;
                                }
                        }
                        byte[] headerHash = new byte[lenghtForHeaderForVersion];
                        Array.Copy(binContent, poz, headerHash, 0, lenghtForHeaderForVersion);
                        header.Add(headerHash);
                        poz += lenghtForHeaderForVersion;
                        startPoz = poz;

                        for (int q = 0; q < 5; q++)
                        {
                            byte[] tempH = new byte[4];
                            Array.Copy(binContent, poz, tempH, 0, 4);
                            header.Add(tempH);
                            poz += 4;
                        }
                    }
                    else
                    { MessageBox.Show("Version " + vers.ToString()); }

                    byte[] tempC = new byte[4];
                    Array.Copy(binContent, poz, tempC, 0, 4);
                    header.Add(tempC);
                    int countOfString = BitConverter.ToInt32(tempC, 0);

                    poz += 4;
                    int c = 0;
                    while (c < countOfString)
                    {
                        byte[] hz_data = new byte[16 + 40];
                        Array.Copy(binContent, poz, hz_data, 0, 16 + 40);
                        poz += 16 + 40;

                        byte[] langID = new byte[4];
                        Array.Copy(hz_data, 0, langID, 0, 4);

                        //4 байта = длина текстового блока
                        byte[] lenght_of_textblok = new byte[4];
                        Array.Copy(binContent, poz, lenght_of_textblok, 0, 4);
                        poz += 4;
                        int len_of_text_blok = BitConverter.ToInt32(lenght_of_textblok, 0);

                        poz += 4;//
                        byte[] lenght_of_name = new byte[4];
                        Array.Copy(binContent, poz, lenght_of_name, 0, 4);
                        poz += 4;
                        //получаем имя
                        int len_name = BitConverter.ToInt32(lenght_of_name, 0);
                        string name = Methods.ConvertHexToString(binContent, poz, len_name, MainMenu.settings.ASCII_N, UnicodeSupport);
                        poz += len_name;

                        poz += 4;
                        byte[] lenght_of_text = new byte[4];
                        Array.Copy(binContent, poz, lenght_of_text, 0, 4);
                        poz += 4;
                        //получаем текст
                        int len_text = BitConverter.ToInt32(lenght_of_text, 0);
                        string text = Methods.ConvertHexToString(binContent, poz, len_text, MainMenu.settings.ASCII_N, UnicodeSupport);

                        //MessageBox.Show(database[number].text);
                        poz += len_text;

                        byte[] magic_bytes = new byte[len_magic];
                        Array.Copy(binContent, poz, magic_bytes, 0, len_magic);
                        poz += len_magic;

                        landb.Add(new Langdb(hz_data, langID, lenght_of_textblok, null, lenght_of_name, name, lenght_of_text, text, null, null, null, null, magic_bytes));
                        c++;
                    }
                    int hvost = lenghtOfTextBlock + startPoz - poz;
                    byte[] t = new byte[hvost];
                    Array.Copy(binContent, poz, t, 0, hvost);
                    end_of_file.Add(t);
                    poz += hvost;

                    t = new byte[binContent.Length - poz];
                    Array.Copy(binContent, poz, t, 0, binContent.Length - poz);
                    end_of_file.Add(t);
                }
            }
            else
            {
                header.Remove(ERTM);
                byte[] vers_bytes = new byte[16];
                Array.Copy(binContent, 0, vers_bytes, 0, 4);
                Array.Copy(binContent, 4, vers_bytes, 0, 4);

                int vers = BitConverter.ToInt32(vers_bytes, 0);
                int end = 0;
                
                if (vers == 8)
                {
                    poz = 120;
                    byte[] headerH = new byte[poz];
                    Array.Copy(binContent, 0, headerH, 0, poz);
                    header.Add(headerH);
                    Array.Copy(binContent, poz, lenght_of_all_text, 0, 4);
                    poz += 4;
                    end = binContent.Length - BitConverter.ToInt32(lenght_of_all_text, 0) - poz + 8;
                    //end = 72;
                    //end2 = 92;
                }
                else if (vers == 9)
                {
                    poz = 132;
                    byte[] headerH = new byte[poz];
                    Array.Copy(binContent, 0, headerH, 0, poz);
                    header.Add(headerH);
                    Array.Copy(binContent, poz, lenght_of_all_text, 0, 4);
                    poz += 4;
                    end = binContent.Length - BitConverter.ToInt32(lenght_of_all_text, 0) - poz + 8;
                    //end = 80;
                    //end2 = 80;
                }
                else if (vers == 5)
                {
                    poz = binContent.Length;
                }
                else
                {
                    MessageBox.Show("Error. Please write me about this!");
                }
                //try
                {
                    while (poz < binContent.Length)
                    {
                        //if (poz == binContent.Length - end2)
                        //{
                        //    end_of_file = new byte[end2];
                        //    Array.Copy(binContent, poz, end_of_file, 0, end2);
                        //    break;
                        //}
                        //else 
                        if (poz == binContent.Length - end)
                        {
                            byte[] end_of_fileb = new byte[end];
                            Array.Copy(binContent, poz, end_of_fileb, 0, end);
                            end_of_file.Add(end_of_fileb);
                            break;
                        }

                        byte[] hz_data = new byte[16];
                        Array.Copy(binContent, poz, hz_data, 0, 16);
                        poz += 16;

                        byte[] langID = new byte[4];
                        Array.Copy(hz_data, 4, langID, 0, 4);

                        poz += 4;
                        byte[] lenght_of_animation = new byte[4];
                        Array.Copy(binContent, poz, lenght_of_animation, 0, 4);
                        poz += 4;

                        //получаем анимацию
                        int len_animation = BitConverter.ToInt32(lenght_of_animation, 0);
                        string animation = Methods.ConvertHexToString(binContent, poz, len_animation, MainMenu.settings.ASCII_N, 1);
                        poz += len_animation;

                        poz += 4;
                        byte[] lenght_of_waw = new byte[4];
                        Array.Copy(binContent, poz, lenght_of_waw, 0, 4);
                        poz += 4;
                        //получаем озвучки
                        int len_waw = BitConverter.ToInt32(lenght_of_waw, 0);
                        string waw = Methods.ConvertHexToString(binContent, poz, len_waw, MainMenu.settings.ASCII_N, 1);
                        poz += len_waw;

                        byte[] count_text = new byte[12];
                        Array.Copy(binContent, poz, count_text, 0, 12);
                        poz += 12;

                        //4 байта = длина текстового блока
                        byte[] lenght_of_textblok = new byte[4];
                        Array.Copy(binContent, poz, lenght_of_textblok, 0, 4);
                        poz += 4;
                        int len_of_text_blok = BitConverter.ToInt32(lenght_of_textblok, 0);

                        poz += 4;
                        byte[] lenght_of_name = new byte[4];
                        Array.Copy(binContent, poz, lenght_of_name, 0, 4);
                        poz += 4;
                        //получаем имя
                        int len_name = BitConverter.ToInt32(lenght_of_name, 0);
                        string name = Methods.ConvertHexToString(binContent, poz, len_name, MainMenu.settings.ASCII_N, 1);
                        poz += len_name;

                        poz += 4;
                        byte[] lenght_of_text = new byte[4];
                        Array.Copy(binContent, poz, lenght_of_text, 0, 4);
                        poz += 4;
                        //получаем текст
                        int len_text = BitConverter.ToInt32(lenght_of_text, 0);
                        string text = Methods.ConvertHexToString(binContent, poz, len_text, MainMenu.settings.ASCII_N, 1);
                        //MessageBox.Show(database[number].text);
                        poz += len_text;

                        //получаем магические байты
                        int len_magic = len_of_text_blok - 4 - 8 - len_name - len_text - 8;
                        byte[] magic_bytes = new byte[len_magic];
                        Array.Copy(binContent, poz, magic_bytes, 0, len_magic);
                        poz += len_magic;
                        landb.Add(new Langdb(hz_data, langID, lenght_of_textblok, count_text, lenght_of_name, name, lenght_of_text, text, lenght_of_waw, waw, lenght_of_animation, animation, magic_bytes));
                    }
                }
            }
            //catch { }
        }
        public static void ReadLandbTFTB(byte[] binContent, List<Langdb> landb, ref List<byte[]> header, ref byte[] lenght_of_all_text, ref List<byte[]> end_of_file)
        {
            int poz = 0;

            byte[] ERTM = new byte[4];//первые 4 байта - версия
            Array.Copy(binContent, 0, ERTM, 0, 4);
            header.Add(ERTM);
            poz += 4;

            string etalon = "5VSM";
            if (ConvertHexToString(ERTM, 0, 4) == etalon)
            {
                poz = 4;
                byte[] textBlock = new byte[4];//вторые - длина блока с текстом
                Array.Copy(binContent, poz, textBlock, 0, 4);
                int lenghtOfTextBlock = BitConverter.ToInt32(textBlock, 0);
                header.Add(textBlock);
                poz += 4;
                if (lenghtOfTextBlock > 60)
                {
                    byte[] otherBlock = new byte[4];//третьи - длина блока с остальным
                    Array.Copy(binContent, poz, otherBlock, 0, 4);
                    header.Add(otherBlock);
                    poz += 4;
                    byte[] somthing4Byte = new byte[4];
                    Array.Copy(binContent, poz, somthing4Byte, 0, 4);
                    header.Add(somthing4Byte);
                    poz += 4;
                    //проверяем версию
                    byte[] vers_bytes = new byte[4];
                    Array.Copy(binContent, poz, vers_bytes, 0, 4);
                    header.Add(vers_bytes);
                    poz += 4;
                    int vers = BitConverter.ToInt32(vers_bytes, 0);
                    int startPoz = 0;
                    if (vers == 10)
                    {

                        byte[] headerHash = new byte[120];
                        Array.Copy(binContent, poz, headerHash, 0, 120);
                        header.Add(headerHash);
                        poz += 120;
                        startPoz = poz;
                        for (int q = 0; q < 5; q++)
                        {
                            byte[] tempH = new byte[4];
                            Array.Copy(binContent, poz, tempH, 0, 4);
                            header.Add(tempH);
                            poz += 4;
                        }
                    }
                    else { MessageBox.Show("Version 8"); }

                    byte[] tempC = new byte[4];
                    Array.Copy(binContent, poz, tempC, 0, 4);
                    header.Add(tempC);
                    int countOfString = BitConverter.ToInt32(tempC, 0);

                    poz += 4;
                    int c = 0;
                    while (c < countOfString)
                    {
                        byte[] hz_data = new byte[16 + 40];
                        Array.Copy(binContent, poz, hz_data, 0, 16 + 40);
                        poz += 16 + 40;

                        byte[] langID = new byte[4];
                        Array.Copy(hz_data, 0, langID, 0, 4);

                        //4 байта = длина текстового блока
                        byte[] lenght_of_textblok = new byte[4];
                        Array.Copy(binContent, poz, lenght_of_textblok, 0, 4);
                        poz += 4;
                        int len_of_text_blok = BitConverter.ToInt32(lenght_of_textblok, 0);

                        poz += 4;//
                        byte[] lenght_of_name = new byte[4];
                        Array.Copy(binContent, poz, lenght_of_name, 0, 4);
                        poz += 4;
                        //получаем имя
                        int len_name = BitConverter.ToInt32(lenght_of_name, 0);
                        string name = Methods.ConvertHexToString(binContent, poz, len_name, MainMenu.settings.ASCII_N, 0);
                        poz += len_name;

                        poz += 4;
                        byte[] lenght_of_text = new byte[4];
                        Array.Copy(binContent, poz, lenght_of_text, 0, 4);
                        poz += 4;
                        //получаем текст
                        int len_text = BitConverter.ToInt32(lenght_of_text, 0);
                        string text = Methods.ConvertHexToString(binContent, poz, len_text, MainMenu.settings.ASCII_N, 0);
                        //MessageBox.Show(database[number].text);
                        poz += len_text;


                        int len_magic = 20;//len_of_text_blok - 4 - 8 - len_name - len_text - 8;
                        byte[] magic_bytes = new byte[len_magic];
                        Array.Copy(binContent, poz, magic_bytes, 0, len_magic);
                        poz += len_magic;

                        landb.Add(new Langdb(hz_data, langID, lenght_of_textblok, null, lenght_of_name, name, lenght_of_text, text, null, null, null, null, magic_bytes));
                        c++;
                    }
                    int hvost = lenghtOfTextBlock + startPoz - poz;
                    byte[] t = new byte[hvost];
                    Array.Copy(binContent, poz, t, 0, hvost);
                    end_of_file.Add(t);
                    poz += hvost;

                    t = new byte[binContent.Length - poz];
                    Array.Copy(binContent, poz, t, 0, binContent.Length - poz);
                    end_of_file.Add(t);

                }


            }
            else
            {
                byte[] vers_bytes = new byte[16];
                Array.Copy(binContent, 0, vers_bytes, 0, 4);
                Array.Copy(binContent, 4, vers_bytes, 0, 4);
                int vers = BitConverter.ToInt32(vers_bytes, 0);
                int end = 0;
                //int end2 = 0;
                if (vers == 8)
                {
                    poz = 120;
                    byte[] headerH = new byte[poz];
                    Array.Copy(binContent, 0, headerH, 0, poz);
                    header.Add(headerH);
                    Array.Copy(binContent, poz, lenght_of_all_text, 0, 4);
                    poz += 4;
                    end = binContent.Length - BitConverter.ToInt32(lenght_of_all_text, 0) - poz + 8;
                    //end = 72;
                    //end2 = 92;
                }
                else if (vers == 9)
                {
                    poz = 132;
                    byte[] headerH = new byte[poz];
                    Array.Copy(binContent, 0, headerH, 0, poz);
                    header.Add(headerH);
                    Array.Copy(binContent, poz, lenght_of_all_text, 0, 4);
                    poz += 4;
                    end = binContent.Length - BitConverter.ToInt32(lenght_of_all_text, 0) - poz + 8;
                    //end = 80;
                    //end2 = 80;
                }
                else if (vers == 5)
                {
                    poz = binContent.Length;
                }
                else
                {
                    MessageBox.Show("Error. Please write me about this!");
                }
                //try
                {
                    while (poz < binContent.Length)
                    {
                        //if (poz == binContent.Length - end2)
                        //{
                        //    end_of_file = new byte[end2];
                        //    Array.Copy(binContent, poz, end_of_file, 0, end2);
                        //    break;
                        //}
                        //else 
                        if (poz == binContent.Length - end)
                        {
                            byte[] end_of_fileb = new byte[end];
                            Array.Copy(binContent, poz, end_of_fileb, 0, end);
                            end_of_file.Add(end_of_fileb);
                            break;
                        }

                        byte[] hz_data = new byte[16];
                        Array.Copy(binContent, poz, hz_data, 0, 16);
                        poz += 16;

                        byte[] langID = new byte[4];
                        Array.Copy(hz_data, 0, langID, 0, 4);

                        poz += 4;
                        byte[] lenght_of_animation = new byte[4];
                        Array.Copy(binContent, poz, lenght_of_animation, 0, 4);
                        poz += 4;

                        //получаем анимацию
                        int len_animation = BitConverter.ToInt32(lenght_of_animation, 0);
                        string animation = Methods.ConvertHexToString(binContent, poz, len_animation, MainMenu.settings.ASCII_N, 0);
                        poz += len_animation;

                        poz += 4;
                        byte[] lenght_of_waw = new byte[4];
                        Array.Copy(binContent, poz, lenght_of_waw, 0, 4);
                        poz += 4;
                        //получаем озвучки
                        int len_waw = BitConverter.ToInt32(lenght_of_waw, 0);
                        string waw = Methods.ConvertHexToString(binContent, poz, len_waw, MainMenu.settings.ASCII_N, 0);
                        poz += len_waw;

                        byte[] count_text = new byte[12];
                        Array.Copy(binContent, poz, count_text, 0, 12);
                        poz += 12;

                        //4 байта = длина текстового блока
                        byte[] lenght_of_textblok = new byte[4];
                        Array.Copy(binContent, poz, lenght_of_textblok, 0, 4);
                        poz += 4;
                        int len_of_text_blok = BitConverter.ToInt32(lenght_of_textblok, 0);

                        poz += 4;
                        byte[] lenght_of_name = new byte[4];
                        Array.Copy(binContent, poz, lenght_of_name, 0, 4);
                        poz += 4;
                        //получаем имя
                        int len_name = BitConverter.ToInt32(lenght_of_name, 0);
                        string name = Methods.ConvertHexToString(binContent, poz, len_name, MainMenu.settings.ASCII_N, 0);
                        poz += len_name;

                        poz += 4;
                        byte[] lenght_of_text = new byte[4];
                        Array.Copy(binContent, poz, lenght_of_text, 0, 4);
                        poz += 4;
                        //получаем текст
                        int len_text = BitConverter.ToInt32(lenght_of_text, 0);
                        string text = Methods.ConvertHexToString(binContent, poz, len_text, MainMenu.settings.ASCII_N, 0);
                        //MessageBox.Show(database[number].text);
                        poz += len_text;

                        //получаем магические байты
                        int len_magic = len_of_text_blok - 4 - 8 - len_name - len_text - 8;
                        byte[] magic_bytes = new byte[len_magic];
                        Array.Copy(binContent, poz, magic_bytes, 0, len_magic);
                        poz += len_magic;
                        landb.Add(new Langdb(hz_data, langID, lenght_of_textblok, count_text, lenght_of_name, name, lenght_of_text, text, lenght_of_waw, waw, lenght_of_animation, animation, magic_bytes));
                    }
                }
            }
            //catch { }
        }

        public class chapterOfDDS
        {
            public byte[] number_of_chapter;
            public byte[] one;
            public byte[] lenght_of_chapter;
            public byte[] kratnost;
            public byte[] content_chapter;
            public byte[] hz;

            public chapterOfDDS() { }
            public chapterOfDDS(byte[] number_of_chapter, byte[] one, byte[] lenght_of_chapter, byte[] kratnost, byte[] content_chapter, byte[] hz)
            {
                this.number_of_chapter = number_of_chapter;
                this.one = one;
                this.lenght_of_chapter = lenght_of_chapter;
                this.kratnost = kratnost;
                this.content_chapter = content_chapter;
                this.hz = hz;
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

        public static void CreateDlog(dlog[] first_database, List<Langdb> database, byte version, string path)
        {
            //проверяем наличие файла, удаляем его и создаем пустой
            FileStream MyFileStream;
            if (System.IO.File.Exists(path) == true)
            {
                System.IO.File.Delete(path);
            }
            MyFileStream = new FileStream(path, FileMode.CreateNew);
            //записываем заголовок
            int numb = 0;

            //
            //высчитываем размер langdb-файла
            int sizeLangdb1 = 0;
            int sizeLangdb2 = 0;
            sizeLangdb1 += 4;//длина длины файла =)
            sizeLangdb1 += 16;//какая-то херь в начале
            for (int i = 0; i < database.Count; i++)
            {
                //int len_animation = database[i].animation.Length;
                //int len_waw = database[i].waw.Length;
                //int len_name = database[i].name.Length;
                //int len_text = database[i].text.Length;
                //int len_magic = database[i].magic_bytes.Length;
                //sizeLangdb1 += 20 + 4 + 4 + len_animation + 4 + 4 + len_waw + 8 + 4 + 4 + len_name + 4 + 4 + len_text + len_magic;
                if (BitConverter.ToInt32(database[i].lenght_of_textblok, 0) == 8)
                {
                    sizeLangdb1 += database[i].hz_data.Length;
                    sizeLangdb1 += database[i].lenght_of_animation.Length + database[i].lenght_of_animation.Length;//две длины анимации
                    sizeLangdb1 += database[i].animation.Length;
                    sizeLangdb1 += database[i].lenght_of_waw.Length + database[i].lenght_of_waw.Length;//две длины озвучки
                    sizeLangdb1 += database[i].waw.Length;
                    sizeLangdb1 += database[i].lenght_of_textblok.Length + GetSizeOfByteMassiv(database[i].count_text);// длина текстового блока
                    sizeLangdb1 += database[i].magic_bytes.Length;// длина магических байт   
                }
                else
                {
                    if (BitConverter.ToInt32(database[i].count_text, 0) == 1)
                    {
                        sizeLangdb1 += database[i].hz_data.Length;
                        sizeLangdb1 += database[i].lenght_of_animation.Length + database[i].lenght_of_animation.Length;//две длины анимации
                        sizeLangdb1 += database[i].animation.Length;
                        sizeLangdb1 += database[i].lenght_of_waw.Length + database[i].lenght_of_waw.Length;//две длины озвучки
                        sizeLangdb1 += database[i].waw.Length;
                        sizeLangdb1 += database[i].lenght_of_textblok.Length + GetSizeOfByteMassiv(database[i].count_text);// длина текстового блока
                        sizeLangdb1 += GetSizeOfByteMassiv(database[i].lenght_of_name) + GetSizeOfByteMassiv(database[i].lenght_of_name);//две длины имени
                        sizeLangdb1 += GetSizeOfString(database[i].name);
                        sizeLangdb1 += GetSizeOfByteMassiv(database[i].lenght_of_text) + GetSizeOfByteMassiv(database[i].lenght_of_text);//две длины текста
                        sizeLangdb1 += GetSizeOfString(database[i].text);
                        sizeLangdb1 += database[i].magic_bytes.Length;// длина магических байт   
                    }
                    else if (BitConverter.ToInt32(database[i].count_text, 0) == 2)
                    {
                        sizeLangdb1 += GetSizeOfByteMassiv(database[i].hz_data);
                        sizeLangdb1 += GetSizeOfByteMassiv(database[i].lenght_of_animation) + GetSizeOfByteMassiv(database[i].lenght_of_animation);//две длины анимации
                        sizeLangdb1 += GetSizeOfString(database[i].animation);
                        sizeLangdb1 += GetSizeOfByteMassiv(database[i].lenght_of_waw) + GetSizeOfByteMassiv(database[i].lenght_of_waw);//две длины озвучки
                        sizeLangdb1 += GetSizeOfString(database[i].waw);
                        sizeLangdb1 += GetSizeOfByteMassiv(database[i].lenght_of_textblok) + GetSizeOfByteMassiv(database[i].count_text);// длина текстового блока + инфа о количестве строк
                        sizeLangdb1 += GetSizeOfByteMassiv(database[i].lenght_of_name) + GetSizeOfByteMassiv(database[i].lenght_of_name);//две длины имени
                        sizeLangdb1 += GetSizeOfString(database[i].name);
                        sizeLangdb1 += GetSizeOfByteMassiv(database[i].lenght_of_text) + GetSizeOfByteMassiv(database[i].lenght_of_text);//две длины текста
                        sizeLangdb1 += GetSizeOfString(database[i].text);
                        sizeLangdb1 += GetSizeOfByteMassiv(database[i].magic_bytes);

                        sizeLangdb1 += GetSizeOfByteMassiv(database[i + 1].lenght_of_name) + GetSizeOfByteMassiv(database[i + 1].lenght_of_name);//две длины имени
                        sizeLangdb1 += GetSizeOfString(database[i + 1].name);
                        sizeLangdb1 += GetSizeOfByteMassiv(database[i + 1].lenght_of_text) + GetSizeOfByteMassiv(database[i + 1].lenght_of_text);//две длины текста
                        sizeLangdb1 += GetSizeOfString(database[i + 1].text);
                        sizeLangdb1 += database[i + 1].magic_bytes.Length;// длина магических байт  
                        i++;
                    }
                }
            }



            sizeLangdb1 += first_database[0].end_langdb.Length; //конец langdb

            sizeLangdb2 = sizeLangdb1 - first_database[0].end_langdb.Length - 12;

            // int temp = BitConverter.ToInt32(first_database[0].lenght_of_langdb1, 0);

            //MessageBox.Show(sizeLangdb2.ToString("x"));
            //длина файла найдена!
            byte[] temp = new byte[4];
            //Array.Copy(database[0].hz_data,0, ss, 0, 4);

            //MessageBox.Show("Высчитано: "+sizeLangdb.ToString() +" Оригинал: "+ (first_database[0].langdb.Length+4)+" "+ (BitConverter.ToInt32(ss,0).ToString()));
            //SaveHexInfo(MyFileStream, new byte[4]);
            //SaveHexInfo(MyFileStream, first_database[0].langdb);

            MyFileStream.Write(first_database[0].head, 0, first_database[0].head.Length);
            SaveStringInfo(MyFileStream, first_database[0].name_of_langdb, MainMenu.settings.ASCII_N, 1, true);
            MyFileStream.Write(first_database[0].hlam, 0, first_database[0].hlam.Length);
            //сохраняем всю длинну langdb
            temp = BitConverter.GetBytes(sizeLangdb1);
            MyFileStream.Write(temp, 0, temp.Length);
            MyFileStream.Write(first_database[0].start_langdb, 0, first_database[0].start_langdb.Length);
            //SaveHexInfo(MyFileStream, new byte[4]);//сохранить вторую длину

            ////записываем всё остальное
            while (numb < database.Count)
            {
                //сохраняем хз байты =)
                if (numb == 0)
                {
                    temp = BitConverter.GetBytes(sizeLangdb2);
                    MyFileStream.Write(temp, 0, temp.Length);
                    //MessageBox.Show("2 "+sizeLangdb2.ToString("x"));
                    byte[] hz_temp = new byte[(database[numb].hz_data.Length - 4)];
                    //Array.Copy(database[numb].hz_data,);
                    Array.Copy(database[numb].hz_data, 4, hz_temp, 0, (database[numb].hz_data.Length - 4));
                    MyFileStream.Write(hz_temp, 0, hz_temp.Length);
                }
                else
                {
                    MyFileStream.Write(database[numb].hz_data, 0, database[numb].hz_data.Length);
                }
                //анимация
                SaveStringInfo(MyFileStream, database[numb].animation, MainMenu.settings.ASCII_N, 1, true);
                //озвучка
                SaveStringInfo(MyFileStream, database[numb].waw, MainMenu.settings.ASCII_N, 1, true);

                if (BitConverter.ToInt32(database[numb].lenght_of_textblok, 0) != 8)
                {
                    if (BitConverter.ToInt32(database[numb].count_text, 0) == 1)
                    {
                        temp = BitConverter.GetBytes((database[numb].name.Length + database[numb].text.Length + 8 + 8 + 8 + database[numb].magic_bytes.Length));
                        MyFileStream.Write(temp, 0, temp.Length);

                        MyFileStream.Write(database[numb].count_text, 0, database[numb].count_text.Length);
                        //имя
                        SaveStringInfo(MyFileStream, database[numb].name, MainMenu.settings.ASCII_N, 1, true);
                        //текст
                        SaveStringInfo(MyFileStream, database[numb].text, MainMenu.settings.ASCII_N, 1, true);
                        //магические байты
                        MyFileStream.Write(database[numb].magic_bytes, 0, database[numb].magic_bytes.Length);
                    }
                    else if (BitConverter.ToInt32(database[numb].count_text, 0) == 2)
                    {
                        temp = BitConverter.GetBytes((database[numb].name.Length + database[numb].text.Length + 8 + 8 + 8 + database[numb].magic_bytes.Length + database[numb + 1].name.Length + database[numb + 1].text.Length) + 8 + 8 + database[numb + 1].magic_bytes.Length);
                        MyFileStream.Write(temp, 0, temp.Length);

                        MyFileStream.Write(database[numb].count_text, 0, database[numb].count_text.Length);
                        //имя
                        SaveStringInfo(MyFileStream, database[numb].name, MainMenu.settings.ASCII_N, 1, true);
                        //текст
                        SaveStringInfo(MyFileStream, database[numb].text, MainMenu.settings.ASCII_N, 1, true);
                        //имя
                        //магические байты
                        MyFileStream.Write(database[numb].magic_bytes, 0, database[numb].magic_bytes.Length);
                        SaveStringInfo(MyFileStream, database[numb + 1].name, MainMenu.settings.ASCII_N, 1, true);
                        //текст
                        SaveStringInfo(MyFileStream, database[numb + 1].text, MainMenu.settings.ASCII_N, 1, true);
                        //магические байты
                        MyFileStream.Write(database[numb + 1].magic_bytes, 0, database[numb + 1].magic_bytes.Length);
                        numb++;
                    }
                }
                else
                {
                    MyFileStream.Write(database[numb].lenght_of_textblok, 0, database[numb].lenght_of_textblok.Length);
                    //магические байты
                    MyFileStream.Write(database[numb].magic_bytes, 0, database[numb].magic_bytes.Length);
                }


                //счетчик++
                numb++;
            }

            MyFileStream.Write(first_database[0].end_langdb, 0, first_database[0].end_langdb.Length);
            //temp = BitConverter.GetBytes(83);
            //MyFileStream.Write(temp, 0, temp.Length);
            MyFileStream.Write(first_database[0].other, 0, first_database[0].other.Length);
            //закрываем поток
            MyFileStream.Close();
        }
        public static void CreateLandb(List<byte[]> header, List<Langdb> landb, List<byte[]> end_of_file, string path, string verOfGame)
        {
            //проверяем наличие файла, удаляем его и создаем пустой
            FileStream MyFileStream;
            if (System.IO.File.Exists(path) == true)
            {
                System.IO.File.Delete(path);
            }
            MyFileStream = new FileStream(path, FileMode.OpenOrCreate);

            //записываем заголовок
            if (ConvertHexToString(header[0], 0, 4) == "5VSM" || ConvertHexToString(header[0], 0, 4) == "6VSM")
            {
                int sizeLangdb = 8;
                for (int i = 0; i < landb.Count; i++)
                {
                    sizeLangdb += landb[i].hz_data.Length;
                    sizeLangdb += landb[i].lenght_of_textblok.Length;// длина текстового блока
                    sizeLangdb += GetSizeOfByteMassiv(landb[i].lenght_of_name) + GetSizeOfByteMassiv(landb[i].lenght_of_name);//две длины имени
                    sizeLangdb += BitConverter.ToInt32(landb[i].lenght_of_name, 0);//GetSizeOfString(landb[i].name);
                    sizeLangdb += GetSizeOfByteMassiv(landb[i].lenght_of_text) + GetSizeOfByteMassiv(landb[i].lenght_of_text);//две длины текста
                    sizeLangdb += BitConverter.ToInt32(landb[i].lenght_of_text, 0);//GetSizeOfString(landb[i].text);
                    sizeLangdb += landb[i].magic_bytes.Length;// длина магических байт   
                }
                sizeLangdb += end_of_file[0].Length + 16;
                byte[] hex_size = BitConverter.GetBytes(sizeLangdb);
                header[1] = hex_size;

                sizeLangdb += -end_of_file[0].Length - 16;
                hex_size = BitConverter.GetBytes(sizeLangdb);
                header[10] = hex_size;
                for (int i = 0; i < header.Count; i++)
                {
                    MyFileStream.Write(header[i], 0, header[i].Length);
                }
                int numb = 0;
                ////записываем всё остальное
                while (numb < landb.Count)
                {
                    MyFileStream.Write(landb[numb].hz_data, 0, landb[numb].hz_data.Length);
                    bool oldFormat = verOfGame == "WAU";

                    //byte[] temp = BitConverter.GetBytes((landb[numb].name.Length + landb[numb].text.Length + 8 + 8 + landb[numb].magic_bytes.Length) - 8);//-8 надо для борды, указывается всегда 12 хотя в Борде уже 20?

                    byte[] temp = BitConverter.GetBytes((BitConverter.ToInt32(landb[numb].lenght_of_name, 0) + BitConverter.ToInt32(landb[numb].lenght_of_text, 0) + 8 + 8 + landb[numb].magic_bytes.Length) - 8);//-8 надо для борды, указывается всегда 12 хотя в Борде уже 20?
                    if (verOfGame == "WAU") temp = BitConverter.GetBytes((BitConverter.ToInt32(landb[numb].lenght_of_name, 0) + BitConverter.ToInt32(landb[numb].lenght_of_text, 0) + 8 + 8 + landb[numb].magic_bytes.Length));//-8 надо для борды, указывается всегда 12 хотя в Борде уже 20?
                    //if ()
                    MyFileStream.Write(temp, 0, temp.Length);
                    if ((verOfGame == "TFTB") && (MainMenu.settings.unicodeSettings != 1))
                    {
                        //имя
                        SaveStringInfo(MyFileStream, landb[numb].name, MainMenu.settings.ASCII_N, 0, oldFormat);

                        //текст
                        if (landb[numb].text.IndexOf("\0") > 0)
                        {
                            SaveStringInfo(MyFileStream, landb[numb].text, MainMenu.settings.ASCII_N, 1, oldFormat);
                        }
                        else SaveStringInfo(MyFileStream, landb[numb].text, MainMenu.settings.ASCII_N, 0, oldFormat);
                    }
                    else
                    {
                        //имя
                        SaveStringInfo(MyFileStream, landb[numb].name, MainMenu.settings.ASCII_N, 1, oldFormat);
                        //текст
                        SaveStringInfo(MyFileStream, landb[numb].text, MainMenu.settings.ASCII_N, 1, oldFormat);
                    }

                    //магические байты
                    MyFileStream.Write(landb[numb].magic_bytes, 0, landb[numb].magic_bytes.Length);
                    //счетчик++
                    numb++;
                }
                for (int i = 0; i < end_of_file.Count; i++)
                {
                    MyFileStream.Write(end_of_file[i], 0, end_of_file[i].Length);
                }
                //закрываем поток
                MyFileStream.Close();
            }
            else
            {
                for (int i = 0; i < header.Count; i++)
                {
                    MyFileStream.Write(header[i], 0, header[i].Length);
                }
                int sizeLangdb = 8;
                for (int i = 0; i < landb.Count; i++)
                {

                    sizeLangdb += landb[i].hz_data.Length;
                    sizeLangdb += landb[i].lenght_of_animation.Length + landb[i].lenght_of_animation.Length;//две длины анимации
                    sizeLangdb += landb[i].animation.Length;
                    sizeLangdb += landb[i].lenght_of_waw.Length + landb[i].lenght_of_waw.Length;//две длины озвучки
                    sizeLangdb += landb[i].waw.Length;
                    sizeLangdb += landb[i].count_text.Length;
                    sizeLangdb += landb[i].lenght_of_textblok.Length;// длина текстового блока
                    sizeLangdb += GetSizeOfByteMassiv(landb[i].lenght_of_name) + GetSizeOfByteMassiv(landb[i].lenght_of_name);//две длины имени
                    sizeLangdb += GetSizeOfString(landb[i].name);
                    sizeLangdb += GetSizeOfByteMassiv(landb[i].lenght_of_text) + GetSizeOfByteMassiv(landb[i].lenght_of_text);//две длины текста
                    sizeLangdb += GetSizeOfString(landb[i].text);
                    sizeLangdb += landb[i].magic_bytes.Length;// длина магических байт   
                }
                byte[] hex_size = BitConverter.GetBytes(sizeLangdb);
                MyFileStream.Write(hex_size, 0, hex_size.Length);


                int numb = 0;
                ////записываем всё остальное
                while (numb < landb.Count)
                {

                    MyFileStream.Write(landb[numb].hz_data, 0, landb[numb].hz_data.Length);
                    //анимация
                    SaveStringInfo(MyFileStream, landb[numb].animation, MainMenu.settings.ASCII_N, 1, true);
                    //озвучка
                    SaveStringInfo(MyFileStream, landb[numb].waw, MainMenu.settings.ASCII_N, 1, true);

                    MyFileStream.Write(landb[numb].count_text, 0, landb[numb].count_text.Length);

                    byte[] temp = BitConverter.GetBytes((landb[numb].name.Length + landb[numb].text.Length + 4 + 8 + 8 + landb[numb].magic_bytes.Length));
                    MyFileStream.Write(temp, 0, temp.Length);

                    //имя
                    SaveStringInfo(MyFileStream, landb[numb].name, MainMenu.settings.ASCII_N, 1, true);
                    //текст
                    SaveStringInfo(MyFileStream, landb[numb].text, MainMenu.settings.ASCII_N, 1, true);

                    //магические байты
                    MyFileStream.Write(landb[numb].magic_bytes, 0, landb[numb].magic_bytes.Length);
                    //счетчик++
                    numb++;
                }
                for (int i = 0; i < end_of_file.Count; i++)
                {
                    MyFileStream.Write(end_of_file[i], 0, end_of_file[i].Length);
                }
            }
            //закрываем поток
            MyFileStream.Close();
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
                comboBox1.Items.Add(i + " " + MainMenu.gamelist[i].gamename);
            }

            #endregion

            comboBox1.SelectedIndex = MainMenu.settings.encKeyIndex;
            comboBox2.SelectedIndex = MainMenu.settings.versionEnc;
            checkUnicode.Checked = (MainMenu.settings.unicodeSettings == 0);
            if (MainMenu.settings.tsvFormat)
            {
                tsvFilesRB.Checked = true;
            }
            else txtFilesRB.Checked = true;
            checkEncDDS.Checked = MainMenu.settings.encDDSonly;
            checkIOS.Checked = MainMenu.settings.iOSsupport;
            checkEncLangdb.Checked = MainMenu.settings.encLangdb;
            CheckNewEngine.Checked = MainMenu.settings.encNewLua;
            checkBox1.Checked = MainMenu.settings.swizzleNintendoSwitch;

            if (MainMenu.settings.customKey && Methods.stringToKey(MainMenu.settings.encCustomKey) != null)
            {
                checkCustomKey.Checked = MainMenu.settings.customKey;
                textBox1.Text = MainMenu.settings.encCustomKey;
            }

            if(MainMenu.settings.ASCII_N == 1252)
            {
                //Make unvisible that option for users with windows-1252 encoding
                checkUnicode.Checked = true;
                checkUnicode.Visible = false;
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

        private void checkUnicode_CheckedChanged(object sender, EventArgs e)
        {
            MainMenu.settings.unicodeSettings = 1;
            if (checkUnicode.Checked) MainMenu.settings.unicodeSettings = 0;
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

        private void tsvFilesRB_CheckedChanged(object sender, EventArgs e)
        {
            if (tsvFilesRB.Checked)
            {
                MainMenu.settings.tsvFormat = true;
                Settings.SaveConfig(MainMenu.settings);
            }
        }

        private void txtFilesRB_CheckedChanged(object sender, EventArgs e)
        {
            if (txtFilesRB.Checked)
            {
                MainMenu.settings.tsvFormat = false;
                Settings.SaveConfig(MainMenu.settings);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            MainMenu.settings.swizzleNintendoSwitch = checkBox1.Checked;
            Settings.SaveConfig(MainMenu.settings);
        }
    }
}
