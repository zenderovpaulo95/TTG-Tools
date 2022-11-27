using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TTG_Tools.ClassesStructs.Text;
using System.IO;

namespace TTG_Tools.Texts
{
    public class SaveText
    {
        public static void OldMethod(List<CommonText> txt, bool isDoubledFile, bool isUnicode, string outputPath)
        {
            if (File.Exists(outputPath)) File.Delete(outputPath);
            FileStream fs = new FileStream(outputPath, FileMode.CreateNew);
            StreamWriter tw = new StreamWriter(fs, Encoding.UTF8);

            try
            {
                string tmpString = "";
                byte[] tmpVal = null;

                for (int i = 0; i < txt.Count; i++)
                {
                    tmpString = MainMenu.settings.tsvFormat ? txt[i].strNumber + "\t" + txt[i].actorName + "\t" : txt[i].strNumber + ") " + txt[i].actorName + "\r\n";
                    tw.Write(tmpString);
                    tmpString = txt[i].actorSpeechOriginal;

                    if ((tmpString.Contains("\r") || tmpString.Contains("\n")) && !tmpString.Contains("\r\n"))
                    {
                        if (tmpString.Contains("\r")) tmpString = MainMenu.settings.tsvFormat ? tmpString.Replace("\r", "\\r") : tmpString.Replace("\r", "\r\n");
                        else tmpString = MainMenu.settings.tsvFormat ? tmpString.Replace("\n", "\\n") : tmpString.Replace("\n", "\r\n");
                    }
                    else if (tmpString.Contains("\r\n") && MainMenu.settings.tsvFormat)
                    {
                        tmpString = tmpString.Replace("\r\n", "\\r\\n");
                    }
                    else if (txt[i].actorSpeechOriginal.Contains("\t") && MainMenu.settings.tsvFormat)
                    {
                        tmpString = tmpString.Replace("\t", "\\t");
                    }

                    tmpString = isUnicode && MainMenu.settings.unicodeSettings == 1 ? Methods.ConvertString(tmpString, true) : tmpString;

                    tw.Write(tmpString);

                    if (isDoubledFile)
                    {
                        tmpString += MainMenu.settings.tsvFormat ? "\t" : "\r\n";
                        tw.Write(tmpString);

                        if(!MainMenu.settings.tsvFormat)
                        {
                            tmpString = txt[i].strNumber + ") " + txt[i].actorName + "\r\n";
                            tw.Write(tmpString);
                        }

                        tmpString = txt[i].actorSpeechTranslation;

                        if ((tmpString.Contains("\r") || tmpString.Contains("\n")) && !tmpString.Contains("\r\n"))
                        {
                            if (tmpString.Contains("\r")) tmpString = MainMenu.settings.tsvFormat ? tmpString.Replace("\r", "\\r") : tmpString.Replace("\r", "\r\n");
                            else tmpString = MainMenu.settings.tsvFormat ? tmpString.Replace("\n", "\\n") : tmpString.Replace("\n", "\r\n");
                        }
                        else if (tmpString.Contains("\r\n") && MainMenu.settings.tsvFormat)
                        {
                            tmpString = tmpString.Replace("\r\n", "\\r\\n");
                        }
                        else if (tmpString.Contains("\t") && MainMenu.settings.tsvFormat)
                        {
                            tmpString = tmpString.Replace("\t", "\\t");
                        }

                        tmpString = isUnicode && MainMenu.settings.unicodeSettings == 1 ? Methods.ConvertString(tmpString, true) : tmpString;

                        tw.Write(tmpString);
                    }

                    tmpString = "\r\n";
                    tw.Write(tmpString);
                }

                tw.Close();
                fs.Close();
            }
            catch
            {
                if (fs != null) fs.Close();
                if (tw != null) tw.Close();
            }
        }

        public static void NewMethod(List<CommonText> txt, bool isUnicode, string outputPath)
        {
            if(File.Exists(outputPath)) File.Delete(outputPath);

            FileStream fs = new FileStream(outputPath, FileMode.CreateNew);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);

            try
            {
                string tmpString = "";

                for(int i = 0; i < txt.Count; i++)
                {
                    tmpString = "langid=" + txt[i].strNumber + "\r\n";
                    sw.Write(tmpString);
                    
                    tmpString = "actor=" + txt[i].actorName + "\r\n";
                    sw.Write(tmpString);
                    
                    tmpString = "speechOriginal=" + txt[i].actorSpeechOriginal;
                    if (tmpString.Contains("\r")) tmpString = tmpString.Replace("\r", "\\r");
                    if (tmpString.Contains("\n")) tmpString = tmpString.Replace("\n", "\\n");
                    if (tmpString.Contains("\t")) tmpString = tmpString.Replace("\t", "\\t");
                    tmpString += "\r\n";

                    tmpString = isUnicode && MainMenu.settings.unicodeSettings == 1 ? Methods.ConvertString(tmpString, true) : tmpString;
                    sw.Write(tmpString);

                    tmpString = "speechTranslation=" + txt[i].actorSpeechTranslation;
                    if (tmpString.Contains("\r")) tmpString = tmpString.Replace("\r", "\\r");
                    if (tmpString.Contains("\n")) tmpString = tmpString.Replace("\n", "\\n");
                    if (tmpString.Contains("\t")) tmpString = tmpString.Replace("\t", "\\t");
                    tmpString += "\r\n";

                    tmpString = isUnicode && MainMenu.settings.unicodeSettings == 1 ? Methods.ConvertString(tmpString, true) : tmpString;
                    sw.Write(tmpString);

                    tmpString = "flags=" + txt[i].flags + "\r\n\r\n";
                    sw.Write(tmpString);
                }

                sw.Close();
                fs.Close();
            }
            catch
            {
                if (sw != null) sw.Close();
                if (fs != null) fs.Close();
            }
        }
    }
}
