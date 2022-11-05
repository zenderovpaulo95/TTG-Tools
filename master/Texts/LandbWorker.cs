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
        private static LandbClass GetStringsFromLandb(BinaryReader br, bool hasFlags)
        {
            LandbClass landb = new LandbClass();

            try
            {
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
                    landb.landbs[i].anmID = br.ReadUInt32();
                    landb.landbs[i].zero1 = br.ReadInt32();

                    landb.landbs[i].blockAnmNameSize = br.ReadInt32();
                    landb.landbs[i].anmNameSize = br.ReadInt32();
                    tmp = br.ReadBytes(landb.landbs[i].anmNameSize);
                    landb.landbs[i].anmName = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(tmp);

                    landb.landbs[i].blockWavNameSize = br.ReadInt32();
                    landb.landbs[i].wavNameSize = br.ReadInt32();
                    tmp = br.ReadBytes(landb.landbs[i].wavNameSize);
                    landb.landbs[i].wavName = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(tmp);

                    landb.landbs[i].blockUnknownNameSize = br.ReadInt32();
                    landb.landbs[i].unknownNameSize = br.ReadInt32();
                    tmp = br.ReadBytes(landb.landbs[i].unknownNameSize);
                    landb.landbs[i].unknownName = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(tmp);

                    landb.landbs[i].zero2 = br.ReadInt32();

                    landb.landbs[i].blockLangresSize = br.ReadInt32();
                    landb.landbs[i].blockActorNameSize = br.ReadInt32();
                    landb.landbs[i].actorNameSize = br.ReadInt32();
                    tmp = br.ReadBytes(landb.landbs[i].actorNameSize);
                    landb.landbs[i].actorName = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(tmp);

                    landb.landbs[i].blockActorSpeechSize = br.ReadInt32();
                    landb.landbs[i].actorSpeechSize = br.ReadInt32();
                    tmp = br.ReadBytes(landb.landbs[i].actorSpeechSize);
                    landb.landbs[i].actorSpeech = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(tmp);

                    landb.landbs[i].blockSize = br.ReadInt32();
                    landb.landbs[i].someValue = br.ReadInt32();

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

                landb.unknownData = null;
                if((br.BaseStream.Length - br.BaseStream.Position) > 0)
                {
                    int size = (int)(br.BaseStream.Length - br.BaseStream.Position);
                    landb.unknownData = br.ReadBytes(size);

                }

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
                int countBlocks = br.ReadInt32();

                string[] classes = new string[countBlocks];

                bool hasFlags = false;
                byte[] checkBlock = br.ReadBytes(8);
                br.BaseStream.Seek(8, SeekOrigin.Begin);
                ulong checkCRC64 = 0;

                for (int i = 0; i < countBlocks; i++)
                {
                   byte[] tmp = br.ReadBytes(8);
                   classes[i] = BitConverter.ToString(tmp);
                   if (classes[i].ToLower() == BitConverter.ToString(BitConverter.GetBytes(CRCs.CRC64(checkCRC64, InEngineWords.ClassStructsNames.flagsClass.ToLower())))) hasFlags = true;
                   tmp = br.ReadBytes(4); //Some values (in oldest games I found some values in *.vers files
                }

                LandbClass landbs = GetStringsFromLandb(br, hasFlags);
                br.Close();
                ms.Close();

                buffer = null;

                if (landbs == null)
                {
                    return "File " + fi.Name + ": unknown error.";
                }
                if (landbs != null && landbs.landbCount == 0)
                {
                    landbs = null;
                    GC.Collect();
                    return fi.Name + " is EMPTY.";
                }

                if (extract)
                {
                    //TO DO:
                    //Need to think about sort same strings!

                    if (File.Exists(MainMenu.settings.pathForOutputFolder + "\\" + fi.Name.Remove(fi.Name.Length - 5, 5) + "txt")) File.Delete(MainMenu.settings.pathForOutputFolder + "\\" + fi.Name.Remove(fi.Name.Length - 5, 5) + "txt");
                    FileStream fs = new FileStream(MainMenu.settings.pathForOutputFolder + "\\" + fi.Name.Remove(fi.Name.Length - 5, 5) + "txt", FileMode.CreateNew);
                    StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);

                    for (int i = 0; i < landbs.landbCount; i++)
                    {
                        if (MainMenu.settings.exportRealID) sw.WriteLine(landbs.landbs[i].wavID + ") " + landbs.landbs[i].actorName);
                        else sw.WriteLine(landbs.landbs[i].stringNumber + ") " + landbs.landbs[i].actorName);
                        sw.WriteLine(landbs.landbs[i].actorSpeech);
                    }

                    sw.Close();
                    fs.Close();

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
