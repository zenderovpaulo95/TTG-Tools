using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTG_Tools.ClassesStructs
{
    public class Ttarch2Class
    {
        public struct Ttarch2files
        {
            public ulong fileOffset;
            public int fileSize;
            public ulong fileNameCRC64;
            public string fileName;
        }

        public bool isCompressed;
        public bool isEncrypted;
        public Ttarch2files[] files;

        public Ttarch2Class()
        {
            isCompressed = false;
            isEncrypted = false;
        }
    }
}
