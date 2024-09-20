using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTG_Tools.ClassesStructs
{
    public class TtarchClass
    {
        public struct ttarchFiles
        {
            public string fileName;
            public uint fileOffset;
            public int fileSize;
        }

        public bool isEncrypted;
        public bool isCompressed;
        public bool isXmode;
        public int[] compressedBlocks;
        public int chunkSize;
        public int version;
        public List<string> fileFormats;
        public byte[] key; //key of encrypted archive
        public ttarchFiles[] files;

        public TtarchClass()
        {
            isCompressed = false;
            isEncrypted = false;
            isXmode = false;
            key = null;
            version = 0;
            chunkSize = 0;
        }
    }
}
