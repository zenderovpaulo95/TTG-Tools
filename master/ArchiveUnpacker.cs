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

        FileStruct[] FileList;

        private FileStruct[] GetFileListTTARCH(string FileName)
        {
            FileStruct[] Files = null;

            try
            {
                FileStream fs = new FileStream(FileName, FileMode.Open);
                BinaryReader br = new BinaryReader(fs);
                uint f_off = 0;
                int version = br.ReadInt32();
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

                int ArcSize = br.ReadInt32();
                f_off += Convert.ToUInt32(ArcSize) + 4;

                byte[] block = br.ReadBytes(ArcSize);

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
                    br.BaseStream.Seek(FileList[i].FileOffset, SeekOrigin.Begin);
                    byte[] tmp = br.ReadBytes(FileList[i].Size);
                    string FilePath = dirPathTB.Text + "\\" + FileList[i].FileName;

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
