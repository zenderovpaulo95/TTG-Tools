using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TTG_Tools
{
    public delegate void ProgressHandler(int progress);
    public delegate void ReportHandler(string report);
    public delegate void GlossaryAdd(string orText, string trText);

    public class ForThreads
    {
        public event ProgressHandler Progress;
        public event ReportHandler ReportForWork;

        //Import files
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
            bool isNewEngine = param[8] == "True";
            byte[] encKey = Methods.stringToKey(param[9]);

            bool[] show = { false, false, false, false, false, false, false };

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
            destination.Add(".lua");
            destination.Add(".lenc");
            List<string> extention = new List<string>();
            extention.Add(".dds");
            extention.Add(".pvr");
            extention.Add(".txt");
            extention.Add(".tsv");
            extention.Add(".ttf");
            extention.Add(".txt");
            extention.Add(".tsv");
            extention.Add(".txt");
            extention.Add(".tsv");
            extention.Add(".txt");
            extention.Add(".lua");
            extention.Add(".lenc");

            bool emptyFiles = true;

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
                                            result = Graphics.TextureWorker.DoWork(inputFiles[i].FullName, pathOutput, false, FullEncrypt, ref encKey, version);
                                            ReportForWork(result);
                                            emptyFiles = false;
                                            show[0] = true;    
                                                break;
                                        case ".landb":
                                            result = Texts.LandbWorker.DoWork(inputFiles[i].FullName, fileDestination[j].FullName, false, encKey, version);
                                            ReportForWork(result);
                                            emptyFiles = false;
                                            show[1] = true;
                                                break;
                                        case ".langdb":
                                            result = Texts.LangdbWorker.DoWork(inputFiles[i].FullName, fileDestination[j].FullName, false, FullEncrypt, ref encKey, version);
                                            ReportForWork(result);
                                            emptyFiles = false;
                                            show[2] = true;
                                                break;
                                        case ".dlog":
                                            result = Texts.DlogWorker.DoWork(inputFiles[i].FullName, fileDestination[j].FullName, false, ref encKey, ref version);
                                            ReportForWork(result);
                                            emptyFiles = false;
                                            show[3] = true;
                                                break;
                                        case ".prop":
                                            ImportTXTinPROP(inputFiles[i], fileDestination[j]);
                                            emptyFiles = false;
                                            show[4] = true;
                                                break;
                                        case ".font":
                                            result = Graphics.FontWorker.DoWork(inputFiles[i].FullName, false);
                                            ReportForWork(result);
                                            emptyFiles = false;
                                            show[5] = true;
                                            break;
                                        case ".lua":
                                        case ".lenc":

                                            if (MainMenu.settings.customKey)
                                            {
                                                encKey = Methods.stringToKey(MainMenu.settings.encCustomKey);

                                                if (encKey == null)
                                                {
                                                    ReportForWork("You must enter key encryption!");
                                                    //MessageBox.Show("You must enter key encryption!", "Error");
                                                    //return;
                                                }
                                            }

                                            FileStream fs = new FileStream(inputFiles[i].FullName, FileMode.Open);
                                            byte[] luaContent = Methods.ReadFull(fs);
                                            fs.Close();

                                            luaContent = Methods.encryptLua(luaContent, encKey, isNewEngine, version);

                                            if (File.Exists(TTG_Tools.MainMenu.settings.pathForOutputFolder + "\\" + inputFiles[i].Name)) File.Delete(TTG_Tools.MainMenu.settings.pathForOutputFolder + "\\" + inputFiles[i].Name);
                                            fs = new FileStream(TTG_Tools.MainMenu.settings.pathForOutputFolder + "\\" + inputFiles[i].Name, FileMode.CreateNew);
                                            fs.Write(luaContent, 0, luaContent.Length);
                                            fs.Close();

                                            ReportForWork("File " + inputFiles[i].Name + " encrypted.");
                                            emptyFiles = false;
                                            show[6] = true;
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
                    message = "IMPORT *";
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

                        case 5:
                            message += ".FONT";
                            break;

                        case 6:
                            message += ".LUA/.LENC";
                            break;
                    }
                    message += " FILES COMPLETE!";

                    ReportForWork(message);
                }
            }

            if (emptyFiles) ReportForWork("Nothing to import. Empty folder.");
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
        }

        //Экспорт
        public void DoExportEncoding(object parametres)
        {
            List<string> param = parametres as List<string>;
            string pathInput = param[0];
            string pathOutput = param[1];
            string versionOfGame = param[2];
            byte[] key = Methods.stringToKey(param[3]);
            int version = Convert.ToInt32(param[4]);

            if (Directory.Exists(pathInput) && Directory.Exists(pathOutput))
            {
                List<string> destinationForExportList = new List<string>();
                destinationForExportList.Add(".langdb");
                destinationForExportList.Add(".d3dtx");
                destinationForExportList.Add(".landb");
                destinationForExportList.Add(".dlog");
                destinationForExportList.Add(".prop");
                destinationForExportList.Add(".font");
                destinationForExportList.Add(".lua");
                destinationForExportList.Add(".lenc");

                List<int> extractedFormat = new List<int>();
                extractedFormat.Add(-1);
                extractedFormat.Add(-1);
                extractedFormat.Add(-1);
                extractedFormat.Add(-1);
                extractedFormat.Add(-1);
                extractedFormat.Add(-1);
                extractedFormat.Add(-1);
                extractedFormat.Add(-1);

                string message = "";
                bool emptyFiles = true;

                foreach (string destinationForExport in destinationForExportList)
                {
                    DirectoryInfo dir = new DirectoryInfo(pathInput);
                    FileInfo[] inputFiles = dir.GetFiles('*' + destinationForExport);

                    if (inputFiles.Length > 0)
                    {
                        emptyFiles = false;

                        for (int i = 0; i < inputFiles.Length; i++)
                        {
                            switch (destinationForExport)
                            {
                                case ".langdb":
                                    message = Texts.LangdbWorker.DoWork(inputFiles[i].FullName, "", true, false, ref key, 2);
                                    ReportForWork(message);
                                    extractedFormat[0] = 0;
                                        break;

                                case ".d3dtx":
                                    message = Graphics.TextureWorker.DoWork(inputFiles[i].FullName, pathOutput, true, false, ref key, version);
                                    ReportForWork(message);
                                    extractedFormat[1] = 1;
                                        break;

                                case ".landb":
                                    message = Texts.LandbWorker.DoWork(inputFiles[i].FullName, "", true, key, version);
                                    ReportForWork(message);
                                    extractedFormat[2] = 2;
                                       break;

                                case ".dlog":
                                    message = Texts.DlogWorker.DoWork(inputFiles[i].FullName, "", true, ref key, ref version);
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

                                case ".font":
                                    message = Graphics.FontWorker.DoWork(inputFiles[i].FullName, true);
                                    extractedFormat[5] = 5;
                                    ReportForWork(message);
                                    break;

                                case ".lenc":
                                case ".lua":
                                    FileStream fs = new FileStream(inputFiles[i].FullName, FileMode.Open);
                                    byte[] luaContent = Methods.ReadFull(fs);
                                    fs.Close();

                                    
                                    luaContent = Methods.decryptLua(luaContent, key, version);

                                    fs = new FileStream(TTG_Tools.MainMenu.settings.pathForOutputFolder + "\\" + inputFiles[i].Name, FileMode.OpenOrCreate);
                                    fs.Write(luaContent, 0, luaContent.Length);
                                    fs.Close();

                                    ReportForWork("File " + inputFiles[i].Name + " decrypted.");
                                    if (destinationForExport == ".lua") extractedFormat[6] = 6;
                                    else extractedFormat[7] = 7;
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

                if(emptyFiles) ReportForWork("Nothing to extract. Empty folder.");
            }

        }
    }
}
