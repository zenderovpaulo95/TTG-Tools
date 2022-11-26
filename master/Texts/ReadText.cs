using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TTG_Tools.ClassesStructs.Text;

namespace TTG_Tools.Texts
{
    public class ReadText
    {
        private static int GetType(string FilePath)
        {
            FileInfo fi = new FileInfo(FilePath);

            if (fi.Extension.ToLower() == ".tsv") return 1;

            string[] strs = File.ReadAllLines(fi.FullName, Encoding.UTF8);
            if(strs.Length >= 5)
            {
                if ((strs[0].IndexOf("langid=") == 0) && (strs[0].IndexOf("actor=") == 0) && (strs[0].IndexOf("speechOriginal=") == 0) && (strs[0].IndexOf("speechTranslation=") == 0) && (strs[0].IndexOf("flags=") == 0))
                {
                    return 2;
                }
            }
            
            return 0;
        }

        private static List<CommonText> OldTextMode(string FilePath)
        {
            List<CommonText> txts = new List<CommonText>();
            FileStream fs = new FileStream(FilePath, FileMode.Open);
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);

            try
            {
                CommonText tmpTxt;
                bool hasNumber = false;
                int prevNum = -1;
                int curNum = -1;
                string tmpString = "";
                int lastListIndex = -1;

                while (!sr.EndOfStream)
                {
                    tmpString = sr.ReadLine();

                    if (tmpString.IndexOf(")") > -1 && !hasNumber)
                    {
                        if(Methods.IsNumeric(tmpString.Substring(0, tmpString.IndexOf(")"))))
                        {
                            curNum = Convert.ToInt32(tmpString.Substring(0, tmpString.IndexOf(")")));
                            tmpTxt.isBothSpeeches = false;

                            if(curNum == prevNum)
                            {
                                lastListIndex = txts.Count - 1;

                                tmpTxt.strNumber = txts[lastListIndex].strNumber;
                                tmpTxt.isBothSpeeches = true;
                                tmpTxt.actorName = txts[lastListIndex].actorName;
                                tmpTxt.actorSpeechOriginal = txts[lastListIndex].actorSpeechOriginal;
                                tmpTxt.actorSpeechTranslation = txts[lastListIndex].actorSpeechTranslation;
                                tmpTxt.flags = txts[lastListIndex].flags;
                                txts[lastListIndex] = tmpTxt;
                            }

                            try
                            {
                                tmpTxt.strNumber = (uint)curNum;
                                int numPos = tmpString.IndexOf(")") + 2;
                                tmpTxt.actorName = tmpString.Substring(numPos, tmpString.Length - numPos);
                                tmpTxt.actorSpeechOriginal = "";
                                tmpTxt.actorSpeechTranslation = "";
                                tmpTxt.flags = "000";

                                txts.Add(tmpTxt);

                                hasNumber = true;
                                prevNum = curNum;
                                lastListIndex = txts.Count - 1;
                            }
                            catch
                            {
                                System.Windows.Forms.MessageBox.Show("Error in string " + tmpString, "Error");
                                if (sr != null) sr.Close();
                                if (fs != null) fs.Close();

                                return null;
                            }
                        }
                    }
                    else
                    {

                    }
                }

                sr.Close();
                fs.Close();

                return txts;
            }
            catch
            {
                if(sr != null) sr.Close();
                if(fs != null) fs.Close();

                return null;
            }
        }

        private static List<CommonText> TsvTextMode(string FilePath)
        {
            List<CommonText> txts = new List<CommonText>();
            FileStream fs = new FileStream(FilePath, FileMode.Open);
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);

            try
            {

                sr.Close();
                fs.Close();

                return txts;
            }
            catch
            {
                if (sr != null) sr.Close();
                if (fs != null) fs.Close();

                return null;
            }
        }

        private static List<CommonText> NewTextMode(string FilePath)
        {
            List<CommonText> txts = new List<CommonText>();
            FileStream fs = new FileStream(FilePath, FileMode.Open);
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);

            try
            {

                sr.Close();
                fs.Close();

                return txts;
            }
            catch
            {
                if (sr != null) sr.Close();
                if (fs != null) fs.Close();

                return null;
            }
        }

        public static List<CommonText> GetStrings(string FilePath)
        {
            List<CommonText> strings = new List<CommonText>();
            int type = GetType(FilePath);

            switch(type)
            {
                case 0:
                    strings = OldTextMode(FilePath);
                    break;

                case 1:
                    strings = TsvTextMode(FilePath);
                    break;

                case 2:
                    strings = NewTextMode(FilePath);
                    break;
            }

            return strings;
        }
    }
}
