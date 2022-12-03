using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTG_Tools.ClassesStructs.Text;
using System.IO;

namespace TTG_Tools.Texts
{
    public class LandbWorker
    {
        private static LandbClass GetStringsFromLandb(BinaryReader br, bool hasCRC64Langres, bool newFormat, bool isUnicode)
        {
            LandbClass landb = new LandbClass();

            try
            { 
                landb.isNewFormat = newFormat;
                landb.hasMetaLangresName = hasCRC64Langres;
                landb.isUnicode = isUnicode;

                landb.newBlockLength = 0;
                landb.newLandbFileSize = 0;
                landb.newLandbLastFileSize = 0;

                if(landb.isNewFormat)
                {
                    var pos = br.BaseStream.Position;
                    br.BaseStream.Seek(4, SeekOrigin.Begin);
                    landb.landbFileSize = br.ReadInt32();
                    landb.landbLastFileSize = br.ReadInt32();
                    br.BaseStream.Seek(pos, SeekOrigin.Begin);
                }

                landb.blockSize1 = br.ReadInt32();
                landb.newLandbFileSize += 4;
                landb.someValue1 = br.ReadInt32();
                landb.newLandbFileSize += 4;
                landb.blockSize2 = br.ReadInt32();
                landb.newLandbFileSize += 4;
                landb.someValue2 = br.ReadInt32();
                landb.newLandbFileSize += 4;

                landb.blockLength = br.ReadInt32();
                landb.newLandbFileSize += 4;
                landb.landbCount = br.ReadInt32();
                landb.newLandbFileSize += 4;
                landb.newBlockLength = 8;

                landb.landbs = new Landb[landb.landbCount];
                landb.flags = new ClassesStructs.FlagsClass.LangdbFlagClass[landb.landbCount];

                byte[] tmp = null;                

                for (int i = 0; i < landb.landbCount; i++)
                {
                    landb.landbs[i].stringNumber = (uint)(i + 1);
                    landb.landbs[i].wavID = br.ReadUInt32();
                    landb.newLandbFileSize += 4;
                    landb.newBlockLength += 4;

                    if (landb.hasMetaLangresName)
                    {
                        landb.landbs[i].crc64Langres = br.ReadUInt64();
                        landb.newLandbFileSize += 8;
                        landb.newBlockLength += 8;
                    }
                    landb.landbs[i].anmID = br.ReadUInt32();
                    landb.newLandbFileSize += 4;
                    landb.newBlockLength += 4;

                    landb.landbs[i].zero1 = br.ReadInt32();
                    landb.newLandbFileSize += 4;
                    landb.newBlockLength += 4;

                    landb.landbs[i].blockAnmNameSize = br.ReadInt32();
                    landb.newLandbFileSize += 4;
                    landb.newBlockLength += 4;

                    if (landb.hasMetaLangresName)
                    {
                        tmp = br.ReadBytes(8);
                        landb.newLandbFileSize += 8;
                        landb.newBlockLength += 8;

                        landb.landbs[i].anmNameSize = 8;

                        landb.landbs[i].anmName = BitConverter.ToString(tmp);
                    }
                    else
                    {
                        landb.landbs[i].anmNameSize = br.ReadInt32();
                        landb.newLandbFileSize += 4;
                        landb.newBlockLength += 4;

                        tmp = br.ReadBytes(landb.landbs[i].anmNameSize);
                        landb.newLandbFileSize += landb.landbs[i].anmNameSize;
                        landb.newBlockLength += landb.landbs[i].anmNameSize;

                        landb.landbs[i].anmName = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(tmp);
                    }

                    landb.landbs[i].blockWavNameSize = br.ReadInt32();
                    landb.newLandbFileSize += 4;
                    landb.newBlockLength += 4;

                    if (landb.hasMetaLangresName)
                    {
                        tmp = br.ReadBytes(8);
                        landb.newLandbFileSize += 8;
                        landb.newBlockLength += 8;

                        landb.landbs[i].wavNameSize = 8;
                        landb.landbs[i].wavName = BitConverter.ToString(tmp);
                    }
                    else
                    {
                        landb.landbs[i].wavNameSize = br.ReadInt32();
                        landb.newLandbFileSize += 4;
                        landb.newBlockLength += 4;

                        tmp = br.ReadBytes(landb.landbs[i].wavNameSize);
                        landb.newLandbFileSize += landb.landbs[i].wavNameSize;
                        landb.newBlockLength += landb.landbs[i].wavNameSize;

                        landb.landbs[i].wavName = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(tmp);
                    }

                    landb.landbs[i].blockUnknownNameSize = br.ReadInt32();
                    landb.newLandbFileSize += 4;
                    landb.newBlockLength += 4;

                    landb.landbs[i].unknownNameSize = br.ReadInt32();
                    landb.newLandbFileSize += 4;
                    landb.newBlockLength += 4;

                    tmp = br.ReadBytes(landb.landbs[i].unknownNameSize);
                    landb.newLandbFileSize += landb.landbs[i].unknownNameSize;
                    landb.newBlockLength += landb.landbs[i].unknownNameSize;

                    landb.landbs[i].unknownName = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(tmp);

                    landb.landbs[i].zero2 = br.ReadInt32();
                    landb.newLandbFileSize += 4;
                    landb.newBlockLength += 4;

                    landb.landbs[i].blockLangresSize = br.ReadInt32();
                    landb.newLandbFileSize += 4;
                    landb.newBlockLength += 4;

                    landb.landbs[i].blockActorNameSize = br.ReadInt32();
                    landb.newLandbFileSize += 4;
                    landb.newBlockLength += 4;

                    //Don't calculate new size with actor name!
                    landb.landbs[i].actorNameSize = br.ReadInt32();
                    tmp = br.ReadBytes(landb.landbs[i].actorNameSize);
                    landb.landbs[i].actorName = landb.isUnicode ? Encoding.UTF8.GetString(tmp) : Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(tmp);

                    landb.landbs[i].blockActorSpeechSize = br.ReadInt32();
                    landb.newLandbFileSize += 4;
                    landb.newBlockLength += 4;

                    landb.landbs[i].actorSpeechSize = br.ReadInt32();
                    tmp = br.ReadBytes(landb.landbs[i].actorSpeechSize);
                    landb.landbs[i].actorSpeech = landb.isUnicode ? Encoding.UTF8.GetString(tmp) : Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(tmp);

                    //Don't calculate actor speech's size!

                    landb.landbs[i].blockSize = br.ReadInt32();
                    landb.newLandbFileSize += 4;
                    landb.newBlockLength += 4;

                    landb.landbs[i].someValue = br.ReadInt32();
                    landb.newLandbFileSize += 4;
                    landb.newBlockLength += 4;

                    if (landb.isUnicode)
                    {
                        landb.landbs[i].blockSizeUni = br.ReadInt32();
                        landb.newLandbFileSize += 4;
                        landb.newBlockLength += 4;

                        landb.landbs[i].someDataUni = br.ReadBytes(landb.landbs[i].blockSizeUni - 4);
                        landb.newLandbFileSize += landb.landbs[i].someDataUni.Length;
                        landb.newBlockLength += landb.landbs[i].someDataUni.Length;
                    }

                    landb.landbs[i].flags = br.ReadInt32();
                    landb.newLandbFileSize += 4;
                    landb.newBlockLength += 4;

                    tmp = Encoding.ASCII.GetBytes(Convert.ToString(landb.landbs[i].flags, 2));

                    landb.flags[i] = new ClassesStructs.FlagsClass.LangdbFlagClass();
                    landb.flags[i].flags = new byte[8];

                    int maxSize = tmp.Length < 8 ? tmp.Length : 8;

                    for(int f = 0; f < 8; f++)
                    {
                        landb.flags[i].flags[f] = Convert.ToByte('0');
                    }
                    
                    for(int f = 0; f < maxSize; f++)
                    {
                        landb.flags[i].flags[f] = tmp[f];
                    }

                    tmp = null;
                }

                landb.commonSomeDataLen = br.ReadInt32();
                landb.someData = br.ReadBytes(landb.commonSomeDataLen - 4);
                landb.newLandbFileSize += landb.commonSomeDataLen;

                landb.lastLandbData = new LastLandbData();
                landb.lastLandbData.Unknown1 = br.ReadInt32();
                landb.lastLandbData.Unknown2 = br.ReadInt32();
                landb.lastLandbData.Unknown3 = br.ReadInt32();
                landb.lastLandbData.Unknown4 = br.ReadInt32();

                landb.newLandbFileSize += 16;
            }
            catch
            {
                return null;
            }

            return landb;
        }

        private static int CheckNumbers(List<CommonText> txts, LandbClass landb)
        {
            int result = -1;
            int countLangres = 0;
            int countStrings = 0;

            for(int i = 0; i < landb.landbCount; i++)
            {
                for(int j = 0; j < txts.Count; j++)
                {
                    if (landb.landbs[i].anmID == txts[j].strNumber) countLangres++;
                    if (landb.landbs[i].stringNumber == txts[i].strNumber) countStrings++;
                }
            }

            if (countLangres < countStrings) result = 0;
            else if (countLangres > countStrings) result = 1;

            return result;
        }

        private static int GetIndex(LandbClass landb, uint searchNum)
        {
            for(int i = 0; i < landb.landbCount; i++)
            {
                if (landb.landbs[i].anmID == searchNum) return i;
            }

            return 0;
        }

        private static LandbClass ReplaceStrings(LandbClass landb, List<CommonText> commonTexts, int type)
        {
            for(int i = 0; i < landb.landbCount; i++)
            {
                if (MainMenu.settings.importingOfName) landb.landbs[i].actorName = type == 1 ? commonTexts[GetIndex(landb, landb.landbs[i].anmID)].actorName : commonTexts[(int)landb.landbs[i].stringNumber - 1].actorName;
                landb.landbs[i].actorSpeech = type == 1 ? commonTexts[GetIndex(landb, landb.landbs[i].anmID)].actorSpeechTranslation : commonTexts[(int)landb.landbs[i].stringNumber - 1].actorSpeechTranslation;

                if (landb.isUnicode && MainMenu.settings.unicodeSettings == 1) landb.landbs[i].actorSpeech = Methods.ConvertString(landb.landbs[i].actorSpeech, false);
            }

            return landb;
        }

        public static string DoWork(string InputFile, string TxtFile, bool extract)
        {
            string result = "";

            FileInfo fi = new FileInfo(InputFile);

            byte[] buffer = File.ReadAllBytes(InputFile);
            MemoryStream ms = new MemoryStream(buffer);
            BinaryReader br = new BinaryReader(ms);

            try
            {
                byte[] checkHeader = br.ReadBytes(4);

                bool newFormat = false;
                bool hasCRC64Langres = false;
                bool isUnicode = false;
                int pos = 4;

                if ((Encoding.ASCII.GetString(checkHeader) == "5VSM") || (Encoding.ASCII.GetString(checkHeader) == "6VSM"))
                {
                    newFormat = true;
                    pos = 16;
                }

                br.BaseStream.Seek(pos, SeekOrigin.Begin);

                int countBlocks = br.ReadInt32();

                string[] classes = new string[countBlocks];

                for (int i = 0; i < countBlocks; i++)
                {
                   byte[] tmp = br.ReadBytes(8);
                   classes[i] = BitConverter.ToString(tmp);
                   if(classes[i] == "B0-9F-D8-63-34-02-4F-00") hasCRC64Langres = true;
                   if(classes[i] == "53-DC-A5-33-DB-D6-DC-7E") isUnicode = true;
                   tmp = br.ReadBytes(4); //Some values (in oldest games I found some values in *.vers files)
                }

                LandbClass landbs = GetStringsFromLandb(br, hasCRC64Langres, newFormat, isUnicode);
                br.Close();
                ms.Close();

                buffer = null;

                if (landbs == null)
                {
                    return "File " + fi.Name + ": unknown error.";
                }
                if ((landbs != null) && (landbs.landbCount == 0))
                {
                    landbs = null;
                    GC.Collect();
                    return fi.Name + " is EMPTY.";
                }

                if (extract)
                {
                    ClassesStructs.Text.CommonTextClass txts = new CommonTextClass();

                    txts.txtList = new List<CommonText>();

                    for (int i = 0; i < landbs.landbCount; i++)
                    {
                        ClassesStructs.Text.CommonText txt;

                        txt.isBothSpeeches = true;
                        txt.strNumber = MainMenu.settings.exportRealID || MainMenu.settings.newTxtFormat ? landbs.landbs[i].anmID : landbs.landbs[i].stringNumber;
                        txt.actorName = landbs.landbs[i].actorName;
                        txt.actorSpeechOriginal = landbs.landbs[i].actorSpeech;
                        txt.actorSpeechTranslation = landbs.landbs[i].actorSpeech;
                        txt.flags = Encoding.ASCII.GetString(landbs.flags[i].flags);

                        txts.txtList.Add(txt);
                    }

                    if (MainMenu.settings.sortSameString) txts = Methods.SortString(txts);

                    string outputFile = MainMenu.settings.pathForOutputFolder + "\\" + fi.Name.Remove(fi.Name.Length - 5, 5);
                    outputFile += MainMenu.settings.tsvFormat ? "tsv" : "txt";

                    switch(MainMenu.settings.newTxtFormat)
                    {
                        case true:
                            Texts.SaveText.NewMethod(txts.txtList, landbs.isUnicode, outputFile);
                            break;

                        default:
                            Texts.SaveText.OldMethod(txts.txtList, false, landbs.isUnicode, outputFile);
                            break;
                    }

                    txts.txtList.Clear();
                    txts = null;

                    result = fi.Name + " successfully extracted.";
                }
                else
                {
                    ClassesStructs.Text.CommonTextClass txts = new CommonTextClass();
                    txts.txtList = ReadText.GetStrings(TxtFile);

                    if (txts.txtList.Count < landbs.landbCount)
                    {
                        FileInfo txtFI = new FileInfo(TxtFile);
                        return "Not enough strings in " + txtFI.Name + " for " + fi.Name + " file.";
                    }

                    int type = CheckNumbers(txts.txtList, landbs);

                    if (type == -1) return "I don't know which type of number strings select in " + fi.Name + " file.";

                    landbs = ReplaceStrings(landbs, txts.txtList, type);
                }
            }
            catch
            {
                if (br != null) br.Close();
                if (ms != null) ms.Close();

                result = "Something wrong with langdb file " + fi.Name;
            }

            GC.Collect();
            return result;
        }
    }
}
