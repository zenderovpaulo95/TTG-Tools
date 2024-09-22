using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace TTG_Tools
{
    public partial class ArchiveUnpacker : Form
    {
        public ArchiveUnpacker()
        {
            InitializeComponent();
        }

        public static ClassesStructs.TtarchClass ttarch;

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private static byte[] decompressBlock(byte[] bytes)
        {
            try
            {
                byte[] buf = ZLibDecompressor(bytes);
                ttarch.compressAlgorithm = 0;
                return buf;
            }
            catch
            {
                try
                {
                    //Try deflate decompress
                    byte[] buf = DeflateDecompressor(bytes);
                    ttarch.compressAlgorithm = 1;
                    return buf;
                }
                catch
                {
                    //Else return empty bytes
                    ttarch.compressAlgorithm = -1; //Unknown algorithm
                    return null;
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

        private static void ReadTtarch2(string path)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "All supported files (*.ttarch, *.ttarch2) | *.ttarch;*.ttarch2| TTARCH archives (*.ttarch) | *.ttarch| TTARCH2 archives (*.ttarch2) | *.ttarch2";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                FileInfo fi = new FileInfo(ofd.FileName);

                ttarch = null;

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
                            string chunkSzStr = "Chunk size: " + Convert.ToString(ttarch.chunkSize);

                            compressionLabel.Text = compressedStr;
                            encryptionLabel.Text = encryptedStr;
                            xmodeLabel.Text = xmodeStr;
                            chunkSizeLabel.Text = chunkSzStr;
                            versionLabel.Text = "Version: " + Convert.ToString(ttarch.version);

                            if(ttarch.fileFormats.Count > 0)
                            {
                                fileFormatsCB.Items.Add("All files");

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

                                        tmp = decompressBlock(tmp);

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
            else
            {
                MessageBox.Show("Nothing to extract. Please open ttarch/ttarch2 file and then extract.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
