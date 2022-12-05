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

            bool[] show = { false, false, false, false, false };

            string result = "";

            List<string> destination = new List<string>();
            destination.Add(".d3dtx");
            destination.Add(".d3dtx");
            destination.Add(".langdb");
            destination.Add(".langdb");
            destination.Add(".font");
            destination.Add(".landb");
            destination.Add(".landb");
            destination.Add(".dlog");
            destination.Add(".dlog");
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
                                            result = TextureWorker.DoWork(inputFiles[i].FullName, pathOutput, false, FullEncrypt, ref encKey, version);
                                            ReportForWork(result);
                                            show[0] = true;    
                                                break;
                                        case ".landb":
                                            result = Texts.LandbWorker.DoWork(inputFiles[i].FullName, fileDestination[j].FullName, false);
                                            ReportForWork(result);
                                            show[1] = true;
                                                break;
                                        case ".langdb":
                                            //ImportTXTinLANGDB(inputFiles, fileDestination, i, j, pathOutput, ref correct_work, versionOfGame);
                                            result = Texts.LangdbWorker.DoWork(inputFiles[i].FullName, fileDestination[j].FullName, false, FullEncrypt, ref encKey, version);
                                            ReportForWork(result);
                                            show[2] = true;
                                                break;
                                        case ".dlog":
                                            result = Texts.DlogWorker.DoWork(inputFiles[i].FullName, fileDestination[j].FullName, false);
                                            ReportForWork(result);
                                            show[3] = true;
                                                break;
                                        case ".prop":
                                            ImportTXTinPROP(inputFiles[i], fileDestination[j]);
                                            show[4] = true;
                                                break;
                                        default:
                                                MessageBox.Show("Error in Switch!");
                                                break;
                                    }

                                    if (correct_work) //If file imported correct then we can delete file if it set in settings form
                                    {
                                        countCorrectWork++;
                                        if (deleteFromInputImported)
                                        {
                                            Methods.DeleteCurrentFile(fileDestination[j].FullName);
                                        }
                                    }
                                }
                            }
                            if (deleteFromInputSource && countCorrectWork == countOfAllFiles)//if all files were imported correctly, then delete the file if necessary
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
                            message += ".DLOG";
                            break;

                        case 4:
                            message += ".PROP";
                            break;
                    }
                    message += " IS COMPLETE!";

                    ReportForWork(message);
                }
            }
        }

        public void ImportTXTinPROP(FileInfo inputFile, FileInfo DestinationFile)
        {
            byte[] binContent = File.ReadAllBytes(inputFile.FullName);
            string[] strs = File.ReadAllLines(DestinationFile.FullName);
            int posBlSize = 0;
            int blockSize = 0;
            int blHeadSize = 0;
            MemoryStream ms = new MemoryStream(binContent);
            BinaryReader br = new BinaryReader(ms);
            string destFilePath = MainMenu.settings.pathForOutputFolder + "\\" + inputFile.Name;
            if (File.Exists(destFilePath)) File.Delete(destFilePath);
            FileStream fs = new FileStream(destFilePath, FileMode.CreateNew);
            BinaryWriter bw = new BinaryWriter(fs);
            try
            {
                byte[] header = br.ReadBytes(4);
                bw.Write(header);
                if ((Encoding.ASCII.GetString(header) == "5VSM") || (Encoding.ASCII.GetString(header) == "6VSM"))
                {
                    blHeadSize = br.ReadInt32();
                    bw.Write(blHeadSize);
                    blHeadSize = 0;
                    byte[] tmp_bl = br.ReadBytes(8);
                    bw.Write(tmp_bl);
                }
                int count = br.ReadInt32();
                bw.Write(count);
                for (int i = 0; i < count; i++)
                {
                    byte[] block = br.ReadBytes(8);
                    bw.Write(block);
                    block = br.ReadBytes(4);
                    bw.Write(block);
                }
                int one = br.ReadInt32();
                bw.Write(one);
                blHeadSize += 4;
                int one2 = -1;
                int unknown1 = br.ReadInt32();
                bw.Write(unknown1);
                blHeadSize += 4;
                int blLen = -1;
                byte[] bl = null;
                if (Encoding.ASCII.GetString(header) != "6VSM")
                {
                    blLen = br.ReadInt32();
                    bw.Write(blLen);
                    blHeadSize += blLen;
                    bl = br.ReadBytes(blLen - 4);
                    bw.Write(bl);
                    blHeadSize += blLen;
                }
                posBlSize = (int)br.BaseStream.Position;
                int orBlSize = br.ReadInt32();
                bw.Write(orBlSize);
                blockSize += 4;
                blHeadSize += 4;
                one = br.ReadInt32();
                bw.Write(one);
                blockSize += 4;
                blHeadSize += 4;
                if (Encoding.ASCII.GetString(header) == "6VSM")
                {
                    one2 = br.ReadInt32();
                    bw.Write(one2);
                    blHeadSize += 4;
                    blockSize += 4;
                }
                byte[] check_bl = br.ReadBytes(8);
                bw.Write(check_bl);
                blockSize += 8;
                blHeadSize += 8;
                if (Encoding.ASCII.GetString(header) == "ERTM")
                {
                    one = br.ReadInt32();
                    bw.Write(one);
                    blockSize += 4;
                }
                count = br.ReadInt32();
                bw.Write(count);
                blockSize += 4;
                blHeadSize += 4;
                int c = 1;
                if (BitConverter.ToString(check_bl) == "B4-F4-5A-5F-60-6E-9C-CD")
                {
                    for (int i = 0; i < count; i++)
                    {
                        bl = br.ReadBytes(8);
                        bw.Write(bl);
                        blockSize += 8;
                        blHeadSize += 8;
                        if (Encoding.ASCII.GetString(header) == "ERTM")
                        {
                            one = br.ReadInt32();
                            bw.Write(one);
                            blockSize += 4;
                        }
                        blLen = br.ReadInt32();
                        bl = br.ReadBytes(blLen);
                        blockSize += 4;
                        blHeadSize += 4;
                        bl = Encoding.ASCII.GetString(header) == "6VSM" ? Encoding.UTF8.GetBytes(strs[c]) : Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetBytes(strs[c]);
                        blLen = bl.Length;
                        bw.Write(blLen);
                        bw.Write(bl);
                        blockSize += blLen;
                        blHeadSize += blLen;
                        c += 2;
                    }
                }
                if (BitConverter.ToString(check_bl) == "25-03-C6-1F-D8-64-1B-4F")
                {
                    for (int i = 0; i < count; i++)
                    {
                        int subCount = 0;
                        bl = br.ReadBytes(8);
                        bw.Write(bl);
                        blockSize += 8;
                        blHeadSize += 8;
                        if (Encoding.ASCII.GetString(header) == "ERTM")
                        {
                            one = br.ReadInt32();
                            bw.Write(one);
                            blockSize += 4;
                        }
                        subCount = br.ReadInt32();
                        bw.Write(subCount);
                        blockSize += 4;
                        blHeadSize += 4;
                        for (int j = 0; j < subCount * 2; j++)
                        {
                            blLen = br.ReadInt32();
                            bl = br.ReadBytes(blLen);
                            bl = Encoding.ASCII.GetString(header) == "6VSM" ? Encoding.UTF8.GetBytes(strs[c]) : Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetBytes(strs[c]);
                            blLen = bl.Length;
                            bw.Write(blLen);
                            bw.Write(bl);
                            blockSize += 4 + blLen;
                            blHeadSize += 4 + blLen;
                            c += 2;
                        }
                    }
                }
                bw.BaseStream.Seek(posBlSize, SeekOrigin.Begin);
                bw.Write(blockSize);
                if ((Encoding.ASCII.GetString(header) == "5VSM") || (Encoding.ASCII.GetString(header) == "6VSM"))
                {
                    bw.BaseStream.Seek(4, SeekOrigin.Begin);
                    bw.Write(blHeadSize);
                }
                bw.Close();
                fs.Close();
                br.Close();
                ms.Close();
                ReportForWork("File " + DestinationFile.Name + " imported in " + inputFile.Name + ".");
            }
            catch
            {
                if (br != null) br.Close();
                if (ms != null) ms.Close();
                if (bw != null) bw.Close();
                if (fs != null) fs.Close();
                ReportForWork("Something wrong with file " + inputFile.Name);
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
                //Read for checkpoint_text.prop and statsInfo_text.prop files
                byte[] header = br.ReadBytes(4);
                int blockSize = 0;
                long subblockSize = 0; //default 0
                if ((Encoding.ASCII.GetString(header) == "5VSM") || (Encoding.ASCII.GetString(header) == "6VSM"))
                {
                    blockSize = br.ReadInt32();
                    subblockSize = br.ReadInt64();
                }
                int countHeaders = br.ReadInt32();
                for (int i = 0; i < countHeaders; i++)
                {
                    byte[] crc64Val = br.ReadBytes(8);
                    byte[] values = br.ReadBytes(4);   //Some values after crc64's strings
                }
                int one = br.ReadInt32();
                int someValue1 = br.ReadInt32();
                if (Encoding.ASCII.GetString(header) != "6VSM")
                {
                    int blSize1 = br.ReadInt32();
                    byte[] someContent = br.ReadBytes(blSize1 - 4);
                }
                int blSize2 = br.ReadInt32();
                int one1 = br.ReadInt32();
                int one2 = -1;
                if (Encoding.ASCII.GetString(header) == "6VSM") one2 = br.ReadInt32();
                byte[] bValue = br.ReadBytes(8);
                int c_str = 1;
                if (BitConverter.ToString(bValue) == "B4-F4-5A-5F-60-6E-9C-CD")
                {
                    if (Encoding.ASCII.GetString(header) == "ERTM") one1 = br.ReadInt32();
                    int countBlocks = br.ReadInt32();
                    string[] strs = new string[countBlocks];
                    for (int i = 0; i < countBlocks; i++)
                    {
                        bValue = br.ReadBytes(8);
                        if (Encoding.ASCII.GetString(header) == "ERTM") one1 = br.ReadInt32();
                        int len = br.ReadInt32();
                        bValue = br.ReadBytes(len);
                        strs[i] = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(bValue);
                        if (Encoding.ASCII.GetString(header) == "6VSM") strs[i] = Encoding.UTF8.GetString(bValue);
                        sw.WriteLine(Convert.ToString(c_str) + ")");
                        sw.WriteLine(strs[i]);
                        c_str++;
                    }
                }
                if (BitConverter.ToString(bValue) == "25-03-C6-1F-D8-64-1B-4F")
                {
                    if (Encoding.ASCII.GetString(header) == "ERTM") one1 = br.ReadInt32();
                    int countBlocks = br.ReadInt32();
                    string[][] strs = new string[countBlocks][];
                    int countSubBlocks = 0;
                    for (int i = 0; i < countBlocks; i++)
                    {
                        bValue = br.ReadBytes(8);
                        if (Encoding.ASCII.GetString(header) == "ERTM") one1 = br.ReadInt32();
                        countSubBlocks = br.ReadInt32();
                        var Pos = br.BaseStream.Position;
                        strs[i] = new string[countSubBlocks * 2];
                        for (int j = 0; j < countSubBlocks * 2; j++)
                        {
                            int len = br.ReadInt32();
                            bValue = br.ReadBytes(len);
                            strs[i][j] = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(bValue);
                            if (Encoding.ASCII.GetString(header) == "6VSM") strs[i][j] = Encoding.UTF8.GetString(bValue);
                            sw.WriteLine(Convert.ToString(c_str) + ")");
                            sw.WriteLine(strs[i][j]);
                            c_str++;
                        }
                    }
                }
                ReportForWork("File " + inputFile.Name + " successfully extracted.");
                br.Close();
                fs.Close();
                sw.Close();
                fsw.Close();
            }
            catch
            {
                if (br != null) br.Close();
                if (fs != null) fs.Close();
                if (sw != null) sw.Close();
                if (fsw != null) fsw.Close();
                ReportForWork("Something wrong with file " + inputFile.Name);
            }
            /*try
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
            }*/
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

                string message = "";

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
                                    message = Texts.LangdbWorker.DoWork(inputFiles[i].FullName, "", true, false, ref encKey, 2);
                                    ReportForWork(message);
                                    extractedFormat[0] = 0;
                                        break;

                                case ".d3dtx":
                                    message = TextureWorker.DoWork(inputFiles[i].FullName, pathOutput, true, false, ref encKey, version);
                                    ReportForWork(message);
                                    extractedFormat[1] = 1;
                                        break;

                                case ".landb":
                                    message = Texts.LandbWorker.DoWork(inputFiles[i].FullName, "", true);
                                    ReportForWork(message);
                                    extractedFormat[2] = 2;
                                       break;

                                case ".dlog":
                                    message = Texts.DlogWorker.DoWork(inputFiles[i].FullName, "", true);
                                    //ExtractDlogFile(inputFiles, i);
                                    ReportForWork(message);
                                    extractedFormat[3] = 3;
                                       break;

                                case ".prop":
                                    int lenExt = inputFiles[i].Extension.Length;
                                    string f_name = inputFiles[i].Name.Remove(inputFiles[i].Name.Length - lenExt, lenExt) + ".txt";
                                    ExportPROP(inputFiles[i], f_name);
                                    extractedFormat[4] = 4;
                                       break;

                                default:
                                    MessageBox.Show("Error in Switch!");
                                      break;
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
    }
}
