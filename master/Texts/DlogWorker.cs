using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TTG_Tools.ClassesStructs.Text;
using TTG_Tools.InEngineWords;

namespace TTG_Tools.Texts
{
    public class DlogWorker
    {
        private static DlogClass GetStringsFromDlog(BinaryReader br)
        {
            DlogClass dlog = new DlogClass();
            try
            {
                dlog.unknown1 = br.ReadInt32();
                dlog.unknown2 = br.ReadInt32();

                dlog.longUnknown1 = br.ReadUInt64();
                dlog.zero = br.ReadInt32();
                dlog.blockSize = br.ReadInt32();
                dlog.block = br.ReadBytes(dlog.blockSize - 4);
                dlog.blockFileNameSize = br.ReadInt32();
                dlog.fileNameSize = br.ReadInt32();
                byte[] tmp = br.ReadBytes(dlog.fileNameSize);
                dlog.dlogFileName = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(tmp);

                dlog.someValue1 = br.ReadInt32();
                dlog.someValue2 = br.ReadInt32();
                dlog.longUnknown2 = br.ReadUInt64();
                dlog.someValue3 = br.ReadInt32();
                dlog.langdbBLockSize = br.ReadInt32();

                dlog.landb = new DlogLandb();

                dlog.landb.blockSize1 = br.ReadInt32();
                dlog.landb.someValue1 = br.ReadInt32();

                dlog.landb.blockSize2 = br.ReadInt32();
                dlog.landb.someValue2 = br.ReadInt32();

                dlog.landb.blockLength = br.ReadInt32();
                dlog.landb.landbCount = br.ReadInt32();

                dlog.landb.landbs = new DlogLandbs[dlog.landb.landbCount];

                for (int i = 0; i < dlog.landb.landbCount; i++)
                {
                    dlog.landb.landbs[i].stringNumber = ((uint)i + 1);
                    dlog.landb.landbs[i].anmID = br.ReadUInt32();
                    dlog.landb.landbs[i].wavID = br.ReadUInt32();
                    dlog.landb.landbs[i].zero = br.ReadInt32();

                    dlog.landb.landbs[i].blockAnmNameSize = br.ReadInt32();
                    dlog.landb.landbs[i].anmNameSize = br.ReadInt32();
                    tmp = br.ReadBytes(dlog.landb.landbs[i].anmNameSize);
                    dlog.landb.landbs[i].anmName = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(tmp);

                    dlog.landb.landbs[i].blockWavNameSize = br.ReadInt32();
                    dlog.landb.landbs[i].wavNameSize = br.ReadInt32();
                    tmp = br.ReadBytes(dlog.landb.landbs[i].wavNameSize);
                    dlog.landb.landbs[i].wavName = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(tmp);

                    dlog.landb.landbs[i].blockLangresSize = br.ReadInt32();
                    dlog.landb.landbs[i].langresStrsCount = br.ReadInt32();

                    if ((dlog.landb.landbs[i].blockLangresSize > 8) && (dlog.landb.landbs[i].langresStrsCount > 0))
                    {
                        dlog.landb.landbs[i].lang = new LangresDB[dlog.landb.landbs[i].langresStrsCount];

                        for (int j = 0; j < dlog.landb.landbs[i].langresStrsCount; j++)
                        {
                            dlog.landb.landbs[i].lang[j].blockActorNameSize = br.ReadInt32();
                            dlog.landb.landbs[i].lang[j].actorNameSize = br.ReadInt32();
                            tmp = br.ReadBytes(dlog.landb.landbs[i].lang[j].actorNameSize);
                            dlog.landb.landbs[i].lang[j].actorName = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(tmp);

                            dlog.landb.landbs[i].lang[j].blockActorSpeechSize = br.ReadInt32();
                            dlog.landb.landbs[i].lang[j].actorSpeechSize = br.ReadInt32();
                            tmp = br.ReadBytes(dlog.landb.landbs[i].lang[j].actorSpeechSize);
                            dlog.landb.landbs[i].lang[j].actorSpeech = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(tmp);

                            dlog.landb.landbs[i].lang[j].someValue1 = br.ReadInt32();
                            dlog.landb.landbs[i].lang[j].someValue2 = br.ReadInt32();
                        }
                    }

                    dlog.landb.landbs[i].someValue3 = br.ReadInt32();
                    dlog.landb.landbs[i].someValue4 = br.ReadInt32();
                }

                dlog.landb.someDataAfterLandb = new DlogSomeDataAfterLandb();
                dlog.landb.someDataAfterLandb.commonBlockSize = br.ReadInt32();
                dlog.landb.someDataAfterLandb.firstBlockSize = br.ReadInt32();
                dlog.landb.someDataAfterLandb.firstBlock = br.ReadBytes(dlog.landb.someDataAfterLandb.firstBlockSize - 4);
                dlog.landb.someDataAfterLandb.secondBlockSize = br.ReadInt32();
                dlog.landb.someDataAfterLandb.secondBlock = br.ReadBytes(dlog.landb.someDataAfterLandb.secondBlockSize - 4);

                dlog.landb.lastLandbData = new DlogLastLandbData();
                dlog.landb.lastLandbData.Unknown1 = br.ReadInt32();
                dlog.landb.lastLandbData.Unknown2 = br.ReadInt32();
                dlog.landb.lastLandbData.Unknown3 = br.ReadInt32();
                dlog.landb.lastLandbData.Unknown4 = br.ReadInt32();

                return dlog;
            }
            catch
            {
                return null;
            }
        }

        public static string DoWork(string InputFile, bool extract)
        {
            string result = "";

            FileInfo fi = new FileInfo(InputFile);
            byte[] buffer = File.ReadAllBytes(fi.FullName);

            MemoryStream ms = new MemoryStream(buffer);
            BinaryReader br = new BinaryReader(ms);

            try
            {
                byte[] header = br.ReadBytes(4);
                int count = br.ReadInt32();

                byte[] tmp;
                int val;

                bool hasLandb = false;

                for(int i = 0; i < count; i++)
                {
                    tmp = br.ReadBytes(8);
                    val = br.ReadInt32();

                    if(BitConverter.ToString(tmp) == BitConverter.ToString(BitConverter.GetBytes(CRCs.CRC64(0, ClassStructsNames.languagedbClass.ToLower())))) hasLandb = true;
                }

                if(!hasLandb)
                {
                    br.Close();
                    ms.Close();

                    return "File " + fi.Name + " doesn't have langdb strings.";
                }

                DlogClass dlog = GetStringsFromDlog(br);

                br.Close();
                ms.Close();

                if(dlog == null)
                {
                    return "File " + fi.Name + ": unknown error.";
                }

                if((dlog != null) && (dlog.landb.landbCount == 0))
                {
                    dlog = null;
                    GC.Collect();
                    return fi.Name + " is EMPTY.";
                }

                if(extract)
                {
                    ClassesStructs.Text.CommonTextClass txts = new CommonTextClass();

                    txts.txtList = new List<CommonText>();
                    uint c = 1;

                    for (int i = 0; i < dlog.landb.landbCount; i++)
                    {
                        ClassesStructs.Text.CommonText txt;

                        txt.isBothSpeeches = true;
                        txt.strNumber = MainMenu.settings.exportRealID ? dlog.landb.landbs[i].anmID : c;

                        for (int j = 0; j < dlog.landb.landbs[i].langresStrsCount; j++)
                        {
                            if(dlog.landb.landbs[i].langresStrsCount > 1)
                            {
                                txt.strNumber = MainMenu.settings.exportRealID ? dlog.landb.landbs[i].anmID : c;
                            }
                            txt.actorName = dlog.landb.landbs[i].lang[j].actorName;
                            txt.actorSpeechOriginal = dlog.landb.landbs[i].lang[j].actorSpeech;
                            txt.actorSpeechTranslation = dlog.landb.landbs[i].lang[j].actorSpeech;
                            txt.flags = "000"; //default will be 000

                            txts.txtList.Add(txt);
                            c++;
                        }
                    }

                    if (MainMenu.settings.sortSameString) txts = Methods.SortString(txts);

                    string outputFile = MainMenu.settings.pathForOutputFolder + "\\" + fi.Name.Remove(fi.Name.Length - 4, 4);
                    outputFile += MainMenu.settings.tsvFormat ? "tsv" : "txt";

                    switch(MainMenu.settings.newTxtFormat)
                    {
                        case true:
                            Texts.SaveText.NewMethod(txts.txtList, false, outputFile);
                            break;

                        default:
                            Texts.SaveText.OldMethod(txts.txtList, false, false, outputFile);
                            break;
                    }

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

                result = "Something wrong with file " + fi.Name + ".";
            }

            GC.Collect();
            return result;
        }
    }
}
