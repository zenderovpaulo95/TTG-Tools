using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace TTG_Tools
{
    public partial class ArchiveUnpacker : Form
    {
        public ArchiveUnpacker()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private static List<string> fileFormats = new List<string>();

        public struct ttarchFiles
        {
            public string fileName;
            public uint fileOffset;
            public int fileSize;
        }

        private static byte[] DeflateDecompressor(byte[] bytes) //Для старых (версии 8 и 9) и новых архивов
        {
            byte[] retVal;
            using (MemoryStream decompressedMemoryStream = new MemoryStream(bytes))
            {
                using (System.IO.Compression.DeflateStream decompressStream = new System.IO.Compression.DeflateStream(decompressedMemoryStream, System.IO.Compression.CompressionMode.Decompress, true))
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

        private static ttarchFiles[] ReadTtarch(string path, byte[] key)
        {
            try
            {
                fileFormats = new List<string>();
                FileStream fs = new FileStream(path, FileMode.Open);
                BinaryReader br = new BinaryReader(fs);
                int version = br.ReadInt32();
                int encryption = br.ReadInt32();
                int two = br.ReadInt32();

                int countCompressedBlocks;
                int[] compressedSize;
                int val = 0;

                if (version > 2)
                {
                    val = br.ReadInt32();
                    countCompressedBlocks = br.ReadInt32();

                    if(val == 2)
                    {
                        compressedSize = new int[countCompressedBlocks];

                        for(int k = 0; k < countCompressedBlocks; k++)
                        {
                            compressedSize[k] = br.ReadInt32();
                        }
                    }

                    uint arcSize = br.ReadUInt32(); //Size of block with files

                    if (version >= 4)
                    {
                        int priority = br.ReadInt32();
                        int priority2 = br.ReadInt32();

                        if(version >= 7)
                        {
                            int someVal = br.ReadInt32();
                            int someVal2 = br.ReadInt32();
                            int chunkSize = br.ReadInt32();

                            if(version > 7)
                            {
                                byte b = br.ReadByte();

                                if(version == 9)
                                {
                                    uint crc32 = br.ReadUInt32();
                                }
                            }
                        }
                    }
                }

                int headerSize = br.ReadInt32();
                int cHeaderSize = -1;

                if (version >= 7 && val == 2)
                {
                    cHeaderSize = br.ReadInt32();
                }

                byte[] header = version >= 7 && val == 2 ? br.ReadBytes(cHeaderSize) : br.ReadBytes(headerSize);

                if(version >= 7 && val == 2)
                {
                    switch(version)
                    {
                        case 7:
                            header = ZLibDecompressor(header);
                            break;

                        default:
                            header = DeflateDecompressor(header);
                            break;
                    }
                }

                if(encryption == 1)
                {
                    BlowFishCS.BlowFish dec = new BlowFishCS.BlowFish(key, version);
                    header = dec.Crypt_ECB(header, version, true);
                }

                ttarchFiles[] files = null;

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

                        files = new ttarchFiles[filesCount];

                        for(int f = 0; f < filesCount; f++)
                        {
                            int nameLen = mbr.ReadInt32();
                            byte[] tmpName = mbr.ReadBytes(nameLen);
                            files[f].fileName = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(tmpName);
                            int zeroVal = mbr.ReadInt32(); //always shows 0 value
                            files[f].fileOffset = mbr.ReadUInt32();
                            files[f].fileSize = mbr.ReadInt32();

                            string ext = Methods.GetExtension(files[f].fileName);

                            if ((ext != "") && !fileFormats.Contains(ext))
                            {
                                fileFormats.Add(ext);
                            }
                        }
                    }
                }

                br.Close();
                fs.Close();

                return files;
            }
            catch
            {
                MessageBox.Show("Something goes wrong", "Unknown error. Please try another archive.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private static void ReadTtarch2(string path)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "TTARCH archives (*.ttarch) | *.ttarch| TTARCH2 archives (*.ttarch2) | *.ttarch2";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                FileInfo fi = new FileInfo(ofd.FileName);

                switch (fi.Extension.ToLower())
                {
                    case ".ttarch":
                        ttarchFiles[] files = ReadTtarch(fi.FullName, MainMenu.gamelist[gameListCB.SelectedIndex].key);

                        if (files != null)
                        {
                            filesDataGridView.ColumnCount = 4;
                            filesDataGridView.RowCount = files.Length;
                            fileFormatsCB.Items.Clear();

                            if(fileFormats.Count > 0)
                            {
                                fileFormatsCB.Items.Add("All files");

                                for(int f = 0; f < fileFormats.Count; f++)
                                {
                                    fileFormatsCB.Items.Add(fileFormats[f]);
                                }

                                fileFormatsCB.SelectedIndex = 0;
                            }

                            for (int i = 0; i < files.Length; i++)
                            {
                                filesDataGridView[0, i].Value = Convert.ToString(i + 1);
                                filesDataGridView[1, i].Value = files[i].fileName;
                                filesDataGridView[2, i].Value = Convert.ToString(files[i].fileOffset);
                                filesDataGridView[3, i].Value = Convert.ToString(files[i].fileSize);
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
    }
}
