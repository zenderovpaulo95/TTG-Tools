using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using BlowFishCS;
using zlib;

namespace TTG_Tools
{
    public partial class ArchiveUnpacker : Form
    {
        public ArchiveUnpacker()
        {
            InitializeComponent();
        }

        struct FileStruct
        {
            public string FileName;
            public uint Offset;
            public uint FileOffset; //In oldest archives
            public int Size;
        }

        void Progress(int i)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new ProgressHandler(Progress), i);
            }
            else
            {
                progressBar1.Value = i;
            }
        }

        byte[] key = null;
        int version;
        int[] compressedBlocks = null;
        uint arcSize = 0;
        uint cArcSize = 0;
        FileStruct[] FileList;

        private static void CopyStream(Stream inStream, Stream outStream)
        {
            byte[] buffer = new byte[2000];
            int len;
            while ((len = inStream.Read(buffer, 0, 2000)) > 0)
            {
                outStream.Write(buffer, 0, len);
            }
            outStream.Flush();
        }

        private static byte[] ZlibDeCompressor(byte[] bytes) //Для старых архивов (с версии 3 по 7)
        {
            byte[] retBytes;
            using (MemoryStream outMemoryStream = new MemoryStream())
            using (ZOutputStream outZStream = new ZOutputStream(outMemoryStream))
            using (Stream inMemoryStream = new MemoryStream(bytes))
            {
                CopyStream(inMemoryStream, outZStream);
                outZStream.finish();
                retBytes = outMemoryStream.ToArray();
            }

            return retBytes;
        }

        private static byte[] DeflateDeCompressor(byte[] bytes) //Для старых (версии 8 и 9) и новых архивов
        {
            byte[] retVal;
            using (MemoryStream compressedMemoryStream = new MemoryStream())
            {
                System.IO.Compression.DeflateStream compressStream = new System.IO.Compression.DeflateStream(compressedMemoryStream, System.IO.Compression.CompressionMode.Decompress, true);
                compressStream.Write(bytes, 0, bytes.Length);
                compressStream.Close();
                retVal = new byte[compressedMemoryStream.Length];
                compressedMemoryStream.Position = 0L;
                compressedMemoryStream.Read(retVal, 0, retVal.Length);
                compressedMemoryStream.Close();
                compressStream.Close();
            }
            return retVal;
        }

        private FileStruct[] GetFileListTTARCH(string FileName)
        {
            FileStruct[] Files = null;

            try
            {
                FileStream fs = new FileStream(FileName, FileMode.Open);
                BinaryReader br = new BinaryReader(fs);
                uint f_off = 0;
                version = br.ReadInt32();
                f_off += 4;

                if(version < 2 || version > 9)
                {
                    MessageBox.Show("This archive doesn't support", "Error");
                    return null;
                }

                int encrypted = br.ReadInt32();
                int two = br.ReadInt32();
                f_off += 8;

                if(two != 2)
                {
                    MessageBox.Show("Unknown archive", "Error");
                    return null;
                }

                if (version > 2)
                {
                    int archMode = br.ReadInt32(); //Compressed or not archive
                    f_off += 4;
                    int blockCount = br.ReadInt32();
                    f_off += 4;

                    compressedBlocks = blockCount == 2 ? null : new int[blockCount];

                    for(int i = 0; i < blockCount; i++)
                    {
                        compressedBlocks[i] = br.ReadInt32();
                        f_off += 4;
                    }

                    arcSize = br.ReadUInt32();
                }

                int headerSize = br.ReadInt32();
                f_off += Convert.ToUInt32(headerSize) + 4;

                int blockSize = cArcSize != 0 ? 0 : headerSize;

                byte[] block = br.ReadBytes(blockSize);

                if(encrypted == 1)
                {
                    key = MainMenu.gamelist[encKeyListCB.SelectedIndex].key;
                    BlowFish blow = new BlowFish(key, version);
                    block = blow.Crypt_ECB(block, version, true);
                }

                MemoryStream ms = new MemoryStream(block);
                BinaryReader brms = new BinaryReader(ms);
                int dirCount = brms.ReadInt32();
                string[] dirs = new string[dirCount];

                for(int i = 0; i < dirCount; i++)
                {
                    int len = brms.ReadInt32();
                    byte[] tmp = brms.ReadBytes(len);
                    dirs[i] = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(tmp);
                }

                int fileCount = brms.ReadInt32();
                Files = new FileStruct[fileCount];

                for(int i = 0;i < fileCount; i++)
                {
                    int len = brms.ReadInt32();
                    byte[] tmp = brms.ReadBytes(len);
                    Files[i].FileName = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(tmp);
                    int zero = brms.ReadInt32();
                    Files[i].Offset = brms.ReadUInt32();
                    Files[i].FileOffset = Files[i].Offset + f_off;
                    Files[i].Size = brms.ReadInt32();
                }

                brms.Close();
                ms.Close();

                block = null;
                dirs = null;

                br.Close();
                fs.Close();

                GC.Collect();
            }
            catch
            {
                return null;
            }


            return Files;
        }

        private void FileBrowseBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filePathTB.Text = ofd.FileName;

                FileList = GetFileListTTARCH(ofd.FileName);

                if((FileList != null) && (FileList.Length > 0))
                {
                    dataGridView1.ColumnCount = 3;
                    dataGridView1.RowCount = FileList.Length;
                    dataGridView1.Columns[0].HeaderText = "File name";
                    dataGridView1.Columns[1].HeaderText = "File offset";
                    dataGridView1.Columns[2].HeaderText = "File size";

                    for(int i = 0; i < FileList.Length; i++)
                    {
                        dataGridView1[0, i].Value = FileList[i].FileName;
                        dataGridView1[1, i].Value = Convert.ToString(FileList[i].Offset);
                        dataGridView1[2, i].Value = Convert.ToString(FileList[i].Size);
                    }
                }
            }
        }

        private void ResourceDirBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if(fbd.ShowDialog() == DialogResult.OK)
            {
                dirPathTB.Text = fbd.SelectedPath;
            }
        }

        private void ArchiveUnpacker_Load(object sender, EventArgs e)
        {
            for(int i = 0; i < MainMenu.gamelist.Count; i++)
            {
                encKeyListCB.Items.Add((i + 1).ToString() + ". " + MainMenu.gamelist[i].gamename);
            }

            encKeyListCB.SelectedIndex = 0;
        }

        private void unpackBtn_Click(object sender, EventArgs e)
        {
            if(Directory.Exists(dirPathTB.Text) && File.Exists(filePathTB.Text)
                && (FileList != null) && (FileList.Length > 0))
            {
                progressBar1.Minimum = 0;
                progressBar1.Maximum = FileList.Length - 1;

                FileStream fs = new FileStream(filePathTB.Text, FileMode.Open);
                BinaryReader br = new BinaryReader(fs);

                for(int i = 0; i < FileList.Length; i++)
                {
                    string format = FileList[i].FileName.Substring(FileList[i].FileName.Length - 5, 5);
                    br.BaseStream.Seek(FileList[i].FileOffset, SeekOrigin.Begin);
                    byte[] tmp = br.ReadBytes(FileList[i].Size);
                    Methods.meta_crypt(tmp, key, version, true);

                    string FileName = FileList[i].FileName;
                    if (format.ToLower() == ".lenc")
                    {
                        FileName = FileList[i].FileName.Remove(FileList[i].FileName.Length - 5, 5) + ".lua";

                        BlowFish decLua = new BlowFish(key, version);
                        tmp = decLua.Crypt_ECB(tmp, version, true);
                    }

                    string FilePath = dirPathTB.Text + "\\" + FileName;

                    if (File.Exists(FilePath)) File.Delete(FilePath);

                    FileStream fsw = new FileStream(FilePath, FileMode.CreateNew);
                    BinaryWriter bw = new BinaryWriter(fsw);
                    bw.Write(tmp);
                    bw.Close();
                    fsw.Close();

                    Progress(i);

                    tmp = null;
                }

                br.Close();
                fs.Close();

                GC.Collect();
            }
        }
    }
}
