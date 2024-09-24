using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Security.Cryptography;

namespace TTG_Tools
{
    public partial class ArchiveUnpacker : Form
    {
        public ArchiveUnpacker()
        {
            InitializeComponent();
        }

        public static ClassesStructs.TtarchClass ttarch;
        public static ClassesStructs.Ttarch2Class ttarch2;

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private static byte[] decompressBlock(byte[] bytes, int algorithmCompress)
        {
            try
            {
                byte[] retBuf = null;
                switch (algorithmCompress)
                {
                    case 0:
                        retBuf = ZLibDecompressor(bytes);
                        break;

                    case 1:
                        retBuf = DeflateDecompressor(bytes);
                        break;

                    case 2:
                        retBuf = OodleDecompressor(bytes);
                        break;
                        
                }

                return retBuf;
            }
            catch
            {
                return null;
            }
        }

        private static byte[] decompressBlock(byte[] bytes)
        {
            try
            {
                byte[] buf = ZLibDecompressor(bytes);
                if (ttarch != null) ttarch.compressAlgorithm = 0;
                else ttarch2.compressAlgorithm = 0;
                return buf;
            }
            catch
            {
                try
                {
                    //Try deflate decompress
                    byte[] buf = DeflateDecompressor(bytes);
                    if (ttarch != null) ttarch.compressAlgorithm = 1;
                    else ttarch2.compressAlgorithm = 1;
                    return buf;
                }
                catch
                {
                    try
                    {
                        byte[] buf = OodleDecompressor(bytes);
                        if (ttarch != null) ttarch.compressAlgorithm = -1;
                        else ttarch2.compressAlgorithm = 2;
                        return buf;
                    }
                    catch
                    {
                        //Else return empty bytes
                        if (ttarch != null) ttarch.compressAlgorithm = -1; //Unknown algorithm
                        else ttarch2.compressAlgorithm = -1;
                        return null;
                    }
                }
            }
        }

        private static byte[] DeflateDecompressor(byte[] bytes) //Для старых (версии 8 и 9) и новых архивов
        {
            byte[] retVal;
            using (MemoryStream decompressedMemoryStream = new MemoryStream(bytes))
            {
                using (System.IO.Compression.DeflateStream decompressStream = new System.IO.Compression.DeflateStream(decompressedMemoryStream, System.IO.Compression.CompressionMode.Decompress))
                {
                    using (MemoryStream memOutStream = new MemoryStream())
                    {
                        decompressStream.CopyTo(memOutStream);
                        retVal = memOutStream.ToArray();
                    }
                }
            }
            return retVal;
        }

        private static byte[] ZLibDecompressor(byte[] bytes)
        {
            byte[] retBytes = new byte[bytes.Length];

            using (Stream inMemoryStream = new MemoryStream(bytes))
            {
                using (Joveler.ZLibWrapper.ZLibStream inZStream = new Joveler.ZLibWrapper.ZLibStream(inMemoryStream, Joveler.ZLibWrapper.ZLibMode.Decompress))
                {
                    using (MemoryStream outMemoryStream = new MemoryStream())
                    {

                        Methods.CopyStream(inZStream, outMemoryStream);
                        inZStream.Flush();
                        retBytes = outMemoryStream.ToArray();
                    }
                }
            }

            return retBytes;
        }

        private static byte[] OodleDecompressor(byte[] bytes)
        {
            byte[] retBytes = new byte[bytes.Length];

            long bufSize = bytes.Length;
            long decBufSize = bytes.Length;
            if (ttarch2 != null)
            {
                decBufSize = ttarch2.chunkSize;
                retBytes = new byte[decBufSize];
            }

            int size = OodleTools.Imports.OodleLZ_Decompress(bytes, bufSize, retBytes, decBufSize, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);

            byte[] tmp = new byte[size];
            Array.Copy(retBytes, 0, tmp, 0, tmp.Length);

            return tmp;
        }

        private static void ReadHeaderTtarch(string path, byte[] key)
        {
            try
            {
                ttarch.filePath = path;
                ttarch.fileFormats = new List<string>();
                FileStream fs = new FileStream(path, FileMode.Open);
                BinaryReader br = new BinaryReader(fs);
                ttarch.version = br.ReadInt32();
                int encryption = br.ReadInt32();
                int two = br.ReadInt32();

                int countCompressedBlocks;
                int val = 0;

                if (ttarch.version > 2)
                {
                    val = br.ReadInt32();
                    countCompressedBlocks = br.ReadInt32();

                    if(val == 2)
                    {
                        ttarch.compressedBlocks = new int[countCompressedBlocks];
                        ttarch.isCompressed = true;

                        for(int k = 0; k < countCompressedBlocks; k++)
                        {
                            ttarch.compressedBlocks[k] = br.ReadInt32();
                        }
                    }

                    uint arcSize = br.ReadUInt32(); //Size of block with files

                    if (ttarch.version >= 4)
                    {
                        int priority = br.ReadInt32();
                        int priority2 = br.ReadInt32();

                        if(ttarch.version >= 7)
                        {
                            int someVal = br.ReadInt32();
                            int someVal2 = br.ReadInt32();

                            ttarch.isXmode = someVal == 1 || someVal2 == 1;
                            ttarch.chunkSize = br.ReadInt32();

                            if(ttarch.version > 7)
                            {
                                byte b = br.ReadByte();

                                if(ttarch.version == 9)
                                {
                                    uint crc32 = br.ReadUInt32();
                                }
                            }
                        }
                    }
                }

                int headerSize = br.ReadInt32();
                int cHeaderSize = -1;

                if (ttarch.version >= 7 && val == 2)
                {
                    cHeaderSize = br.ReadInt32();
                }

                byte[] header = ttarch.version >= 7 && val == 2 ? br.ReadBytes(cHeaderSize) : br.ReadBytes(headerSize);

                ttarch.filesOffset = (uint)br.BaseStream.Position;

                if(ttarch.version >= 7 && val == 2)
                {
                    header = decompressBlock(header);
                }

                if(encryption == 1)
                {
                    ttarch.isEncrypted = true;
                    BlowFishCS.BlowFish dec = new BlowFishCS.BlowFish(key, ttarch.version);
                    header = dec.Crypt_ECB(header, ttarch.version, true);
                }

                using (MemoryStream ms = new MemoryStream(header))
                {
                    using (BinaryReader mbr = new BinaryReader(ms))
                    {
                        int dirsCount = mbr.ReadInt32();
                        
                        for(int d = 0; d < dirsCount; d++)
                        {
                            int nameLen = mbr.ReadInt32();
                            byte[] tmpName = mbr.ReadBytes(nameLen);
                            string name = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(tmpName);
                        }

                        int filesCount = mbr.ReadInt32();

                        ttarch.files = new ClassesStructs.TtarchClass.ttarchFiles[filesCount];

                        for(int f = 0; f < filesCount; f++)
                        {
                            int nameLen = mbr.ReadInt32();
                            byte[] tmpName = mbr.ReadBytes(nameLen);
                            ttarch.files[f].fileName = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(tmpName);
                            int zeroVal = mbr.ReadInt32(); //always shows 0 value
                            ttarch.files[f].fileOffset = mbr.ReadUInt32();
                            ttarch.files[f].fileSize = mbr.ReadInt32();

                            string ext = Methods.GetExtension(ttarch.files[f].fileName);

                            if ((ext != "") && !ttarch.fileFormats.Contains(ext))
                            {
                                ttarch.fileFormats.Add(ext);
                            }
                        }
                    }
                }

                //Check oldest compressed archives. Need to find out compression algorithm (default it must be zlib)
                if(ttarch.version < 7 && ttarch.isCompressed)
                {
                    byte[] tmp = br.ReadBytes(ttarch.compressedBlocks[0]);

                    if(ttarch.isEncrypted)
                    {
                        BlowFishCS.BlowFish dec = new BlowFishCS.BlowFish(key, ttarch.version);
                        tmp = dec.Crypt_ECB(tmp, ttarch.version, true);
                    }

                    tmp = decompressBlock(tmp);
                    tmp = null;
                }

                br.Close();
                fs.Close();
            }
            catch
            {
                MessageBox.Show("Something goes wrong", "Unknown error. Please try another archive.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ttarch = null;
            }
        }

        private static void ReadHeaderTtarch2(string path, byte[] key)
        {
            try
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                BinaryReader br = new BinaryReader(fs);

                ulong foffset = 0;
                ttarch2.fileFormats = new List<string>();
                ttarch2.fileName = path;

                byte[] header = br.ReadBytes(4);
                foffset += 4;
                ttarch2.compressAlgorithm = -1;
                ttarch2.isCompressed = Encoding.ASCII.GetString(header) != "NCTT"; //If it's a NCTT header then archive is not compressed
                ttarch2.isEncrypted = Encoding.ASCII.GetString(header) == "ECTT" || Encoding.ASCII.GetString(header) == "eCTT";

                if (ttarch2.isCompressed)
                {
                    ttarch2.compressAlgorithm = Encoding.ASCII.GetString(header) == "eCTT" || Encoding.ASCII.GetString(header) == "zCTT" ? 2 : 1;
                    if (Encoding.ASCII.GetString(header) == "eCTT" || Encoding.ASCII.GetString(header) == "zCTT")
                    {
                        int one = br.ReadInt32();
                        foffset += 4;
                    }
                    ttarch2.chunkSize = br.ReadUInt32();
                    int blocksCount = br.ReadInt32();
                    foffset += 4 + 4;
                    ttarch2.compressedBlocks = new ulong[blocksCount + 1];

                    for (int i = 0; i < ttarch2.compressedBlocks.Length; i++)
                    {
                        ttarch2.compressedBlocks[i] = br.ReadUInt64();
                        foffset += 8;
                    }

                    long pos = br.BaseStream.Position;
                    ttarch2.cFilesOffset = (ulong)br.BaseStream.Position;

                    byte[] tmp = br.ReadBytes((int)ttarch2.compressedBlocks[1] - (int)ttarch2.compressedBlocks[0]);

                    if(ttarch2.isEncrypted)
                    {
                        BlowFishCS.BlowFish dec = new BlowFishCS.BlowFish(key, 7);
                        tmp = dec.Crypt_ECB(tmp, 7, true);
                    }

                    tmp = decompressBlock(tmp, ttarch2.compressAlgorithm);

                    int suboff = 0;
                    uint filesCount = 0;

                    using (MemoryStream ms = new MemoryStream(tmp))
                    {
                        using (BinaryReader mbr = new BinaryReader(ms))
                        {
                            byte[] subHeader = mbr.ReadBytes(4);
                            suboff += 4;
                            //foffset += 4;
                            if (Encoding.ASCII.GetString(subHeader) == "3ATT")
                            {
                                int two = mbr.ReadInt32();
                                suboff += 4;
                                //foffset += 4;
                            }

                            ttarch2.version = Encoding.ASCII.GetString(subHeader) == "3ATT" ? 1 : 2;

                            uint nameSize = mbr.ReadUInt32();
                            suboff += 4;
                            //foffset += 4;
                            filesCount = mbr.ReadUInt32();
                            suboff += 4;

                            ttarch2.filesOffset = (ulong)suboff + (28 * filesCount) + nameSize;
                            ttarch2.files = new ClassesStructs.Ttarch2Class.Ttarch2files[filesCount];
                        }
                    }

                    if (ttarch2.filesOffset > (ulong)tmp.Length)
                    {
                        br.BaseStream.Seek(pos, SeekOrigin.Begin);
                        int index = (int)(ttarch2.filesOffset / ttarch2.chunkSize) + 1;

                        using (MemoryStream ms = new MemoryStream())
                        {
                            for (int i = 0; i < index; i++)
                            {
                                tmp = br.ReadBytes((int)(ttarch2.compressedBlocks[i + 1] - ttarch2.compressedBlocks[i]));

                                if(ttarch2.isEncrypted)
                                {
                                    BlowFishCS.BlowFish dec = new BlowFishCS.BlowFish(key, 7);
                                    tmp = dec.Crypt_ECB(tmp, 7, true);
                                }

                                tmp = decompressBlock(tmp, ttarch2.compressAlgorithm);

                                ms.Write(tmp, 0, tmp.Length);
                            }

                            tmp = ms.ToArray();
                        }
                    }

                    using (MemoryStream ms = new MemoryStream(tmp))
                    {
                        using (BinaryReader mbr = new BinaryReader(ms))
                        {
                            mbr.BaseStream.Seek(suboff, SeekOrigin.Begin);

                            for(int i = 0; i < (int)filesCount; i++)
                            {
                                ttarch2.files[i].fileNameCRC64 = mbr.ReadUInt64();
                                ttarch2.files[i].fileOffset = mbr.ReadUInt64();
                                ttarch2.files[i].fileSize = mbr.ReadInt32();
                                int unknown = mbr.ReadInt32();
                                ushort nameBlock = mbr.ReadUInt16();
                                ushort nameOff = mbr.ReadUInt16();
                                pos = mbr.BaseStream.Position;
                                ulong nameOffset = (ulong)suboff + (28 * (ulong)filesCount) + (ulong)nameOff + ((ulong)nameBlock * 0x10000);
                                mbr.BaseStream.Seek((long)nameOffset, SeekOrigin.Begin);

                                using (MemoryStream mms = new MemoryStream())
                                {
                                    byte[] bytes = null;

                                    while (true)
                                    {
                                        bytes = mbr.ReadBytes(1);
                                        if (bytes[0] == 0) break;
                                        mms.Write(bytes, 0, bytes.Length);
                                    }

                                    bytes = mms.ToArray();
                                    ttarch2.files[i].fileName = Encoding.ASCII.GetString(bytes);

                                    string ext = Methods.GetExtension(ttarch2.files[i].fileName);

                                    if ((ext != "") && !ttarch2.fileFormats.Contains(ext))
                                    {
                                        ttarch2.fileFormats.Add(ext);
                                    }
                                }

                                mbr.BaseStream.Seek(pos, SeekOrigin.Begin);
                            }
                        }
                    }
                }
                else
                {
                    ulong archSize = br.ReadUInt64();
                    foffset += 8;
                    byte[] subHeader = br.ReadBytes(4);
                    foffset += 4;
                    if (Encoding.ASCII.GetString(subHeader) == "3ATT")
                    {
                        int two = br.ReadInt32();
                        foffset += 4;
                    }

                    ttarch2.version = Encoding.ASCII.GetString(subHeader) == "3ATT" ? 1 : 2;

                    uint nameSize = br.ReadUInt32();
                    foffset += 4;
                    uint filesCount = br.ReadUInt32();
                    foffset += 4;
                    ttarch2.files = new ClassesStructs.Ttarch2Class.Ttarch2files[filesCount];
                    ttarch2.filesOffset = foffset + (28 * (ulong)filesCount) + (ulong)nameSize;

                    for (int i = 0; i < filesCount; i++)
                    {
                        ttarch2.files[i].fileNameCRC64 = br.ReadUInt64();
                        ttarch2.files[i].fileOffset = br.ReadUInt64();
                        ttarch2.files[i].fileSize = br.ReadInt32();
                        int unknown = br.ReadInt32();
                        ushort nameBlock = br.ReadUInt16();
                        ushort nameOff = br.ReadUInt16();
                        long pos = br.BaseStream.Position;
                        ulong nameOffset = foffset + (28 * (ulong)filesCount) + (ulong)nameOff + ((ulong)nameBlock * 0x10000);
                        br.BaseStream.Seek((long)nameOffset, SeekOrigin.Begin);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            byte[] bytes = null;

                            while (true)
                            {
                                bytes = br.ReadBytes(1);
                                if (bytes[0] == 0) break;
                                ms.Write(bytes, 0, bytes.Length);
                            }

                            bytes = ms.ToArray();
                            ttarch2.files[i].fileName = Encoding.ASCII.GetString(bytes);

                            string ext = Methods.GetExtension(ttarch2.files[i].fileName);

                            if ((ext != "") && !ttarch2.fileFormats.Contains(ext))
                            {
                                ttarch2.fileFormats.Add(ext);
                            }
                        }

                        br.BaseStream.Seek(pos, SeekOrigin.Begin);
                    }
                }

                br.Close();
                fs.Close();
            }
            catch
            {
                MessageBox.Show("Something goes wrong", "Unknown error. Please try another archive.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ttarch2 = null;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "All supported files (*.ttarch, *.ttarch2) | *.ttarch;*.ttarch2| TTARCH archives (*.ttarch) | *.ttarch| TTARCH2 archives (*.ttarch2) | *.ttarch2";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                FileInfo fi = new FileInfo(ofd.FileName);

                ttarch = null;
                ttarch2 = null;

                switch (fi.Extension.ToLower())
                {
                    case ".ttarch":
                        ttarch = new ClassesStructs.TtarchClass();
                        ReadHeaderTtarch(fi.FullName, MainMenu.gamelist[gameListCB.SelectedIndex].key);

                        if (ttarch != null)
                        {
                            filesDataGridView.ColumnCount = 4;
                            filesDataGridView.RowCount = ttarch.files.Length;
                            fileFormatsCB.Items.Clear();

                            string compressedStr = "Compressed: ";
                            compressedStr += ttarch.isCompressed ? "Yes" : "No";
                            if (ttarch.isCompressed)
                            {
                                compressedStr += " (";
                                compressedStr += ttarch.compressAlgorithm == 0 ? "zlib)" : "deflate)";
                            }
                            string encryptedStr = "Encrypted: ";
                            encryptedStr += ttarch.isEncrypted ? "Yes" : "No";
                            string xmodeStr = "Has X mode: ";
                            xmodeStr += ttarch.isXmode ? "Yes" : "No";
                            string chunkSzStr = "Chunk size: " + Convert.ToString(ttarch.chunkSize) + "KB";

                            compressionLabel.Text = compressedStr;
                            encryptionLabel.Text = encryptedStr;
                            xmodeLabel.Text = xmodeStr;
                            chunkSizeLabel.Text = chunkSzStr;
                            versionLabel.Text = "Version: " + Convert.ToString(ttarch.version);

                            if(ttarch.fileFormats.Count > 0)
                            {
                                if(ttarch.fileFormats.Count > 1) fileFormatsCB.Items.Add("All files");

                                for(int f = 0; f < ttarch.fileFormats.Count; f++)
                                {
                                    fileFormatsCB.Items.Add(ttarch.fileFormats[f]);
                                }

                                fileFormatsCB.SelectedIndex = 0;
                            }

                            filesDataGridView.Columns[0].HeaderText = "No.";
                            filesDataGridView.Columns[1].HeaderText = "File name";
                            filesDataGridView.Columns[2].HeaderText = "File offset";
                            filesDataGridView.Columns[3].HeaderText = "File size";

                            for (int i = 0; i < ttarch.files.Length; i++)
                            {
                                filesDataGridView[0, i].Value = Convert.ToString(i + 1);
                                filesDataGridView[1, i].Value = ttarch.files[i].fileName;
                                filesDataGridView[2, i].Value = Convert.ToString(ttarch.files[i].fileOffset);
                                filesDataGridView[3, i].Value = Convert.ToString(ttarch.files[i].fileSize);
                            }
                        }
                        break;

                    case ".ttarch2":
                        ttarch2 = new ClassesStructs.Ttarch2Class();
                        ReadHeaderTtarch2(fi.FullName, MainMenu.gamelist[gameListCB.SelectedIndex].key);

                        if(ttarch2 != null)
                        {
                            filesDataGridView.ColumnCount = 4;
                            filesDataGridView.RowCount = ttarch2.files.Length;
                            fileFormatsCB.Items.Clear();

                            string compressedStr = "Compressed: ";
                            compressedStr += ttarch2.isCompressed ? "Yes" : "No";
                            if (ttarch2.isCompressed)
                            {
                                compressedStr += " (";
                                compressedStr += ttarch2.compressAlgorithm == 1 ? "deflate)" : "oodle LZ)";
                            }
                            string encryptedStr = "Encrypted: ";
                            encryptedStr += ttarch2.isEncrypted ? "Yes" : "No";
                            string xmodeStr = "Has X mode: No";
                            string chunkSzStr = "Chunk size: " + Convert.ToString(ttarch2.chunkSize / 1024) + "KB";

                            compressionLabel.Text = compressedStr;
                            encryptionLabel.Text = encryptedStr;
                            xmodeLabel.Text = xmodeStr;
                            chunkSizeLabel.Text = chunkSzStr;
                            versionLabel.Text = "Version: " + Convert.ToString(ttarch2.version);

                            if (ttarch2.fileFormats.Count > 0)
                            {
                                if (ttarch2.fileFormats.Count > 1) fileFormatsCB.Items.Add("All files");

                                for (int f = 0; f < ttarch2.fileFormats.Count; f++)
                                {
                                    fileFormatsCB.Items.Add(ttarch2.fileFormats[f]);
                                }

                                fileFormatsCB.SelectedIndex = 0;
                            }

                            filesDataGridView.Columns[0].HeaderText = "No.";
                            filesDataGridView.Columns[1].HeaderText = "File name";
                            filesDataGridView.Columns[2].HeaderText = "File offset";
                            filesDataGridView.Columns[3].HeaderText = "File size";

                            for (int i = 0; i < ttarch2.files.Length; i++)
                            {
                                filesDataGridView[0, i].Value = Convert.ToString(i + 1);
                                filesDataGridView[1, i].Value = ttarch2.files[i].fileName;
                                filesDataGridView[2, i].Value = Convert.ToString(ttarch2.files[i].fileOffset);
                                filesDataGridView[3, i].Value = Convert.ToString(ttarch2.files[i].fileSize);
                            }
                        }

                        break;
                }

                Form.ActiveForm.Text = "Archive unpacker. Opened file: " + fi.Name;
            }
        }

        private void ArchiveUnpacker_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < MainMenu.gamelist.Count; i++)
            {
                gameListCB.Items.Add(i + ". " + MainMenu.gamelist[i].gamename);
            }

            gameListCB.SelectedIndex = MainMenu.settings.encKeyIndex;
        }

        private void unpackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(ttarch != null)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    FileStream fs = new FileStream(ttarch.filePath, FileMode.Open);
                    BinaryReader br = new BinaryReader(fs);

                    bool decrypt = decryptLuaCB.Checked;
                    byte[] key = MainMenu.gamelist[gameListCB.SelectedIndex].key;

                    progressBar1.Minimum = 0;
                    progressBar1.Maximum = ttarch.files.Length > 1 ? ttarch.files.Length - 1 : 1;

                    int chunkSz = ttarch.chunkSize * 1024;
                    
                    for(int i = 0; i < ttarch.files.Length; i++)
                    {
                        if(!ttarch.isCompressed)
                        {
                            br.BaseStream.Seek(ttarch.files[i].fileOffset + ttarch.filesOffset, SeekOrigin.Begin);
                            byte[] tmp = br.ReadBytes(ttarch.files[i].fileSize);
                            Methods.meta_crypt(tmp, key, ttarch.version, true);
                            string fileName = ttarch.files[i].fileName;
                            if ((fileName.Substring(fileName.Length - 5, 5) == ".lenc") && decrypt)
                            {
                                fileName = fileName.Remove(fileName.Length - 4, 4) + "lua";
                                tmp = Methods.decryptLua(tmp, key, ttarch.version);
                            }

                            File.WriteAllBytes(fbd.SelectedPath + Path.DirectorySeparatorChar + fileName, tmp);
                        }
                        else
                        {
                            int index = (int)ttarch.files[i].fileOffset / chunkSz;
                            int index2 = (int)(ttarch.files[i].fileOffset + ttarch.files[i].fileSize) / chunkSz;
                            uint off = 0;

                            if(index > ttarch.compressedBlocks.Length)
                            {
                                MessageBox.Show("Something wrong with offset in compressed archive", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            if (index2 > ttarch.compressedBlocks.Length)
                            {
                                MessageBox.Show("Something wrong with offset in compressed archive", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            for(int c = 0; c < index; c++)
                            {
                                off += (uint)ttarch.compressedBlocks[c];
                            }

                            br.BaseStream.Seek(ttarch.filesOffset + off, SeekOrigin.Begin);

                            uint c_off = (uint)(ttarch.files[i].fileOffset - (chunkSz * index));

                            using (MemoryStream ms = new MemoryStream())
                            {
                                using (BinaryWriter mbw = new BinaryWriter(ms))
                                {
                                    for (int c = index; c <= index2; c++)
                                    {
                                        byte[] tmp = br.ReadBytes(ttarch.compressedBlocks[c]);
                                        
                                        if(ttarch.isEncrypted)
                                        {
                                            BlowFishCS.BlowFish dec = new BlowFishCS.BlowFish(key, ttarch.version);
                                            tmp = dec.Crypt_ECB(tmp, ttarch.version, true);
                                        }

                                        tmp = decompressBlock(tmp, ttarch.compressAlgorithm);

                                        mbw.Write(tmp);
                                    }

                                    byte[] block = ms.ToArray();

                                    byte[] file = new byte[ttarch.files[i].fileSize];

                                    Array.Copy(block, c_off, file, 0, file.Length);

                                    string fileName = ttarch.files[i].fileName;
                                    if ((fileName.Substring(fileName.Length - 5, 5) == ".lenc") && decrypt)
                                    {
                                        fileName = fileName.Remove(fileName.Length - 4, 4) + "lua";
                                        file = Methods.decryptLua(file, key, ttarch.version);
                                    }

                                    File.WriteAllBytes(fbd.SelectedPath + Path.DirectorySeparatorChar + fileName, file);
                                }
                            }
                        }

                        progressBar1.Value = i;
                    }

                    br.Close();
                    fs.Close();
                }
            }
            else if(ttarch2 != null)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    progressBar1.Minimum = 0;
                    progressBar1.Maximum = ttarch2.files.Length > 1 ? ttarch2.files.Length : 1;

                    bool decrypt = decryptLuaCB.Checked;
                    byte[] key = MainMenu.gamelist[gameListCB.SelectedIndex].key;

                    FileStream fs = new FileStream(ttarch2.fileName, FileMode.Open);
                    BinaryReader br = new BinaryReader(fs);
                    for (int i = 0; i < ttarch2.files.Length; i++)
                    {
                        if (ttarch2.isCompressed)
                        {
                            int index = (int)((ttarch2.filesOffset + ttarch2.files[i].fileOffset) / ttarch2.chunkSize);
                            int index2 = (int)((ttarch2.filesOffset + ttarch2.files[i].fileOffset + (ulong)ttarch2.files[i].fileSize) / (ulong)(ttarch2.chunkSize));
                            if(index2 + 2 < ttarch2.compressedBlocks.Length) index2++;
                            ulong cOff = 0;

                            for(int c = 0; c < index; c++)
                            {
                                cOff += (ttarch2.compressedBlocks[c + 1] - ttarch2.compressedBlocks[c]);
                            }

                            //br.BaseStream.Seek((long)(cOff + ttarch2.filesOffset), SeekOrigin.Begin);
                            br.BaseStream.Seek((long)(cOff + ttarch2.cFilesOffset), SeekOrigin.Begin);

                            using (MemoryStream ms = new MemoryStream())
                            {
                                for (int c = index; c <= index2; c++)
                                {
                                    var posi = br.BaseStream.Position;
                                    byte[] tmp = br.ReadBytes((int)(ttarch2.compressedBlocks[c + 1] - ttarch2.compressedBlocks[c]));

                                    ulong bl = ttarch2.compressedBlocks[c + 1];
                                    ulong bl2 = ttarch2.compressedBlocks[c];

                                    if(tmp.Length >= 0x10000)
                                    {
                                        int pause = 1;
                                    }

                                    if (ttarch2.isEncrypted)
                                    {
                                        BlowFishCS.BlowFish dec = new BlowFishCS.BlowFish(key, 7);
                                        tmp = dec.Crypt_ECB(tmp, 7, true);
                                    }

                                    tmp = decompressBlock(tmp, ttarch2.compressAlgorithm);

                                    if(tmp == null || tmp.Length == 0)
                                    {
                                        MessageBox.Show("TTG Tools couldn't decompress block. Compress algorithm is " + Convert.ToString(ttarch2.compressAlgorithm), "Decompress error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }

                                    ms.Write(tmp, 0, tmp.Length);
                                }

                                byte[] block = ms.ToArray();
                                byte[] file = new byte[ttarch2.files[i].fileSize];
                                //ulong dOff = cOff - (ulong)(ttarch2.chunkSize * index);
                                ulong dOff = (ttarch2.filesOffset + ttarch2.files[i].fileOffset) - (ulong)(ttarch2.chunkSize * index);
                                Array.Copy(block, (long)dOff, file, 0, file.Length);
                                string fileName = ttarch2.files[i].fileName;

                                if ((Methods.GetExtension(fileName).ToLower() == ".lenc") || (Methods.GetExtension(fileName).ToLower() == ".lua") && decrypt)
                                {
                                    if (fileName.Substring(fileName.Length - 4, 4).ToLower() == "lenc") fileName = fileName.Remove(fileName.Length - 4, 4) + "lua";
                                    file = Methods.decryptLua(file, key, 7);
                                }

                                File.WriteAllBytes(fbd.SelectedPath + Path.DirectorySeparatorChar + fileName, file);
                            }
                        }
                        else
                        {
                            br.BaseStream.Seek((long)ttarch2.filesOffset + (long)ttarch2.files[i].fileOffset, SeekOrigin.Begin);
                            byte[] file = br.ReadBytes(ttarch2.files[i].fileSize);
                            string fileName = ttarch2.files[i].fileName;

                            if ((Methods.GetExtension(fileName).ToLower() == ".lenc") || (Methods.GetExtension(fileName).ToLower() == ".lua") && decrypt)
                            {
                                if(fileName.Substring(fileName.Length - 4, 4).ToLower() == "lenc") fileName = fileName.Remove(fileName.Length - 4, 4) + "lua";
                                file = Methods.decryptLua(file, key, 7);
                            }

                            File.WriteAllBytes(fbd.SelectedPath + Path.DirectorySeparatorChar + fileName, file);
                        }

                        progressBar1.Value = i + 1;
                    }
                    br.Close();
                    fs.Close();
                }
            }
            else
            {
                MessageBox.Show("Nothing to extract. Please open ttarch/ttarch2 file and then extract.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
