using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTG_Tools.ClassesStructs.Text
{
    public struct langdb
    {
        public uint anmID; //Actor's animation
        public uint voxID; //Actor's speech
        public string actorName;
        public string actorSpeech;
        public string anmFile;
        public string voxFile;
    }
    public class LangdbClass
    {
        public bool isBlockLength;
        public int blockLength;
        public int langdbCount;

        langdb[] langdbs;
        FlagsClass.LangdbFlagClass[] flags;
    }
}
