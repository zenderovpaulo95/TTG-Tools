using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Windows.Forms;

namespace TTG_Tools
{
    public delegate void ProgressHandler(int progress);
    public delegate void ReportHandler(string report);
    public delegate void GlossaryAdd(string orText, string trText);
    public delegate void RefAllText(List<TextEditor.AllText> allText);
    public delegate void RefAllText2(List<TextEditor.AllText> allText2);

    public class ForThreads
    {
        public event ProgressHandler Progress;
        public event ReportHandler ReportForWork;
        public event RefAllText BackAllText;
        public event RefAllText2 BackAllText2;

        public void ExtractDlogFile(FileInfo[] fi, int i)
        {
            List<AutoPacker.Langdb> database = new List<AutoPacker.Langdb>();
            FileStream fs = new FileStream(fi[i].FullName, FileMode.Open);
            byte[] binContent = Methods.ReadFull(fs);
            AutoPacker.ReadDlog(binContent, AutoPacker.first_database, database, 0);
            fs.Close();
            List<TextCollector.TXT_collection> all_text = new List<TextCollector.TXT_collection>();

            for (int q = 0; q < database.Count; q++)
            {
                if (BitConverter.ToInt32(database[q].lenght_of_textblok, 0) != 8)
                {
                    if (database[q].text != "" && database[q].name != "" || (database[q].text != "" && database[q].name == ""))
                    {
                        all_text.Add(new TextCollector.TXT_collection((q + 1), 0, database[q].name, database[q].text, false));
                    }
                }
            }
            List<TextCollector.TXT_collection> all_text_for_export = new List<TextCollector.TXT_collection>();
            TextCollector.CreateExportingTXTfromOneFile(all_text, ref all_text_for_export);

            string path = "";
            if (i > 0)
            {
                if (fi[i].Extension == ".dlog" && fi[i - 1].Extension == ".txt" && Methods.DeleteCommentary(AutoPacker.GetNameOnly(i - 1), "(", ")") == AutoPacker.GetNameOnly(i))
                {
                    path = MainMenu.settings.pathForOutputFolder + "\\" + fi[i - 1].Name;
                }
                else
                {
                    path = MainMenu.settings.pathForOutputFolder + "\\" + Methods.GetNameOfFileOnly(fi[i].Name, ".dlog") + ".txt";
                }
            }
            else
            {
                path = MainMenu.settings.pathForOutputFolder + "\\" + Methods.GetNameOfFileOnly(fi[i].Name, ".dlog") + ".txt";
            }
            Methods.DeleteCurrentFile(path);

            FileStream MyExportStream = new FileStream(path, FileMode.OpenOrCreate);
            int w = 0;
            while (w < all_text_for_export.Count)
            {
                all_text_for_export[w].text = all_text_for_export[w].text.Replace("\n", "\r\n");
                TextCollector.SaveString(MyExportStream, (all_text_for_export[w].number + ") " + all_text_for_export[w].name + "\r\n"), MainMenu.settings.ASCII_N);
                //TextCollector.SaveString(MyExportStream, (BitConverter.ToString(database[all_text_for_export[w].number-1].hz_data)+"\r\n"), MainMenu.settings.ASCII_N);
                TextCollector.SaveString(MyExportStream, (all_text_for_export[w].text + "\r\n"), MainMenu.settings.ASCII_N);
                w++;
            }
            MyExportStream.Close();
            if (i > 0)
            {
                if (fi[i].Extension == ".dlog" && fi[i - 1].Extension == ".txt" && Methods.DeleteCommentary(AutoPacker.GetNameOnly(i - 1), "(", ")") == AutoPacker.GetNameOnly(i))
                {
                    ReportForWork("File " + fi[i].Name + " exported in " + fi[i - 1].Name);
                }
                else
                {
                    ReportForWork("File " + fi[i].Name + " exported in " + Methods.GetNameOfFileOnly(fi[i].Name, ".dlog") + ".txt");
                }
            }
            else
            {
                ReportForWork("File " + fi[i].Name + " exported in " + Methods.GetNameOfFileOnly(fi[i].Name, ".dlog") + ".txt");
            }
        }

        public void ExtractLandbFile(FileInfo[] fi, int i, string versionOfGame)
        {
            List<byte[]> header = new List<byte[]>();
            byte[] lenght_of_all_text = new byte[4];

            List<byte[]> end_of_file = new List<byte[]>();
            List<AutoPacker.Langdb> landb = new List<AutoPacker.Langdb>();
            FileStream fs = new FileStream(fi[i].FullName, FileMode.Open);
            byte[] binContent = Methods.ReadFull(fs);
            AutoPacker.ReadLandb(binContent, landb, ref header, ref lenght_of_all_text, ref end_of_file);
            fs.Close();

            byte[] new_header = new byte[4];
            Array.Copy(binContent, 0, new_header, 0, 4);
            if (Encoding.ASCII.GetString(new_header) == "5VSM")
            {
                byte[] vers = new byte[4];
                Array.Copy(binContent, 16, vers, 0, 4);
                switch (BitConverter.ToInt32(vers, 0))
                {
                    case 9:
                        versionOfGame = "WAU";
                        break;
                    case 10:
                        versionOfGame = "TFTB";
                        break;
                }
            }
            else if (Encoding.ASCII.GetString(new_header) == "6VSM")
            {
                byte[] vers = new byte[4];
                Array.Copy(binContent, 16, vers, 0, 4);

                if (BitConverter.ToInt32(vers, 0) == 10) versionOfGame = "Batman";
            }

            if (landb.Count > 0)
            {
                List<TextCollector.TXT_collection> all_text = new List<TextCollector.TXT_collection>();

                for (int q = 0; q < landb.Count; q++)
                {
                    if (landb[q].text != "" && landb[q].name != "" || (landb[q].text != "" && landb[q].name == ""))
                    {
                        if (MainMenu.settings.exportRealID) all_text.Add(new TextCollector.TXT_collection((q + 1), BitConverter.ToUInt32(landb[q].realID, 0), landb[q].name, landb[q].text, false));
                        else all_text.Add(new TextCollector.TXT_collection((q + 1), 0, landb[q].name, landb[q].text, false));
                    }
                }
                List<TextCollector.TXT_collection> all_text_for_export = new List<TextCollector.TXT_collection>();
                TextCollector.CreateExportingTXTfromOneFile(all_text, ref all_text_for_export);

                string path = "";
                if (i > 0)
                {
                    if (fi[i].Extension == ".landb" && fi[i - 1].Extension == ".txt" && Methods.DeleteCommentary(AutoPacker.GetNameOnly(i - 1), "(", ")") == AutoPacker.GetNameOnly(i))
                    {
                        path = MainMenu.settings.pathForOutputFolder + "\\" + fi[i - 1].Name;
                    }
                    else
                    {
                        if (MainMenu.settings.tsvFormat) path = MainMenu.settings.pathForOutputFolder + "\\" + Methods.GetNameOfFileOnly(fi[i].Name, ".landb") + ".tsv";
                        else path = MainMenu.settings.pathForOutputFolder + "\\" + Methods.GetNameOfFileOnly(fi[i].Name, ".landb") + ".txt";
                    }
                }
                else
                {
                    if (MainMenu.settings.tsvFormat) path = MainMenu.settings.pathForOutputFolder + "\\" + Methods.GetNameOfFileOnly(fi[i].Name, ".landb") + ".tsv";
                    else path = MainMenu.settings.pathForOutputFolder + "\\" + Methods.GetNameOfFileOnly(fi[i].Name, ".landb") + ".txt";
                }
                Methods.DeleteCurrentFile(path);


                FileStream MyExportStream = new FileStream(path, FileMode.CreateNew);
                int w = 0;
                while (w < all_text_for_export.Count)
                {
                    byte[] name_of_file = new byte[4];
                    Array.Copy(landb[all_text_for_export[w].number - 1].hz_data, 0, name_of_file, 0, 4);
                    int qwer = BitConverter.ToInt32(name_of_file, 0);
                    if (!MainMenu.settings.tsvFormat) all_text_for_export[w].text = all_text_for_export[w].text.Replace("\n", "\r\n");
                    else all_text_for_export[w].text = all_text_for_export[w].text.Replace("\n", "\\n");

                    //тут добавил
                    if (versionOfGame != "TFTB")
                    {
                        if (MainMenu.settings.unicodeSettings == 1)
                        {
                            byte[] temp_string = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetBytes(all_text_for_export[w].name);
                            temp_string = Encoding.Convert(Encoding.GetEncoding(MainMenu.settings.ASCII_N), Encoding.UTF8, temp_string);
                            all_text_for_export[w].name = UnicodeEncoding.UTF8.GetString(temp_string);

                            if (all_text_for_export[w].text.IndexOf("\0") > 0)
                            {
                                all_text_for_export[w].text = all_text_for_export[w].text.Replace("\0", "(ANSI)");
                            }
                            else
                            {
                                temp_string = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetBytes(all_text_for_export[w].text);
                                temp_string = Encoding.Convert(Encoding.GetEncoding(MainMenu.settings.ASCII_N), Encoding.UTF8, temp_string);
                                all_text_for_export[w].text = UnicodeEncoding.UTF8.GetString(temp_string);
                            }
                        }
                        else
                        {
                            if (all_text_for_export[w].text.IndexOf("\0") > 0)
                            {
                                all_text_for_export[w].text = all_text_for_export[w].text.Replace("\0", "(ANSI)");
                            }
                        }
                    }
                    if (MainMenu.settings.tsvFormat)
                    {
                        string tsv_str = all_text_for_export[w].number + "\t" + all_text_for_export[w].name + "\t" + all_text_for_export[w].text + "\r\n";
                        if (MainMenu.settings.exportRealID) tsv_str = all_text_for_export[w].realId + "\t" + all_text_for_export[w].name + "\t" + all_text_for_export[w].text + "\r\n";
                        TextCollector.SaveString(MyExportStream, tsv_str, MainMenu.settings.unicodeSettings);
                    }
                    else
                    {
                        if (MainMenu.settings.exportRealID) TextCollector.SaveString(MyExportStream, (all_text_for_export[w].realId + ") " + all_text_for_export[w].name + "\r\n"), MainMenu.settings.unicodeSettings);
                        else TextCollector.SaveString(MyExportStream, (all_text_for_export[w].number + ") " + all_text_for_export[w].name + "\r\n"), MainMenu.settings.unicodeSettings); //+ qwer.ToString() +  "\r\n"), MainMenu.settings.ASCII_N);
                        TextCollector.SaveString(MyExportStream, (all_text_for_export[w].text + "\r\n"), MainMenu.settings.unicodeSettings);
                    }
                    w++;

                }
                MyExportStream.Close();

                if (i > 0)
                {
                    if (fi[i].Extension == ".landb" && fi[i - 1].Extension == ".txt" && Methods.DeleteCommentary(AutoPacker.GetNameOnly(i - 1), "(", ")") == AutoPacker.GetNameOnly(i))
                    {
                        //listBox1.Items.Add("File " + fi[i].Name + " exported in " + fi[i - 1].Name);
                        ReportForWork("File " + fi[i].Name + " exported in " + fi[i - 1].Name);
                    }
                    else
                    {
                        //listBox1.Items.Add("File " + fi[i].Name + " exported in " + Methods.GetNameOfFileOnly(fi[i].Name, ".landb") + ".txt");
                        if (MainMenu.settings.tsvFormat) ReportForWork("File " + fi[i].Name + " exported in " + Methods.GetNameOfFileOnly(fi[i].Name, ".landb") + ".tsv");
                        else ReportForWork("File " + fi[i].Name + " exported in " + Methods.GetNameOfFileOnly(fi[i].Name, ".landb") + ".txt");
                    }
                }
                else
                {
                    //listBox1.Items.Add("File " + fi[i].Name + " exported in " + Methods.GetNameOfFileOnly(fi[i].Name, ".landb") + ".txt");
                    if (MainMenu.settings.tsvFormat) ReportForWork("File " + fi[i].Name + " exported in " + Methods.GetNameOfFileOnly(fi[i].Name, ".landb") + ".tsv");
                    else ReportForWork("File " + fi[i].Name + " exported in " + Methods.GetNameOfFileOnly(fi[i].Name, ".landb") + ".txt");
                }
            }
            else
            {
                ReportForWork("File " + fi[i].Name + " is EMPTY!");
            }
        }

        public void CreateExportingTXTfromAllTextN(ref List<TextEditor.AllText> allText)
        {
            for (int i = 0; i < allText.Count; i++)
            {
                if (allText[i].exported == false && allText[i].isChecked == false)
                {
                    allText[i].exported = true;
                    for (int j = 0; j < allText.Count; j++)
                    {
                        if (!MainMenu.settings.exportRealID)
                        {
                            if (TextCollector.IsStringsSame(allText[i].orText, allText[j].orText, false) && allText[j].exported == false)
                            {
                                allText[j].exported = true;
                                allText.Insert(i + 1, allText[j]);
                                allText[i + 1].exported = true;
                                allText.RemoveAt(j + 1);
                            }
                        }
                        else
                        {
                            if ((allText[i].realID == allText[j].realID) && allText[j].exported == false)
                            {
                                allText[j].exported = true;
                                allText.Insert(i + 1, allText[j]);
                                allText[i + 1].exported = true;
                                allText.RemoveAt(j + 1);
                            }
                        }
                    }
                }
                allText[i].isChecked = true;
                Progress(i);
            }
        }

        public void CreateExportingTXTfromAllText(object inputList)
        {
            List<TextEditor.AllText> allText = inputList as List<TextEditor.AllText>;
            CreateExportingTXTfromAllTextN(ref allText);
            BackAllText(allText);
        }

        public void CreateExportingTXTfromAllText2(object inputList)
        {

            List<TextEditor.AllText> allText2 = inputList as List<TextEditor.AllText>;
            CreateExportingTXTfromAllTextN(ref allText2);
            BackAllText2(allText2);
        }

        public void CreateGlossaryFromFirstAndSecondAllText(object TwoList)
        {
            //потом передавать мега-класс, содержащий всё это 
            List<List<TextEditor.AllText>> twoAllText = TwoList as List<List<TextEditor.AllText>>;
            List<TextEditor.AllText> firstText = twoAllText[0];
            List<TextEditor.AllText> secondText = twoAllText[1];
            List<TextEditor.Glossary> glossary = new List<TextEditor.Glossary>();

            for (int i = 0; i < firstText.Count; i++)
            {
                if (firstText[i].orText.IndexOf('–') > -1) firstText[i].orText = firstText[i].orText.Replace('–', '-');
                if (firstText[i].orText.IndexOf('—') > -1) firstText[i].orText = firstText[i].orText.Replace('—', '-');
                if (firstText[i].orText.IndexOf('―') > -1) firstText[i].orText = firstText[i].orText.Replace('―', '-');
                if (firstText[i].orText.IndexOf('‗') > -1) firstText[i].orText = firstText[i].orText.Replace('‗', '_');
                if (firstText[i].orText.IndexOf('‘') > -1) firstText[i].orText = firstText[i].orText.Replace('‘', '\'');
                if (firstText[i].orText.IndexOf('’') > -1) firstText[i].orText = firstText[i].orText.Replace('’', '\'');
                if (firstText[i].orText.IndexOf('‚') > -1) firstText[i].orText = firstText[i].orText.Replace('‚', ',');
                if (firstText[i].orText.IndexOf('‛') > -1) firstText[i].orText = firstText[i].orText.Replace('‛', '\'');
                if (firstText[i].orText.IndexOf('“') > -1) firstText[i].orText = firstText[i].orText.Replace('“', '\"');
                if (firstText[i].orText.IndexOf('”') > -1) firstText[i].orText = firstText[i].orText.Replace('”', '\"');
                if (firstText[i].orText.IndexOf('„') > -1) firstText[i].orText = firstText[i].orText.Replace('„', '\"');
                if (firstText[i].orText.IndexOf('…') > -1) firstText[i].orText = firstText[i].orText.Replace("…", "...");
                if (firstText[i].orText.IndexOf('′') > -1) firstText[i].orText = firstText[i].orText.Replace('′', '\'');
                if (firstText[i].orText.IndexOf('″') > -1) firstText[i].orText = firstText[i].orText.Replace('″', '\"');

                for (int j = 0; j < secondText.Count; j++)
                {
                    if (secondText[j].orText.IndexOf('–') > -1) secondText[j].orText = secondText[j].orText.Replace('–', '-');
                    if (secondText[j].orText.IndexOf('—') > -1) secondText[j].orText = secondText[j].orText.Replace('—', '-');
                    if (secondText[j].orText.IndexOf('―') > -1) secondText[j].orText = secondText[j].orText.Replace('―', '-');
                    if (secondText[j].orText.IndexOf('‗') > -1) secondText[j].orText = secondText[j].orText.Replace('‗', '_');
                    if (secondText[j].orText.IndexOf('‘') > -1) secondText[j].orText = secondText[j].orText.Replace('‘', '\'');
                    if (secondText[j].orText.IndexOf('’') > -1) secondText[j].orText = secondText[j].orText.Replace('’', '\'');
                    if (secondText[j].orText.IndexOf('‚') > -1) secondText[j].orText = secondText[j].orText.Replace('‚', ',');
                    if (secondText[j].orText.IndexOf('‛') > -1) secondText[j].orText = secondText[j].orText.Replace('‛', '\'');
                    if (secondText[j].orText.IndexOf('“') > -1) secondText[j].orText = secondText[j].orText.Replace('“', '\"');
                    if (secondText[j].orText.IndexOf('”') > -1) secondText[j].orText = secondText[j].orText.Replace('”', '\"');
                    if (secondText[j].orText.IndexOf('„') > -1) secondText[j].orText = secondText[j].orText.Replace('„', '\"');
                    if (secondText[j].orText.IndexOf('…') > -1) secondText[j].orText = secondText[j].orText.Replace("…", "...");
                    if (secondText[j].orText.IndexOf('′') > -1) secondText[j].orText = secondText[j].orText.Replace('′', '\'');
                    if (secondText[j].orText.IndexOf('″') > -1) secondText[j].orText = secondText[j].orText.Replace('″', '\"');

                    if ((firstText[i].orText != "" || firstText[i].orText != string.Empty) && firstText[i].orText == secondText[j].orText)
                    {
                        if (IsStringExist(secondText[j].orText, glossary) == false)
                        {
                            glossary.Add(new TextEditor.Glossary(firstText[i].orText, firstText[i].trText, false, false));
                        }
                    }
                }
                Progress(i);
            }
            FileStream ExportStream = new FileStream("txt.txt", FileMode.OpenOrCreate);
            for (int i = 0; i < glossary.Count; i++)
            {
                TextCollector.SaveString(ExportStream, glossary[i].orText + "\r\n", MainMenu.settings.ASCII_N);
                TextCollector.SaveString(ExportStream, glossary[i].trText + "\r\n\r\n", MainMenu.settings.ASCII_N);
            }
            ExportStream.Close();


            for (int i = 0; i < secondText.Count; i++)
            {
                int q = IsStringinGlossary(glossary, secondText[i].orText, secondText[i].trText);
                if (q >= 0)
                {
                    secondText[i].trText = glossary[q].trText;
                }
            }
            BackAllText2(secondText);
        }

        public int IsStringinGlossary(List<TextEditor.Glossary> glossary, string orText, string trText)
        {

            for (int e = 0; e < glossary.Count; e++)
            {
                if (glossary[e].orText == orText && glossary[e].trText != trText)
                {
                    //System.Windows.Forms.MessageBox.Show(glossary[e].trText+"\r\n"+trText);
                    return e;
                }
            }
            return -1;
        }

        public bool IsStringExist(string str, List<TextEditor.Glossary> glossary)
        {
            for (int e = 0; e < glossary.Count; e++)
            {
                if (glossary[e].orText == str)
                {
                    return true;
                }
            }
            return false;
        }

        //Импорт
        public void DoImportEncoding(object parametres)
        {

            List<string> param = parametres as List<string>;
            string versionOfGame = param[0];
            string encrypt = param[1];
            string destinationForExport;
            string whatImport;
            string pathInput = param[2];
            string pathOutput = param[3];
            bool deleteFromInputSource = param[5] == "True";//false;
            bool deleteFromInputImported = param[4] == "True";//false;
            int version = Convert.ToInt32(param[6]);
            bool FullEncrypt = param[7] == "True";
            byte[] encKey = null;
            /*if (param[5] == "True")
            {
                deleteFromInputSource = true;
            }
            if (param[4] == "True")
            {
                deleteFromInputImported = true;
            }*/

            bool[] show = { false, false, false, false };

            List<string> destination = new List<string>();
            destination.Add(".d3dtx");
            destination.Add(".d3dtx");
            destination.Add(".langdb");
            destination.Add(".langdb");
            destination.Add(".font");
            destination.Add(".landb");
            destination.Add(".landb");
            destination.Add(".prop");
            List<string> extention = new List<string>();
            extention.Add(".dds");
            extention.Add(".pvr");
            extention.Add(".txt");
            extention.Add(".tsv");
            extention.Add("NOTHING");
            extention.Add(".txt");
            extention.Add(".tsv");
            extention.Add(".txt");

            for (int d = 0; d < destination.Count; d++)
            {
                destinationForExport = destination[d];
                whatImport = extention[d];

                if (Directory.Exists(pathInput) && Directory.Exists(pathOutput))
                {
                    DirectoryInfo dir = new DirectoryInfo(pathInput);
                    FileInfo[] inputFiles = dir.GetFiles('*' + destinationForExport);
                    
                        for (int i = 0; i < inputFiles.Length; i++)
                        {
                            int countCorrectWork = 0;//переменная для подсчёта корректного импорта текстур
                            int countOfAllFiles = 0;//всего файлов для импорта
                            string onlyNameImporting = inputFiles[i].Name.Split('(')[0].Split('.')[0];
                            for (int q = 0; q < 2; q++)
                            {
                                bool correct_work = true;

                                FileInfo[] fileDestination;
                                if (q == 0)
                                {
                                    fileDestination = dir.GetFiles(onlyNameImporting + whatImport);
                                }
                                else
                                {
                                    fileDestination = dir.GetFiles(onlyNameImporting + "(*)" + whatImport);
                                }
                                countOfAllFiles += fileDestination.Count();

                                for (int j = 0; j < fileDestination.Length; j++)
                                {
                                    switch (destinationForExport)
                                    {
                                        case ".d3dtx":
                                            {
                                            //ImportDDSinD3DTX(inputFiles, fileDestination, i, j, pathOutput, ref correct_work, versionOfGame);
                                            string result = TextureWorker.DoWork(inputFiles[i].FullName, pathOutput, false, FullEncrypt, ref encKey, version);
                                            ReportForWork(result);
                                            show[0] = true;    
                                                break;
                                            }
                                        case ".landb":
                                            {
                                                ImportTXTinLANDB(inputFiles, fileDestination, i, j, pathOutput, ref correct_work, versionOfGame);
                                            show[1] = true;
                                                break;
                                            }
                                        case ".langdb":
                                            {
                                                ImportTXTinLANGDB(inputFiles, fileDestination, i, j, pathOutput, ref correct_work, versionOfGame);
                                            show[2] = true;
                                                break;
                                            }
                                        case ".prop":
                                            {
                                                ImportTXTinPROP(inputFiles, fileDestination, i, j, pathOutput, ref correct_work);
                                            show[3] = true;
                                                break;
                                            }
                                        default:
                                            {
                                                MessageBox.Show("Error in Switch!");
                                                break;
                                            }
                                    }
                                    if (correct_work)//если файл импортирован был хорошо, то удаляем
                                    {
                                        countCorrectWork++;
                                        if (deleteFromInputImported)
                                        {
                                            Methods.DeleteCurrentFile(fileDestination[j].FullName);
                                        }
                                    }
                                }
                            }
                            if (deleteFromInputSource && countCorrectWork == countOfAllFiles)//если все файлы были импортированы правильно, то удаляем при необходимости файл
                            {
                                Methods.DeleteCurrentFile(inputFiles[i].FullName);
                            }
                        }
                }
                else
                {
                    ReportForWork("Check for existing Input and Output folders and check pathes in config.xml!");
                    return;
                }
            }

            string message = "";

            for (int i = 0; i < show.Length; i++)
            {
                if (show[i])
                {
                    message = "IMPORT OF ALL *";
                    switch (i)
                    {
                        case 0:
                            message += ".D3DTX";
                            break;

                        case 1:
                            message += ".LANGDB";
                            break;

                        case 2:
                            message += ".LANDB";
                            break;

                        case 3:
                            message += ".PROP";
                            break;
                    }
                    message += " IS COMPLETE!";

                    ReportForWork(message);
                }
            }
        }

        public void ImportTXTinPROP(FileInfo[] inputFiles, FileInfo[] fileDestination, int i, int j, string pathOutput, ref bool correctWork)
        {
            FileStream fs = new FileStream(inputFiles[i].FullName, FileMode.Open);
            byte[] binContent = Methods.ReadFull(fs);
            fs.Close();

            List<AutoPacker.Prop> proplist = new List<AutoPacker.Prop>();
            List<string> strs = new List<string>();

            byte[] header = null, countOfBlock = null, lengthAllText = null;
            int type = -1;

            AutoPacker.ReadProp(binContent, proplist, ref header, ref countOfBlock, ref lengthAllText, ref type);

            string[] texts = File.ReadAllLines(fileDestination[j].FullName);

            bool number_find = false;
            int n_str = -1;
            //int number_of_next_list = 0;

            for (int c = 0; c < texts.Length; c++)
            {
                if (texts[c].IndexOf(")") > -1 && number_find == false)
                {
                    if ((Methods.IsNumeric(texts[c].Substring(0, texts[c].IndexOf(")")))))
                    {
                        number_find = true;
                        n_str++;
                        strs.Add("");
                    }
                    else
                    {
                        strs[n_str] += "\r\n" + texts[c];
                        number_find = false;
                    }
                }
                else
                {
                    strs[n_str] += texts[c];
                    number_find = false;
                }
            }

            if (proplist.Count == strs.Count)
            {
                try
                {
                    for (int c = 0; c < proplist.Count; c++)
                    {
                        proplist[c].text = strs[c];
                        if (proplist[c].text.Contains("\r\n")) proplist[c].text = proplist[c].text.Replace("\r\n", "\n");
                        byte[] tmp = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetBytes(proplist[c].text);
                        proplist[c].lenght_of_text = BitConverter.GetBytes((int)tmp.Length);
                    }

                    AutoPacker.CreateProp(header, countOfBlock, proplist, (pathOutput + "\\" + inputFiles[i].Name), type);
                    ReportForWork("File " + Methods.GetNameOfFileOnly(inputFiles[i].Name, ".prop") + ".txt imported in " + inputFiles[i].Name);
                }
                catch
                {
                    ReportForWork("Something wrong with import file " + inputFiles[i].Name);
                }
            }
            else ReportForWork("Count of strings doesn't equal in files " + inputFiles[i].Name + " & " + fileDestination[j].Name);
        }

        public void findStringByID(List<TextCollector.TXT_collection> all_text, int c, ref int id)
        {
            for (int i = 0; i < all_text.Count; i++)
            {
                if (all_text[i].number == c)
                {
                    id = i;
                    break;
                }
            }
        }

        public void ExportPROP(FileInfo inputFile, string destFilePath)
        {
            FileStream fs = new FileStream(inputFile.FullName, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);

            if (File.Exists(MainMenu.settings.pathForOutputFolder + "\\" + destFilePath)) File.Delete(MainMenu.settings.pathForOutputFolder + "\\" + destFilePath);

            FileStream fsw = new FileStream(MainMenu.settings.pathForOutputFolder + "\\" + destFilePath, FileMode.CreateNew);
            StreamWriter sw = new StreamWriter(fsw);

            try
            {
                byte[] header = br.ReadBytes(4);
                int count = br.ReadInt32();
                for(int i = 0; i < count; i++)
                {
                    int len = br.ReadInt32();
                    byte[] name = br.ReadBytes(len);
                    byte[] block = br.ReadBytes(4);
                }

                int one = br.ReadInt32();
                int zero = br.ReadInt32();

                ClassesStructs.PropertyClass.ClassPropertySet propClass = new ClassesStructs.PropertyClass.ClassPropertySet();
                propClass.properties.imProps.block_length = br.ReadInt32();
                propClass.properties.imProps.count = br.ReadInt32();

                propClass.properties.imProps.import_props = new ClassesStructs.PropertyClass.ClassPropertySet.im_props[propClass.properties.imProps.count];

                for(int i = 0; i < propClass.properties.imProps.count; i++)
                {
                    propClass.properties.imProps.import_props[i].str_name_len = br.ReadInt32();
                    byte[] tmp = br.ReadBytes(propClass.properties.imProps.import_props[i].str_name_len);
                    propClass.properties.imProps.import_props[i].str_name = Encoding.ASCII.GetString(tmp);

                    sw.WriteLine("refs " + propClass.properties.imProps.import_props[i].str_name);
                }

                propClass.properties.block_length = br.ReadInt32();
                propClass.properties.count = br.ReadInt32();
                propClass.properties.vals = new ClassesStructs.PropertyClass.ClassPropertySet.prop_values[propClass.properties.count];

                for(int i = 0; i < propClass.properties.count; i++)
                {
                    propClass.properties.vals[i].crc64_value = br.ReadBytes(8);
                    propClass.properties.vals[i].str_val_len = br.ReadInt32();
                    propClass.properties.vals[i].str_val_len = br.ReadInt32();
                    byte[] tmp_str = br.ReadBytes(propClass.properties.vals[i].str_val_len);
                    propClass.properties.vals[i].str_val = Encoding.ASCII.GetString(tmp_str);
                    propClass.properties.vals[i].count_values = br.ReadInt32();

                    propClass.properties.vals[i].values_s = new ClassesStructs.PropertyClass.ClassPropertySet.values[propClass.properties.vals[i].count_values];

                    string str_tmp;
                    int tmp_i;

                    if (propClass.properties.vals[i].str_val.Contains("class Handle<"))
                    {
                        int beg = propClass.properties.vals[i].str_val.IndexOf('<') + 1;
                        int end = propClass.properties.vals[i].str_val.IndexOf('>');
                        str_tmp = propClass.properties.vals[i].str_val.Substring(beg, end - beg);
                        propClass.properties.vals[i].str_val = str_tmp;
                    }

                    for (int j = 0; j < propClass.properties.vals[i].count_values; j++)
                    {
                        str_tmp = "";

                        propClass.properties.vals[i].values_s[j].crc64_val_type = br.ReadBytes(8);
                        propClass.properties.vals[i].values_s[j].block_len_str_type = br.ReadInt32();
                        propClass.properties.vals[i].values_s[j].str_type_len = br.ReadInt32();
                        tmp_str = br.ReadBytes(propClass.properties.vals[i].values_s[j].str_type_len);
                        propClass.properties.vals[i].values_s[j].str_type = "\"" + Encoding.ASCII.GetString(tmp_str) + "\"";

                        switch (propClass.properties.vals[i].str_val)
                        {
                            case "int":
                                tmp_str = br.ReadBytes(4);
                                str_tmp = propClass.properties.vals[i].str_val + " ";
                                propClass.properties.vals[i].values_s[j].str_val = "\"" + BitConverter.ToInt32(tmp_str, 0).ToString() + "\"";
                                break;

                            case "bool":
                                tmp_str = br.ReadBytes(1);
                                str_tmp = propClass.properties.vals[i].str_val + " ";
                                propClass.properties.vals[i].values_s[j].str_val = BitConverter.ToBoolean(tmp_str, 0) == true ? "\"True\"" : "\"False\"";
                                break;

                            case "float":
                                tmp_str = br.ReadBytes(4);
                                str_tmp = propClass.properties.vals[i].str_val + " ";
                                propClass.properties.vals[i].values_s[j].str_val = "\"" + BitConverter.ToSingle(tmp_str, 0).ToString() + "\"";
                                break;

                            case "class String":
                                tmp_i = br.ReadInt32();
                                tmp_str = br.ReadBytes(tmp_i);
                                str_tmp = "string ";
                                propClass.properties.vals[i].values_s[j].str_val = "\"" + Encoding.ASCII.GetString(tmp_str).ToString() + "\"";
                                break;

                            case "class Color":
                                float[] colors = new float[4]; //RGBA
                                colors[0] = br.ReadSingle();
                                colors[1] = br.ReadSingle();
                                colors[2] = br.ReadSingle();
                                colors[3] = br.ReadSingle();
                                str_tmp = "color ";
                                propClass.properties.vals[i].values_s[j].str_val = "\"(" + Convert.ToString(colors[0]) + ", " + Convert.ToString(colors[1]) + ", " + Convert.ToString(colors[2]) + ", " + Convert.ToString(colors[3]) + ")\"";
                                break;

                            case "class Vector2":
                                float[] vec2 = new float[2];
                                vec2[0] = br.ReadSingle();
                                vec2[1] = br.ReadSingle();

                                str_tmp = "vec2 ";
                                propClass.properties.vals[i].values_s[j].str_val = "\"(" + Convert.ToString(vec2[0]) + ", " + Convert.ToString(vec2[1]) + ")\"";
                                break;

                            case "class Vector3":
                                float[] vec3 = new float[3];
                                vec3[0] = br.ReadSingle();
                                vec3[1] = br.ReadSingle();
                                vec3[2] = br.ReadSingle();

                                str_tmp = "vec3 ";
                                propClass.properties.vals[i].values_s[j].str_val = "\"(" + Convert.ToString(vec3[0]) + ", " + Convert.ToString(vec3[1]) + ", " + Convert.ToString(vec3[2]) + ")\"";
                                break;

                            case "class Rect":
                                int[] rect = new int[4];

                                rect[0] = br.ReadInt32();
                                rect[1] = br.ReadInt32();
                                rect[2] = br.ReadInt32();
                                rect[3] = br.ReadInt32();

                                str_tmp = "rect ";
                                propClass.properties.vals[i].values_s[j].str_val = "\"(" + Convert.ToString(rect[0]) + ", " + Convert.ToString(rect[1]) + ", " + Convert.ToString(rect[2]) + ", " + Convert.ToString(rect[3]) + ")\"";
                                break;

                            case "class Font":
                                tmp_i = br.ReadInt32();
                                tmp_str = br.ReadBytes(tmp_i);
                                str_tmp = "Font ";
                                propClass.properties.vals[i].values_s[j].str_val = "\"" + Encoding.ASCII.GetString(tmp_str).ToString() + "\"";
                                break;

                            case "class DialogResource":
                                tmp_i = br.ReadInt32();
                                tmp_str = br.ReadBytes(tmp_i);
                                str_tmp = "DialogResource ";
                                propClass.properties.vals[i].values_s[j].str_val = "\"" + Encoding.ASCII.GetString(tmp_str).ToString() + "\"";
                                break;
                        }

                        str_tmp += propClass.properties.vals[i].values_s[j].str_type + " : " + propClass.properties.vals[i].values_s[j].str_val;

                        sw.WriteLine(str_tmp);
                    }
                }

                br.Close();
                fs.Close();

                sw.Close();
                fsw.Close();

                ReportForWork("File " + inputFile.Name + " successfully extracted.");
            }
            catch
            {
                if (sw != null) sw.Close();
                if (fsw != null) fsw.Close();

                if(br != null) br.Close();
                if(fs != null) fs.Close();
                ReportForWork("Something wrong with file " + inputFile.Name);
            }
        }

        public void ImportTXTinLANDB(FileInfo[] inputFiles, FileInfo[] fileDestination, int i, int j, string pathOutput, ref bool correctWork, string versionOfGame)
        {
            int index = -1;
            try
            {
                List<AutoPacker.Langdb> landb = new List<AutoPacker.Langdb>();
                FileStream fs = new FileStream(inputFiles[i].FullName, FileMode.Open);
                byte[] binContent = Methods.ReadFull(fs);

                List<byte[]> header = new List<byte[]>();
                List<byte[]> end_of_file = new List<byte[]>();
                byte[] lenght_of_all_text = new byte[4];
                AutoPacker.ReadLandb(binContent, landb, ref header, ref lenght_of_all_text, ref end_of_file);
                fs.Close();

                byte[] check_header = new byte[4];
                Array.Copy(binContent, 16, check_header, 0, 4);

                if (BitConverter.ToInt32(check_header, 0) < 9) versionOfGame = " ";
                if (BitConverter.ToInt32(check_header, 0) == 9) versionOfGame = "WAU";
                else if (BitConverter.ToInt32(check_header, 0) == 10) versionOfGame = "TFTB";

                if (landb.Count != 0)
                {
                    List<TextCollector.TXT_collection> all_text = new List<TextCollector.TXT_collection>();
                    string error = string.Empty;
                    if (fileDestination[j].Extension == ".tsv" && MainMenu.settings.tsvFormat == true) AutoPacker.ImportTSV(fileDestination[j].FullName, ref all_text, "\\n", ref error);
                    else AutoPacker.ImportTXT(fileDestination[j].FullName, ref all_text, false, MainMenu.settings.ASCII_N, "\r\n", ref error);

                    if (error == string.Empty)
                    {
                        if (landb.Count == all_text.Count)
                        {
                            for (int q = 0; q < all_text.Count; q++)
                            {
                                if (MainMenu.settings.importingOfName == true)
                                {
                                    landb[all_text[q].number - 1].name = all_text[q].name;
                                    landb[all_text[q].number - 1].lenght_of_name = BitConverter.GetBytes(landb[all_text[q].number - 1].name.Length);
                                }

                                if ((versionOfGame == "TFTB") && (MainMenu.settings.unicodeSettings == 2))
                                {
                                    byte[] tmp = UnicodeEncoding.UTF8.GetBytes(all_text[q].text);
                                    tmp = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(MainMenu.settings.ASCII_N), tmp);
                                    tmp = Encoding.Convert(Encoding.GetEncoding(1252), Encoding.UTF8, tmp);
                                    all_text[q].text = UnicodeEncoding.UTF8.GetString(tmp);
                                    tmp = null;
                                }

                                //index = all_text[q].number;

                                if (fileDestination[j].Extension == ".txt") landb[all_text[q].number - 1].text = all_text[q].text.Replace("\r\n", "\n");
                                else if (fileDestination[j].Extension != ".txt" && MainMenu.settings.tsvFormat) landb[all_text[q].number - 1].text = all_text[q].text;
                                else if (fileDestination[j].Extension != ".txt" && landb[all_text[q].number - 1].text.Contains("\\n")) landb[all_text[q].number - 1].text = all_text[q].text.Replace("\\n", "\n");

                                if ((versionOfGame == "TFTB") && (MainMenu.settings.unicodeSettings != 1))
                                {
                                    if (landb[all_text[q].number - 1].text.IndexOf("(ANSI)") > 0)
                                    {
                                        landb[all_text[q].number - 1].text = landb[all_text[q].number - 1].text.Replace("(ANSI)", "\0");
                                        landb[all_text[q].number - 1].lenght_of_text = BitConverter.GetBytes(landb[all_text[q].number - 1].text.Length);
                                    }
                                    else
                                    {
                                        byte[] unicode_bin = (byte[])Encoding.UTF8.GetBytes(landb[all_text[q].number - 1].text);
                                        landb[all_text[q].number - 1].lenght_of_text = BitConverter.GetBytes(unicode_bin.Length);
                                    }
                                }
                                else
                                {
                                    if (landb[all_text[q].number - 1].text.IndexOf("(ANSI)") > 0)
                                    {
                                        landb[all_text[q].number - 1].text = landb[all_text[q].number - 1].text.Replace("(ANSI)", "\0");
                                    }

                                    landb[all_text[q].number - 1].lenght_of_text = BitConverter.GetBytes(landb[all_text[q].number - 1].text.Length);
                                }
                            }
                            Methods.DeleteCurrentFile(pathOutput + "\\" + inputFiles[i].Name);
                            AutoPacker.CreateLandb(header, landb, end_of_file, (pathOutput + "\\" + inputFiles[i].Name), versionOfGame);

                            ReportForWork("File: " + fileDestination[j].Name + " imported in " + inputFiles[i].Name);
                        }
                        else ReportForWork("Import in file: " + inputFiles[i].Name + " is incorrect! Incorrect count of text file and landb file.");
                    }
                    else
                    {
                        ReportForWork("Import in file: " + inputFiles[i].Name + " is incorrect! \r\n" + error);
                    }
                }
            }
            catch
            {
                ReportForWork("Import in file: " + inputFiles[i].Name + " is incorrect! Index: " + index);
            }
        }

        public void ImportTXTinLANGDB(FileInfo[] inputFiles, FileInfo[] fileDestination, int i, int j, string pathOutput, ref bool correctWork, string versionOfGame)
        {
            AutoPacker.langdb[] database = new AutoPacker.langdb[5000];
            FileStream fs = new FileStream(inputFiles[i].FullName, FileMode.Open);
            byte[] binContent = Methods.ReadFull(fs);
            byte[] encKey = null;
            int encVer = 2;

            tryRead:

            byte version = 0;
            try
            {
                AutoPacker.ReadLangdb(binContent, database, version);
            }
            catch
            {
                version = 1;
            }
            try
            {
                AutoPacker.ReadLangdb(binContent, database, version);
            }
            catch
            {
                version = 2;
            }
            try
            {
                AutoPacker.ReadLangdb(binContent, database, version);
            }
            catch
            {
                version = 4; //FIX THAT LATER
            }
            try
            {
                AutoPacker.ReadLangdb(binContent, database, version);
            }
            catch
            {
                try
                {
                    string info = Methods.FindingDecrytKey(binContent, "text", ref encKey, ref encVer); //Пытаемся расшифровать текстовый файл.
                    ReportForWork("File " + inputFiles[i].Name + " decrypted. " + info);
                    goto tryRead;
                }
                catch 
                {
                    System.Windows.Forms.MessageBox.Show("ERROR! Unknown langdb.");
                    return;
                }
                
                
            }


            byte[] header = new byte[0];
            byte[] end_of_file = new byte[0];
            byte[] lenght_of_all_text = new byte[4];
            AutoPacker.ReadLangdb(binContent, database, version);
            fs.Close();

            if (database.Length != 0)
            {
                List<TextCollector.TXT_collection> all_text = new List<TextCollector.TXT_collection>();
                string error = string.Empty;
                string pathForFinalFile = MainMenu.settings.pathForOutputFolder + "\\" + inputFiles[i].Name;
                if ((MainMenu.settings.tsvFormat == false) && fileDestination[j].Extension == ".txt") AutoPacker.ImportTXT(fileDestination[j].FullName, ref all_text, false, MainMenu.settings.ASCII_N, "\n", ref error);
                else if ((MainMenu.settings.tsvFormat == true) && (fileDestination[j].Extension == ".tsv")) AutoPacker.ImportTSV(fileDestination[j].FullName, ref all_text, "\\n", ref error);

                if (error == string.Empty)
                {
                    for (int q = 0; q < all_text.Count; q++)
                    {
                            if (MainMenu.settings.importingOfName == true)
                            {
                                database[all_text[q].number - 1].name = all_text[all_text[q].number - 1].name;
                                database[all_text[q].number - 1].lenght_of_name = BitConverter.GetBytes(database[all_text[q].number - 1].name.Length);
                            }

                            if(MainMenu.settings.tsvFormat == false) database[all_text[q].number - 1].text = all_text[q].text.Replace("\r\n", "\n");
                            else database[all_text[q].number - 1].text = all_text[q].text;
                            database[all_text[q].number - 1].lenght_of_text = BitConverter.GetBytes(database[all_text[q].number - 1].text.Length);
                    }
                    Methods.DeleteCurrentFile(pathForFinalFile);
                    AutoPacker.CreateLangdb(database, version, pathForFinalFile);
                    ReportForWork("File: " + fileDestination[j].Name + " imported in " + inputFiles[i].Name);

                    if ((versionOfGame == " ") && MainMenu.settings.encLangdb == true)
                    {
                        if (MainMenu.settings.customKey) encKey = Methods.stringToKey(MainMenu.settings.encCustomKey);
                        else encKey = MainMenu.gamelist[TTG_Tools.AutoPacker.numKey].key;

                        fs = new FileStream(MainMenu.settings.pathForOutputFolder + "\\" + inputFiles[i].Name, FileMode.Open);
                        byte[] temp_file = new byte[fs.Length];
                        fs.Read(temp_file, 0, temp_file.Length);
                        fs.Close();

                        
                        if (AutoPacker.selected_index == 0) Methods.meta_crypt(temp_file, encKey, 2, false);
                        else Methods.meta_crypt(temp_file, encKey, 7, false);

                        fs = new FileStream(MainMenu.settings.pathForOutputFolder + "\\" + inputFiles[i].Name, FileMode.OpenOrCreate);
                        fs.Write(temp_file, 0, temp_file.Length);
                        fs.Close();

                        ReportForWork("File " + inputFiles[i].Name + " encrypted!");
                    }
                }
                else
                {
                    ReportForWork("Import in file: " + inputFiles[i].Name + " is incorrect! \r\n" + error);
                }
            }
            else
            {
                ReportForWork("File " + inputFiles[i].Name + " is EMPTY!");
            }
        }

        //Экспорт
        public void DoExportEncoding(object parametres)
        {
            List<string> param = parametres as List<string>;
            string pathInput = param[0];
            string pathOutput = param[1];
            string versionOfGame = param[2];
            byte[] key = Methods.stringToKey(param[3]);
            byte[] encKey = null;
            int version = Convert.ToInt32(param[4]);

            if (Directory.Exists(pathInput) && Directory.Exists(pathOutput))
            {
                List<string> destinationForExportList = new List<string>();
                destinationForExportList.Add(".langdb");
                destinationForExportList.Add(".d3dtx");
                destinationForExportList.Add(".landb");
                destinationForExportList.Add(".dlog");
                destinationForExportList.Add(".prop");

                List<int> extractedFormat = new List<int>();
                extractedFormat.Add(-1);
                extractedFormat.Add(-1);
                extractedFormat.Add(-1);
                extractedFormat.Add(-1);
                extractedFormat.Add(-1);

                foreach (string destinationForExport in destinationForExportList)
                {
                    DirectoryInfo dir = new DirectoryInfo(pathInput);
                    FileInfo[] inputFiles = dir.GetFiles('*' + destinationForExport);

                    if (inputFiles.Length > 0)
                    {
                        for (int i = 0; i < inputFiles.Length; i++)
                        {
                            switch (destinationForExport)
                            {
                                case ".langdb":
                                    {
                                        string message = Texts.LangdbWorker.DoWork(inputFiles[i].FullName, true, false, ref encKey, 2);
                                        ReportForWork(message);
                                        /*int lenghtOfExtension = inputFiles[i].Extension.Length;
                                        string fileName = inputFiles[i].Name.Remove(inputFiles[i].Name.Length - lenghtOfExtension, lenghtOfExtension) + ".txt";
                                        if (MainMenu.settings.tsvFormat) fileName = inputFiles[i].Name.Remove(inputFiles[i].Name.Length - lenghtOfExtension, lenghtOfExtension) + ".tsv";
                                        ExportTXTfromLANGDB(inputFiles, i, pathOutput, fileName, versionOfGame);*/
                                        extractedFormat[0] = 0;
                                        break;
                                    }
                                case ".d3dtx":
                                    {
                                        string message = TextureWorker.DoWork(inputFiles[i].FullName, pathOutput, true, false, ref encKey, version);
                                        ReportForWork(message);
                                        extractedFormat[1] = 1;
                                        //string message = TextureWorker.ExportTexture(inputFiles, i, AutoPacker.selected_index, key, version, versionOfGame, MainMenu.settings.iOSsupport);
                                        //if (message != "") ReportForWork(message);
                                        //else ReportForWork("Unknown error. Please send me file.");
                                        break;
                                    }
                                case ".landb":
                                    ExtractLandbFile(inputFiles, i, versionOfGame);
                                    extractedFormat[2] = 2;
                                    break;
                                case ".dlog":
                                    ExtractDlogFile(inputFiles, i);
                                    extractedFormat[3] = 3;
                                    break;
                                case ".prop":
                                    int lenExt = inputFiles[i].Extension.Length;
                                    string f_name = inputFiles[i].Name.Remove(inputFiles[i].Name.Length - lenExt, lenExt) + ".txt";
                                    ExportPROP(inputFiles[i], f_name);
                                    extractedFormat[4] = 4;
                                    break;
                                default:
                                    {
                                        System.Windows.Forms.MessageBox.Show("Error in Switch!");
                                        break;
                                    }
                            }

                        }
                    }
                }


                foreach(int extractList in extractedFormat)
                {
                    if(extractList != -1) ReportForWork("EXPORT OF ALL *" + destinationForExportList[extractList].ToUpper() + " FILES COMPLETE!");
                }
            }

        }
        public void ExportTXTfromLANGDB(FileInfo[] inputFiles, int i, string pathOutput, string fileName, string versionOfGame)
        {
            AutoPacker.langdb[] database = new AutoPacker.langdb[5000];
            //try
            {
                List<AutoPacker.Langdb> landb = new List<AutoPacker.Langdb>();
                FileStream fs = new FileStream(inputFiles[i].FullName, FileMode.Open);
                byte[] binContent = Methods.ReadFull(fs);
                fs.Close();
                int encVer = 2;

            tryAgain:
                byte[] header = new byte[0];
                byte[] end_of_file = new byte[0];
                byte[] lenght_of_all_text = new byte[4];
                byte[] encKey = null;

                byte version = 0;
                try
                {
                    AutoPacker.ReadLangdb(binContent, database, version);
                }
                catch
                {
                    version = 1;
                }
                try
                {
                    AutoPacker.ReadLangdb(binContent, database, version);
                }
                catch
                {
                    version = 2;
                }
                try
                {
                    AutoPacker.ReadLangdb(binContent, database, version);
                }
                catch
                {
                    version = 4; //FIX THAT LATER
                }
                try
                {
                    AutoPacker.ReadLangdb(binContent, database, version);
                }
                catch
                {

                    try 
                    {
                        string info = Methods.FindingDecrytKey(binContent, "text", ref encKey, ref encVer);
                        ReportForWork("File " + inputFiles[i].Name + " decrypted. " + info);
                        goto tryAgain;
                    }
                    catch
                    {
                        ReportForWork("ERROR! Unknown langdb.");
                        return;
                    }
                }

                AutoPacker.ReadLangdb(binContent, database, version);


                if (database.Length != 0)
                {
                    List<TextCollector.TXT_collection> all_text = new List<TextCollector.TXT_collection>();

                    Methods.DeleteCurrentFile(pathOutput + "\\" + fileName);

                    List<TextCollector.TXT_collection> allTextForExport = new List<TextCollector.TXT_collection>();

                    for (int q = 0; q < database.Length; q++)
                    {
                        if (database[q].text != null)
                        {
                            UInt32 realID = BitConverter.ToUInt32(database[q].realID, 0);
                            all_text.Add(new TextCollector.TXT_collection((q + 1), realID, database[q].name, database[q].text, false));
                        }
                    }

                    TextCollector.CreateExportingTXTfromOneFile(all_text, ref allTextForExport);

                    //allTextForExport = all_text;
                    FileStream MyExportStream = new FileStream(pathOutput + "\\" + fileName, FileMode.OpenOrCreate);
                    int w = 0;

                    if (!MainMenu.settings.tsvFormat)
                    {
                        while (w < allTextForExport.Count)
                        {
                            try { int u = allTextForExport[w].text.Length; }
                            catch { break; }
                            if (allTextForExport[w].text != null)
                            {
                                if (MainMenu.settings.exportRealID)
                                {
                                    TextCollector.SaveString(MyExportStream, (allTextForExport[w].realId + ") " + allTextForExport[w].name + "\r\n"), MainMenu.settings.ASCII_N);//проверка
                                }
                                else
                                {
                                    TextCollector.SaveString(MyExportStream, (allTextForExport[w].number + ") " + allTextForExport[w].name + "\r\n"), MainMenu.settings.ASCII_N);//проверка
                                }
                                //TextCollector.SaveString(MyExportStream, (allTextForExport[w].number + ") " + allTextForExport[w].name + "\r\n"), MainMenu.settings.ASCII_N);
                                //TextCollector.SaveString(MyExportStream, (BitConverter.ToString(database[all_text_for_export[w].number-1].hz_data)+"\r\n"), MainMenu.settings.ASCII_N);
                                allTextForExport[w].text = allTextForExport[w].text.Replace("\n", "\r\n");
                                TextCollector.SaveString(MyExportStream, (allTextForExport[w].text + "\r\n"), MainMenu.settings.ASCII_N);
                                w++;
                            }
                            else
                            { }
                        }
                    }
                    else
                    {
                        while (w < allTextForExport.Count)
                        {
                            try { int u = allTextForExport[w].text.Length; }
                            catch { break; }

                            string export_tsv;

                            if (allTextForExport[w].text != null)
                            {
                                if (MainMenu.settings.exportRealID)
                                {
                                    export_tsv = allTextForExport[w].realId + "\t" + allTextForExport[w].name + "\t";
                                    //TextCollector.SaveString(MyExportStream, (allTextForExport[w].realId + ") " + allTextForExport[w].name + "\r\n"), MainMenu.settings.ASCII_N);//проверка
                                }
                                else
                                {
                                    export_tsv = allTextForExport[w].number + "\t" + allTextForExport[w].name + "\t";
                                    //TextCollector.SaveString(MyExportStream, (allTextForExport[w].number + ") " + allTextForExport[w].name + "\r\n"), MainMenu.settings.ASCII_N);//проверка
                                }

                                allTextForExport[w].text = allTextForExport[w].text.Replace("\n", "\\n");
                                export_tsv += allTextForExport[w].text + "\r\n";
                                byte[] bin_export = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetBytes(export_tsv);
                                bin_export = Encoding.Convert(Encoding.GetEncoding(MainMenu.settings.ASCII_N), Encoding.UTF8, bin_export);
                                export_tsv = Encoding.UTF8.GetString(bin_export);
                                TextCollector.SaveString(MyExportStream, export_tsv, 0);
                                w++;

                                //TextCollector.SaveString(MyExportStream, (allTextForExport[w].number + ") " + allTextForExport[w].name + "\r\n"), MainMenu.settings.ASCII_N);
                                //TextCollector.SaveString(MyExportStream, (BitConverter.ToString(database[all_text_for_export[w].number-1].hz_data)+"\r\n"), MainMenu.settings.ASCII_N);
                                //TextCollector.SaveString(MyExportStream, (allTextForExport[w].text + "\r\n"), MainMenu.settings.ASCII_N);
                            }
                            else
                            { }
                        }
                    }



                    MyExportStream.Close();
                    ReportForWork("File " + inputFiles[i].Name + " exported in " + fileName);
                }
                else
                {
                    ReportForWork("File " + inputFiles[i].Name + " is EMPTY!");
                }
            }
            //catch
            //{
            //    ReportForWork("Import in file: " + inputFiles[i].Name + " is incorrect!");
            //}
        }


        public void DoWork()
        {
            for (int i = 1; i <= 100; ++i)
            {
                Thread.Sleep(100);
                if (Progress != null)
                    Progress(i);
            }
        }
    }
}
