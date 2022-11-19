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

                    if ((txt[i].actorSpeechOriginal.Contains("\r") || txt[i].actorSpeechOriginal.Contains("\n")) && !txt[i].actorSpeechOriginal.Contains("\r\n"))
                    {
                        if (txt[i].actorSpeechOriginal.Contains("\r")) tmpString = MainMenu.settings.tsvFormat ? tmpString.Replace("\r", "\\r") : tmpString.Replace("\r", "\r\n");
                        else tmpString = MainMenu.settings.tsvFormat ? tmpString.Replace("\n", "\\n") : tmpString.Replace("\n", "\r\n");
                    }
                    else if (txt[i].actorSpeechOriginal.Contains("\r\n") && MainMenu.settings.tsvFormat)
                    {
                        tmpString = tmpString.Replace("\r\n", "\\r\\n");
                    }
                    else if (txt[i].actorSpeechOriginal.Contains("\t") && MainMenu.settings.tsvFormat)
                    {
                        tmpString = tmpString.Replace("\t", "\\t");
                    }

                    if(isUnicode && (MainMenu.settings.unicodeSettings == 1))
                    {
                        tmpVal = Encoding.UTF8.GetBytes(tmpString);
                        tmpVal = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(1252), tmpVal);
                        tmpVal = Encoding.Convert(Encoding.GetEncoding(1252), Encoding.GetEncoding(MainMenu.settings.ASCII_N), tmpVal);
                        tmpVal = Encoding.Convert(Encoding.GetEncoding(MainMenu.settings.ASCII_N), Encoding.UTF8, tmpVal);
                        tmpString = Encoding.UTF8.GetString(tmpVal);
                    }

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

                        if ((txt[i].actorSpeechTranslation.Contains("\r") || txt[i].actorSpeechTranslation.Contains("\n")) && !txt[i].actorSpeechTranslation.Contains("\r\n"))
                        {
                            if (tmpString.Contains("\r")) tmpString = MainMenu.settings.tsvFormat ? tmpString.Replace("\r", "\\r") : tmpString.Replace("\r", "\r\n");
                            else tmpString = MainMenu.settings.tsvFormat ? tmpString.Replace("\n", "\\n") : tmpString.Replace("\n", "\r\n");
                        }
                        else if (txt[i].actorSpeechTranslation.Contains("\r\n") && MainMenu.settings.tsvFormat)
                        {
                            tmpString = tmpString.Replace("\r\n", "\\r\\n");
                        }
                        else if (txt[i].actorSpeechTranslation.Contains("\t") && MainMenu.settings.tsvFormat)
                        {
                            tmpString = tmpString.Replace("\t", "\\t");
                        }

                        if (isUnicode && (MainMenu.settings.unicodeSettings == 1))
                        {
                            tmpVal = Encoding.UTF8.GetBytes(tmpString);
                            tmpVal = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(1252), tmpVal);
                            tmpVal = Encoding.Convert(Encoding.GetEncoding(1252), Encoding.GetEncoding(MainMenu.settings.ASCII_N), tmpVal);
                            tmpVal = Encoding.Convert(Encoding.GetEncoding(MainMenu.settings.ASCII_N), Encoding.UTF8, tmpVal);
                            tmpString = Encoding.UTF8.GetString(tmpVal);
                        }

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
    }
}
