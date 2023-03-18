using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTG_Tools.ClassesStructs.Text;
using System.IO;
using System.Windows.Forms.VisualStyles;

namespace TTG_Tools.Texts
{
    public class LangdbWorker
    {
        private static LangdbClass GetStringsFromLangdb(BinaryReader br, bool hasFlags)
        {
            try
            {
                LangdbClass langdb = new LangdbClass();
                langdb.blockLength = 0;
                int checkBlockLength = br.ReadInt32();
                long checkSize = br.BaseStream.Length - br.BaseStream.Position + 4;

                langdb.isBlockLength = false;

                if (checkSize == checkBlockLength)
                {
                    langdb.blockLength = checkBlockLength;
                    langdb.newBlockLength = 8;
                    langdb.isBlockLength = true;
                    langdb.langdbCount = br.ReadInt32();
                }
                else
                {
                    langdb.blockLength = -1;
                    langdb.isBlockLength = false;
                    langdb.langdbCount = checkBlockLength;
                }

                langdb.langdbs = new langdb[langdb.langdbCount];

                langdb.flags = new ClassesStructs.FlagsClass.LangdbFlagClass[langdb.langdbCount];

                for (int i = 0; i < langdb.langdbCount; i++)
                {
                    langdb.langdbs[i].stringNumber = (uint)(i + 1);
                    langdb.langdbs[i].anmID = br.ReadUInt32();
                    langdb.newBlockLength += 4;

                    langdb.langdbs[i].voxID = br.ReadUInt32();
                    langdb.newBlockLength += 4;

                    int blockSize = -1;

                    if (langdb.isBlockLength)
                    {
                        blockSize = br.ReadInt32();
                        langdb.newBlockLength += 4;
                    }

                    int stringLength = br.ReadInt32();
                    langdb.newBlockLength += 4;

                    //Don't calculate actor name's length
                    byte[] tmp = br.ReadBytes(stringLength);
                    langdb.langdbs[i].actorName = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(tmp);

                    if (langdb.isBlockLength)
                    {
                        blockSize = br.ReadInt32();
                        langdb.newBlockLength += 4;
                    }

                    stringLength = br.ReadInt32();
                    langdb.newBlockLength += 4;

                    //Don't calculate actor speech's length
                    tmp = br.ReadBytes(stringLength);
                    langdb.langdbs[i].actorSpeech = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(tmp);

                    if (langdb.isBlockLength)
                    {
                        blockSize = br.ReadInt32();
                        langdb.newBlockLength += 4;
                    }

                    stringLength = br.ReadInt32();
                    langdb.newBlockLength += 4;

                    tmp = br.ReadBytes(stringLength);
                    langdb.langdbs[i].anmFile = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(tmp);
                    langdb.newBlockLength += stringLength;

                    if (langdb.isBlockLength)
                    {
                        blockSize = br.ReadInt32();
                        langdb.newBlockLength += 4;
                    }

                    stringLength = br.ReadInt32();
                    langdb.newBlockLength += 4;

                    tmp = br.ReadBytes(stringLength);
                    langdb.langdbs[i].voxFile = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(tmp);
                    langdb.newBlockLength += stringLength;

                    langdb.flags[i] = new ClassesStructs.FlagsClass.LangdbFlagClass();
                    langdb.flags[i].flags = br.ReadBytes(3);
                    langdb.newBlockLength += 3;

                    langdb.langdbs[i].zero = br.ReadInt32();
                    langdb.newBlockLength += 4;
                }

                return langdb;
            }
            catch
            {
                return null;
            }
        }

        private static int RebuildLangdb(BinaryReader br, string outputFile, LangdbClass langdb)
        {
            if (File.Exists(outputFile)) File.Delete(outputFile);
            FileStream fs = new FileStream(outputFile, FileMode.CreateNew);
            BinaryWriter bw = new BinaryWriter(fs);

            try
            {
                byte[] header = br.ReadBytes(4);
                bw.Write(header);

                int count = br.ReadInt32();
                bw.Write(count);

                byte[] tmp = br.ReadBytes(8);
                br.BaseStream.Seek(8, SeekOrigin.Begin);

                if (BitConverter.ToString(tmp) == BitConverter.ToString(BitConverter.GetBytes(CRCs.CRC64(0, InEngineWords.ClassStructsNames.languagedatabaseClass.ToLower()))))
                {
                    for (int i = 0; i < count; i++)
                    {
                        tmp = br.ReadBytes(8);
                        bw.Write(tmp);

                        tmp = br.ReadBytes(4);
                        bw.Write(tmp);
                    }
                }
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        int len = br.ReadInt32();
                        bw.Write(len);

                        tmp = br.ReadBytes(len);
                        bw.Write(tmp);

                        tmp = br.ReadBytes(4);
                        bw.Write(tmp);
                    }
                }

                var pos = br.BaseStream.Position;

                if(langdb.isBlockLength)
                {
                    bw.Write(langdb.blockLength);
                }

                bw.Write(langdb.langdbCount);

                for(int i = 0; i < langdb.langdbCount; i++)
                {
                    bw.Write(langdb.langdbs[i].anmID);
                    bw.Write(langdb.langdbs[i].voxID);

                    byte[] tmpActorName = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetBytes(langdb.langdbs[i].actorName);
                    int actorNameLen = tmpActorName.Length;
                    int actorNameBlockLen = actorNameLen + 8;

                    byte[] tmpActorSpeech = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetBytes(langdb.langdbs[i].actorSpeech);
                    int actorSpeechLen = tmpActorSpeech.Length;
                    int actorSpeechBlockLen = tmpActorSpeech.Length + 8;

                    if (langdb.isBlockLength)
                    {
                        bw.Write(actorNameBlockLen);
                    }
                    
                    bw.Write(actorNameLen);
                    bw.Write(tmpActorName);
                    langdb.newBlockLength += tmpActorName.Length;

                    if(langdb.isBlockLength)
                    {
                        bw.Write(actorSpeechBlockLen);
                    }

                    bw.Write(actorSpeechLen);
                    bw.Write(tmpActorSpeech);
                    langdb.newBlockLength += tmpActorSpeech.Length;

                    int blockVoxLen = 0, blockAnmLen = 0;
                    int voxLen = 0, anmLen = 0;

                    tmp = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetBytes(langdb.langdbs[i].anmFile);
                    anmLen = tmp.Length;
                    blockAnmLen = anmLen + 8;

                    if(langdb.isBlockLength)
                    {
                        bw.Write(blockAnmLen);
                    }

                    bw.Write(anmLen);
                    bw.Write(tmp);

                    tmp = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetBytes(langdb.langdbs[i].voxFile);
                    voxLen = tmp.Length;
                    blockVoxLen = voxLen + 8;

                    if (langdb.isBlockLength)
                    {
                        bw.Write(blockVoxLen);
                    }

                    bw.Write(voxLen);
                    bw.Write(tmp);

                    bw.Write(langdb.flags[i].flags);
                    bw.Write(langdb.langdbs[i].zero);
                }

                if (langdb.isBlockLength)
                {
                    bw.BaseStream.Seek(pos, SeekOrigin.Begin);
                    bw.Write(langdb.newBlockLength);
                }

                bw.Close();
                fs.Close();

                return 0;
            }
            catch
            {
                if (bw != null) bw.Close();
                if (fs != null) fs.Close();
                return -1;
            }
        }

        private static LangdbClass ReplaceStrings(LangdbClass langdb, List<CommonText> commonTexts, int type)
        {
            int index;
            for (int i = 0; i < langdb.langdbCount; i++)
            {
                index = -1;

                if (MainMenu.settings.importingOfName)
                {
                    index = type == 1 ? Methods.GetIndex(commonTexts, langdb.langdbs[i].anmID) : Methods.GetIndex(commonTexts, langdb.langdbs[i].stringNumber);
                    if(index != -1) langdb.langdbs[i].actorName = commonTexts[index].actorName;
                }

                index = type == 1 ? Methods.GetIndex(commonTexts, langdb.langdbs[i].anmID) : Methods.GetIndex(commonTexts, langdb.langdbs[i].stringNumber);
                if (index != -1) langdb.langdbs[i].actorSpeech = commonTexts[index].actorSpeechTranslation;

                if(MainMenu.settings.newTxtFormat && MainMenu.settings.changeLangFlags
                    && (index != -1))
                {
                    string tmpFlags = commonTexts[index].flags;

                    byte[] tmpBytesFlags = Encoding.ASCII.GetBytes(tmpFlags);

                    int count = tmpBytesFlags.Length > 3 ? 3 : tmpBytesFlags.Length;

                    for (int j = 0; j < count; j++)
                    {
                        langdb.flags[i].flags[j] = tmpBytesFlags[j];
                    }

                }
            }

            return langdb;
        }

        private static int CheckNumbers(List<CommonText> txts, LangdbClass langdb)
        {
            int result = -1;
            int countLangres = 0;
            int countStrings = 0;

            for (int i = 0; i < langdb.langdbCount; i++)
            {
                for (int j = 0; j < txts.Count; j++)
                {
                    if (langdb.langdbs[i].anmID == txts[j].strNumber) countLangres++;
                    if (langdb.langdbs[i].stringNumber == txts[i].strNumber) countStrings++;
                }
            }

            if (countLangres < countStrings) result = 0;
            else if (countLangres > countStrings) result = 1;

            return result;
        }

        public static string DoWork(string InputFile, string txtFile, bool extract, bool FullEncrypt, ref byte[] EncKey, int version)
        {
            string result = "";

            FileInfo fi = new FileInfo(InputFile);

            byte[] buffer = File.ReadAllBytes(InputFile);
            MemoryStream ms = new MemoryStream(buffer);
            BinaryReader br = new BinaryReader(ms);

            string additionalMessage = "";

            try
            {
                byte[] checkHeader = br.ReadBytes(4);

                if (Encoding.ASCII.GetString(checkHeader) != "ERTM") //Supposed this langdb file encrypted
                {
                    //First trying decrypt probably encrypted langdb
                    try
                    {
                        string info = Methods.FindLangresDecryptKey(buffer, ref EncKey, ref version);

                        if ((info != null) && (info != "OK"))
                        {
                            additionalMessage = "Langdb file was encrypted, but I decrypted. " + info;
                        }
                    }
                    catch
                    {
                        result = "Maybe that LANGDB file encrypted. Try to decrypt first: " + fi.Name;

                        return result;
                    }
                }

                int countBlocks = br.ReadInt32();

                string[] classes = new string[countBlocks];

                bool hasFlags = false;
                byte[] checkBlock = br.ReadBytes(8);
                br.BaseStream.Seek(8, SeekOrigin.Begin);
                ulong checkCRC64 = 0;
                bool isHashStrings = false;

                if (BitConverter.ToString(checkBlock) == BitConverter.ToString(BitConverter.GetBytes(CRCs.CRC64(checkCRC64, InEngineWords.ClassStructsNames.languagedatabaseClass.ToLower()))))
                {
                    isHashStrings = true;

                    for (int i = 0; i < countBlocks; i++)
                    {
                        byte[] tmp = br.ReadBytes(8);
                        classes[i] = BitConverter.ToString(tmp);
                        if (classes[i].ToLower() == BitConverter.ToString(BitConverter.GetBytes(CRCs.CRC64(checkCRC64, InEngineWords.ClassStructsNames.flagsClass.ToLower())))) hasFlags = true;
                        tmp = br.ReadBytes(4); //Some values (in oldest games I found some values in *.vers files)
                    }
                }
                else
                {
                    for (int i = 0; i < countBlocks; i++)
                    {
                        int len = br.ReadInt32();
                        byte[] tmp = br.ReadBytes(len);
                        classes[i] = Encoding.ASCII.GetString(tmp);
                        if (classes[i].ToLower() == "class flags") hasFlags = true;
                        tmp = br.ReadBytes(4); //Some values (in oldest games I found some values in *.vers files)
                    }
                }

                LangdbClass langdbs = GetStringsFromLangdb(br, hasFlags);
                br.Close();
                ms.Close();

                if (langdbs == null)
                {
                    return "File " + fi.Name + ": unknown error.";
                }
                if ((langdbs != null) && (langdbs.langdbCount == 0))
                {
                    langdbs = null;
                    GC.Collect();
                    return fi.Name + " is EMPTY.";
                }

                if (extract)
                {
                    ClassesStructs.Text.CommonTextClass txts = new CommonTextClass();

                    txts.txtList = new List<CommonText>();

                    for (int i = 0; i < langdbs.langdbCount; i++)
                    {
                        ClassesStructs.Text.CommonText txt;

                        txt.isBothSpeeches = true;
                        txt.strNumber = MainMenu.settings.exportRealID ? langdbs.langdbs[i].anmID : langdbs.langdbs[i].stringNumber;
                        txt.actorName = langdbs.langdbs[i].actorName;
                        txt.actorSpeechOriginal = langdbs.langdbs[i].actorSpeech;
                        txt.actorSpeechTranslation = langdbs.langdbs[i].actorSpeech;
                        txt.flags = Encoding.ASCII.GetString(langdbs.flags[i].flags);

                        if (((txt.actorSpeechOriginal == "") && !MainMenu.settings.ignoreEmptyStrings)
                              || (txt.actorSpeechOriginal != "")) txts.txtList.Add(txt);
                    }

                    if (MainMenu.settings.sortSameString) txts = Methods.SortString(txts);

                    string outputFile = MainMenu.settings.pathForOutputFolder + "\\" + fi.Name.Remove(fi.Name.Length - 6, 6);
                    outputFile += MainMenu.settings.tsvFormat ? "tsv" : "txt";

                    switch (MainMenu.settings.newTxtFormat)
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
                    ClassesStructs.Text.CommonTextClass txt = new CommonTextClass();
                    txt.txtList = ReadText.GetStrings(txtFile);

                    /*if(txt.txtList.Count < langdbs.langdbCount)
                    {
                        FileInfo txtFI = new FileInfo(txtFile);
                        return "Not enough strings in " + txtFI.Name + " for " + fi.Name + " file.";
                    }*/

                    int type = CheckNumbers(txt.txtList, langdbs);
                    if (type == -1) return "I don't know which type of number strings select for " + fi.Name + " file.";

                    langdbs = ReplaceStrings(langdbs, txt.txtList, type);

                    ms = new MemoryStream(buffer);
                    br = new BinaryReader(ms);

                    string outputFile = MainMenu.settings.pathForOutputFolder + "\\" + fi.Name;

                    int rebuildResult = RebuildLangdb(br, outputFile, langdbs);

                    result = "File " + fi.Name + " successfully imported.";

                    if (rebuildResult == -1)
                    {
                        result = "Unknown error while rebuild file " + fi.Name;
                    }

                    br.Close();
                    ms.Close();

                    langdbs = null;

                    if ((EncKey != null) || MainMenu.settings.encLangdb)
                    {
                        buffer = File.ReadAllBytes(outputFile);

                        if ((EncKey != null) && !MainMenu.settings.encLangdb)
                        {
                            if (Methods.meta_crypt(buffer, EncKey, version, false) != 0)
                            {
                                File.WriteAllBytes(outputFile, buffer);
                                result += " Successfull encrypted back!";
                            }
                        }
                        else if (MainMenu.settings.encLangdb)
                        {
                            byte[] key = new byte[MainMenu.gamelist[MainMenu.settings.encKeyIndex].key.Length];
                            Array.Copy(MainMenu.gamelist[MainMenu.settings.encKeyIndex].key, 0, key, 0, key.Length);

                            if(MainMenu.settings.customKey)
                            {
                                key = Methods.stringToKey(MainMenu.settings.encCustomKey);
                            }

                            version = MainMenu.settings.versionEnc == 0 ? 2 : 7;
                            
                            if (Methods.meta_crypt(buffer, key, version, false) != 0)
                            {
                                File.WriteAllBytes(outputFile, buffer);
                                result += " Successfull encrypted!";
                            }
                        }
                    }
                }

                buffer = null;
            }
            catch
            {
                if(br != null) br.Close();
                if(ms != null) ms.Close();

                result = "Something wrong with langdb file " + fi.Name;
            }

            GC.Collect();
            return result;
        }
    }
}
