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

                if(landb.isNewFormat)
                {
                    var pos = br.BaseStream.Position;
                    br.BaseStream.Seek(4, SeekOrigin.Begin);
                    landb.landbFileSize = br.ReadInt32();
                    landb.landbLastFileSize = br.ReadInt32();
                    br.BaseStream.Seek(pos, SeekOrigin.Begin);
                }

                landb.blockSize1 = br.ReadInt32();
                landb.someValue1 = br.ReadInt32();
                landb.blockSize2 = br.ReadInt32();
                landb.someValue2 = br.ReadInt32();

                landb.blockLength = br.ReadInt32();
                landb.landbCount = br.ReadInt32();

                landb.landbs = new Landb[landb.landbCount];
                landb.flags = new ClassesStructs.FlagsClass.LangdbFlagClass[landb.landbCount];

                byte[] tmp = null;                

                for (int i = 0; i < landb.landbCount; i++)
                {
                    landb.landbs[i].stringNumber = (uint)(i + 1);
                    landb.landbs[i].wavID = br.ReadUInt32();
                    if (landb.hasMetaLangresName) landb.landbs[i].crc64Langres = br.ReadUInt64();
                    landb.landbs[i].anmID = br.ReadUInt32();
                    landb.landbs[i].zero1 = br.ReadInt32();

                    landb.landbs[i].blockAnmNameSize = br.ReadInt32();
                    if(landb.hasMetaLangresName)
                    {
                        tmp = br.ReadBytes(8);
                        landb.landbs[i].anmNameSize = 8;
                        landb.landbs[i].anmName = BitConverter.ToString(tmp);
                    }
                    else
                    {
                        landb.landbs[i].anmNameSize = br.ReadInt32();
                        tmp = br.ReadBytes(landb.landbs[i].anmNameSize);
                        landb.landbs[i].anmName = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(tmp);
                    }

                    landb.landbs[i].blockWavNameSize = br.ReadInt32();
                    if (landb.hasMetaLangresName)
                    {
                        tmp = br.ReadBytes(8);
                        landb.landbs[i].wavNameSize = 8;
                        landb.landbs[i].wavName = BitConverter.ToString(tmp);
                    }
                    else
                    {
                        landb.landbs[i].wavNameSize = br.ReadInt32();
                        tmp = br.ReadBytes(landb.landbs[i].wavNameSize);
                        landb.landbs[i].wavName = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(tmp);
                    }

                    landb.landbs[i].blockUnknownNameSize = br.ReadInt32();
                    landb.landbs[i].unknownNameSize = br.ReadInt32();
                    tmp = br.ReadBytes(landb.landbs[i].unknownNameSize);
                    landb.landbs[i].unknownName = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(tmp);

                    landb.landbs[i].zero2 = br.ReadInt32();

                    landb.landbs[i].blockLangresSize = br.ReadInt32();
                    landb.landbs[i].blockActorNameSize = br.ReadInt32();
                    landb.landbs[i].actorNameSize = br.ReadInt32();
                    tmp = br.ReadBytes(landb.landbs[i].actorNameSize);
                    landb.landbs[i].actorName = landb.isUnicode ? Encoding.UTF8.GetString(tmp) : Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(tmp);

                    landb.landbs[i].blockActorSpeechSize = br.ReadInt32();
                    landb.landbs[i].actorSpeechSize = br.ReadInt32();
                    tmp = br.ReadBytes(landb.landbs[i].actorSpeechSize);
                    landb.landbs[i].actorSpeech = landb.isUnicode ? Encoding.UTF8.GetString(tmp) : Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(tmp);

                    landb.landbs[i].blockSize = br.ReadInt32();
                    landb.landbs[i].someValue = br.ReadInt32();

                    if(landb.isUnicode)
                    {
                        landb.landbs[i].blockSizeUni = br.ReadInt32();
                        landb.landbs[i].someDataUni = br.ReadBytes(landb.landbs[i].blockSizeUni - 4);
                    }

                    landb.landbs[i].flags = br.ReadInt32();
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

                landb.someAfterData = new SomeDataAfterLandb();
                landb.someAfterData.commonBlockSize = br.ReadInt32();
                landb.someAfterData.firstBlockSize = br.ReadInt32();
                landb.someAfterData.firstBlock = br.ReadBytes(landb.someAfterData.firstBlockSize - 4);
                landb.someAfterData.secondBlockSize = br.ReadInt32();
                landb.someAfterData.secondBlock = br.ReadBytes(landb.someAfterData.secondBlockSize - 4);

                landb.lastLandbData = new LastLandbData();
                landb.lastLandbData.Unknown1 = br.ReadInt32();
                landb.lastLandbData.Unknown2 = br.ReadInt32();
                landb.lastLandbData.Unknown3 = br.ReadInt32();
                landb.lastLandbData.Unknown4 = br.ReadInt32();
            }
            catch
            {
                return null;
            }

            return landb;
        }

        public static string DoWork(string InputFile, bool extract)
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
                   tmp = br.ReadBytes(4); //Some values (in oldest games I found some values in *.vers files
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
                        txt.strNumber = MainMenu.settings.exportRealID ? landbs.landbs[i].anmID : landbs.landbs[i].stringNumber;
                        txt.actorName = landbs.landbs[i].actorName;
                        txt.actorSpeechOriginal = landbs.landbs[i].actorSpeech;
                        txt.actorSpeechTranslation = landbs.landbs[i].actorSpeech;
                        txt.flags = Encoding.ASCII.GetString(landbs.flags[i].flags);

                        txts.txtList.Add(txt);
                    }

                    if (MainMenu.settings.sortSameString) txts = Methods.SortString(txts);

                    if (File.Exists(MainMenu.settings.pathForOutputFolder + "\\" + fi.Name.Remove(fi.Name.Length - 5, 5) + "txt")) File.Delete(MainMenu.settings.pathForOutputFolder + "\\" + fi.Name.Remove(fi.Name.Length - 5, 5) + "txt");
                    FileStream fs = new FileStream(MainMenu.settings.pathForOutputFolder + "\\" + fi.Name.Remove(fi.Name.Length - 5, 5) + "txt", FileMode.CreateNew);
                    StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);

                    for (int i = 0; i < txts.txtList.Count; i++)
                    {
                        sw.WriteLine(txts.txtList[i].strNumber + ") " + landbs.landbs[i].actorName);
                        sw.WriteLine(txts.txtList[i].actorSpeechOriginal);
                    }

                    sw.Close();
                    fs.Close();

                    txts.txtList.Clear();
                    txts = null;

                    result = fi.Name + " successfully extracted.";
                }
                else
                {

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
